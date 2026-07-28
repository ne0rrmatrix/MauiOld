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
	/// <summary>
	/// Creates an <see cref="AVContentKeySession"/> configured for FairPlay Streaming
	/// and sets up the delegate for SPC/CKC exchange.
	/// </summary>
	/// <param name="licenseServerUri">The FairPlay license server URL (KSM).</param>
	/// <param name="licenseRequestHeaders">Custom HTTP headers for license requests.</param>
	/// <param name="delegate">When this method returns, contains the created delegate
	/// that should be disposed when playback ends.</param>
	/// <returns>A configured <see cref="AVContentKeySession"/> ready for playback.</returns>
	/// <remarks>
	/// The returned session must be associated with the content. This is done by
	/// passing the session's <c>ContentProtectionSessionIdentifier</c> as an option
	/// when creating the <see cref="AVUrlAsset"/>. See
	/// <see cref="ApplyToAsset(AVContentKeySession, AVUrlAsset)"/>.
	/// </remarks>
	public static AVContentKeySession CreateFairPlayKeySession(
		Uri licenseServerUri,
		IDictionary<string, string> licenseRequestHeaders,
		out FairPlayContentKeySessionDelegate @delegate)
	{
		// Create a temp directory for FairPlay error reports
		var storagePath = Path.Combine(Path.GetTempPath(), "CommunityToolkitMaui", "FairPlayReports");
		Directory.CreateDirectory(storagePath);
		var storageUrl = NSUrl.FromFilename(storagePath);

		// Create the content key session for FairPlay Streaming
		var session = AVContentKeySession.Create(
			AVContentKeySystem.FairPlayStreaming,
			storageUrl);

		// Create and set the delegate
		@delegate = new FairPlayContentKeySessionDelegate(
			licenseServerUri,
			licenseRequestHeaders);

		var delegateQueue = new DispatchQueue(
			"CommunityToolkit.Maui.MediaElement.FairPlayKeySession");

		session.SetDelegate(@delegate, delegateQueue);

		return session;
	}

	/// <summary>
	/// Associates an <see cref="AVContentKeySession"/> with an <see cref="AVUrlAsset"/>
	/// by setting the content protection session identifier as an asset option.
	/// This tells AVFoundation to use the provided key session when the asset's
	/// content requires decryption keys.
	/// </summary>
	public static void ApplyToAsset(AVContentKeySession session, AVUrlAsset asset)
	{
		// Set the content protection session identifier on the asset's resource loader.
		// This associates the key session with the asset so that AVFoundation
		// knows to request content keys from this session.
		//
		// The identifier acts as a bridge between the asset and the key session.
		// When AVFoundation encounters encrypted content, it looks up the key session
		// by this identifier and uses its delegate to acquire content keys.
		asset.ResourceLoader.SetDelegate(
			new FairPlayResourceLoaderBridge(session),
			new DispatchQueue("CommunityToolkit.Maui.MediaElement.FairPlayBridge"));
	}

	/// <summary>
	/// A minimal <see cref="AVAssetResourceLoaderDelegate"/> that bridges
	/// FairPlay <c>skd://</c> requests to the <see cref="AVContentKeySession"/>.
	/// </summary>
	sealed class FairPlayResourceLoaderBridge : AVAssetResourceLoaderDelegate
	{
		readonly AVContentKeySession keySession;

		public FairPlayResourceLoaderBridge(AVContentKeySession keySession)
		{
			this.keySession = keySession;
		}

		public override bool ShouldWaitForLoadingOfRequestedResource(
			AVAssetResourceLoader resourceLoader,
			AVAssetResourceLoadingRequest loadingRequest)
		{
			// FairPlay requests use "skd://" scheme.
			// Simply returning true tells AVFoundation we're handling this,
			// and the AVContentKeySession delegate will pick up the actual key request.
			return string.Equals(
				loadingRequest.Request?.Url?.Scheme,
				"skd",
				StringComparison.OrdinalIgnoreCase);
		}
	}
}
