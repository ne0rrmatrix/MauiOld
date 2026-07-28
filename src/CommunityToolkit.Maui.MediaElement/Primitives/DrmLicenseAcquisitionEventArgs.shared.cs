namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Provides data for the <c>MediaElement.DrmLicenseAcquisitionStarting</c> event,
/// allowing consumers to observe or intercept the DRM license acquisition handshake.
/// </summary>
public sealed class DrmLicenseAcquisitionEventArgs : EventArgs
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DrmLicenseAcquisitionEventArgs"/> class.
	/// </summary>
	/// <param name="licenseServerUrl">The license server URI being contacted.</param>
	/// <param name="challenge">The raw challenge data sent to the license server (e.g., PlayReady challenge, Widevine license request, FairPlay SPC).</param>
	internal DrmLicenseAcquisitionEventArgs(Uri licenseServerUrl, byte[] challenge)
	{
		LicenseServerUrl = licenseServerUrl;
		Challenge = challenge;
	}

	/// <summary>
	/// Gets the license server URI that the platform DRM subsystem is contacting.
	/// </summary>
	public Uri LicenseServerUrl { get; }

	/// <summary>
	/// Gets the raw challenge data being sent to the license server.
	/// </summary>
	/// <remarks>
	/// The format of this data depends on the DRM scheme:
	/// <list type="bullet">
	/// <item><description><b>PlayReady:</b> Binary SOAP envelope containing the license challenge</description></item>
	/// <item><description><b>Widevine:</b> Binary license request protobuf</description></item>
	/// <item><description><b>FairPlay:</b> SPC (Server Playback Context) data as <c>NSData</c> bytes</description></item>
	/// </list>
	/// </remarks>
	public byte[] Challenge { get; }

	/// <summary>
	/// Gets or sets a pre-acquired license response to bypass the built-in HTTP license request.
	/// </summary>
	/// <remarks>
	/// <para>Set this to a non-<see langword="null"/> value if you have already acquired
	/// the license response (CKC for FairPlay, license blob for PlayReady/Widevine) through
	/// an out-of-band mechanism. The platform will use this response directly and skip its
	/// own HTTP POST to <see cref="LicenseServerUrl"/>.</para>
	/// <para>Leave as <see langword="null"/> (the default) to let the platform perform
	/// its own HTTP license acquisition using the configured
	/// <see cref="DrmConfiguration.LicenseServerUrl"/> and
	/// <see cref="DrmConfiguration.LicenseRequestHeaders"/>.</para>
	/// </remarks>
	public byte[]? Response { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the built-in license acquisition
	/// should be cancelled.
	/// </summary>
	/// <remarks>
	/// Set to <see langword="true"/> to prevent the platform from contacting the
	/// license server. Use this together with <see cref="Response"/> to supply a
	/// pre-acquired license, or to implement fully custom license acquisition logic
	/// entirely outside the MediaElement pipeline.
	/// </remarks>
	public bool Cancel { get; set; }
}

/// <summary>
/// Describes the state of a DRM license acquisition operation.
/// </summary>
public enum DrmLicenseAcquisitionState
{
	/// <summary>The license acquisition request is in progress.</summary>
	Acquiring = 0,

	/// <summary>The license was successfully acquired and content keys are installed.</summary>
	Completed = 1,

	/// <summary>The license acquisition failed. See error details in the event args.</summary>
	Failed = 2,
}
