namespace CommunityToolkit.Maui.Core.Primitives;
/// <summary>
/// Android surface type for the <see cref="IMediaElement"/>.
/// </summary>
public enum AndroidSurfaceType
{
	/// <summary>
	/// A <see cref="SurfaceView"/> is used for rendering the media.
	/// </summary>
	SurfaceView,

	/// <summary>
	/// A <see cref="TextureView"/> is used for rendering the media.
	/// </summary>
	TextureView,
}
