﻿using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Media.Session;
using Android.Views;
using Android.Widget;
using AndroidX.LocalBroadcastManager.Content;
using AndroidX.Media3.Common;
using AndroidX.Media3.Common.Text;
using AndroidX.Media3.Common.Util;
using AndroidX.Media3.ExoPlayer;
using AndroidX.Media3.UI;
using CommunityToolkit.Maui.ApplicationModel.Permissions;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Media.Services;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using AudioAttributes = AndroidX.Media3.Common.AudioAttributes;
using DeviceInfo = AndroidX.Media3.Common.DeviceInfo;
using SystemClock = Android.OS.SystemClock;

namespace CommunityToolkit.Maui.Core.Views;

public partial class MediaManager : Java.Lang.Object, IPlayerListener
{
	static readonly HttpClient client = new();

	readonly SemaphoreSlim seekToSemaphoreSlim = new(1, 1);

	Task? checkPermissionsTask;
	CancellationTokenSource checkPermissionSourceToken = new();
	CancellationTokenSource startServiceSourceToken = new();
	double? previousSpeed;
	float volumeBeforeMute = 1;
	MediaItem.Builder? mediaItem;
	TaskCompletionSource? seekToTaskCompletionSource;
	AndroidX.Media3.Session.MediaSession? session;
	MediaElementState currentState;
	UIUpdateReceiver? uiUpdateReceiver;

	/// <summary>
	/// The platform native counterpart of <see cref="MediaElement"/>.
	/// </summary>
	protected PlayerView? PlayerView { get; set; }

