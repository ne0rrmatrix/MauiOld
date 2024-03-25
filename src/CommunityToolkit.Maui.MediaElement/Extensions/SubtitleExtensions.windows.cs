
using Windows.Media.Playback;
using Windows.Media.Core;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// The SubtitleExtensions class provides methods to add subtitles to a media element.
/// </summary>
public static class SubtitleExtensions
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SubtitleExtensions"/> class.
	/// </summary>
	/// <param name="mediaSource"></param>
	/// <param name="MediaElement"></param>
	/// <returns></returns>
	public static MediaPlaybackItem SetSubTitles(Windows.Media.Core.MediaSource mediaSource, IMediaElement MediaElement)
	{
		if (MediaElement.SrtFile is UriMediaSource uriMediaSource)
		{
			if (uriMediaSource.Uri is null)
			{
				return new MediaPlaybackItem(mediaSource);
			}
			return CreateSRTTextSource(mediaSource, uriMediaSource.Uri);
		}

		if (MediaElement.SrtFile is ResourceMediaSource resourceMediaSource)
		{
			string path = "ms-appx:///" + resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				return CreateSRTTextSource(mediaSource, new Uri(path));
			}
		}
		else if (MediaElement.SrtFile is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				return CreateSRTTextSource(mediaSource, new Uri(filename));
			}
		}

		var mediaItem = new MediaPlaybackItem(mediaSource);
		return mediaItem;
	}

	static MediaPlaybackItem CreateSRTTextSource(Windows.Media.Core.MediaSource mediaSource, Uri path)
	{
		var timedTextSource = TimedTextSource.CreateFromUri(path);
		mediaSource.ExternalTimedTextSources.Add(timedTextSource);
		MediaPlaybackItem playbackItem = new(mediaSource);

		playbackItem.TimedMetadataTracksChanged += (item, args) =>
		{
			playbackItem.TimedMetadataTracks.SetPresentationMode(0, TimedMetadataTrackPresentationMode.PlatformPresented);
		};
		return playbackItem;
	}
}
