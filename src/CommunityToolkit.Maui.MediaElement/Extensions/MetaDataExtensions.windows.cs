using Windows.Media;
using Windows.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// A class that provides methods to update the system UI for media transport controls to display media metadata.
/// </summary>
public class MetaDataExtensions
{
	IMediaElement mediaElement { get; set; }
	readonly SystemMediaTransportControls systemMediaControls;

	/// <summary>
	/// Initializes a new instance of the <see cref="MetaDataExtensions"/> class.
	/// </summary>
	public MetaDataExtensions(SystemMediaTransportControls systemMediaTransportControls, IMediaElement mediaElement)
	{
		this.systemMediaControls = systemMediaTransportControls;
		this.mediaElement = mediaElement;
	}

	/// <summary>
	/// Sets the metadata for the given MediaElement.
	/// </summary>
	public async Task SetMetaData(IMediaElement MediaElement)
	{
		systemMediaControls.DisplayUpdater.ClearAll();
		mediaElement = MediaElement;
		if (mediaElement is null)
		{
			return;
		}

		if (MediaElement.Source is UriMediaSource mediaSource)
		{
			if (mediaSource.Uri is null)
			{
				return;
			}
			ManuallyUpdateMetaData();
			return;
		}

		if (MediaElement.Source is FileMediaSource fileMediaSource)
		{
			if (fileMediaSource.Path is null)
			{
				return;
			}
			var mediaFile = await StorageFile.GetFileFromPathAsync(fileMediaSource.Path);
			await UpdateSystemMediaControlsDisplayAsync(mediaFile);
		}

		if (MediaElement.Source is ResourceMediaSource resourceMediaSource)
		{
			if (resourceMediaSource.Path is null)
			{
				return;
			}
			string path = "ms-appx:///" + resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
				await UpdateSystemMediaControlsDisplayAsync(file);
			}
		}
		systemMediaControls.DisplayUpdater.Update();
	}

	
	/// <summary>
	/// Manually updates the system UI for media transport controls to display media metadata from the given MediaPlaybackType.
	/// </summary>
	void ManuallyUpdateMetaData()
	{
		systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(mediaElement.Artwork ?? string.Empty));
		if (mediaElement.SourceType == Primitives.MediaElementSourceType.Video)
		{
			systemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Video;
			systemMediaControls.DisplayUpdater.VideoProperties.Title = mediaElement.Title;
		}
		else if (mediaElement.SourceType == Primitives.MediaElementSourceType.Audio)
		{
			systemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;
			systemMediaControls.DisplayUpdater.MusicProperties.AlbumTitle = mediaElement.Album;
			systemMediaControls.DisplayUpdater.MusicProperties.Title = mediaElement.Title;
			systemMediaControls.DisplayUpdater.MusicProperties.Artist = mediaElement.Artist;
			systemMediaControls.DisplayUpdater.MusicProperties.AlbumArtist = mediaElement.AlbumArtist;
		}

		systemMediaControls.DisplayUpdater.Update();
	}

	/// <summary>
	/// Updates the system UI for media transport controls to display media metadata from the given StorageFile.
	/// </summary>
	/// <param name="mediaFile">
	/// The media file being loaded.  This method will try to extract media metadata from the file for use in
	/// the system UI for media transport controls.
	/// </param>
	/// <returns></returns>
	async Task UpdateSystemMediaControlsDisplayAsync(StorageFile mediaFile)
	{
		MediaPlaybackType mediaType = GetMediaTypeFromFileContentType(mediaFile);
		bool copyFromFileAsyncSuccessful = false;
		if (MediaPlaybackType.Unknown != mediaType)
		{
			try
			{
				copyFromFileAsyncSuccessful = await systemMediaControls.DisplayUpdater.CopyFromFileAsync(mediaType, mediaFile);
			}
			catch (Exception)

			{
				ManuallyUpdateMetaData();
			}
		}
		else
		{
			ManuallyUpdateMetaData();
		}

		if (!copyFromFileAsyncSuccessful)
		{
			systemMediaControls.DisplayUpdater.ClearAll();
		}
	}

	/// <summary>
	/// Returns an appropriate MediaPlaybackType value based on the given StorageFile's ContentType (MIME type).
	/// </summary>
	/// <param name="file"></param>
	/// <returns>
	/// One of the three valid MediaPlaybackType enum values, or MediaPlaybackType.Unknown if the ContentType 
	/// is not a media type (audio, video, image) or cannot be determined.
	/// </returns>
	/// <remarks>
	/// For use with SystemMediaTransportControlsDisplayUpdater.CopyFromFileAsync() in UpdateSystemMediaControlsDisplayAsync().
	/// </remarks>
	static MediaPlaybackType GetMediaTypeFromFileContentType(StorageFile file)
	{
		MediaPlaybackType mediaPlaybackType = MediaPlaybackType.Unknown;
		string fileMimeType = file.ContentType.ToLowerInvariant();

		if (fileMimeType.StartsWith("audio/"))
		{
			mediaPlaybackType = MediaPlaybackType.Music;
		}
		else if (fileMimeType.StartsWith("video/"))
		{
			mediaPlaybackType = MediaPlaybackType.Video;
		}
		else if (fileMimeType.StartsWith("image/"))
		{
			mediaPlaybackType = MediaPlaybackType.Image;
		}

		return mediaPlaybackType;
	}
}
