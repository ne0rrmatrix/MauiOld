using System.Diagnostics;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// PlayReady DRM entry point for the Windows platform.
/// WinUI 3 has no protected media path (PMP) for Win32 apps, so PlayReady
/// playback is delegated to Edge's rendering engine via WebView2, which has
/// full EME + PlayReady CDM support. The WebView2 setup, JS bridge, and
/// transport overlay live in <c>PlayReadyWebView2Manager.windows.cs</c>.
/// </summary>
partial class MediaManager
{
	/// <summary>
	/// Loads PlayReady-protected content using WebView2 + dash.js.
	/// WinUI 3 has no protected media path (PMP) for Win32 apps, so PlayReady
	/// playback is delegated to Edge's rendering engine via WebView2, which has
	/// full EME + PlayReady CDM support. A JS bridge synchronizes state between
	/// the dash.js player in the browser and the MAUI MediaElement API surface.
	/// </summary>
	async Task SetUriSourceWithPlayReadyAsync(string manifestUrl, IDictionary<string, string> headers, DrmConfiguration drmConfig)
	{
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] SetUriSourceWithPlayReadyAsync (WebView2) — URI={manifestUrl}");

		try
		{
			// Clean up any previous WebView2 DRM session
			CleanupWebView2Drm();

			// Set up WebView2 + dash.js with PlayReady EME
			await SetupWebView2DrmAsync(manifestUrl, drmConfig);
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] SetUriSourceWithPlayReadyAsync exception: {ex}");
			ReportMediaFailed($"PlayReady WebView2 playback setup failed: {ex.Message}");
		}
	}

	/// <summary>
	/// Reports a media failure on the UI thread, guarding against late-arriving
	/// callbacks after the manager has been disposed.
	/// </summary>
	void ReportMediaFailed(string message)
	{
		if (isDisposed)
		{
			return;
		}

		if (Dispatcher.IsDispatchRequired)
		{
			Dispatcher.Dispatch(() =>
			{
				if (!isDisposed)
				{
					MediaElement.MediaFailed(new MediaFailedEventArgs(message));
				}
			});
		}
		else
		{
			MediaElement.MediaFailed(new MediaFailedEventArgs(message));
		}
	}

	/// <inheritdoc />
	protected virtual partial void PlatformUpdateDrmConfiguration()
	{
		// DRM routing is handled by PlatformUpdateSource in MediaManager.windows.cs.
		// This partial exists for MAUI lifecycle wiring only.
		Trace.WriteLine("[MediaElement.Windows.PlayReady] PlatformUpdateDrmConfiguration (no-op — handled by PlatformUpdateSource)");
	}
}
