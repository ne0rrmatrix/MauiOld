using System.Diagnostics;
using System.Numerics;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System.Display;
using ParentWindow = CommunityToolkit.Maui.Extensions.PageExtensions.ParentWindow;
using WindowsMediaElement = Windows.Media.Playback.MediaPlayer;
using WinMediaSource = Windows.Media.Core.MediaSource;

namespace CommunityToolkit.Maui.Core.Views;

partial class MediaManager : IDisposable
{
	Metadata? metadata;
	SystemMediaTransportControls? systemMediaControls;
	MediaPlaybackItem? playbackItem = null;

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
		WindowsMediaElement mediaPlayer = new();
		mediaPlayer.MediaOpened += OnMediaElementMediaOpened;

		Player.SetMediaPlayer(mediaPlayer);
		Player.MediaPlayer.PlaybackSession.NaturalVideoSizeChanged += OnNaturalVideoSizeChanged;
		Player.MediaPlayer.PlaybackSession.PlaybackRateChanged += OnPlaybackSessionPlaybackRateChanged;
		Player.MediaPlayer.PlaybackSession.PlaybackStateChanged += OnPlaybackSessionPlaybackStateChanged;
		Player.MediaPlayer.PlaybackSession.SeekCompleted += OnPlaybackSessionSeekCompleted;
		Player.MediaPlayer.MediaFailed += OnMediaElementMediaFailed;
		Player.MediaPlayer.MediaEnded += OnMediaElementMediaEnded;
		Player.MediaPlayer.VolumeChanged += OnMediaElementVolumeChanged;
		Player.MediaPlayer.IsMutedChanged += OnMediaElementIsMutedChanged;

		Player.MediaPlayer.SystemMediaTransportControls.IsEnabled = false;
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

	protected virtual partial void PlatformSelectTrack(string trackId)
	{
		if (Player?.MediaPlayer.Source is not MediaPlaybackItem playbackItem)
		{
			return;
		}

		try
		{
			if (MediaElement.Handler?.VirtualView is not MediaElement mediaElement)
			{
				System.Diagnostics.Debug.WriteLine("MediaManager: PlatformSelectTrack - MediaElement is not available.");
				return;
			}

			var track = mediaElement.AvailableTracks.FirstOrDefault(t => t.Id == trackId);
			if (track is null)
			{
				return;
			}

			switch (track.Type)
			{
				case MediaTrackType.Audio:
					SelectAudioTrack(playbackItem, track);
					break;
				case MediaTrackType.Text:
					SelectSubtitleTrack(playbackItem, track);
					break;
			}

			mediaElement.OnTrackSelectionChanged(track);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error selecting track: {TrackId}", trackId);
		}
	}

	void SelectAudioTrack(MediaPlaybackItem playbackItem, CommunityToolkit.Maui.Core.MediaTrack track)
	{
		try
		{
			var audioTracks = playbackItem.AudioTracks;
			if (track.Metadata?.TryGetValue("TrackIndex", out var indexObj) is true &&
				indexObj is int trackIndex &&
				trackIndex >= 0 &&
				trackIndex < audioTracks.Count)
			{
				playbackItem.AudioTracks.SelectedIndex = trackIndex;
			}
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error selecting audio track");
		}
	}

	void SelectSubtitleTrack(MediaPlaybackItem playbackItem, CommunityToolkit.Maui.Core.MediaTrack track)
	{
		// For now, subtitle track selection is limited due to Windows API constraints
		// This would require more complex implementation with TimedMetadataTracks
		System.Diagnostics.Debug.WriteLine($"Subtitle track selection requested: {track.Name}");
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
		if (Player is null)
		{
			return;
		}
		Dispatcher.Dispatch(() => Player.MediaPlayer.IsMuted = MediaElement.ShouldMute);
	}

