using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.Media3.Common;
using AndroidX.Media3.Common.Text;
using AndroidX.Media3.Common.Util;
using AndroidX.Media3.DataSource;
using AndroidX.Media3.ExoPlayer;
using AndroidX.Media3.ExoPlayer.TrackSelection;
using AndroidX.Media3.Session;
using AndroidX.Media3.UI;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Media.Services;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using AudioAttributes = AndroidX.Media3.Common.AudioAttributes;
using DeviceInfo = AndroidX.Media3.Common.DeviceInfo;
using MediaMetadata = AndroidX.Media3.Common.MediaMetadata;
using Uri = System.Uri;

namespace CommunityToolkit.Maui.Core.Views;
public partial class MediaManager : Java.Lang.Object, IPlayerListener
{
	const int bufferState = 2;
	const int readyState = 3;
	const int endedState = 4;

	static readonly HttpClient client = new();
	readonly SemaphoreSlim seekToSemaphoreSlim = new(1, 1);

	double? previousSpeed;
	float volumeBeforeMute = 1;

	TaskCompletionSource? seekToTaskCompletionSource;
	CancellationTokenSource? cancellationTokenSource;
	MediaSession? session;
	MediaItem.Builder? mediaItem;
	BoundServiceConnection? connection;

	/// <summary>
	/// The platform native counterpart of <see cref="MediaElement"/>.
	/// </summary>
	CustomPlayerViewWithCast? PlayerView { get; set; }
	readonly Dictionary<string, int> trackIndexMap = [];

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

	public void UpdateNotifications()
	{
		if (connection?.Binder?.Service is null)
		{
			System.Diagnostics.Trace.TraceInformation("Notification Service not running.");
			return;
		}

		if (session is not null && Player is not null)
		{
			connection.Binder.Service.UpdateNotifications(session, Player);
		}
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
	public void OnPlayerStateChanged(bool playWhenReady, int playbackState)
	{
		if (Player is null || MediaElement.Source is null)
		{
			return;
		}

		var newState = playbackState switch
		{
			PlaybackState.StateFastForwarding
				or PlaybackState.StateRewinding
				or PlaybackState.StateSkippingToNext
				or PlaybackState.StateSkippingToPrevious
				or PlaybackState.StateSkippingToQueueItem
				or PlaybackState.StatePlaying => playWhenReady
					? MediaElementState.Playing
					: MediaElementState.Paused,

			PlaybackState.StatePaused => MediaElementState.Paused,

			PlaybackState.StateConnecting
				or PlaybackState.StateBuffering => MediaElementState.Buffering,

			PlaybackState.StateNone => MediaElementState.None,
			PlaybackState.StateStopped => MediaElement.CurrentState is not MediaElementState.Failed
				? MediaElementState.Stopped
				: MediaElementState.Failed,

			PlaybackState.StateError => MediaElementState.Failed,

			_ => MediaElementState.None,
		};

		MediaElement.CurrentStateChanged(newState);
		if (playbackState is readyState)
		{
			MediaElement.Duration = TimeSpan.FromMilliseconds(Player.Duration < 0 ? 0 : Player.Duration);
			MediaElement.Position = TimeSpan.FromMilliseconds(Player.CurrentPosition < 0 ? 0 : Player.CurrentPosition);
		}
	}

	/// <summary>
	/// Creates the corresponding platform view of <see cref="MediaElement"/> on Android.
	/// </summary>
	/// <returns>The platform native counterpart of <see cref="MediaElement"/>.</returns>
	/// <exception cref="NullReferenceException">Thrown when <see cref="Context"/> is <see langword="null"/> or when the platform view could not be created.</exception>
	[MemberNotNull(nameof(Player), nameof(PlayerView), nameof(session))]
	public (PlatformMediaElement platformView, CustomPlayerViewWithCast PlayerView) CreatePlatformView(AndroidViewType androidViewType)
	{
		var trackSelector = new DefaultTrackSelector(MauiContext.Context);
		var parametersBuilder = new AndroidX.Media3.ExoPlayer.TrackSelection.DefaultTrackSelector.Parameters.Builder();
		parametersBuilder.SetSelectUndeterminedTextLanguage(false);
		parametersBuilder.SetIgnoredTextSelectionFlags(C.SelectionFlagAutoselect);
		trackSelector.SetParameters(parametersBuilder);

		var temp = new ExoPlayerBuilder(MauiContext.Context) ?? throw new InvalidOperationException("Player cannot be null");
		temp.SetTrackSelector(trackSelector);

		var loadControlBuilder = new DefaultLoadControl.Builder();
		loadControlBuilder.SetBufferDurationsMs(
			DefaultLoadControl.DefaultMinBufferMs,
			DefaultLoadControl.DefaultMaxBufferMs,
			DefaultLoadControl.DefaultBufferForPlaybackMs,
			DefaultLoadControl.DefaultBufferForPlaybackAfterRebufferMs
		);

		temp.SetLoadControl(loadControlBuilder.Build());

		Player = temp.Build() ?? throw new InvalidOperationException("Player cannot be null");
		Player.AddListener(this);

		if (androidViewType is AndroidViewType.SurfaceView)
		{
			PlayerView = new CustomPlayerViewWithCast(MauiContext.Context!)
			{
				Player = Player,
				UseController = true,
				ControllerAutoShow = true,
				LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
			};
		}
		else if (androidViewType is AndroidViewType.TextureView)
		{
			if (MauiContext.Context?.Resources is null)
			{
				throw new InvalidOperationException("Unable to retrieve Android Resources");
			}

			var resources = MauiContext.Context.Resources;
			var xmlResource = resources.GetXml(Microsoft.Maui.Resource.Layout.textureview);
			xmlResource.Read();

			var attributes = Android.Util.Xml.AsAttributeSet(xmlResource)!;

			PlayerView = new CustomPlayerViewWithCast(MauiContext.Context, attributes)
			{
				Player = Player,
				UseController = true,
				ControllerAutoShow = true,
				LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
			};
		}
		else
		{
			throw new NotSupportedException($"{androidViewType} is not yet supported");
		}

		PlayerView.TrackSelectionRequested += OnTrackSelectionRequested;

		var mediaSession = new MediaSession.Builder(Platform.AppContext, Player);
		mediaSession.SetId(Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..8]);
		var dataSourceBitmapLoader = new DataSourceBitmapLoader(Platform.AppContext);
		mediaSession.SetBitmapLoader(dataSourceBitmapLoader);
		session ??= mediaSession.Build() ?? throw new InvalidOperationException("Session cannot be null");
		ArgumentNullException.ThrowIfNull(session.Id);

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
			case bufferState:
				newState = MediaElementState.Buffering;
				break;
			case endedState:
				newState = MediaElementState.Stopped;
				MediaElement.MediaEnded();
				break;
			case readyState:
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
			errorCode = $"Error code: {error.ErrorCode}";
		}

