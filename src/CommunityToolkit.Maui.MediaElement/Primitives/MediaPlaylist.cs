namespace CommunityToolkit.Maui.Primitives;

/// <summary>
/// A class that represents a media playlist.
/// </summary>
public partial class MediaPlaylist
{
	/// <summary>
	/// Gets the list of media sources.
	/// </summary>
	public List<MediaItem> MediaItem => mediaItem;

	readonly List<MediaItem> mediaItem;
	/// <summary>
	/// Initializes a new instance of the <see cref="MediaPlaylist"/> class.
	/// </summary>
	public MediaPlaylist()
	{
		mediaItem = [];
	}

	/// <summary>
	/// Adds a media item to the playlist.
	/// </summary>
	/// <param name="mediaItem"></param>
	public void AddMediaItem(MediaItem mediaItem)
	{
		this.mediaItem.Add(mediaItem);
	}

	/// <summary>
	/// Removes a media item from the playlist.
	/// </summary>
	/// <param name="mediaItem"></param>
	public void RemoveMediaItem(MediaItem mediaItem)
	{
		this.mediaItem.Remove(mediaItem);
	}
}
