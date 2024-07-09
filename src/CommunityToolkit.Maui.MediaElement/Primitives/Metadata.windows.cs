using CommunityToolkit.Maui.Views;
using Windows.Media;
using Windows.Storage;

namespace CommunityToolkit.Maui.Core.Primitives;

class Metadata
{
	readonly IMediaElement? mediaElement;
	readonly SystemMediaTransportControls? systemMediaControls;
	readonly IDispatcher dispatcher;
	/// <summary>
	/// Initializes a new instance of the <see cref="Metadata"/> class.
	/// </summary>
	public Metadata(SystemMediaTransportControls systemMediaTransportControls, IMediaElement MediaElement, IDispatcher Dispatcher)
	{
		mediaElement = MediaElement;
		this.dispatcher = Dispatcher;
		systemMediaControls = systemMediaTransportControls;
		systemMediaControls.ButtonPressed += OnSystemMediaControlsButtonPressed;
	}


	void OnSystemMediaControlsButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
	{
		if (mediaElement is null)
		{
			return;
		}

		if (args.Button == SystemMediaTransportControlsButton.Play)
		{
			if (dispatcher.IsDispatchRequired)
			{
				dispatcher.Dispatch(() => mediaElement.Play());
			}
			else
			{
				mediaElement.Play();
			}
		}
		else if (args.Button == SystemMediaTransportControlsButton.Pause)
		{
			if (dispatcher.IsDispatchRequired)
			{
				dispatcher.Dispatch(() => mediaElement.Pause());
			}
			else
			{
				mediaElement.Pause();
			}
		}
	}

	/// <summary>
	/// Sets the metadata for the given MediaElement.
	/// </summary>
	public async Task SetMetadata(IMediaElement mp)
	{
		if (systemMediaControls is null || mediaElement is null)
		{
			return;
		}
		if (mediaElement.MetadataArtworkUrl is UriMediaSource uri)
		{
			systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(uri.Uri);
		}
		
		if (mediaElement.MetadataArtworkUrl is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				var fileInfo = new FileInfo(filename);
				StorageFile? storageFile = null;
				if(fileInfo.FullName is not null)
				{
					storageFile = await StorageFile.GetFileFromPathAsync(fileInfo.FullName);
				}

				if (storageFile is not null)
				{
					systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(storageFile);
				}
				else
				{
					System.Diagnostics.Trace.TraceError($"FileMediaSource: {fileInfo.FullName} not found");
				}
			}
		}
		else if (mediaElement.MetadataArtworkUrl is ResourceMediaSource resourceMediaSource)
		{
			var filename = resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri($"ms-appx:///{filename}"));
			}
		}
		systemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;
		systemMediaControls.DisplayUpdater.MusicProperties.Artist = mp.MetadataTitle;
		systemMediaControls.DisplayUpdater.MusicProperties.Title = mp.MetadataArtist;
		systemMediaControls.DisplayUpdater.Update();
	}
}