using System.Diagnostics;
using System.Numerics;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Display;
using MediaSource = CommunityToolkit.Maui.Views.MediaSource;
using ParentWindow = CommunityToolkit.Maui.Extensions.PageExtensions.ParentWindow;
using WindowsMediaElement = Windows.Media.Playback.MediaPlayer;
using WinMediaSource = Windows.Media.Core.MediaSource;

namespace CommunityToolkit.Maui.Core.Views;

partial class MediaManager : IDisposable
{
	MediaPlaybackItem? playbackItem;
	Dictionary<TimedTextSource, string> timedTextSourceDictionary = [];
	TimedTextSource? timedTextSource = null;
	SystemMediaTransportControls? systemMediaControls;

	// States that allow changing position
	readonly IReadOnlyList<MediaElementState> allowUpdatePositionStates =
	[
		MediaElementState.Playing,
		MediaElementState.Paused,
		MediaElementState.Stopped,
	];

	// The requests to keep display active are cumulative, this bool makes sure it only gets requested once
	bool displayActiveRequested;

	/// <summary>
	/// The <see cref="DisplayRequest"/> is used to enable the <see cref="MediaElement.ShouldKeepScreenOn"/> functionality.
	/// </summary>
	/// <remarks>
	/// Calls to <see cref="DisplayRequest.RequestActive"/> and <see cref="DisplayRequest.RequestRelease"/> should be in balance.
	/// Not doing so will result in the screen staying on and negatively impacting the environment :(
	/// </remarks>
	protected DisplayRequest DisplayRequest { get; } = new();

	/// <summary>
	/// Creates the corresponding platform view of <see cref="MediaElement"/> on Windows.
	/// </summary>
	/// <returns>The platform native counterpart of <see cref="MediaElement"/>.</returns>
	public PlatformMediaElement CreatePlatformView()
	{
		Player = new();
		WindowsMediaElement MediaElement = new();
		MediaElement.MediaOpened += OnMediaElementMediaOpened;

		Player.SetMediaPlayer(MediaElement);
		Player.MediaPlayer.PlaybackSession.NaturalVideoSizeChanged += OnNaturalVideoSizeChanged;
		Player.MediaPlayer.PlaybackSession.PlaybackRateChanged += OnPlaybackSessionPlaybackRateChanged;
		Player.MediaPlayer.PlaybackSession.PlaybackStateChanged += OnPlaybackSessionPlaybackStateChanged;
		Player.MediaPlayer.PlaybackSession.SeekCompleted += OnPlaybackSessionSeekCompleted;
		Player.MediaPlayer.MediaFailed += OnMediaElementMediaFailed;
		Player.MediaPlayer.MediaEnded += OnMediaElementMediaEnded;
		Player.MediaPlayer.VolumeChanged += OnMediaElementVolumeChanged;
		Player.MediaPlayer.IsMutedChanged += OnMediaElementIsMutedChanged;

		systemMediaControls = Player.MediaPlayer.SystemMediaTransportControls;
		return Player;
	}

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MediaManager"/>.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual partial void PlatformPlay()
	{
		Player?.MediaPlayer.Play();

		if (MediaElement.ShouldKeepScreenOn
			&& !displayActiveRequested)
		{
			DisplayRequest.RequestActive();
			displayActiveRequested = true;
		}
	}

	protected virtual partial void PlatformPause()
	{
		Player?.MediaPlayer.Pause();

		if (displayActiveRequested)
		{
			DisplayRequest.RequestRelease();
			displayActiveRequested = false;
		}
	}

	protected virtual async partial Task PlatformSeek(TimeSpan position, CancellationToken token)
	{
		if (Player?.MediaPlayer.CanSeek is true)
		{
			if (Dispatcher.IsDispatchRequired)
			{
				await Dispatcher.DispatchAsync(() => UpdatePosition(Player, position)).WaitAsync(token);
			}
			else
			{
				token.ThrowIfCancellationRequested();
				UpdatePosition(Player, position);
			}
		}

		static void UpdatePosition(in MediaPlayerElement mediaPlayerElement, in TimeSpan position) => mediaPlayerElement.MediaPlayer.Position = position;
	}