	protected virtual async partial ValueTask PlatformUpdateSource()
	{
		if (Player is null)
		{
			return;
		}

		await Dispatcher.DispatchAsync(() => Player.PosterSource = new BitmapImage());

		if (MediaElement.Source is null)
		{
			Player.Source = null;
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;

			MediaElement.CurrentStateChanged(MediaElementState.None);

			return;
		}

		MediaElement.Position = TimeSpan.Zero;
		MediaElement.Duration = TimeSpan.Zero;
		playbackItem = null;
		Player.MediaPlayer.Source = null;
		Player.AutoPlay = MediaElement.ShouldAutoPlay;

		if (MediaElement.Source is UriMediaSource uriMediaSource)
		{
			
			var uri = uriMediaSource.Uri?.AbsoluteUri;
			if (!string.IsNullOrWhiteSpace(uri))
			{
				var mediaSource = WinMediaSource.CreateFromUri(new Uri(uri));
				playbackItem = new MediaPlaybackItem(mediaSource);
				playbackItem.AudioTracksChanged += PlaybackItem_AudioTracksChanged;
				playbackItem.VideoTracksChanged += PlaybackItem_VideoTracksChanged;
				playbackItem.TimedMetadataTracksChanged += PlaybackItem_TimedMetadataTracksChanged;
				playbackItem.AutoLoadedDisplayProperties = AutoLoadedDisplayPropertyKind.MusicOrVideo;

				Player.MediaPlayer.Source = playbackItem;
			}
		}
		else if (MediaElement.Source is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filename);
				Player.Source = WinMediaSource.CreateFromStorageFile(storageFile);
			}
		}
		else if (MediaElement.Source is ResourceMediaSource resourceMediaSource)
		{
			string path = "ms-appx:///" + resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				Player.Source = WinMediaSource.CreateFromUri(new Uri(path));
			}
		}
	}

	void PlaybackItem_TimedMetadataTracksChanged(MediaPlaybackItem sender, IVectorChangedEventArgs args)
	{
		System.Diagnostics.Debug.WriteLine($"TimedMetadataTracksChanged: {args.Index}");
		//ExtractAndUpdateTracks(sender);
		ExtractTextTracks(sender);
	}

	void PlaybackItem_VideoTracksChanged(MediaPlaybackItem sender, IVectorChangedEventArgs args)
	{
		System.Diagnostics.Debug.WriteLine($"VideoTracksChanged: {args.Index}");
		//ExtractAndUpdateTracks(sender);
	}

	void PlaybackItem_AudioTracksChanged(MediaPlaybackItem sender, IVectorChangedEventArgs args)
	{
		System.Diagnostics.Debug.WriteLine($"AudioTracksChanged: {args.Index}");
		//ExtractAndUpdateTracks(sender);
		ExtractAudioTracks(sender);
	}

	void ExtractVideoTracks(MediaPlaybackItem playbackItem)
	{
		try
		{
			if (MediaElement.Handler?.VirtualView is not MediaElement mediaElement)
			{
				return;
			}
			var tracks = new List<CommunityToolkit.Maui.Core.MediaTrack>();
			// Extract video tracks for quality selection
			var videoTracks = playbackItem.VideoTracks;
			for (int i = 0; i < videoTracks.Count; i++)
			{
				var videoTrack = videoTracks[i];
				var props = videoTrack.GetEncodingProperties();
				var displayName = !string.IsNullOrEmpty(videoTrack.Name)
					? videoTrack.Name
					: $"{props?.Width}x{props?.Height}";
				var track = new CommunityToolkit.Maui.Core.MediaTrack
				{
					Id = $"video-{i}",
					Type = MediaTrackType.Video,
					Name = displayName,
					Language = videoTrack.Language ?? string.Empty,
					IsSelected = i == videoTracks.SelectedIndex,
					IsDefault = i == 0,
					Bitrate = (int)(props?.Bitrate ?? 0),
					Width = (int)(props?.Width ?? 0),
					Height = (int)(props?.Height ?? 0),
					Metadata = new Dictionary<string, object>
					{
						{ "TrackIndex", i }
					}
				};
				System.Diagnostics.Debug.WriteLine($"Found video track: {track.Name} ({track.Width}x{track.Height} @ {track.Bitrate}bps)");
				tracks.Add(track);
			}
			// Update MediaElement with tracks
			Dispatcher.Dispatch(() =>
			{
				mediaElement.OnTracksChanged(tracks);
			});
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error extracting video tracks");
		}
	}

	void ExtractTextTracks(MediaPlaybackItem playbackItem)
	{
		try
		{
			if (MediaElement.Handler?.VirtualView is not MediaElement mediaElement)
			{
				return;
			}
			var tracks = new List<CommunityToolkit.Maui.Core.MediaTrack>();
			// Extract text tracks (subtitles/captions)
			var timedTracks = playbackItem.TimedMetadataTracks;
			for (int i = 0; i < timedTracks.Count; i++)
			{
				var timedTrack = timedTracks[i];
				if (timedTrack.TimedMetadataKind == Windows.Media.Core.TimedMetadataKind.Caption ||
					timedTrack.TimedMetadataKind == Windows.Media.Core.TimedMetadataKind.Subtitle)
				{
					var displayName = !string.IsNullOrEmpty(timedTrack.Name)
						? timedTrack.Name
						: $"Subtitle {i + 1}";
					var track = new CommunityToolkit.Maui.Core.MediaTrack
					{
						Id = $"subtitle-{i}",
						Type = MediaTrackType.Text,
						Name = displayName,
						Language = timedTrack.Language ?? string.Empty,
						IsSelected = false, // Subtitles typically start disabled
						IsDefault = false,
						Metadata = new Dictionary<string, object>
						{
							{ "TrackIndex", i }
						}
					};
					tracks.Add(track);
				}
			}
			// Update MediaElement with tracks
			Dispatcher.Dispatch(() =>
			{
				mediaElement.OnTracksChanged(tracks);
			});
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error extracting text tracks");
		}
	}

	void ExtractAudioTracks(MediaPlaybackItem playbackItem)
	{
		try
		{
			if (MediaElement.Handler?.VirtualView is not MediaElement mediaElement)
			{
				return;
			}
			var tracks = new List<CommunityToolkit.Maui.Core.MediaTrack>();
			// Extract audio tracks
			var audioTracks = playbackItem.AudioTracks;
			for (int i = 0; i < audioTracks.Count; i++)
			{
				var audioTrack = audioTracks[i];
				var props = audioTrack.GetEncodingProperties();
				var displayName = !string.IsNullOrEmpty(audioTrack.Name)
					? audioTrack.Name
					: $"Audio Track {i + 1}";
				var track = new CommunityToolkit.Maui.Core.MediaTrack
				{
					Id = $"audio-{i}",
					Type = MediaTrackType.Audio,
					Name = displayName,
					Language = audioTrack.Language ?? string.Empty,
					IsSelected = i == audioTracks.SelectedIndex,
					IsDefault = i == 0,
					Bitrate = (int)(props?.Bitrate ?? 0),
					Metadata = new Dictionary<string, object>
					{
						{ "TrackIndex", i }
					}
				};
				tracks.Add(track);
			}
			// Update MediaElement with tracks
			Dispatcher.Dispatch(() =>
			{
				mediaElement.OnTracksChanged(tracks);
			});
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error extracting audio tracks");
		}
	}
	/*
	void ExtractAndUpdateTracks(MediaPlaybackItem playbackItem)
	{
		try
		{
			if (MediaElement.Handler?.VirtualView is not MediaElement mediaElement)
			{
				return;
			}

			var tracks = new List<CommunityToolkit.Maui.Core.MediaTrack>();

			// Extract audio tracks
			var audioTracks = playbackItem.AudioTracks;
			for (int i = 0; i < audioTracks.Count; i++)
			{
				var audioTrack = audioTracks[i];
				var props = audioTrack.GetEncodingProperties();

				var displayName = !string.IsNullOrEmpty(audioTrack.Name) 
					? audioTrack.Name 
					: $"Audio Track {i + 1}";

				var track = new CommunityToolkit.Maui.Core.MediaTrack
				{
					Id = $"audio-{i}",
					Type = MediaTrackType.Audio,
					Name = displayName,
					Language = audioTrack.Language ?? string.Empty,
					IsSelected = i == audioTracks.SelectedIndex,
					IsDefault = i == 0,
					Bitrate = (int)(props?.Bitrate ?? 0),
					Metadata = new Dictionary<string, object>
					{
						{ "TrackIndex", i }
					}
				};

				tracks.Add(track);
			}

			// Extract video tracks for quality selection
			var videoTracks = playbackItem.VideoTracks;
			for (int i = 0; i < videoTracks.Count; i++)
			{
				var videoTrack = videoTracks[i];
				var props = videoTrack.GetEncodingProperties();

				var displayName = !string.IsNullOrEmpty(videoTrack.Name)
					? videoTrack.Name
					: $"{props?.Width}x{props?.Height}";

				var track = new CommunityToolkit.Maui.Core.MediaTrack
				{
					Id = $"video-{i}",
					Type = MediaTrackType.Video,
					Name = displayName,
					Language = videoTrack.Language ?? string.Empty,
					IsSelected = i == videoTracks.SelectedIndex,
					IsDefault = i == 0,
					Bitrate = (int)(props?.Bitrate ?? 0),
					Width = (int)(props?.Width ?? 0),
					Height = (int)(props?.Height ?? 0),
					Metadata = new Dictionary<string, object>
					{
						{ "TrackIndex", i }
					}
				};
				System.Diagnostics.Debug.WriteLine($"Found video track: {track.Name} ({track.Width}x{track.Height} @ {track.Bitrate}bps)");

				tracks.Add(track);
			}

			// Add basic subtitle track support
			var timedTracks = playbackItem.TimedMetadataTracks;
			var hasSubtitles = false;
			for (int i = 0; i < timedTracks.Count; i++)
			{
				var timedTrack = timedTracks[i];
				if (timedTrack.TimedMetadataKind == Windows.Media.Core.TimedMetadataKind.Caption ||
					timedTrack.TimedMetadataKind == Windows.Media.Core.TimedMetadataKind.Subtitle)
				{
					hasSubtitles = true;
					var displayName = !string.IsNullOrEmpty(timedTrack.Name)
						? timedTrack.Name
						: $"Subtitle {i + 1}";

					var track = new CommunityToolkit.Maui.Core.MediaTrack
					{
						Id = $"subtitle-{i}",
						Type = MediaTrackType.Text,
						Name = displayName,
						Language = timedTrack.Language ?? string.Empty,
						IsSelected = false, // Subtitles typically start disabled
						IsDefault = false,
						Metadata = new Dictionary<string, object>
						{
							{ "TrackIndex", i }
						}
					};

					tracks.Add(track);
				}
			}

			// Add "Off" option for subtitles
			if (hasSubtitles)
			{
				tracks.Add(new CommunityToolkit.Maui.Core.MediaTrack
				{
					Id = "subtitle-none",
					Type = MediaTrackType.Text,
					Name = "Off",
					Language = string.Empty,
					IsSelected = true,
					IsDefault = true
				});
			}

			// Update MediaElement with tracks
			Dispatcher.Dispatch(() =>
			{
				mediaElement.OnTracksChanged(tracks);
			});

		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error extracting tracks");
		}
	}
	*/

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

		metadata ??= new(systemMediaControls, MediaElement, Dispatcher);
		metadata.SetMetadata(MediaElement);
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

		// Extract tracks if we have a MediaPlaybackItem
		if (playbackItem is not null)
		{
			System.Diagnostics.Debug.WriteLine("MediaOpened - extracting tracks");
			//ExtractAndUpdateTracks(playbackItem);
			ExtractAudioTracks(playbackItem);
			ExtractVideoTracks(playbackItem);
			ExtractTextTracks(playbackItem);
		}
		else
		{
			System.Diagnostics.Debug.WriteLine("MediaOpened - no playback item available for track extraction");
		}
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
		if (!AreFloatingPointNumbersEqual(MediaElement.Speed, sender.PlaybackRate))
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
		if (sender.PlaybackState == MediaPlaybackState.Playing && sender.PlaybackRate == 0)
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