		if (!string.IsNullOrWhiteSpace(error?.ErrorCodeName))
		{
			errorCodeName = $"Error codename: {error.ErrorCodeName}";
		}

		var message = string.Join(", ", new[]
		{
			errorCodeName,
			errorCode,
			errorMessage
		}.Where(static s => !string.IsNullOrEmpty(s)));

		MediaElement.MediaFailed(new MediaFailedEventArgs(message));

		Logger.LogError("{LogMessage}", message);
	}

	public void OnVideoSizeChanged(VideoSize? videoSize)
	{
		MediaElement.MediaWidth = videoSize?.Width ?? 0;
		MediaElement.MediaHeight = videoSize?.Height ?? 0;
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
	}

	protected virtual partial void PlatformPause()
	{
		if (Player is null || MediaElement.Source is null)
		{
			return;
		}
		Player.Pause();
	}

	[MemberNotNull(nameof(Player))]
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
		Player.SeekTo(0);
		Player.Stop();
		MediaElement.Position = TimeSpan.Zero;
	}

	protected virtual async partial ValueTask PlatformUpdateSource()
	{
		if (Player is null)
		{
			return;
		}

		if (connection is null)
		{
			StartService();
		}

		if (MediaElement.Source is null)
		{
			Player.ClearMediaItems();
			MediaElement.Duration = TimeSpan.Zero;
			MediaElement.CurrentStateChanged(MediaElementState.None);
			return;
		}

		MediaElement.CurrentStateChanged(MediaElementState.Opening);
		Player.PlayWhenReady = MediaElement.ShouldAutoPlay;
		cancellationTokenSource ??= new();

		try
		{
			var result = await CreateMediaItemAsync(GetSource(MediaElement.Source), MediaElement.Drmextensions, cancellationTokenSource.Token);
			var item = result?.Build();

			if (item is not null)
			{
				Player.SetMediaItem(item);
				Player.Prepare();
			}

			if (Player.PlayerError is null)
			{
				MediaElement.MediaOpened();
				UpdateNotifications();
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error updating media source: {Message}", ex.Message);
			MediaElement.CurrentStateChanged(MediaElementState.Failed);
		}
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

			if (previousSpeed is 0)
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

		// We're going to mute state. Capture the current volume first so we can restore later.
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

	protected virtual partial void PlatformSelectTrack(string trackId)
	{
		if (Player is null || string.IsNullOrWhiteSpace(trackId))
		{
			return;
		}
		if(Player.TrackSelector is not DefaultTrackSelector trackSelector)
		{
			System.Diagnostics.Debug.WriteLine("TrackSelector is not DefaultTrackSelector, cannot select track.");
			return;
		}
		var mapped = trackSelector?.CurrentMappedTrackInfo;
		if (mapped is null)
		{
			System.Diagnostics.Debug.WriteLine("MappedTrackInfo is null, cannot select track.");
			return;
		}
		
		if (!trackIndexMap.TryGetValue(trackId, out var rendererIndex) ||
			!trackIndexMap.TryGetValue($"{trackId}_groupIndex", out var groupIndex) ||
			!trackIndexMap.TryGetValue($"{trackId}_trackIndex", out var trackIndex))
		{
			System.Diagnostics.Debug.WriteLine($"Track id {trackId} not found in mapping.");
			return;
		}
		
		var groups = mapped.GetTrackGroups(rendererIndex);
		if (groups is null || groupIndex < 0 || groupIndex >= groups.Length)
		{
			System.Diagnostics.Debug.WriteLine("Invalid group index for track selection.");
			return;
		}

		var group = groups.Get(groupIndex);
		if (group is null || trackIndex < 0 || trackIndex >= group.Length)
		{
			System.Diagnostics.Debug.WriteLine("Invalid track index for track selection.");
			return;
		}
	
		// Use the modern TrackSelectionOverride API
		var trackIndices = new List<Java.Lang.Integer> { Java.Lang.Integer.ValueOf(trackIndex) };
		var overrideForType = new TrackSelectionOverride(group, trackIndices);
		var builder = Player.TrackSelectionParameters?.BuildUpon();

		// Get the renderer type to clear only overrides for this type
		var rendererType = mapped.GetRendererType(rendererIndex);
		builder?.ClearOverridesOfType(rendererType);
		builder?.SetOverrideForType(overrideForType);

		if (builder is not null)
		{
			Player.TrackSelectionParameters = builder.Build();
			OnTracksChanged(Player.CurrentTracks);
		}
	}

protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (disposing)
		{
			session?.Release();
			session?.Dispose();
			session = null;
			trackIndexMap.Clear();

			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;

			if (connection is not null)
			{
				StopService(connection);
				connection.Dispose();
				connection = null;
			}

			client.Dispose();
		}
	}

	

	async Task<MediaItem.Builder> CreateMediaItemAsync(string url, DrmExtensions? drmExtensions, CancellationToken cancellationToken = default)
	{
		mediaItem = new MediaItem.Builder();

		if (!string.IsNullOrWhiteSpace(drmExtensions?.WideVineServerUrl))
		{
			var builder = CreateDrmConfiguration(drmExtensions);
			mediaItem.SetDrmConfiguration(builder);
		}

		MediaMetadata.Builder mediaMetaData = new();
		mediaMetaData.SetArtist(MediaElement.MetadataArtist);
		mediaMetaData.SetTitle(MediaElement.MetadataTitle);
		var artworkData = await GetBytesFromMetadataArtworkUrl(MediaElement.MetadataArtworkUrl, cancellationToken);
		if (artworkData.Length > 0)
		{
			mediaMetaData.MaybeSetArtworkData(artworkData, MediaMetadata.PictureTypeFrontCover);
		}

		if (!string.IsNullOrWhiteSpace(MediaElement.Subtitles?.Source))
		{
			mediaMetaData.SetSubtitle(MediaElement.Subtitles.Language);
			var mediaItemList = CreateSubtitleConfigurationList(MediaElement.Subtitles);
			if (mediaItemList is not null)
			{
				mediaItem.SetSubtitleConfigurations(mediaItemList);
			}
		}

		mediaItem.SetUri(url);
		mediaItem.SetMediaId(url);
		mediaItem.SetMediaMetadata(mediaMetaData.Build());

		return mediaItem;
	}

	static MediaItem.DrmConfiguration? CreateDrmConfiguration(DrmExtensions? drmExtensions)
	{
		if (drmExtensions is null || string.IsNullOrWhiteSpace(drmExtensions.WideVineServerUrl))
		{
			return null;
		}

		try
		{
			var drmConfiguration = new MediaItem.DrmConfiguration.Builder(C.WidevineUuid);
			drmConfiguration.SetLicenseUri(drmExtensions.WideVineServerUrl);
			drmConfiguration.SetLicenseRequestHeaders(drmExtensions.Headers);
			return drmConfiguration.Build();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to create DRM configuration: {ex.Message}");
			// Return null to allow playback without DRM if configuration fails
			return null;
		}
	}

	List<MediaItem.SubtitleConfiguration>? CreateSubtitleConfigurationList(Subtitle? subtitleSource)
	{
		var subtitleConfigurations = new List<MediaItem.SubtitleConfiguration>();
		var uri = Android.Net.Uri.Parse(GetSource(subtitleSource?.Source));

		if (uri is null)
		{
			return null;
		}
		var mimeType = MimeType(subtitleSource?.Type);
		var subtitleConfiguration = BuildSubtitleConfiguration(uri, mimeType, subtitleSource?.Language);
		if (subtitleConfiguration is not null)
		{
			subtitleConfiguration.Label = subtitleSource?.Language;
			subtitleConfigurations.Add(subtitleConfiguration);
		}
		return subtitleConfigurations;
	}
	
	static MediaItem.SubtitleConfiguration? BuildSubtitleConfiguration(Android.Net.Uri uri, string mimeType, string? language)
	{
		var subtitleConfiguration = new MediaItem.SubtitleConfiguration.Builder(uri);
		subtitleConfiguration.SetMimeType(mimeType);
		subtitleConfiguration.SetLanguage(language);
		subtitleConfiguration.SetId(language);
		subtitleConfiguration.SetSelectionFlags(C.SelectionFlagDefault);
		var subtitles = subtitleConfiguration.Build();
		return subtitles;
	}

	string GetSource(MediaSource? source)
	{
		if (source is null)
		{
			return string.Empty;
		}
		switch (source)
		{
			case UriMediaSource uriMediaSource:
				{
					var uri = uriMediaSource.Uri;
					if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
					{
						System.Diagnostics.Debug.WriteLine($"Setting media item from URI: {uri.AbsoluteUri}");
						return uri.AbsoluteUri;
					}

					break;
				}
			case FileMediaSource fileMediaSource:
				{
					var filePath = fileMediaSource.Path;
					if (!string.IsNullOrWhiteSpace(filePath))
					{
						return filePath;
					}

					break;
				}
			case ResourceMediaSource resourceMediaSource:
				{
					var package = PlayerView?.Context?.PackageName ?? "";
					var path = resourceMediaSource.Path;
					if (!string.IsNullOrWhiteSpace(path))
					{
						var assetFilePath = $"asset://{package}{Path.PathSeparator}{path}";
						return assetFilePath;
					}

					break;
				}
			default:
				throw new NotSupportedException($"{MediaElement.Source?.GetType().FullName} is not yet supported for {nameof(MediaElement.Source)}");
		}
		throw new NotSupportedException($"MediaElement.Source is not supported: {source?.GetType().FullName}");
	}

	static string MimeType(Extensions.MimeType? mimeType)
	{
		return mimeType switch
		{
			Extensions.MimeType.SubRip => MimeTypes.ApplicationSubrip,
			Extensions.MimeType.Text_VTT => MimeTypes.TextVtt,
			_ => MimeTypes.TextVtt
		};
	}

	async Task<byte[]> GetBytesFromMetadataArtworkUrl(string url, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return [];
		}

		Stream? stream = null;
		Uri.TryCreate(url, UriKind.Absolute, out var uri);

		try
		{
			byte[] artworkData = [];
			long? contentLength = null;

			// HTTP or HTTPS URL
			if (uri is not null &&
				(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
			{
				var request = new HttpRequestMessage(HttpMethod.Head, url);
				var contentLengthResponse = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
				contentLength = contentLengthResponse.Content.Headers.ContentLength ?? 0;

				var response = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
				stream = response.IsSuccessStatusCode ? await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false) : null;
			}
			// Absolute File Path
			else if (uri is not null && uri.Scheme == Uri.UriSchemeFile)
			{
				var normalizedFilePath = NormalizeFilePath(url);

				stream = File.Open(normalizedFilePath, FileMode.Create);
				contentLength = await GetByteCountFromStream(stream, cancellationToken);
			}
			// Relative File Path
			else if (Uri.TryCreate(url, UriKind.Relative, out _))
			{
				var normalizedFilePath = NormalizeFilePath(url);

				stream = Platform.AppContext.Assets?.Open(normalizedFilePath) ?? throw new InvalidOperationException("Assets cannot be null");
				contentLength = await GetByteCountFromStream(stream, cancellationToken);
			}

			if (stream is not null)
			{
				if (!contentLength.HasValue)
				{
					throw new InvalidOperationException($"{nameof(contentLength)} must be set when {nameof(stream)} is not null");
				}

				artworkData = new byte[contentLength.Value];
				using var memoryStream = new MemoryStream(artworkData);
				await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
			}

			return artworkData;
		}
		catch (Exception e)
		{
			Trace.WriteLine($"Unable to retrieve {nameof(MediaElement.MetadataArtworkUrl)} for {url}.{e}\n");
			return [];
		}
		finally
		{
			if (stream is not null)
			{
				stream.Close();
				await stream.DisposeAsync();
			}
		}

		static string NormalizeFilePath(string filePath) => filePath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

		static async ValueTask<long> GetByteCountFromStream(Stream stream, CancellationToken token)
		{
			if (stream.CanSeek)
			{
				return stream.Length;
			}

			long countedStreamBytes = 0;

			var buffer = new byte[8192];
			int bytesRead;

			while ((bytesRead = await stream.ReadAsync(buffer, token)) > 0)
			{
				countedStreamBytes += bytesRead;
			}

			return countedStreamBytes;
		}
	}

	[MemberNotNull(nameof(connection))]
	void StartService()
	{
		var intent = new Intent(Android.App.Application.Context, typeof(MediaControlsService));
		connection = new BoundServiceConnection(this);
		connection.MediaControlsServiceTaskRemoved += HandleMediaControlsServiceTaskRemoved;

		Android.App.Application.Context.StartForegroundService(intent);
		Android.App.Application.Context.ApplicationContext?.BindService(intent, connection, Bind.AutoCreate);
	}

	void StopService(in BoundServiceConnection boundServiceConnection)
	{
		boundServiceConnection.MediaControlsServiceTaskRemoved -= HandleMediaControlsServiceTaskRemoved;

		var serviceIntent = new Intent(Platform.AppContext, typeof(MediaControlsService));
		Android.App.Application.Context.StopService(serviceIntent);
		Platform.AppContext.UnbindService(boundServiceConnection);
	}

	void HandleMediaControlsServiceTaskRemoved(object? sender, EventArgs e) => Player?.Stop();
	void OnTrackSelectionRequested(MediaTrackType trackType)
	{
		if (PlayerView?.Context is null || MediaElement is not CommunityToolkit.Maui.Views.MediaElement mediaElement)
		{
			System.Diagnostics.Debug.WriteLine("PlayerView or MediaElement context is null, cannot show track selection dialog.");
			return;
		}

		var tracksOfType = mediaElement.AvailableTracks.Where(t => t.Type == trackType).ToList();
		if (tracksOfType.Count == 0)
		{
			System.Diagnostics.Debug.WriteLine($"No available tracks of type {trackType} to select.");
			return;
		}

		TrackSelectionDialog.Show(
			PlayerView.Context,
			tracksOfType,
			trackType,
			selectedTrack =>
			{
				// Just go through MediaElement so platform mapping stays consistent
				mediaElement.SelectTrack(selectedTrack.Id);
			}
		);
	}

	public void OnTracksChanged(Tracks? tracks)
	{
		try
		{
			if (Player is null)
			{
				System.Diagnostics.Debug.WriteLine("Player or TrackSelector is null, cannot update available tracks.");
				return;
			}
			if (MediaElement is not CommunityToolkit.Maui.Views.MediaElement mediaElement)
			{
				System.Diagnostics.Debug.WriteLine("MediaElement is not concrete MediaElement type.");
				return;
			}

			trackIndexMap.Clear();
			var collected = new List<CommunityToolkit.Maui.Core.MediaTrack>();

			var selectedFormatKeys = new HashSet<string>(StringComparer.Ordinal);
			var selectedGroupTrackIndices = new Dictionary<TrackGroup, HashSet<int>>();

			var trackSelections = Player.CurrentTrackSelections?.GetAll();
			if (trackSelections is not null)
			{
				foreach (var sel in trackSelections)
				{
					if (sel?.TrackGroup is TrackGroup tg)
					{
						int? selectedIdx = null;
						if (sel is AndroidX.Media3.ExoPlayer.TrackSelection.IExoTrackSelection exoSel)
						{
							selectedIdx = exoSel.SelectedIndexInTrackGroup;
						}
						if (selectedIdx.HasValue && selectedIdx.Value >= 0 && selectedIdx.Value < tg.Length)
						{
							if (!selectedGroupTrackIndices.TryGetValue(tg, out var set))
							{
								set = [];
								selectedGroupTrackIndices[tg] = set;
							}
							set.Add(selectedIdx.Value);
							var f = tg.GetFormat(selectedIdx.Value);
							if (f != null)
							{
								selectedFormatKeys.Add($"{f.Id}|{f.Label}|{f.Language}|{f.Width}x{f.Height}|{f.Bitrate}|{f.Codecs}|{f.SampleMimeType}");
							}
						}
					}
				}
			}
			var trackSelector = Player.TrackSelector as DefaultTrackSelector;
			var mapped = trackSelector?.CurrentMappedTrackInfo;
			if (mapped is null)
			{
				System.Diagnostics.Debug.WriteLine("MappedTrackInfo is null.");
				return;
			}

			for (int rendererIndex = 0; rendererIndex < mapped.RendererCount; rendererIndex++)
			{
				var exoType = mapped.GetRendererType(rendererIndex);
				var mediaTrackType = exoType switch
				{
					C.TrackTypeAudio => CommunityToolkit.Maui.Core.MediaTrackType.Audio,
					C.TrackTypeVideo => CommunityToolkit.Maui.Core.MediaTrackType.Video,
					C.TrackTypeText => CommunityToolkit.Maui.Core.MediaTrackType.Text,
					_ => CommunityToolkit.Maui.Core.MediaTrackType.Unknown
				};
				if (mediaTrackType == CommunityToolkit.Maui.Core.MediaTrackType.Unknown)
				{
					continue;
				}

				var groups = mapped.GetTrackGroups(rendererIndex);
				if (groups is null || groups.IsEmpty)
				{
					continue;
				}

				for (int groupIndex = 0; groupIndex < groups.Length; groupIndex++)
				{
					var group = groups.Get(groupIndex);
					if (group is null)
					{
						continue;
					}

					for (int trackIdx = 0; trackIdx < group.Length; trackIdx++)
					{
						var format = group.GetFormat(trackIdx);
						if (format is null)
						{
							continue;
						}

						var id = !string.IsNullOrWhiteSpace(format.Id)
							? format.Id
							: $"{rendererIndex}_{groupIndex}_{trackIdx}_{exoType}";

						trackIndexMap[id] = rendererIndex;
						trackIndexMap[$"{id}_groupIndex"] = groupIndex;
						trackIndexMap[$"{id}_trackIndex"] = trackIdx;

						bool selected = false;
						if (selectedGroupTrackIndices.TryGetValue(group, out var selSet))
						{
							selected = selSet.Contains(trackIdx);
						}
						if (!selected)
						{
							var key = $"{format.Id}|{format.Label}|{format.Language}|{format.Width}x{format.Height}|{format.Bitrate}|{format.Codecs}|{format.SampleMimeType}";
							selected = selectedFormatKeys.Contains(key);
						}

						string resolution = format.Height >= 720 ? $"{format.Height}p" : $"{format.Width}x{format.Height}";
						string frameRate = format.FrameRate > 0 ? $" {format.FrameRate:0.#}fps" : string.Empty;

						string result = $"{resolution}{frameRate}";
						string audioLang = (format.Language ?? "").ToUpperInvariant();
						string audioChannels = format.ChannelCount > 0 ? $"{format.ChannelCount}ch" : "";
						string displayName = !string.IsNullOrWhiteSpace(format.Label)
							? format.Label
							: mediaTrackType switch
							{
								CommunityToolkit.Maui.Core.MediaTrackType.Audio =>
									$"Audio {audioLang} {audioChannels}".Trim(),
								CommunityToolkit.Maui.Core.MediaTrackType.Video =>
									$"{result}",
								CommunityToolkit.Maui.Core.MediaTrackType.Text =>
									$"Subtitle {(format.Language ?? "").ToUpperInvariant()}".Trim(),
								_ => id
							};
						

						string codec;
						if (!string.IsNullOrWhiteSpace(format.SampleMimeType))
						{
							codec = !string.IsNullOrWhiteSpace(format.Codecs)
							? format.Codecs!
							: (format.SampleMimeType!);
						}
						else
						{
							codec = !string.IsNullOrWhiteSpace(format.Codecs)
							? format.Codecs!
							: (string.Empty);
						}
						var averageBitrate = format.AverageBitrate > 0 ? format.AverageBitrate : 0;
						int effectiveBitrate = format.Bitrate > 0 ? format.Bitrate : averageBitrate;
						bool isDefault = (format.SelectionFlags & C.SelectionFlagDefault) != 0;

						collected.Add(new CommunityToolkit.Maui.Core.MediaTrack
						{
							Id = id,
							Name = displayName,
							Language = format.Language ?? string.Empty,
							Type = mediaTrackType,
							IsSelected = selected,
							IsDefault = isDefault || selected,
							Bitrate = effectiveBitrate,
							Width = format.Width,
							Height = format.Height,
							FrameRate = format.FrameRate,
							Codec = codec,
							Metadata =
							{
								["RendererIndex"] = rendererIndex,
								["GroupIndex"] = groupIndex,
								["TrackIndex"] = trackIdx,
								["Channels"] = format.ChannelCount,
								["SampleRate"] = format.SampleRate,
								["PixelWidthHeightRatio"] = format.PixelWidthHeightRatio,
								["RoleFlags"] = format.RoleFlags
							}
						});
					}
				}
			}

			Dispatcher.Dispatch(() =>
			{
				mediaElement.OnTracksChanged(collected);
				foreach (var grp in collected.Where(t => t.IsSelected).GroupBy(t => t.Type))
				{
					mediaElement.OnTrackSelectionChanged(grp.First());
				}
			});
		}
		catch (Exception ex)
		{
			Logger.LogWarning(ex, "Failed to process track change event.");
		}
	}

	#region PlayerListener implementation method stubs
	public void OnAudioAttributesChanged(AudioAttributes? audioAttributes) { }
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
	public void OnPlayWhenReadyChanged(bool playWhenReady, int reason) { }
	public void OnPlaybackSuppressionReasonChanged(int playbackSuppressionReason) { }
	public void OnPlayerErrorChanged(PlaybackException? error) { }
	public void OnPlaylistMetadataChanged(MediaMetadata? mediaMetadata) { }
	public void OnRenderedFirstFrame() { }
	public void OnRepeatModeChanged(int repeatMode) { }
	public void OnSeekBackIncrementChanged(long seekBackIncrementMs) { }
	public void OnSeekForwardIncrementChanged(long seekForwardIncrementMs) { }
	public void OnShuffleModeEnabledChanged(bool shuffleModeEnabled) { }
	public void OnSkipSilenceEnabledChanged(bool skipSilenceEnabled) { }
	public void OnSurfaceSizeChanged(int width, int height) { }
	public void OnTimelineChanged(Timeline? timeline, int reason) { }
	#endregion

	static class PlaybackState
	{
		public const int StateBuffering = 6;
		public const int StateConnecting = 8;
		public const int StateFailed = 7;
		public const int StateFastForwarding = 4;
		public const int StateNone = 0;
		public const int StatePaused = 2;
		public const int StatePlaying = 3;
		public const int StateRewinding = 5;
		public const int StateSkippingToNext = 10;
		public const int StateSkippingToPrevious = 9;
		public const int StateSkippingToQueueItem = 11;
		public const int StateStopped = 1;
		public const int StateError = 7;
	}
}