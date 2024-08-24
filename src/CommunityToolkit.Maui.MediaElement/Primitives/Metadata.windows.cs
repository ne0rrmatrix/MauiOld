using CommunityToolkit.Maui.Core.Views;
using Windows.Media;

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
		var artwork = MediaManager.ArtworkUrl(mp.MetadataArtworkUrl);
		if (!string.IsNullOrEmpty(artwork))
		{
			var file = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(artwork));
			if(file is not null)
			{
				systemMediaControls.DisplayUpdater.Thumbnail = file;
			}
			if (File.Exists(artwork))
			{
				var temp = await Windows.Storage.StorageFile.GetFileFromPathAsync(artwork);
				systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(temp);
			}
		}
		systemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;
		systemMediaControls.DisplayUpdater.MusicProperties.Artist = mp.MetadataTitle;
		systemMediaControls.DisplayUpdater.MusicProperties.Title = mp.MetadataArtist;
		systemMediaControls.DisplayUpdater.Update();
	}
}