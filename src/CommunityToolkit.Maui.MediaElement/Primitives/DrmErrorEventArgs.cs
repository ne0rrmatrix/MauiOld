namespace CommunityToolkit.Maui.Primitives;
/// <summary>
/// Provides data for events that report errors related to Digital Rights Management (DRM).
/// </summary>
/// <remarks>This class contains information about a DRM error, including a descriptive error message and an
/// optional exception that provides additional details about the error.</remarks>
public class DrmErrorEventArgs : EventArgs
{
	/// <summary>
	/// Gets or sets the error message associated with the current operation.
	/// </summary>
	public string? Error { get; set; }

	/// <summary>
	/// Gets or sets the exception associated with the current operation.
	/// </summary>
	public Exception? Exception { get; set; }
	
	/// <summary>
	/// Gets or sets the error code associated with the DRM operation.
	/// </summary>
	/// <remarks>
	/// This can be used to identify specific error types for handling different error scenarios.
	/// Common values might include: CERTIFICATE_ERROR, LICENSE_ERROR, PLAYBACK_ERROR.
	/// </remarks>
	public string? ErrorCode { get; set; }
}