	/// <summary>
	/// Retrieves bitmap for the given url
	/// </summary>
	/// <param name="url"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static async Task<Bitmap?> GetBitmapFromUrl(string? url, CancellationToken cancellationToken = default)
	{
		var bitmapConfig = Bitmap.Config.Argb8888 ?? throw new InvalidOperationException("Bitmap config cannot be null");
		var bitmap = Bitmap.CreateBitmap(1024, 768, bitmapConfig, true);

		Canvas canvas = new();
		canvas.SetBitmap(bitmap);
		canvas.DrawColor(Android.Graphics.Color.White);
		canvas.Save();

		try
		{
			var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
			var stream = response.IsSuccessStatusCode ? await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false) : null;

			return stream switch
			{
				null => bitmap,
				_ => await BitmapFactory.DecodeStreamAsync(stream)
			};
		}
		catch
		{
			return bitmap;
		}
	}

	/// <summary>
	/// Occurs when ExoPlayer changes the playback parameters.
	/// </summary>
	/// <paramref name="playbackParameters">Object containing the new playback parameter values.</paramref>
	/// <remarks>
	/// This is part of the <see cref="IPlayerListener"/> implementation.
	/// While this method does not seem to have any references, it's invoked at runtime.
	/// </remarks>
	public void OnPlaybackParametersChanged(PlaybackParameters? playbackParameters)
	{
		if (playbackParameters is null || AreFloatingPointNumbersEqual(playbackParameters.Speed, MediaElement.Speed))
		{
			return;
		}

		MediaElement.Speed = playbackParameters.Speed;
	}

	/// <summary>
	/// Occurs when ExoPlayer changes the player state.
	/// </summary>
	/// <paramref name="playWhenReady">Indicates whether the player should start playing the media whenever the media is ready.</paramref>
	/// <paramref name="playbackState">The state that the player has transitioned to.</paramref>
	/// <remarks>
	/// This is part of the <see cref="IPlayerListener"/> implementation.
	/// While this method does not seem to have any references, it's invoked at runtime.
	/// </remarks>
	public async void OnPlayerStateChanged(bool playWhenReady, int playbackState)
	{
		if (Player is null || MediaElement.Source is null)
		{
			return;
		}
		var newState = playbackState switch
		{
			PlaybackStateCompat.StateFastForwarding
				or PlaybackStateCompat.StateRewinding
				or PlaybackStateCompat.StateSkippingToNext
				or PlaybackStateCompat.StateSkippingToPrevious
				or PlaybackStateCompat.StateSkippingToQueueItem
				or PlaybackStateCompat.StatePlaying => playWhenReady
					? MediaElementState.Playing
					: MediaElementState.Paused,

			PlaybackStateCompat.StatePaused => MediaElementState.Paused,

			PlaybackStateCompat.StateConnecting
				or PlaybackStateCompat.StateBuffering => MediaElementState.Buffering,

			PlaybackStateCompat.StateNone => MediaElementState.None,
			PlaybackStateCompat.StateStopped => MediaElement.CurrentState is not MediaElementState.Failed
				? MediaElementState.Stopped
				: MediaElementState.Failed,

			PlaybackStateCompat.StateError => MediaElementState.Failed,

			_ => MediaElementState.None,
		};

		MediaElement.CurrentStateChanged(newState);
		if (playbackState is PlaybackStateCompat.StatePlaying)
		{
			MediaElement.Duration = TimeSpan.FromMilliseconds(Player.Duration < 0 ? 0 : Player.Duration);
			MediaElement.Position = TimeSpan.FromMilliseconds(Player.CurrentPosition < 0 ? 0 : Player.CurrentPosition);
			try
			{
				await StartService(startServiceSourceToken.Token);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine($"[error] {e}, {e.Message}");
			}
		}

		if (MediaElement.CurrentState == currentState)
		{
			return;
		}

		currentState = MediaElement.CurrentState;

		BroadcastUpdate(newState is MediaElementState.Playing
			? MediaControlsService.ACTION_PLAY
			: MediaControlsService.ACTION_PAUSE);
	}

	/// <summary>
	/// Creates the corresponding platform view of <see cref="MediaElement"/> on Android.
	/// </summary>
	/// <returns>The platform native counterpart of <see cref="MediaElement"/>.</returns>
	/// <exception cref="NullReferenceException">Thrown when <see cref="Android.Content.Context"/> is <see langword="null"/> or when the platform view could not be created.</exception>
	[MemberNotNull(nameof(PlayerView))]
	[MemberNotNull(nameof(checkPermissionsTask))]
	public (PlatformMediaElement platformView, PlayerView PlayerView) CreatePlatformView()
	{
		ArgumentNullException.ThrowIfNull(MauiContext.Context);
		Player = new ExoPlayerBuilder(MauiContext.Context).Build() ?? throw new NullReferenceException();
		Player.AddListener(this);
		Player.SetHandleAudioBecomingNoisy(true);
		var audioAttributes = new AudioAttributes.Builder();
		audioAttributes.SetUsage(C.UsageMedia);
		audioAttributes.SetContentType(C.DataTypeMedia);
		Player.SetAudioAttributes(audioAttributes.Build(), true);

		PlayerView = new PlayerView(MauiContext.Context)
		{
			Player = Player,
			UseController = false,
			ControllerAutoShow = false,
			LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
		};

		checkPermissionsTask = CheckAndRequestForegroundPermission(checkPermissionSourceToken.Token);

		session = new AndroidX.Media3.Session.MediaSession.Builder(Platform.AppContext, Player).Build();
		uiUpdateReceiver ??= new UIUpdateReceiver(Player);
		if (Build.VERSION.SdkInt <= BuildVersionCodes.Tiramisu)
		{
			LocalBroadcastManager.GetInstance(Platform.AppContext).RegisterReceiver(uiUpdateReceiver, new IntentFilter(MediaControlsService.ACTION_UPDATE_PLAYER));
		}
		return (Player, PlayerView);
	}

	/// <summary>
	/// Occurs when ExoPlayer changes the playback state.
	/// </summary>
	/// <paramref name="playbackState">The state that the player has transitioned to.</paramref>
	/// <remarks>
	/// This is part of the <see cref="IPlayerListener"/> implementation.
	/// While this method does not seem to have any references, it's invoked at runtime.
	/// </remarks>
	public void OnPlaybackStateChanged(int playbackState)
	{
		if (MediaElement.Source is null)
		{
			return;
		}

		MediaElementState newState = MediaElement.CurrentState;
		switch (playbackState)
		{
			case PlaybackStateCompat.StateBuffering:
				newState = MediaElementState.Buffering;
				break;
			case PlaybackStateCompat.StateStopped:
				newState = MediaElementState.Stopped;
				MediaElement.MediaEnded();
				break;
			case PlaybackStateCompat.StatePlaying:
				seekToTaskCompletionSource?.TrySetResult();
				break;
		}

		MediaElement.CurrentStateChanged(newState);
	}

	/// <summary>
	/// Occurs when ExoPlayer encounters an error.
	/// </summary>
	/// <paramref name="error">An instance of <seealso cref="PlaybackException"/> containing details of the error.</paramref>
	/// <remarks>
	/// This is part of the <see cref="IPlayerListener"/> implementation.
	/// While this method does not seem to have any references, it's invoked at runtime.
	/// </remarks>
	public void OnPlayerError(PlaybackException? error)
	{
		var errorMessage = string.Empty;
		var errorCode = string.Empty;
		var errorCodeName = string.Empty;

		if (!string.IsNullOrWhiteSpace(error?.LocalizedMessage))
		{
			errorMessage = $"Error message: {error.LocalizedMessage}";
		}

		if (error?.ErrorCode is not null)
		{
			errorCode = $"Error code: {error?.ErrorCode}";
		}

		if (!string.IsNullOrWhiteSpace(error?.ErrorCodeName))
		{
			errorCodeName = $"Error codename: {error?.ErrorCodeName}";
		}

		var message = string.Join(", ", new[]
		{
			errorCodeName,
			errorCode,
			errorMessage
		}.Where(s => !string.IsNullOrEmpty(s)));

		MediaElement.MediaFailed(new MediaFailedEventArgs(message));

		Logger.LogError("{logMessage}", message);
	}

	/// <summary>
	/// Invoked when a seek operation has been processed.
	/// </summary>
	/// <remarks>
	/// This is part of the <see cref="IPlayerListener"/> implementation.
	/// While this method does not seem to have any references, it's invoked at runtime.
	/// </remarks>
	public void OnSeekProcessed()
	{
		// Deprecated in ExoPlayer v2.12.0
		// Use OnPlaybackStateChanged with STATE_READY instead: https://stackoverflow.com/a/65745607/5953643
	}

	/// <summary>
	/// Occurs when ExoPlayer changes volume.
	/// </summary>
	/// <param name="volume">The new value for volume.</param>
	/// <remarks>
	/// This is part of the <see cref="IPlayerListener"/> implementation.
	/// While this method does not seem to have any references, it's invoked at runtime.
	/// </remarks>
	public void OnVolumeChanged(float volume)
	{
		if (Player is null)
		{
			return;
		}

		// When currently muted, ignore
		if (MediaElement.ShouldMute)
		{
			return;
		}

		MediaElement.Volume = volume;
	}

	protected virtual partial void PlatformPlay()
	{
		if (Player is null || MediaElement.Source is null)
		{
			return;
		}
		Player.Prepare();
		Player.Play();
		BroadcastUpdate(MediaControlsService.ACTION_PLAY);
	}

	protected virtual partial void PlatformPause()
	{
		if (Player is null || MediaElement.Source is null)
		{
			return;
		}
		Player.Pause();
		BroadcastUpdate(MediaControlsService.ACTION_PAUSE);
	}
	protected virtual async partial Task PlatformSeek(TimeSpan position, CancellationToken token)
	{
		if (Player is null)
		{
			throw new InvalidOperationException($"{nameof(IExoPlayer)} is not yet initialized");
		}

		await seekToSemaphoreSlim.WaitAsync(token);

		seekToTaskCompletionSource = new();
		try
		{
			Player.SeekTo((long)position.TotalMilliseconds);
			MediaElement.Position = Player.CurrentPosition < 0 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(Player.CurrentPosition);
			await seekToTaskCompletionSource.Task.WaitAsync(TimeSpan.FromMinutes(2), token).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing | ConfigureAwaitOptions.ContinueOnCapturedContext);
			
			MediaElement.SeekCompleted();
		}
		finally
		{
			seekToSemaphoreSlim.Release();
		}
	}

	protected virtual partial void PlatformStop()
	{
		if (Player is null || MediaElement.Source is null)
		{
			return;
		}
		Player.SeekToDefaultPosition();
		Player.Stop();
	}

	protected virtual partial void PlatformUpdateSource()
	{
		var hasSetSource = false;

		if (Player is null)
		{
			return;
		}

		StopService();
		if (MediaElement.Source is null)
		{
			Player.ClearMediaItems();
			MediaElement.Duration = TimeSpan.Zero;
			MediaElement.CurrentStateChanged(MediaElementState.None);

			return;
		}

		MediaElement.CurrentStateChanged(MediaElementState.Opening);

		Player.PlayWhenReady = MediaElement.ShouldAutoPlay;
		
		if (MediaElement.Source is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri;
			if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
			{
				var item = SetPlayerData(uri.AbsoluteUri);
				Player.SetMediaItem(item.Build());
				Player.Prepare();
				hasSetSource = true;
			}
		}
		else if (MediaElement.Source is FileMediaSource fileMediaSource)
		{
			var filePath = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filePath))
			{
				var item = SetPlayerData(filePath);
				Player.SetMediaItem(item.Build());
				Player.Prepare();
				hasSetSource = true;
			}
		}
		else if (MediaElement.Source is ResourceMediaSource resourceMediaSource)
		{
			var package = PlayerView?.Context?.PackageName ?? "";
			var path = resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				var assetFilePath = $"asset://{package}{System.IO.Path.PathSeparator}{path}";
				var item = SetPlayerData(assetFilePath);
				Player.SetMediaItem(item.Build());
				Player.Prepare();
				hasSetSource = true;
			}
		}

		if (hasSetSource && Player.PlayerError is null)
		{
			MediaElement.MediaOpened();
		}
	}

	MediaItem.Builder SetPlayerData(string url)
	{
		MediaMetadata.Builder mediaMetaData = new MediaMetadata.Builder();
		mediaMetaData.SetAlbumArtist(MediaElement.MetadataArtist);
		mediaMetaData.SetArtist(MediaElement.MetadataArtist);
		mediaMetaData.SetTitle(MediaElement.MetadataTitle);
		mediaMetaData.SetArtworkUri(Android.Net.Uri.Parse(MediaElement.MetadataArtworkUrl));
		mediaMetaData.Build();

		mediaItem = new MediaItem.Builder();
		mediaItem.SetUri(url);
		mediaItem.SetMediaId(url);
		mediaItem.SetMediaMetadata(mediaMetaData.Build());
		ArgumentNullException.ThrowIfNull(session);
		session.SetSessionExtras(mediaItem.Build()?.ToBundle());
		ArgumentNullException.ThrowIfNull(Player);
		return mediaItem;
	}

	protected virtual partial void PlatformUpdateAspect()
	{
		if (PlayerView is null)
		{
			return;
		}
		
		PlayerView.ResizeMode = MediaElement.Aspect switch
		{
			Aspect.AspectFill => AspectRatioFrameLayout.ResizeModeZoom,
			Aspect.Fill => AspectRatioFrameLayout.ResizeModeFill,
			Aspect.Center or Aspect.AspectFit => AspectRatioFrameLayout.ResizeModeFit,
			_ => throw new NotSupportedException($"{nameof(Aspect)}: {MediaElement.Aspect} is not yet supported")
		};
	}

	protected virtual partial void PlatformUpdateSpeed()
	{
		if (Player is null)
		{
			return;
		}

		// First time we're getting a playback speed, set initial value
		previousSpeed ??= MediaElement.Speed;

		if (MediaElement.Speed > 0)
		{
			Player.SetPlaybackSpeed((float)MediaElement.Speed);

			if (previousSpeed == 0)
			{
				Player.Play();
			}

			previousSpeed = MediaElement.Speed;
		}
		else
		{
			previousSpeed = 0;
			Player.Pause();
		}
	}

	protected virtual partial void PlatformUpdateShouldShowPlaybackControls()
	{
		if (PlayerView is null)
		{
			return;
		}
		
		PlayerView.UseController = MediaElement.ShouldShowPlaybackControls;
	}

	protected virtual partial void PlatformUpdatePosition()
	{
		if (Player is null)
		{
			return;
		}

		if (MediaElement.Duration != TimeSpan.Zero)
		{
			MediaElement.Position = TimeSpan.FromMilliseconds(Player.CurrentPosition);
		}
	}

	protected virtual partial void PlatformUpdateVolume()
	{
		if (Player is null)
		{
			return;
		}

		// If the user changes while muted, change the internal field
		// and do not update the actual volume.
		if (MediaElement.ShouldMute)
		{
			volumeBeforeMute = (float)MediaElement.Volume;
			return;
		}

		Player.Volume = (float)MediaElement.Volume;
	}

	protected virtual partial void PlatformUpdateShouldKeepScreenOn()
	{
		if (PlayerView is null)
		{
			return;
		}

		PlayerView.KeepScreenOn = MediaElement.ShouldKeepScreenOn;
	}

	protected virtual partial void PlatformUpdateShouldMute()
	{
		if (Player is null)
		{
			return;
		}

		// We're going to muted state, capture current volume first
		// so we can restore later
		if (MediaElement.ShouldMute)
		{
			volumeBeforeMute = Player.Volume;
		}
		else if (!AreFloatingPointNumbersEqual(volumeBeforeMute, Player.Volume) && Player.Volume > 0)
		{
			volumeBeforeMute = Player.Volume;
		}

		Player.Volume = MediaElement.ShouldMute ? 0 : volumeBeforeMute;
	}

	protected virtual partial void PlatformUpdateShouldLoopPlayback()
	{
		if (Player is null)
		{
			return;
		}
 
		Player.RepeatMode = MediaElement.ShouldLoopPlayback ? RepeatModeUtil.RepeatToggleModeOne : RepeatModeUtil.RepeatToggleModeNone;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (disposing)
		{
			StopService();
			if (uiUpdateReceiver is not null)
			{
				LocalBroadcastManager.GetInstance(Platform.AppContext).UnregisterReceiver(uiUpdateReceiver);
			}
			Player?.RemoveListener(this);
			Player?.Release();
			uiUpdateReceiver?.Dispose();
			uiUpdateReceiver = null;
			checkPermissionSourceToken.Dispose();
			startServiceSourceToken.Dispose();

			client.Dispose();
		}
	}

	static void StopService()
	{
		var serviceIntent = new Intent(Platform.AppContext, typeof(MediaControlsService));
		Android.App.Application.Context.StopService(serviceIntent);
	}

	static async Task CheckAndRequestForegroundPermission(CancellationToken cancellationToken = default)
	{
		var status = await Permissions.CheckStatusAsync<AndroidMediaPermissions>().WaitAsync(cancellationToken);
		if (status is PermissionStatus.Granted)
		{
			return;
		}

		await Permissions.RequestAsync<AndroidMediaPermissions>().WaitAsync(cancellationToken).ConfigureAwait(false);
	}

	async Task StartService(CancellationToken cancellationToken = default)
	{
		if (checkPermissionsTask is not null)
		{
			await checkPermissionsTask.WaitAsync(cancellationToken);
		}

		var intent = new Intent(Android.App.Application.Context, typeof(MediaControlsService));

		if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
		{
			intent.PutExtra("title", MediaElement.MetadataTitle);
			intent.PutExtra("artist", MediaElement.MetadataArtist);
			intent.PutExtra("albumArtUri", MediaElement.MetadataArtworkUrl);
			intent.PutExtra("position", ((long)MediaElement.Position.TotalSeconds));
			intent.PutExtra("currentTime", SystemClock.ElapsedRealtime());
			intent.PutExtra("duration", ((long)MediaElement.Duration.TotalSeconds));
		}
		
		session?.SetSessionExtras(intent.Extras);
		ArgumentNullException.ThrowIfNull(session);
		intent.PutExtra("token", session.SessionCompatToken);
		
		if (OperatingSystem.IsAndroidVersionAtLeast(26))
		{
			Android.App.Application.Context.StartForegroundService(intent);
		}
		else
		{
			Android.App.Application.Context.StartService(intent);
		}
	}

	void BroadcastUpdate(string action)
	{
		if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
		{
			Logger.LogError("{LocalBroadcastManager} not supported on Android 13 and above.", typeof(LocalBroadcastManager));
			return;
		}
		Intent intent = new(MediaControlsService.ACTION_UPDATE_UI);
		intent.PutExtra("ACTION", action);
		LocalBroadcastManager.GetInstance(Platform.AppContext).SendBroadcast(intent);
	}

	public void OnVideoSizeChanged(VideoSize? videoSize)
	{
		MediaElement.MediaWidth = videoSize?.Width ?? 0;
		MediaElement.MediaHeight = videoSize?.Height ?? 0;
	}

	#region IPlayer.IListener implementation method stubs

	public void OnAudioAttributesChanged(AudioAttributes? audioAttributes) { }
	public void OnAudioSessionIdChanged(int audioSessionId) { }
	public void OnAvailableCommandsChanged(PlayerCommands? availableCommands) { }
	public void OnCues(CueGroup? cueGroup) { }
	public void OnDeviceInfoChanged(DeviceInfo? deviceInfo) { }
	public void OnDeviceVolumeChanged(int volume, bool muted) { }
	public void OnEvents(IPlayer? player, PlayerEvents? events) { }
	public void OnIsLoadingChanged(bool isLoading) { }
	public void OnIsPlayingChanged(bool isPlaying) { }
	public void OnLoadingChanged(bool isLoading) { }
	public void OnMaxSeekToPreviousPositionChanged(long maxSeekToPreviousPositionMs) { }
	public void OnMediaItemTransition(MediaItem? mediaItem, int reason) { }
	public void OnMediaMetadataChanged(MediaMetadata? mediaMetadata) { }
	public void OnMetadata(Metadata? metadata) { }
	public void OnPlayWhenReadyChanged(bool playWhenReady, int reason) { }
	public void OnPlaybackSuppressionReasonChanged(int playbackSuppressionReason) { }
	public void OnPlayerErrorChanged(PlaybackException? error) { }
	public void OnPlaylistMetadataChanged(MediaMetadata? mediaMetadata) { }
	public void OnPositionDiscontinuity(int reason) { }
	public void OnPositionDiscontinuity(PlayerPositionInfo? oldPosition, PlayerPositionInfo? newPosition, int reason) { }
	public void OnRenderedFirstFrame() { }
	public void OnRepeatModeChanged(int repeatMode) { }
	public void OnSeekBackIncrementChanged(long seekBackIncrementMs) { }
	public void OnSeekForwardIncrementChanged(long seekForwardIncrementMs) { }
	public void OnShuffleModeEnabledChanged(bool shuffleModeEnabled) { }
	public void OnSkipSilenceEnabledChanged(bool skipSilenceEnabled) { }
	public void OnSurfaceSizeChanged(int width, int height) { }
	public void OnTimelineChanged(Timeline? timeline, int reason) { }
	public void OnTrackSelectionParametersChanged(TrackSelectionParameters? parameters) { }
	public void OnTracksChanged(Tracks? tracks) { }
	public void OnTracksInfoChanged(Tracks tracksInfo) { }

	#endregion

	/// <summary>
	/// A <see cref="BroadcastReceiver"/> that listens for updates from the <see cref="MediaControlsService"/>.
	/// </summary>
	sealed class UIUpdateReceiver(IExoPlayer player) : BroadcastReceiver
	{
		public override void OnReceive(Context? context, Intent? intent)
		{
			ArgumentNullException.ThrowIfNull(intent);
			ArgumentNullException.ThrowIfNull(intent.Action);
			ArgumentNullException.ThrowIfNull(player);

			if (intent.Action is MediaControlsService.ACTION_UPDATE_PLAYER)
			{
				var action = intent.GetStringExtra("ACTION") ?? string.Empty;
				switch (action)
				{
					case MediaControlsService.ACTION_PLAY:
						player.Play();
						break;
					case MediaControlsService.ACTION_PAUSE:
						player.Pause();
						break;
					case MediaControlsService.ACTION_FASTFORWARD:
						player.SeekTo(player.CurrentPosition + 30_000);
						player.Play();
						break;
					case MediaControlsService.ACTION_REWIND:
						player.SeekTo(player.CurrentPosition - 10_000);
						player.Play();
						break;
				}
			}
		}
	}
}