	protected virtual partial void PlatformStop()
	{
		if (Player is null)
		{
			return;
		}

		// There's no Stop method so pause the video and reset its position
		Player.MediaPlayer.Pause();
		Player.MediaPlayer.Position = TimeSpan.Zero;

		MediaElement.CurrentStateChanged(MediaElementState.Stopped);

		if (displayActiveRequested)
		{
			DisplayRequest.RequestRelease();
			displayActiveRequested = false;
		}
	}

	protected virtual partial void PlatformUpdateAspect()
	{
		if (Player is null)
		{
			return;
		}

		Player.Stretch = MediaElement.Aspect switch
		{
			Aspect.Fill => Microsoft.UI.Xaml.Media.Stretch.Fill,
			Aspect.AspectFill => Microsoft.UI.Xaml.Media.Stretch.UniformToFill,
			_ => Microsoft.UI.Xaml.Media.Stretch.Uniform,
		};
	}

	protected virtual partial void PlatformUpdateSpeed()
	{
		if (Player is null)
		{
			return;
		}

		var previousSpeed = Player.MediaPlayer.PlaybackRate;
		Player.MediaPlayer.PlaybackRate = MediaElement.Speed;

		// Only trigger once when going to the paused state
		if (IsZero<double>(MediaElement.Speed) && previousSpeed > 0)
		{
			Player.MediaPlayer.Pause();
		}
		// Only trigger once when we move from the paused state
		else if (MediaElement.Speed > 0 && IsZero<double>(previousSpeed))
		{
			MediaElement.Play();
		}
	}

	protected virtual partial void PlatformUpdateShouldShowPlaybackControls()
	{
		if (Player is null)
		{
			return;
		}

		Player.AreTransportControlsEnabled =
			MediaElement.ShouldShowPlaybackControls;
	}

	protected virtual partial void PlatformUpdatePosition()
	{
		if (Application.Current?.Windows is null || Application.Current.Windows.Count == 0)
		{
			return;
		}
		if (!ParentWindow.Exists)
		{
			// Parent window is null, so we can't update the position
			// This is a workaround for a bug where the timer keeps running after the window is closed
			return;
		}

		if (Player is not null
			&& allowUpdatePositionStates.Contains(MediaElement.CurrentState))
		{
			MediaElement.Position = Player.MediaPlayer.Position;
		}
	}

	protected virtual partial void PlatformUpdateVolume()
	{
		if (Player is null)
		{
			return;
		}

		// If currently muted, ignore
		if (MediaElement.ShouldMute)
		{
			return;
		}

		if (Dispatcher.IsDispatchRequired)
		{
			Dispatcher.Dispatch(() => UpdateVolume(Player, MediaElement.Volume));
		}
		else
		{
			UpdateVolume(Player, MediaElement.Volume);
		}

		static void UpdateVolume(in MediaPlayerElement mediaPlayerElement, in double volume) => mediaPlayerElement.MediaPlayer.Volume = volume;
	}

	protected virtual partial void PlatformUpdateShouldKeepScreenOn()
	{
		if (MediaElement.ShouldKeepScreenOn)
		{
			if (allowUpdatePositionStates.Contains(MediaElement.CurrentState)
				&& !displayActiveRequested)
			{
				DisplayRequest.RequestActive();
				displayActiveRequested = true;
			}
		}
		else
		{
			if (displayActiveRequested)
			{
				DisplayRequest.RequestRelease();
				displayActiveRequested = false;
			}
		}
	}

	protected virtual partial void PlatformUpdateShouldMute()
	{
		if (Player?.MediaPlayer is null)
		{
			return;
		}

		Player.MediaPlayer.IsMuted = MediaElement.ShouldMute;
	}

