using Windows.Media;
using Windows.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// A class that provides methods to update the system UI for media transport controls to display media metadata.
/// </summary>
public partial class MetaDataExtensions
{
	/// <summary>
	/// The media player to which the metadata will be applied.
	/// </summary>
	protected IMediaElement? mediaElement { get; set; }

	/// <summary>
	/// The system media transport controls for the current app.
	/// </summary>
	public SystemMediaTransportControls? SystemMediaControls { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MetaDataExtensions"/> class.
	/// </summary>
	public MetaDataExtensions(SystemMediaTransportControls systemMediaTransportControls, IMediaElement MediaElement)
	{
		mediaElement = MediaElement;
		SystemMediaControls = systemMediaTransportControls;
		this.SystemMediaControls.ButtonPressed += SystemMediaControls_ButtonPressed;
	}


	void SystemMediaControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
	{
		if (mediaElement is null)
		{
			return;
		}

		if (args.Button == SystemMediaTransportControlsButton.Play)
		{
			MainThread.InvokeOnMainThreadAsync(() => mediaElement.Play());
		}
		else if (args.Button == SystemMediaTransportControlsButton.Pause)
		{
			MainThread.InvokeOnMainThreadAsync(() => mediaElement.Pause());
		}
	}

	/// <summary>
	/// Sets the metadata for the given MediaElement.
	/// </summary>
	public async Task SetMetaData(IMediaElement mp)
	{
		if (SystemMediaControls is null)
		{
			return;
		}

		mediaElement = mp;
		SystemMediaControls.DisplayUpdater.ClearAll();
		if (mp is null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(mp.Title))
		{
			ManuallyUpdateMetaData(mp);
			return;
		}
		if (mp.Source is UriMediaSource uriMediaSource)
		{
			if (uriMediaSource.Uri is null)
			{
				return;
			}
		  var file = await StorageFile.CreateStreamedFileFromUriAsync("video.mp4", uriMediaSource.Uri, null);
			await UpdateSystemMediaControlsDisplayAsync(file);
		}

		if (mp.Source is FileMediaSource fileMediaSource)
		{
			if (fileMediaSource.Path is null)
			{
				return;
			}
			var mediaFile = await StorageFile.GetFileFromPathAsync(fileMediaSource.Path);
			await UpdateSystemMediaControlsDisplayAsync(mediaFile);
		}

		if (mp.Source is ResourceMediaSource resourceMediaSource)
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
	}

	/// <summary>
	/// Manually updates the system UI for media transport controls to display media metadata from the given MediaPlaybackType.
	/// </summary>
	void ManuallyUpdateMetaData(IMediaElement mp)
	{
		if (SystemMediaControls is null || mediaElement is null)
		{
			return;
		}
		if (mp.SourceType == Primitives.MediaElementSourceType.Unknown)
		{
			return;
		}

		if (!string.IsNullOrEmpty(mp.Artwork))
		{
			SystemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(mp.Artwork ?? string.Empty));
		}
		if (mp.SourceType == Primitives.MediaElementSourceType.Video)
		{
			SystemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Video;
			SystemMediaControls.DisplayUpdater.VideoProperties.Title = mp.Title;
		}
		else if (mp.SourceType == Primitives.MediaElementSourceType.Audio)
		{
			SystemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;
			SystemMediaControls.DisplayUpdater.MusicProperties.AlbumTitle = mp.Album;
			SystemMediaControls.DisplayUpdater.MusicProperties.Title = mp.Title;
			SystemMediaControls.DisplayUpdater.MusicProperties.Artist = mp.Artist;
			SystemMediaControls.DisplayUpdater.MusicProperties.AlbumArtist = mp.AlbumArtist;
		}
		SystemMediaControls.DisplayUpdater.Update();
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
		if (SystemMediaControls is null)
		{
			return;
		}
		MediaPlaybackType mediaType = GetMediaTypeFromFileContentType(mediaFile);
		if (MediaPlaybackType.Unknown == mediaType)
		{
			return;
		}
		try
		{
			await SystemMediaControls.DisplayUpdater.CopyFromFileAsync(mediaType, mediaFile);
			SystemMediaControls.DisplayUpdater.Update();
		}
		catch
		{
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
		if (fileMimeType.EndsWith("mp4") || fileMimeType.EndsWith("m4a") || fileMimeType.EndsWith("m4p") || fileMimeType.EndsWith("m4v") || fileMimeType.EndsWith("mov"))
		{
			mediaPlaybackType = MediaPlaybackType.Video;
		}
		else if (fileMimeType.EndsWith("mp3") || fileMimeType.EndsWith("m4a") || fileMimeType.EndsWith("wma") || fileMimeType.EndsWith("wav") || fileMimeType.EndsWith("flac"))
		{
			mediaPlaybackType = MediaPlaybackType.Music;
		}
		else if (fileMimeType.EndsWith("jpg") || fileMimeType.EndsWith("jpeg") || fileMimeType.EndsWith("png") || fileMimeType.EndsWith("gif") || fileMimeType.EndsWith("bmp"))
		{
			mediaPlaybackType = MediaPlaybackType.Image;
		}
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