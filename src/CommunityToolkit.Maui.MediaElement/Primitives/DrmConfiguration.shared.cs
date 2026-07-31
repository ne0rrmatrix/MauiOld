namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Configuration for Digital Rights Management (DRM) on a UriMediaSource.
/// Specifies the DRM scheme, license server, request headers, and security level.
/// </summary>
/// <remarks>
/// <para>Assign an instance of this class to UriMediaSource.DrmConfiguration
/// to enable DRM-protected playback on all supported platforms:</para>
/// <list type="bullet">
/// <item><description><b>Windows:</b> PlayReady via WebView2 + dash.js (EME with the PlayReady CDM)</description></item>
/// <item><description><b>Android:</b> Widevine / PlayReady via ExoPlayer <c>MediaItem.DrmConfiguration</c></description></item>
/// <item><description><b>iOS / macOS:</b> FairPlay Streaming via <c>AVAssetResourceLoaderDelegate</c></description></item>
/// </list>
/// </remarks>
public sealed class DrmConfiguration
{
	/// <summary>
	/// Gets or sets the DRM scheme to use.
	/// </summary>
	/// <remarks>
	/// Must be set to a value other than <see cref="DrmScheme.Unknown"/> for DRM to be active.
	/// Platform support:
	/// <list type="bullet">
	/// <item><description><see cref="DrmScheme.PlayReady"/>: Windows, Android</description></item>
	/// <item><description><see cref="DrmScheme.Widevine"/>: Android</description></item>
	/// <item><description><see cref="DrmScheme.FairPlay"/>: iOS, macOS</description></item>
	/// </list>
	/// </remarks>
	public DrmScheme Scheme { get; set; } = DrmScheme.Unknown;

	/// <summary>
	/// Gets or sets the license server URL for acquiring DRM content keys.
	/// </summary>
	/// <remarks>
	/// The platform's DRM subsystem will POST license challenges to this URL.
	/// The server must respond with the appropriate license/key response blob
	/// for the configured <see cref="Scheme"/>.
	/// </remarks>
	public Uri? LicenseServerUrl { get; set; }

	/// <summary>
	/// Gets a collection of custom HTTP headers to include in license acquisition requests.
	/// </summary>
	/// <remarks>
	/// <para>Use this to supply authentication tokens or other required headers for
	/// the license server. Common use cases:</para>
	/// <list type="bullet">
	/// <item><description><c>Authorization: Bearer &lt;token&gt;</c> for token-based auth</description></item>
	/// <item><description><c>X-AxDRM-Message</c> for Axinom DRM</description></item>
	/// <item><description><c>CustomData</c> for additional license request payload</description></item>
	/// </list>
	/// <para>Mutating the dictionary after playback has started will NOT take effect
	/// until the source is re-assigned, which triggers a full platform pipeline rebuild.</para>
	/// </remarks>
	public IDictionary<string, string> LicenseRequestHeaders { get; } = new Dictionary<string, string>();

	/// <summary>
	/// Gets or sets a value indicating whether hardware-based DRM security is required.
	/// </summary>
	/// <remarks>
	/// <para>When <see langword="true"/>, the platform will attempt to use hardware-backed
	/// content protection (e.g., PlayReady SL3000 on Windows with a TEE, Widevine L1 on Android).
	/// When <see langword="false"/> (default), software-based DRM is used.</para>
	/// <para>Hardware DRM requires a Trusted Execution Environment (TEE) on the device.
	/// If hardware DRM is requested but unavailable, playback will fail.</para>
	/// <para>Not all platforms support hardware DRM. On unsupported platforms,
	/// this property is ignored and software DRM is used.</para>
	/// </remarks>
	public bool RequiresHardwareSecurity { get; set; }
}
