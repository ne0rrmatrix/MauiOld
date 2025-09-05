namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// Provides extension methods for working with Digital Rights Management (DRM) functionality.
/// </summary>
/// <remarks>This class contains static methods that extend the functionality of DRM-related types or operations.
/// Use these methods to simplify common tasks or add additional capabilities when working with DRM.</remarks>
public partial class DrmExtensions
{
	/// <summary>
	/// URL to fetch the FairPlay certificate
	/// </summary>
	public string FairPlayCertificateUrl { get; set; } = string.Empty;

	/// <summary>
	/// URL of the FairPlay server
	/// </summary>
	public string FairPlayServerUrl { get; set; } = string.Empty;

	/// <summary>
	/// URL to fetch the PlayReady license server
	/// </summary>
	public string PlayReadyLicenseUrl { get; set; } = string.Empty;
	/// <summary>
	/// URL of the Widevine license server
	/// </summary>
	public string WideVineServerUrl { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the headers to be used for the license request.
	/// </summary>
	public Dictionary<string, string> Headers { get; set; } = [];

	/// <summary>
	/// DRM token for authentication
	/// </summary>
	public string? DrmToken { get; set; } = string.Empty;
}
