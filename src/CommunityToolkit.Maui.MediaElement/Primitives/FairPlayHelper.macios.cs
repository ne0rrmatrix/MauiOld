using AVFoundation;
using CoreFoundation;
using Foundation;

namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Provides helper methods for configuring FairPlay Streaming (FPS) DRM
/// using the modern <see cref="AVContentKeySession"/> API (iOS 11.2+).
/// </summary>
static class FairPlayHelper
{
	static void Log(string message)
	{
		System.Diagnostics.Trace.WriteLine(message);
		Console.WriteLine(message);
	}

	/// <summary>
	/// Creates an <see cref="AVContentKeySession"/> configured for FairPlay Streaming
	/// and sets up the delegate for SPC/CKC exchange.
	/// </summary>
	/// <param name="licenseServerUri">The FairPlay license server URL (KSM).</param>
	/// <param name="licenseRequestHeaders">Custom HTTP headers for license requests.</param>
	/// <param name="certificateUri">Optional dedicated URL for the FairPlay application certificate. If <see langword="null"/>, the delegate attempts to derive it from the license server URL.</param>
	/// <param name="delegate">When this method returns, contains the created delegate
	/// that should be disposed when playback ends.</param>
	/// <returns>A configured <see cref="AVContentKeySession"/> ready for playback.</returns>
	/// <remarks>
	/// The returned session's <see cref="AVContentKeySession.ContentProtectionSessionIdentifier"/>
	/// must be passed as the <c>AVURLAssetContentProtectionSessionIdentifierKey</c> option
	/// when creating the <see cref="AVUrlAsset"/>. This tells AVFoundation which key session
	/// to use for decrypting FairPlay-protected content.
	/// </remarks>
	public static AVContentKeySession CreateFairPlayKeySession(
		Uri licenseServerUri,
		IDictionary<string, string> licenseRequestHeaders,
		Uri? certificateUri,
		out FairPlayContentKeySessionDelegate @delegate)
	{
		Log($"MediaElement [Apple DRM] CreateFairPlayKeySession — License URL: {licenseServerUri}, Headers count: {licenseRequestHeaders.Count}");

		// Create a temp directory for FairPlay error reports
		var storagePath = Path.Combine(Path.GetTempPath(), "CommunityToolkitMaui", "FairPlayReports");
		Directory.CreateDirectory(storagePath);
		var storageUrl = NSUrl.FromFilename(storagePath);

		Log($"MediaElement [Apple DRM] FairPlay error report storage path: {storagePath}");

		// Create the content key session for FairPlay Streaming
		var session = AVContentKeySession.Create(
			AVContentKeySystem.FairPlayStreaming,
			storageUrl);

		Log($"MediaElement [Apple DRM] AVContentKeySession created — System: {AVContentKeySystem.FairPlayStreaming}, Identifier: {session.ContentProtectionSessionIdentifier}");

		// Create and set the delegate
		@delegate = new FairPlayContentKeySessionDelegate(
			licenseServerUri,
			licenseRequestHeaders,
			certificateUri);

		var delegateQueue = new DispatchQueue(
			"CommunityToolkit.Maui.MediaElement.FairPlayKeySession");

		session.SetDelegate(@delegate, delegateQueue);

		Log("MediaElement [Apple DRM] FairPlay delegate set on dedicated dispatch queue");

		return session;
	}
}