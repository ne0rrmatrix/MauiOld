using System.Diagnostics;
using System.Numerics;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage;
using Windows.System.Display;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpMethod = Windows.Web.Http.HttpMethod;
using HttpRequestMessage = Windows.Web.Http.HttpRequestMessage;
using ParentWindow = CommunityToolkit.Maui.Extensions.PageExtensions.ParentWindow;
using WindowsMediaElement = Windows.Media.Playback.MediaPlayer;
using WinMediaSource = Windows.Media.Core.MediaSource;

namespace CommunityToolkit.Maui.Core.Views;

partial class MediaManager : IDisposable
{
	// States that allow changing position
	readonly IReadOnlyList<MediaElementState> allowUpdatePositionStates =
	[
		MediaElementState.Playing,
		MediaElementState.Paused,
		MediaElementState.Stopped,
	];

	Metadata? metadata;
	SystemMediaTransportControls? systemMediaControls;
	HttpClient? headerHttpClient;
	AdaptiveMediaSource? adaptiveMediaSource;
	MauiMediaElement? mauiMediaElement;

	// The requests to keep display active are cumulative
	bool displayActiveRequested;

	// Guards against late-arriving platform callbacks after Stop() / disconnect
	volatile bool isDisposed;

	/// <summary>
	/// The <see cref="DisplayRequest"/> is used to enable the <see cref="MediaElement.ShouldKeepScreenOn"/> functionality.
	/// </summary>
	protected DisplayRequest DisplayRequest { get; } = new();

	/// <summary>
	/// Creates the corresponding platform view of <see cref="MediaElement"/> on Windows.
	/// </summary>
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

	internal void SetMauiMediaElement(MauiMediaElement mediaElement) => mauiMediaElement = mediaElement;

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MediaManager"/>.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual async partial void PlatformPlay()
	{
		if (isUsingWebView2Drm)
		{
			await WebView2Play();
		}
		else
		{
			Player?.MediaPlayer.Play();
		}

		if (MediaElement.ShouldKeepScreenOn
			&& !displayActiveRequested)
		{
			DisplayRequest.RequestActive();
			displayActiveRequested = true;
		}
	}

	protected virtual async partial void PlatformPause()
	{
		if (isUsingWebView2Drm)
		{
			await WebView2Pause();
		}
		else
		{
			Player?.MediaPlayer.Pause();
		}

		if (displayActiveRequested)
		{
			DisplayRequest.RequestRelease();
			displayActiveRequested = false;
		}
	}

