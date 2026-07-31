namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Provides logging for DRM session lifecycle events from ExoPlayer.
/// </summary>
/// <remarks>
/// <para>ExoPlayer handles DRM internally when MediaItem.DrmConfiguration
/// is set on the <see cref="AndroidX.Media3.Common.MediaItem.Builder"/>. This class
/// provides trace-level logging for DRM session state changes.</para>
/// <para>In a future Media3 version update, this can be extended to implement
/// <c>IDrmSessionEventListener</c> for richer error surfacing.</para>
/// </remarks>
static class DrmSessionEventListener
{
	/// <summary>
	/// Logs that DRM keys have been loaded successfully.
	/// </summary>
	public static void LogKeysLoaded(int windowIndex)
	{
		System.Diagnostics.Trace.WriteLine($"MediaElement [Android]: DRM keys loaded for window {windowIndex}");
	}

	/// <summary>
	/// Logs that DRM keys have been removed.
	/// </summary>
	public static void LogKeysRemoved(int windowIndex)
	{
		System.Diagnostics.Trace.WriteLine($"MediaElement [Android]: DRM keys removed for window {windowIndex}");
	}

	/// <summary>
	/// Logs that a DRM session has been acquired.
	/// </summary>
	public static void LogSessionAcquired(int windowIndex)
	{
		System.Diagnostics.Trace.WriteLine($"MediaElement [Android]: DRM session acquired for window {windowIndex}");
	}

	/// <summary>
	/// Logs a DRM session manager error and surfaces it to the media element.
	/// </summary>
	/// <param name="windowIndex">The window index where the error occurred.</param>
	/// <param name="mediaElement">The media element to surface the error to.</param>
	/// <param name="errorMessage">The error message from the DRM subsystem.</param>
	public static void LogSessionError(int windowIndex, IMediaElement mediaElement, string errorMessage)
	{
		var message = $"DRM session error in window {windowIndex}: {errorMessage}";
		System.Diagnostics.Trace.WriteLine($"MediaElement [Android]: {message}");
		mediaElement.MediaFailed(new MediaFailedEventArgs(message));
	}

	/// <summary>
	/// Logs that a DRM session has been released.
	/// </summary>
	public static void LogSessionReleased(int windowIndex)
	{
		System.Diagnostics.Trace.WriteLine($"MediaElement [Android]: DRM session released for window {windowIndex}");
	}
}