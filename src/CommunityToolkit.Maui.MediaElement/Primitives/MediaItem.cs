using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui.Primitives;

/// <summary>
/// A class that represents a media item.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediaItem"/> class.
/// </remarks>
public partial class MediaItem(MediaSource? mediaSource, string mediaTitle, string mediaArtist, string mediaArtworkUrl)
{

	/// <summary>
	/// Gets the media source.
	/// </summary>
	public MediaSource? Source => mediaSource;

	/// <summary>
	/// Gets the media title.
	/// </summary>
	public string MediaTitle => mediaTitle;

	/// <summary>
	/// Gets the media artist.
	/// </summary>
	public string MediaArtist => mediaArtist;

	/// <summary>
	/// Gets the media artwork URL.
	/// </summary>
	public string MediaArtworkUrl => mediaArtworkUrl;
}

