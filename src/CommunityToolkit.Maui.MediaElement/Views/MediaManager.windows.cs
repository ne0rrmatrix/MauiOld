using System.Diagnostics;
using System.Numerics;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media;
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
	MediaPlaybackList playbackList = new();

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
		Player.MediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
		playbackList.CurrentItemChanged += OnPlaybackListCurrentItemChanged;
		Player.MediaPlayer.SystemMediaTransportControls.IsEnabled = false;
		systemMediaControls = Player.MediaPlayer.SystemMediaTransportControls;

		return Player;
	}

	void MediaPlayer_MediaFailed(WindowsMediaElement sender, MediaPlayerFailedEventArgs args)
	{
		System.Diagnostics.Trace.WriteLine($"Media failed: {args.ErrorMessage}");
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
		if (Player is null)
		{
			return;
		}
		Dispatcher.Dispatch(() => Player.MediaPlayer.IsMuted = MediaElement.ShouldMute);
	}

	protected virtual async partial ValueTask PlatformUpdateSource()
	{
		if (Player is null || string.IsNullOrEmpty(MediaElement.Source?.ToString()))
		{
			return;
		}

		await Dispatcher.DispatchAsync(() => Player.PosterSource = new BitmapImage());
		
		Player.Source = null;

		if (MediaElement.Source is null)
		{
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;

			MediaElement.CurrentStateChanged(MediaElementState.None);

			return;
		}

		MediaElement.Position = TimeSpan.Zero;
		MediaElement.Duration = TimeSpan.Zero;
		Player.AutoPlay = MediaElement.ShouldAutoPlay;

		var source = MediaElement.Source.Source;
		if (source is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri?.AbsoluteUri;
			if (!string.IsNullOrWhiteSpace(uri))
			{
				Player.MediaPlayer.SetUriSource(new Uri(uri));
			}
		}
		else if (source is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filename);
				Player.MediaPlayer.SetFileSource(storageFile);
			}
		}
		else if (source is ResourceMediaSource resourceMediaSource)
		{
			if (string.IsNullOrWhiteSpace(resourceMediaSource.Path))
			{
				Logger.LogInformation("ResourceMediaSource Path is null or empty");
				return;
			}

			string path = GetFullAppPackageFilePath(resourceMediaSource.Path);
			if (!string.IsNullOrWhiteSpace(path))
			{
				Player.MediaPlayer.SetUriSource(new Uri(path));
			}
		}
	}

	protected virtual async partial ValueTask PlatformUpdatePlaylist()
	{
		if (Player is null || MediaElement.Playlist is null || MediaElement.Playlist.Count == 0)
		{
			return;
		}
		await Dispatcher.DispatchAsync(() => Player.PosterSource = new BitmapImage());

		playbackList.Items.Clear();

		Player.MediaPlayer.Source = null;
		MediaElement.MediaWidth = MediaElement.MediaHeight = 0;
		MediaElement.CurrentStateChanged(MediaElementState.None);
		MediaElement.Position = TimeSpan.Zero;
		MediaElement.Duration = TimeSpan.Zero;

		foreach (var item in MediaElement.Playlist)
		{
			if (item is null)
			{
				continue;
			}
			System.Diagnostics.Trace.WriteLine($"MediaElement.Playlist item: {item.Source?.ToString()}");
			var mediaSource = await GetMediaSourceFromMediaItem(item.Source).ConfigureAwait(true);
			if (mediaSource is not null)
			{
				try
				{
					mediaSource.CustomProperties["MetadataTitle"] = item.MetadataTitle ?? string.Empty;
					mediaSource.CustomProperties["MetadataArtist"] = item.MetadataArtist ?? string.Empty;
					mediaSource.CustomProperties["MetadataArtworkUrl"] = item.MetadataArtworkUrl ?? string.Empty;
				}
				catch
				{
					System.Diagnostics.Trace.WriteLine("Failed to set CustomProperties on MediaSource");
				}
				
				var mediaPlaybackItem = new MediaPlaybackItem(mediaSource);
				playbackList.Items.Add(mediaPlaybackItem);
			}
		}

		Player.AutoPlay = MediaElement.ShouldAutoPlay;
		Player.Source = playbackList;
		playbackList.ItemFailed += (s, e) =>
		{
			System.Diagnostics.Trace.WriteLine($"PlaybackList item failed: {e.Error.ToString()}");
		};
	}
	
	async Task<Windows.Media.Core.MediaSource?> GetMediaSourceFromMediaItem(MediaSource? mediaItem)
	{
		if (mediaItem is null)
		{
			return null;
		}

		if (mediaItem is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri?.AbsoluteUri;
			if (!string.IsNullOrWhiteSpace(uri))
			{
				return Windows.Media.Core.MediaSource.CreateFromUri(new Uri(uri));
			}
		}
		else if (mediaItem is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				var storageFile = await StorageFile.GetFileFromPathAsync(filename);
				return Windows.Media.Core.MediaSource.CreateFromStorageFile(storageFile);
			}
		}
		else if (mediaItem is ResourceMediaSource resourceMediaSource)
		{
			if (string.IsNullOrWhiteSpace(resourceMediaSource.Path))
			{
				Logger.LogInformation("ResourceMediaSource Path is null or empty");
				return null;
			}
			string path = GetFullAppPackageFilePath(resourceMediaSource.Path);
			if (!string.IsNullOrWhiteSpace(path))
			{
				return Windows.Media.Core.MediaSource.CreateFromUri(new Uri(path));
			}
		}
		return null;
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
				if(playbackList is not null)
				{
					playbackList.CurrentItemChanged -= OnPlaybackListCurrentItemChanged;
					playbackList.Items.Clear();
				}
				Player.MediaPlayer.MediaOpened -= OnMediaElementMediaOpened;
				Player.MediaPlayer.MediaFailed -= OnMediaElementMediaFailed;
				Player.MediaPlayer.MediaEnded -= OnMediaElementMediaEnded;
				Player.MediaPlayer.VolumeChanged -= OnMediaElementVolumeChanged;
				Player.MediaPlayer.IsMutedChanged -= OnMediaElementIsMutedChanged;

				if(playbackList is not null)
				{
					playbackList.CurrentItemChanged -= OnPlaybackListCurrentItemChanged;
				}

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

	static string GetFullAppPackageFilePath(in string filename)
	{
		ArgumentNullException.ThrowIfNull(filename);

		var normalizedFilename = NormalizePath(filename);
		return Path.Combine(AppPackageService.FullAppPackageFilePath, normalizedFilename);

		static string NormalizePath(string filename) => filename.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
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
		
		metadata ??= new(systemMediaControls);
		metadata.SetMetadata(MediaElement.Source);
		var mediaItem = MediaElement.Source;
		var artworkUrl = mediaItem?.MetadataArtworkUrl;
		if (string.IsNullOrEmpty(artworkUrl))
		{
			return;
		}
		if (!Uri.TryCreate(artworkUrl, UriKind.RelativeOrAbsolute, out var metadataArtworkUri))
		{
			Trace.TraceError($"{nameof(MediaElement)} unable to update artwork because {nameof(artworkUrl)} is not a valid URI");
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

	// Called whenever the playback list advances to a new item
	void OnPlaybackListCurrentItemChanged(MediaPlaybackList? sender, CurrentMediaPlaybackItemChangedEventArgs? args)
	{
		if (sender is null || args is null)
		{
			System.Diagnostics.Trace.WriteLine("Sender is null");
			return;
		}
		try
		{
			// Try to retrieve per-item metadata we stored on the MediaSource.CustomProperties
			var mediaSource = args.NewItem?.Source;
			var mediaItem = new MediaItem();
			if (mediaSource is not null)
			{
				mediaItem.Source = mediaSource.Uri;
				if (mediaSource.CustomProperties.TryGetValue("MetadataTitle", out var titleObj)
					&& titleObj is string title)
				{
					mediaItem.MetadataTitle = title;
				}
				if (mediaSource.CustomProperties.TryGetValue("MetadataArtist", out var artistObj)
					&& artistObj is string artist)
				{
					mediaItem.MetadataArtist = artist;
				}
				if (mediaSource.CustomProperties.TryGetValue("MetadataArtworkUrl", out var artworkObj)
					&& artworkObj is string artworkUrl)
				{
					mediaItem.MetadataArtworkUrl = artworkUrl;
				}
				if (systemMediaControls is null)
				{
					System.Diagnostics.Trace.WriteLine("systemMediaControls is null in OnPlaybackListCurrentItemChanged");
					return;
				}
				if(Player is null)
				{
					System.Diagnostics.Trace.WriteLine("Player is null in OnPlaybackListCurrentItemChanged");
					return;
				}
				MediaElement.Source = mediaItem;
				// Update system metadata and poster
				metadata = new(systemMediaControls);
				metadata.SetMetadata(mediaItem);

				if (!string.IsNullOrEmpty(mediaItem.MetadataArtworkUrl)
					&& Uri.TryCreate(mediaItem.MetadataArtworkUrl, UriKind.RelativeOrAbsolute, out var artworkUri))
				{
					if (Dispatcher.IsDispatchRequired)
					{
						_ = Dispatcher.DispatchAsync(() => UpdatePoster(Player, artworkUri));
					}
					else
					{
						UpdatePoster(Player, artworkUri);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Trace.TraceError($"Error updating metadata from playback list: {ex}");
		}

		static void UpdatePoster(in MediaPlayerElement player, in Uri metadataArtworkUri)
		{
			player.PosterSource = new BitmapImage(metadataArtworkUri);
		}
	}
}