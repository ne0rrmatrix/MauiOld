using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Nodes;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Web.WebView2.Core;
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
	string? manifestUrl;
	DrmConfiguration? drmConfig;
	
	WebView2? drmWebView;
	WebView2TransportOverlay? drmTransportOverlay;
	bool isUsingWebView2Drm;

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
			await drmWebView?.CoreWebView2.ExecuteScriptAsync($"bridgeSetAspect('{objectFit}');");
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
					MainThread.BeginInvokeOnMainThread(async () => await SetupWebView2DrmAsync(uri, drm));
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

	async Task SetupWebView2DrmAsync(string manifestUrl, DrmConfiguration drmConfig)
	{
		this.manifestUrl = manifestUrl;
		this.drmConfig = drmConfig;

		drmWebView = new WebView2
		{
			HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch,
			VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
		};

		drmTransportOverlay = new WebView2TransportOverlay();
		WireTransportOverlayEvents(drmTransportOverlay);

		mauiMediaElement?.SwapToWebView2(drmWebView, drmTransportOverlay);
		drmWebView.CoreWebView2Initialized += DrmWebView_CoreWebView2Initialized;
		await drmWebView.EnsureCoreWebView2Async();
	}

	async void DrmWebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
	{
		ArgumentNullException.ThrowIfNull(drmWebView);
		ArgumentNullException.ThrowIfNull(manifestUrl);
		ArgumentNullException.ThrowIfNull(drmConfig);

		drmWebView.CoreWebView2.Settings.IsWebMessageEnabled = true;
		drmWebView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
		drmWebView.CoreWebView2.WebMessageReceived += OnWebView2WebMessageReceived;

		var html = BuildDrmPlayerHtml(manifestUrl, drmConfig);
		var tempDir = Path.Combine(Path.GetTempPath(), "maui-drm-player");
		Directory.CreateDirectory(tempDir);
		await File.WriteAllTextAsync(Path.Combine(tempDir, "player.html"), html);

		drmWebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
			"drmplayer.local", tempDir, CoreWebView2HostResourceAccessKind.Allow);

		drmWebView.CoreWebView2.Navigate("https://drmplayer.local/player.html");
	}

	void OnWebView2WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
	{
		var json = args.TryGetWebMessageAsString();

		var msg = JsonNode.Parse(json);
		if (msg is null)
		{
			return;
		}

		var type = msg["type"]?.GetValue<string>();

		switch (type)
		{
			case "ready":
				isUsingWebView2Drm = true;
				break;

			case "state":
				var state = msg["state"]?.GetValue<string>();
				HandleWebView2StateChange(state);
				break;

			case "time":
				var currentTime = msg["currentTime"]?.GetValue<double>() ?? 0;
				var duration = msg["duration"]?.GetValue<double>() ?? 0;
				HandleWebView2TimeUpdate(currentTime, duration);
				break;

			case "error":
				var errorMessage = msg["message"]?.GetValue<string>() ?? "Unknown error";
				Trace.WriteLine($"[MediaElement.Windows.PlayReady.WebView2] Player error: {errorMessage}");
				break;

			case "ended":
				MediaElement.CurrentStateChanged(MediaElementState.Stopped);
				break;

			case "loadedmetadata":
				var metaDuration = msg["duration"]?.GetValue<double>() ?? 0;
				var width = msg["width"]?.GetValue<int>() ?? 0;
				var height = msg["height"]?.GetValue<int>() ?? 0;
				MediaElement.Duration = TimeSpan.FromSeconds(metaDuration);
				MediaElement.MediaWidth = width;
				MediaElement.MediaHeight = height;
				break;
		}
	}

	void HandleWebView2StateChange(string? state)
	{
		var newState = state switch
		{
			"playing" => MediaElementState.Playing,
			"paused" => MediaElementState.Paused,
			"buffering" or "waiting" => MediaElementState.Buffering,
			"ended" => MediaElementState.Stopped,
			_ => MediaElementState.None,
		};

		if (newState != MediaElementState.None)
		{
			MediaElement.CurrentStateChanged(newState);
			drmTransportOverlay?.UpdateIsPlaying(newState == MediaElementState.Playing);
		}
	}

	void HandleWebView2TimeUpdate(double currentTime, double duration)
	{
		MediaElement.Position = TimeSpan.FromSeconds(currentTime);
		if (duration > 0)
		{
			MediaElement.Duration = TimeSpan.FromSeconds(duration);
		}

		drmTransportOverlay?.UpdatePosition(TimeSpan.FromSeconds(currentTime));
		drmTransportOverlay?.UpdateDuration(TimeSpan.FromSeconds(duration));
	}

	async Task WebView2Play() => await drmWebView?.CoreWebView2.ExecuteScriptAsync("bridgePlay();");
	async Task WebView2Pause() => await drmWebView?.CoreWebView2.ExecuteScriptAsync("bridgePause();");
	async Task WebView2Stop() => await drmWebView?.CoreWebView2.ExecuteScriptAsync("bridgeStop();");
	async Task WebView2Seek(double seconds) => await drmWebView?.CoreWebView2.ExecuteScriptAsync($"bridgeSeek({seconds.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	async Task WebView2Skip(double seconds) => await drmWebView?.CoreWebView2.ExecuteScriptAsync($"bridgeSkip({seconds.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	async Task WebView2SetVolume(double volume) => await drmWebView?.CoreWebView2.ExecuteScriptAsync($"bridgeSetVolume({volume.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	async Task WebView2SetMuted(bool muted) => await drmWebView?.CoreWebView2.ExecuteScriptAsync($"bridgeSetMuted({muted.ToString().ToLowerInvariant()});");
	async Task WebView2SetPlaybackRate(double rate) => await drmWebView?.CoreWebView2.ExecuteScriptAsync($"bridgeSetPlaybackRate({rate.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	async Task WebView2ToggleAspect() => await drmWebView?.CoreWebView2.ExecuteScriptAsync("bridgeToggleAspect();");

	void CleanupWebView2Drm()
	{
		if (drmWebView?.CoreWebView2 is not null)
		{
			drmWebView.CoreWebView2.WebMessageReceived -= OnWebView2WebMessageReceived;
			drmWebView.Close();
			drmWebView = null;
		}

		drmTransportOverlay = null;
		isUsingWebView2Drm = false;
	}

	void WireTransportOverlayEvents(WebView2TransportOverlay overlay)
	{
		overlay.PlayRequested += (s, e) => _ = WebView2Play();
		overlay.PauseRequested += (s, e) => _ = WebView2Pause();
		overlay.SeekRequested += (s, seconds) => _ = WebView2Seek(seconds);
		overlay.VolumeChanged += (s, vol) =>
		{
			_ = WebView2SetVolume(vol);
			MediaElement.Volume = vol;
		};
		overlay.MuteChanged += (s, muted) =>
		{
			_ = WebView2SetMuted(muted);
			MediaElement.ShouldMute = muted;
		};
		overlay.FullScreenRequested += (s, e) => mauiMediaElement?.ToggleFullScreen();
		overlay.StopRequested += (s, e) =>
		{
			_ = WebView2Stop();
			MediaElement.CurrentStateChanged(MediaElementState.Stopped);
		};
		overlay.ZoomRequested += (s, e) => _ = WebView2ToggleAspect();
		overlay.SkipBackwardRequested += (s, seconds) => _ = WebView2Skip(-seconds);
		overlay.SkipForwardRequested += (s, seconds) => _ = WebView2Skip(seconds);
	}

	string BuildDrmPlayerHtml(string manifestUrl, DrmConfiguration drmConfig)
	{
		var licenseUrl = drmConfig.LicenseServerUrl?.AbsoluteUri ?? "";
		var autoplayStr = MediaElement.ShouldAutoPlay ? "true" : "false";

		var headersJson = new JsonObject();
		foreach (var header in drmConfig.LicenseRequestHeaders)
		{
			headersJson[header.Key] = header.Value;
		}
		var headersStr = headersJson.ToJsonString();

		var manifestUrlJs = EscapeJsString(manifestUrl);
		var licenseUrlJs = EscapeJsString(licenseUrl);

		return $$"""
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<style>
  * { margin: 0; padding: 0; box-sizing: border-box; }
  html, body { width: 100%; height: 100%; overflow: hidden; background: #000; }
  video {
    width: 100%;
    height: 100%;
    object-fit: contain;
    background: #000;
  }
  #error-overlay {
    display: none;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    color: #ff4444;
    font-family: Segoe UI, sans-serif;
    font-size: 14px;
    text-align: center;
    padding: 20px;
    max-width: 80%;
  }
</style>
</head>
<body>
<video id="videoPlayer"></video>
<div id="error-overlay"></div>

<script src="https://cdn.dashjs.org/v4.7.4/dash.all.min.js"></script>
<script>
(function() {
  'use strict';

  const MANIFEST_URL = "{{manifestUrlJs}}";
  const LICENSE_URL = "{{licenseUrlJs}}";
  const LICENSE_HEADERS = {{headersStr}};
  const AUTOPLAY = {{autoplayStr}};

  const video = document.getElementById('videoPlayer');
  const errorOverlay = document.getElementById('error-overlay');
  let player = null;
  let timeUpdateInterval = null;

  // ─── JS → C# bridge ───────────────────────────────────────────────
  function postToCSharp(obj) {
    if (window.chrome && window.chrome.webview) {
      window.chrome.webview.postMessage(JSON.stringify(obj));
    }
  }

  function showError(msg) {
    errorOverlay.textContent = msg;
    errorOverlay.style.display = 'block';
    postToCSharp({ type: 'error', message: msg });
  }

  // ─── Initialize dash.js with PlayReady EME ────────────────────────
  function initPlayer() {
    try {
      player = dashjs.MediaPlayer().create();

      // Protection data for PlayReady — must be set BEFORE initialize
      // so the license acquisition is configured before autoplay begins
      const protectionData = {
        'com.microsoft.playready': {
          serverURL: LICENSE_URL,
          httpRequestHeaders: LICENSE_HEADERS,
          // Use persistent licenses for offline support
          persistentState: 'required',
          distinctiveIdentifier: 'required'
        }
      };

      player.setProtectionData(protectionData);
      player.initialize(video, MANIFEST_URL, AUTOPLAY);

      // ─── Video element events → C# ──────────────────────────────
      video.addEventListener('play', () => {
        postToCSharp({ type: 'state', state: 'playing' });
      });

      video.addEventListener('pause', () => {
        if (!video.ended) {
          postToCSharp({ type: 'state', state: 'paused' });
        }
      });

      video.addEventListener('waiting', () => {
        postToCSharp({ type: 'state', state: 'buffering' });
      });

      video.addEventListener('playing', () => {
        postToCSharp({ type: 'state', state: 'playing' });
      });

      video.addEventListener('ended', () => {
        postToCSharp({ type: 'ended' });
        stopTimeUpdates();
      });

      video.addEventListener('loadedmetadata', () => {
        postToCSharp({
          type: 'loadedmetadata',
          duration: video.duration || 0,
          width: video.videoWidth || 0,
          height: video.videoHeight || 0
        });
        startTimeUpdates();
      });

      video.addEventListener('error', () => {
        const err = video.error;
        showError('Video error: ' + (err ? err.message : 'unknown'));
      });

      // ─── dash.js events ─────────────────────────────────────────
      player.on(dashjs.MediaPlayer.events.ERROR, function(e) {
        let msg = 'dash.js error';
        if (e.error) {
          msg += ': ' + (e.error.message || e.error.code || JSON.stringify(e.error));
        }
        if (e.event && e.event.error) {
          msg += ' | ' + JSON.stringify(e.event.error);
        }
        showError(msg);
      });

      player.on(dashjs.MediaPlayer.events.PLAYBACK_STARTED, function() {
        postToCSharp({ type: 'state', state: 'playing' });
      });

      player.on(dashjs.MediaPlayer.events.PLAYBACK_PAUSED, function() {
        postToCSharp({ type: 'state', state: 'paused' });
      });

      player.on(dashjs.MediaPlayer.events.STREAM_INITIALIZED, function() {
        postToCSharp({ type: 'ready' });
      });

      // Signal ready immediately if stream is already initialized
      setTimeout(() => {
        postToCSharp({ type: 'ready' });
      }, 2000);

    } catch (ex) {
      showError('Init failed: ' + ex.message);
    }
  }

  // ─── Time updates (throttled to ~4Hz) ─────────────────────────────
  function startTimeUpdates() {
    stopTimeUpdates();
    timeUpdateInterval = setInterval(() => {
      if (video && !video.paused && !video.ended) {
        postToCSharp({
          type: 'time',
          currentTime: video.currentTime || 0,
          duration: video.duration || 0
        });
      }
    }, 250);
  }

  function stopTimeUpdates() {
    if (timeUpdateInterval) {
      clearInterval(timeUpdateInterval);
      timeUpdateInterval = null;
    }
  }

  // ─── C# → JS bridge functions ─────────────────────────────────────
  window.bridgePlay = function() {
    video.play();
  };

  window.bridgePause = function() {
    video.pause();
  };

  window.bridgeSeek = function(seconds) {
    video.currentTime = seconds;
  };

  window.bridgeSetVolume = function(vol) {
    video.volume = Math.max(0, Math.min(1, vol));
  };

  window.bridgeSetMuted = function(muted) {
    video.muted = muted;
  };

  window.bridgeSetPlaybackRate = function(rate) {
    video.playbackRate = rate;
  };

  window.bridgeStop = function() {
    video.pause();
    video.currentTime = 0;
    postToCSharp({ type: 'state', state: 'paused' });
  };

  window.bridgeSkip = function(seconds) {
    video.currentTime = Math.max(0, Math.min(video.duration || 0, video.currentTime + seconds));
  };

  window.bridgeToggleAspect = function() {
    const fits = ['contain', 'cover', 'fill'];
    const current = video.style.objectFit || 'contain';
    const idx = fits.indexOf(current);
    video.style.objectFit = fits[(idx + 1) % fits.length];
  };

  window.bridgeSetAspect = function(fit) {
    video.style.objectFit = fit;
  };

  window.bridgeDestroy = function() {
    stopTimeUpdates();
    if (player) {
      player.destroy();
      player = null;
    }
  };

  // ─── Boot ─────────────────────────────────────────────────────────
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initPlayer);
  } else {
    initPlayer();
  }
})();
</script>
</body>
</html>
""";
	}

	static string EscapeJsString(string value)
	{
		return value
			.Replace("\\", "\\\\")
			.Replace("\"", "\\\"")
			.Replace("'", "\\'")
			.Replace("\n", "\\n")
			.Replace("\r", "\\r")
			.Replace("\t", "\\t");
	}
}