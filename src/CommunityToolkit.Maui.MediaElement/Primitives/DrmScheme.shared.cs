namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Identifies the Digital Rights Management (DRM) system to use for protected media content.
/// </summary>
public enum DrmScheme
{
	/// <summary>No DRM / unknown scheme.</summary>
	Unknown = 0,

	/// <summary>Google Widevine (CENC). Used on Android.</summary>
	Widevine = 1,

	/// <summary>Microsoft PlayReady. Used on Windows and Android.</summary>
	PlayReady = 2,

	/// <summary>Apple FairPlay Streaming. Used on iOS and macOS.</summary>
	FairPlay = 3,
}
