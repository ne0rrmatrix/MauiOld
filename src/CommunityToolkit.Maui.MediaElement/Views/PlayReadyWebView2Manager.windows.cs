using System.Diagnostics;
using System.Text.Json.Nodes;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// WebView2-based PlayReady DRM playback for WinUI 3.
/// WinUI 3 has no protected media path (PMP) for Win32 apps, so PlayReady
/// playback is delegated to Edge's rendering engine via WebView2, which has
/// full EME + PlayReady CDM support. A JS bridge synchronizes state between
/// the dash.js player in the browser and the MAUI MediaElement API surface.
/// </summary>
partial class MediaManager
{
	WebView2? drmWebView;
	WebView2TransportOverlay? drmTransportOverlay;
	bool isUsingWebView2Drm;
	TaskCompletionSource<bool>? webViewReadyTcs;

	/// <summary>
	/// Sets up WebView2-based PlayReady DRM playback. Creates a WebView2 control,
	/// loads an HTML page with dash.js configured for PlayReady EME, and swaps it
	/// into the view tree in place of the MediaPlayerElement.
	/// </summary>
	async Task SetupWebView2DrmAsync(string manifestUrl, DrmConfiguration drmConfig)
	{
		webViewReadyTcs = new TaskCompletionSource<bool>();

		try
		{
			// Create WebView2 control and transport overlay on the UI thread
			drmWebView = new WebView2
			{
				HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch,
				VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
			};

			drmTransportOverlay = new WebView2TransportOverlay();
			WireTransportOverlayEvents(drmTransportOverlay);

			// Swap into the view tree (replaces MediaPlayerElement)
			mauiMediaElement?.SwapToWebView2(drmWebView, drmTransportOverlay);

			// EnsureCoreWebView2Async must be called before NavigateToString.
			// It initializes the underlying Edge browser engine. This must run
			// on the UI thread and the WebView2 must already be in the visual tree.
			ArgumentNullException.ThrowIfNull(drmWebView);
			await drmWebView.EnsureCoreWebView2Async();

			if (drmWebView?.CoreWebView2 is null)
			{
				ReportMediaFailed("WebView2 CoreWebView2 initialization returned null");
				return;
			}

			// Wire the JS → C# message bridge
			drmWebView.CoreWebView2.WebMessageReceived += OnWebView2WebMessageReceived;
			drmWebView.CoreWebView2.Settings.IsWebMessageEnabled = true;
			drmWebView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;

			// EME (Encrypted Media Extensions) requires a secure context (HTTPS).
			// NavigateToString uses a non-secure virtual origin where MediaKeys
			// is unavailable. Instead, write the HTML to a temp folder and serve
			// it via a virtual HTTPS host mapping.
			var html = BuildDrmPlayerHtml(manifestUrl, drmConfig);
			var tempDir = Path.Combine(Path.GetTempPath(), "maui-drm-player");
			Directory.CreateDirectory(tempDir);
			await File.WriteAllTextAsync(Path.Combine(tempDir, "player.html"), html);

			if (drmWebView?.CoreWebView2 is null)
			{
				return;
			}

			// Map a virtual HTTPS host to the temp folder
			drmWebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
				"drmplayer.local", tempDir, CoreWebView2HostResourceAccessKind.Allow);

			// Navigate to the virtual HTTPS URL — EME works here
			drmWebView.CoreWebView2.Navigate("https://drmplayer.local/player.html");
		}
		catch (Exception ex)
		{
			ReportMediaFailed($"WebView2 DRM setup failed: {ex.Message}");
		}
	}

	/// <summary>
	/// Handles messages from JavaScript (JS → C# bridge).
	/// Messages are JSON: { "type": "state"|"time"|"error"|"ready"|"ended", ... }
	/// </summary>
	void OnWebView2WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
	{
		// TryGetWebMessageAsString returns the raw string without JSON double-encoding
		var json = args.TryGetWebMessageAsString();

		try
		{
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
					webViewReadyTcs?.TrySetResult(true);
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
					ReportMediaFailed(errorMessage);
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
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady.WebView2] Failed to parse JS message: {ex.Message}");
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

	// ─────────────────────────────────────────────────────────────────────
	// C# → JS bridge commands
	// ─────────────────────────────────────────────────────────────────────

	async Task WebView2ExecuteScriptAsync(string script)
	{
		if (drmWebView?.CoreWebView2 is null)
		{
			return;
		}

		try
		{
			await drmWebView.CoreWebView2.ExecuteScriptAsync(script)
				.AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady.WebView2] ExecuteScript failed: {ex.Message}");
		}
	}

	Task WebView2Play() => WebView2ExecuteScriptAsync("bridgePlay();");
	Task WebView2Pause() => WebView2ExecuteScriptAsync("bridgePause();");
	Task WebView2Stop() => WebView2ExecuteScriptAsync("bridgeStop();");
	Task WebView2Seek(double seconds) => WebView2ExecuteScriptAsync($"bridgeSeek({seconds.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	Task WebView2Skip(double seconds) => WebView2ExecuteScriptAsync($"bridgeSkip({seconds.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	Task WebView2SetVolume(double volume) => WebView2ExecuteScriptAsync($"bridgeSetVolume({volume.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	Task WebView2SetMuted(bool muted) => WebView2ExecuteScriptAsync($"bridgeSetMuted({muted.ToString().ToLowerInvariant()});");
	Task WebView2SetPlaybackRate(double rate) => WebView2ExecuteScriptAsync($"bridgeSetPlaybackRate({rate.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
	Task WebView2ToggleAspect() => WebView2ExecuteScriptAsync("bridgeToggleAspect();");

	void CleanupWebView2Drm()
	{
		if (drmWebView is not null)
		{
			if (drmWebView.CoreWebView2 is not null)
			{
				drmWebView.CoreWebView2.WebMessageReceived -= OnWebView2WebMessageReceived;
			}

			drmWebView.Close();
			drmWebView = null;
		}

		drmTransportOverlay = null;
		isUsingWebView2Drm = false;
		webViewReadyTcs = null;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Transport overlay wiring
	// ─────────────────────────────────────────────────────────────────────

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

	// ─────────────────────────────────────────────────────────────────────
	// HTML page generation
	// ─────────────────────────────────────────────────────────────────────

	/// <summary>
	/// Builds the HTML page containing dash.js with PlayReady EME configuration.
	/// The page includes a JS bridge that posts state changes to C# via
	/// <c>window.chrome.webview.postMessage()</c> and exposes functions callable
	/// from C# via <c>ExecuteScriptAsync</c>.
	/// </summary>
	static string BuildDrmPlayerHtml(string manifestUrl, DrmConfiguration drmConfig)
	{
		var licenseUrl = drmConfig.LicenseServerUrl?.AbsoluteUri ?? "";
		var autoplay = "true";

		// Build custom headers JSON for the license request
		var headersJson = new JsonObject();
		foreach (var header in drmConfig.LicenseRequestHeaders)
		{
			headersJson[header.Key] = header.Value;
		}
		var headersStr = headersJson.ToJsonString();

		// Simple JSON string escaping (avoids AOT-incompatible JsonSerializer.Serialize<T>)
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
  const AUTOPLAY = {{autoplay}};

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

	/// <summary>
	/// Escapes a string for safe embedding inside a JavaScript string literal.
	/// Handles backslash, quotes, newlines, and other control characters.
	/// </summary>
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