	protected virtual async partial Task PlatformSeek(TimeSpan position, CancellationToken token)
	{
		if (isUsingWebView2Drm)
		{
			await WebView2Seek(position.TotalSeconds);
			return;
		}

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

	protected virtual async partial void PlatformStop()
	{
		if (isUsingWebView2Drm)
		{
			await WebView2Pause();
			await WebView2Seek(0);
			MediaElement.CurrentStateChanged(MediaElementState.Stopped);

			if (displayActiveRequested)
			{
				DisplayRequest.RequestRelease();
				displayActiveRequested = false;
			}
			return;
		}

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

	protected virtual async partial void PlatformUpdateAspect()
	{
		if (isUsingWebView2Drm)
		{
			var objectFit = MediaElement.Aspect switch
			{
				Aspect.Fill => "fill",
				Aspect.AspectFill => "cover",
				_ => "contain",
			};
			await WebView2ExecuteScriptAsync($"bridgeSetAspect('{objectFit}');");
			return;
		}

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

	protected virtual async partial void PlatformUpdateSpeed()
	{
		if (isUsingWebView2Drm)
		{
			await WebView2SetPlaybackRate(MediaElement.Speed);
			return;
		}

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
		// In WebView2 DRM mode, position updates come from JS via the bridge
		if (isUsingWebView2Drm)
		{
			return;
		}

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

	protected virtual async partial void PlatformUpdateVolume()
	{
		if (isUsingWebView2Drm)
		{
			await WebView2SetVolume(MediaElement.Volume);
			return;
		}

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

	protected virtual async partial void PlatformUpdateShouldMute()
	{
		if (isUsingWebView2Drm)
		{
			await WebView2SetMuted(MediaElement.ShouldMute);
			return;
		}

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

		CleanupWebView2Drm();
		adaptiveMediaSource?.DownloadRequested -= OnAdaptiveMediaSourceDownloadRequested;
		adaptiveMediaSource = null;

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
		Player.AutoPlay = MediaElement.ShouldAutoPlay;

		if (MediaElement.Source is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri?.AbsoluteUri;
			if (!string.IsNullOrWhiteSpace(uri))
			{
				var drm = uriMediaSource.DrmConfiguration;
				var headers = uriMediaSource.HttpHeaders;

				if (drm is { Scheme: DrmScheme.PlayReady, LicenseServerUrl: not null })
				{
					CleanupWebView2Drm();
					MainThread.BeginInvokeOnMainThread(async () => await SetupWebView2DrmAsync(uri, drm, MediaElement.ShouldAutoPlay));
					return;
				}

				if (headers.Count > 0)
				{
					await SetUriSourceWithHeaders(new Uri(uri), headers);
				}
				else
				{
					Player.MediaPlayer.SetUriSource(new Uri(uri));
				}
			}
		}
		else if (MediaElement.Source is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filename);
				Player.MediaPlayer.SetFileSource(storageFile);
			}
		}
		else if (MediaElement.Source is ResourceMediaSource resourceMediaSource)
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
		else if (MediaElement.Source is StreamMediaSource streamMediaSource)
		{
			if (streamMediaSource.Stream is not null)
			{
				var randomAccessStream = streamMediaSource.Stream.AsRandomAccessStream();
				Player.Source = WinMediaSource.CreateFromStream(randomAccessStream, streamMediaSource.Stream.GetMimeType());
			}
		}
	}

	protected virtual partial void PlatformUpdateDrmConfiguration()
	{
		// On Apple platforms, DRM configuration is applied at source creation time.
		// If DRM config changes dynamically, re-run PlatformUpdateSource to rebuild
		// the AVAsset with the new FairPlay resource loader delegate.
		if (Player is not null && MediaElement.Source is UriMediaSource)
		{
			PlatformUpdateSource();
		}
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
	/// Releases unmanaged resources.
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Trace.WriteLine($"[MediaElement.Windows] Dispose — state={MediaElement.CurrentState}");
			isDisposed = true;

			CleanupWebView2Drm();
			mauiMediaElement = null;

			adaptiveMediaSource?.DownloadRequested -= OnAdaptiveMediaSourceDownloadRequested;
			adaptiveMediaSource = null;

			headerHttpClient?.Dispose();
			headerHttpClient = null;

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

	async Task SetUriSourceWithHeaders(Uri uri, IDictionary<string, string> headers)
	{
		if (Player is null)
		{
			return;
		}

		headerHttpClient ??= new HttpClient();
		headerHttpClient.DefaultRequestHeaders.Clear();

		foreach (var header in headers)
		{
			if (!headerHttpClient.DefaultRequestHeaders.TryAppendWithoutValidation(header.Key, header.Value))
			{
				throw new InvalidOperationException($"Failed to append HTTP header '{header.Key}'. The header name may be empty or contain invalid characters.");
			}
		}

		var adaptiveResult = await AdaptiveMediaSource.CreateFromUriAsync(uri, headerHttpClient).AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);

		if (adaptiveResult.Status is AdaptiveMediaSourceCreationStatus.Success && adaptiveResult.MediaSource is not null)
		{
			adaptiveMediaSource = adaptiveResult.MediaSource;
			adaptiveMediaSource.DownloadRequested += OnAdaptiveMediaSourceDownloadRequested;

			var mediaSource = WinMediaSource.CreateFromAdaptiveMediaSource(adaptiveMediaSource);
			await Dispatcher.DispatchAsync(() =>
			{
				Player.AutoPlay = MediaElement.ShouldAutoPlay;
				Player.Source = mediaSource;
			});
		}
		else
		{
			var stream = await HttpRandomAccessStream.CreateAsync(headerHttpClient, uri);
			await Dispatcher.DispatchAsync(() =>
			{
				Player.AutoPlay = MediaElement.ShouldAutoPlay;
				Player.Source = WinMediaSource.CreateFromStream(stream, string.Empty);
			});
		}
	}

	async void OnAdaptiveMediaSourceDownloadRequested(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadRequestedEventArgs args)
	{
		if (headerHttpClient is null)
		{
			return;
		}

		var deferral = args.GetDeferral();
		try
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, args.ResourceUri);
			using var response = await headerHttpClient.SendRequestAsync(request).AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
			response.EnsureSuccessStatusCode();
			args.Result.InputStream = await response.Content.ReadAsInputStreamAsync().AsTask();
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"MediaElement [Windows]: DownloadRequested failed for {args.ResourceUri}: {ex.Message}");
		}
		finally
		{
			deferral.Complete();
		}
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
		if (isDisposed || Player is null)
		{
			Trace.WriteLine($"[MediaElement.Windows] OnMediaOpened — BLOCKED: isDisposed={isDisposed}");
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