	protected virtual partial void PlatformUpdateSource()
	{
		if (Player is null)
		{
			return;
		}
		if (playbackItem is not null)
		{
			playbackItem.TimedMetadataTracksChanged -= PlaybackItem_TimedMetadataTracksChanged;
			playbackItem = null;
		}
		timedTextSourceDictionary = [];

		Dispatcher.Dispatch(() => Player.PosterSource = new BitmapImage());
		if (MediaElement.Source is null)
		{
			Player.Source = null;
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;
			MediaElement.CurrentStateChanged(MediaElementState.None);
			return;
		}
		
		MediaElement.Position = TimeSpan.Zero;
		MediaElement.Duration = TimeSpan.Zero;
		Player.AutoPlay = MediaElement.ShouldAutoPlay;

		WinMediaSource? source = null;
		var temp = GetUrlFromMediaSource(MediaElement.Source);
		if (!string.IsNullOrEmpty(temp))
		{
			source = WinMediaSource.CreateFromUri(new Uri(temp));
		}
		if (source is null)
		{
			Player.Source = null;
			return;
		}

		playbackItem = new MediaPlaybackItem(source);
		SetMediaItemDisplayProperties(playbackItem);
		var subtitle = GetUrlFromMediaSource(MediaElement.SubtitleUrl);
		if (!string.IsNullOrEmpty(subtitle))
		{
			Player.Source =  AddSubtitleToSource(source);
			return;
		}
		Player.Source = playbackItem;
	}

