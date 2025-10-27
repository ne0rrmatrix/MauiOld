using Windows.Media;

namespace CommunityToolkit.Maui.Core.Primitives;

sealed class Metadata
{
	readonly SystemMediaTransportControls? systemMediaControls;
	/// <summary>
	/// Initializes a new instance of the <see cref="Metadata"/> class.
	/// </summary>
	public Metadata(SystemMediaTransportControls systemMediaTransportControls)
	{
		systemMediaControls = systemMediaTransportControls;
		//systemMediaControls.ButtonPressed += OnSystemMediaControlsButtonPressed;
	}

	/*
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
	*/

	/// <summary>
	/// Sets the metadata for the given MediaElement.
	/// </summary>
	public void SetMetadata(MediaItem? mp)
	{
		if (systemMediaControls is null || mp is null)
		{
			return;
		}

		if (!string.IsNullOrEmpty(mp.MetadataArtworkUrl))
		{
			systemMediaControls.DisplayUpdater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(mp.MetadataArtworkUrl));
		}
		systemMediaControls.DisplayUpdater.Type = MediaPlaybackType.Music;
		systemMediaControls.DisplayUpdater.MusicProperties.Artist = mp.MetadataArtist;
		systemMediaControls.DisplayUpdater.MusicProperties.Title = mp.MetadataTitle;
		systemMediaControls.DisplayUpdater.Update();
	}
}