namespace CommunityToolkit.Maui.Sample.Models;

/// <summary>
/// Represents an Axinom public test vector for DRM playback.
/// See: https://github.com/Axinom/public-test-vectors
/// </summary>
public sealed record DrmTestVector(
	string Name,
	string Category,
	string Description,
	string DashManifestUrl,
	string HlsManifestUrl,
	string LicenseToken);
