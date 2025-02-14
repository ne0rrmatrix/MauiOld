using CommunityToolkit.Maui.Primitives;
using Windows.Media;

namespace CommunityToolkit.Maui.Core.Primitives;

sealed class Metadata
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

	~Metadata()
	{
		if (systemMediaControls is not null)
		{
			systemMediaControls.ButtonPressed -= OnSystemMediaControlsButtonPressed;
		}
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
	public void SetMetadata(MediaItem mediaItem)
	{
		if (systemMediaControls is null || mediaElement is null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(mediaItem.MediaArtworkUrl))
		{
			systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(mediaItem.MediaArtworkUrl ?? string.Empty));
		}
		systemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;
		systemMediaControls.DisplayUpdater.MusicProperties.Artist = mediaItem.MediaArtist;
		systemMediaControls.DisplayUpdater.MusicProperties.Title = mediaItem.MediaTitle;
		systemMediaControls.DisplayUpdater.Update();
	}
}