	void SetMediaItemDisplayProperties(MediaPlaybackItem mediaPlaybackItem)
	{
		MediaItemDisplayProperties displayProperties = mediaPlaybackItem.GetDisplayProperties();
		displayProperties.Type = Windows.Media.MediaPlaybackType.Music;
		displayProperties.MusicProperties.Title = MediaElement.MetadataTitle;
		displayProperties.MusicProperties.Artist = MediaElement.MetadataArtist;
		if (!string.IsNullOrEmpty(MediaElement.MetadataArtworkUrl))
		{
			displayProperties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(MediaElement.MetadataArtworkUrl));
		}
		mediaPlaybackItem.ApplyDisplayProperties(displayProperties);
	}
	MediaPlaybackItem AddSubtitleToSource(WinMediaSource source)
	{
		var subtitleLanguage = MediaElement.SubtitleLanguage;
		var subtitleFont = MediaElement.SubtitleFont.ToString();
		string temp = GetUrlFromMediaSource(MediaElement.SubtitleUrl) ?? string.Empty;
		if (!Uri.TryCreate(temp, UriKind.RelativeOrAbsolute, out var subtitleUrl))
		{
			Trace.TraceError($"{nameof(MediaElement)} unable to update artwork because {nameof(MediaElement.SubtitleUrl)} is not a valid URI");
			return new MediaPlaybackItem(source);
		}
		var ttsEnUri = subtitleUrl;
		timedTextSource = TimedTextSource.CreateFromUri(ttsEnUri);
		if (!string.IsNullOrEmpty(subtitleLanguage))
		{
			timedTextSourceDictionary[timedTextSource] = subtitleLanguage;
			timedTextSource.Resolved += Tts_Resolved;
		}

		source.ExternalTimedTextSources.Add(timedTextSource);
		//var item = new MediaPlaybackItem(source);
		if (playbackItem is null)
		{
			return new MediaPlaybackItem(source);
		}
		playbackItem.TimedMetadataTracksChanged += PlaybackItem_TimedMetadataTracksChanged;
		if (Player is not null && subtitleFont is not null)
		{
			Player.FontFamily = GetFontFamily(subtitleFont);
			Player.FontSize = MediaElement.SubtitleFontSize;
		}
		return playbackItem;
	}

	static void PlaybackItem_TimedMetadataTracksChanged(MediaPlaybackItem sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
	{
		sender.TimedMetadataTracks.SetPresentationMode(0, TimedMetadataTrackPresentationMode.PlatformPresented);
	}

	FontFamily? GetFontFamily(string fontName)
	{
		if (string.IsNullOrEmpty(fontName))
		{
			return null;
		}
		try
		{
			return new FontFamily(new Core.FontExtensions.FontFamily(fontName).WindowsFont);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Failed to load font family");
			return null;
		}
	}

	void Tts_Resolved(TimedTextSource sender, TimedTextSourceResolveResultEventArgs args)
	{
		var language = timedTextSourceDictionary[sender];
		// Handle errors
		if (args.Error is not null)
		{
			System.Diagnostics.Trace.TraceError($"Failed to resolve timed text source: {args.Error.ExtendedError}");
			return;
		}
		args.Tracks[0].Label = language;
		if (timedTextSource is not null)
		{
			timedTextSource.Resolved -= Tts_Resolved;
		}
	}

	static string GetUrlFromMediaSource(MediaSource? url)
	{
		if (url is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri?.AbsoluteUri;
			if (!string.IsNullOrWhiteSpace(uri))
			{
				return uri;
			}
		}
		else if (url is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				return filename;
			}
		}
		else if (url is ResourceMediaSource resourceMediaSource)
		{
			string path = "ms-appx:///" + resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				return path;
			}
		}
		return string.Empty;
	}
	protected virtual partial void PlatformUpdateShouldLoopPlayback()
	{
		if (Player is null)
		{
			return;
		}

		Player.MediaPlayer.IsLoopingEnabled = MediaElement.ShouldLoopPlayback;
	}

	/// <summary>
	/// Releases the unmanaged resources used by the <see cref="MediaManager"/> and optionally releases the managed resources.
	/// </summary>
	/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (Player?.MediaPlayer is not null)
			{
				if (displayActiveRequested)
				{
					DisplayRequest.RequestRelease();
					displayActiveRequested = false;
				}

				if (playbackItem is not null)
				{
					playbackItem.TimedMetadataTracksChanged -= PlaybackItem_TimedMetadataTracksChanged;
				}

				Player.MediaPlayer.MediaOpened -= OnMediaElementMediaOpened;
				Player.MediaPlayer.MediaFailed -= OnMediaElementMediaFailed;
				Player.MediaPlayer.MediaEnded -= OnMediaElementMediaEnded;
				Player.MediaPlayer.VolumeChanged -= OnMediaElementVolumeChanged;
				Player.MediaPlayer.IsMutedChanged -= OnMediaElementIsMutedChanged;

				if (Player.MediaPlayer.PlaybackSession is not null)
				{
					Player.MediaPlayer.PlaybackSession.NaturalVideoSizeChanged -= OnNaturalVideoSizeChanged;
					Player.MediaPlayer.PlaybackSession.PlaybackRateChanged -= OnPlaybackSessionPlaybackRateChanged;
					Player.MediaPlayer.PlaybackSession.PlaybackStateChanged -= OnPlaybackSessionPlaybackStateChanged;
					Player.MediaPlayer.PlaybackSession.SeekCompleted -= OnPlaybackSessionSeekCompleted;
				}
			}
		}
	}

	static bool IsZero<TValue>(TValue numericValue) where TValue : INumber<TValue>
	{
		return TValue.IsZero(numericValue);
	}

	async ValueTask UpdateMetadata()
	{
		if (systemMediaControls is null || Player is null)
		{
			return;
		}

		if (string.IsNullOrEmpty(MediaElement.MetadataArtworkUrl))
		{
			return;
		}
		if (!Uri.TryCreate(MediaElement.MetadataArtworkUrl, UriKind.RelativeOrAbsolute, out var metadataArtworkUri))
		{
			Trace.TraceError($"{nameof(MediaElement)} unable to update artwork because {nameof(MediaElement.MetadataArtworkUrl)} is not a valid URI");
			return;
		}

		if (Dispatcher.IsDispatchRequired)
		{
			await Dispatcher.DispatchAsync(() => UpdatePosterSource(Player, metadataArtworkUri));
		}
		else
		{
			UpdatePosterSource(Player, metadataArtworkUri);
		}

		static void UpdatePosterSource(in MediaPlayerElement player, in Uri metadataArtworkUri)
		{
			player.PosterSource = new BitmapImage(metadataArtworkUri);
		}
	}

	async void OnMediaElementMediaOpened(WindowsMediaElement sender, object args)
	{
		if (Player is null)
		{
			return;
		}

		if (Dispatcher.IsDispatchRequired)
		{
			Dispatcher.Dispatch(() => SetDuration(MediaElement, Player));
		}
		else
		{
			SetDuration(MediaElement, Player);
		}

		MediaElement.MediaOpened();

		await UpdateMetadata();

		static void SetDuration(in IMediaElement mediaElement, in MediaPlayerElement mediaPlayerElement)
		{
			mediaElement.Duration = mediaPlayerElement.MediaPlayer.NaturalDuration == TimeSpan.MaxValue
				? TimeSpan.Zero
				: mediaPlayerElement.MediaPlayer.NaturalDuration;
		}
	}

	void OnMediaElementMediaEnded(WindowsMediaElement sender, object args)
	{
		MediaElement?.MediaEnded();
	}

	void OnMediaElementMediaFailed(WindowsMediaElement sender, MediaPlayerFailedEventArgs args)
	{
		string errorMessage = string.Empty;
		string errorCode = string.Empty;
		string error = args.Error.ToString();

		if (!string.IsNullOrWhiteSpace(args.ErrorMessage))
		{
			errorMessage = $"Error message: {args.ErrorMessage}";
		}

		if (args.ExtendedErrorCode != null)
		{
			errorCode = $"Error code: {args.ExtendedErrorCode.Message}";
		}

		var message = string.Join(", ",
			new[] { error, errorCode, errorMessage }
			.Where(s => !string.IsNullOrEmpty(s)));

		MediaElement?.MediaFailed(new MediaFailedEventArgs(message));

		Logger?.LogError("{LogMessage}", message);
	}

	void OnMediaElementIsMutedChanged(WindowsMediaElement sender, object args)
	{
		MediaElement.ShouldMute = sender.IsMuted;
	}

	void OnMediaElementVolumeChanged(WindowsMediaElement sender, object args)
	{
		MediaElement.Volume = sender.Volume;
	}

	void OnNaturalVideoSizeChanged(MediaPlaybackSession sender, object args)
	{
		if (MediaElement is not null)
		{
			MediaElement.MediaWidth = (int)sender.NaturalVideoWidth;
			MediaElement.MediaHeight = (int)sender.NaturalVideoHeight;
		}
	}

	void OnPlaybackSessionPlaybackRateChanged(MediaPlaybackSession sender, object args)
	{
		if (AreFloatingPointNumbersEqual(MediaElement.Speed, sender.PlaybackRate))
		{
			if (Dispatcher.IsDispatchRequired)
			{
				Dispatcher.Dispatch(() => UpdateSpeed(MediaElement, sender.PlaybackRate));
			}
			else
			{
				UpdateSpeed(MediaElement, sender.PlaybackRate);
			}
		}

		static void UpdateSpeed(in IMediaElement mediaElement, in double playbackRate) => mediaElement.Speed = playbackRate;
	}

	void OnPlaybackSessionPlaybackStateChanged(MediaPlaybackSession sender, object args)
	{
		var newState = sender.PlaybackState switch
		{
			MediaPlaybackState.Buffering => MediaElementState.Buffering,
			MediaPlaybackState.Playing => MediaElementState.Playing,
			MediaPlaybackState.Paused => MediaElementState.Paused,
			MediaPlaybackState.Opening => MediaElementState.Opening,
			_ => MediaElementState.None,
		};

		MediaElement?.CurrentStateChanged(newState);
		if (sender.PlaybackState == MediaPlaybackState.Playing && IsZero<double>(sender.PlaybackRate))
		{
			Dispatcher.Dispatch(() =>
			{
				sender.PlaybackRate = 1;
			});
		}
	}

	void OnPlaybackSessionSeekCompleted(MediaPlaybackSession sender, object args)
	{
		MediaElement?.SeekCompleted();
	}
}