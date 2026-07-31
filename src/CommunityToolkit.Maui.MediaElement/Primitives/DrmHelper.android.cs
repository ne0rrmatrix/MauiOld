using AndroidX.Media3.Common;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Provides helper methods and constants for mapping DRM schemes to platform-specific identifiers.
/// </summary>
static class DrmHelper
{
	/// <summary>
	/// Widevine DRM scheme UUID as defined by DASH-IF CENC.
	/// </summary>
	public static readonly Java.Util.UUID WidevineUuid = Java.Util.UUID.FromString("edef8ba9-79d6-4ace-a3c8-27dcd51d21ed")!;

	/// <summary>
	/// PlayReady DRM scheme UUID as defined by DASH-IF CENC.
	/// </summary>
	public static readonly Java.Util.UUID PlayReadyUuid = Java.Util.UUID.FromString("9a04f079-9840-4286-ab92-e65be0885f95")!;

	/// <summary>
	/// Converts a <see cref="DrmScheme"/> to the corresponding CENC UUID
	/// for use with ExoPlayer's <see cref="MediaItem.DrmConfiguration.Builder"/>.
	/// </summary>
	/// <param name="scheme">The cross-platform DRM scheme.</param>
	/// <returns>The CENC scheme UUID, or <see langword="null"/> if the scheme is not supported on Android.</returns>
	public static Java.Util.UUID? ToMedia3Uuid(DrmScheme scheme) => scheme switch
	{
		DrmScheme.Widevine => WidevineUuid,
		DrmScheme.PlayReady => PlayReadyUuid,
		_ => null
	};
}