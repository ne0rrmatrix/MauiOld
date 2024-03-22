
using Windows.Media.Playback;
using Windows.Media.Core;
using Image = Microsoft.UI.Xaml.Controls.Image;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// The SubtitleExtensions class provides methods to add subtitles to a media element.
/// </summary>
public static class SubtitleExtensions
{
	/// <summary>
	/// 
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
			return CreateTextSource(mediaSource, uriMediaSource.Uri);
		}

		if (MediaElement.SrtFile is ResourceMediaSource resourceMediaSource)
		{
			string path = "ms-appx:///" + resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				return CreateTextSource(mediaSource, new Uri(path));
			}
		}
		else if (MediaElement.SrtFile is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				return CreateTextSource(mediaSource, new Uri(filename));
			}
		}
		if (MediaElement.IdxFile is not null && MediaElement.SubFile is not null)
		{
			//return CreateSubIDXWithSource(mediaSource, MediaElement);
		}

		var mediaItem = new MediaPlaybackItem(mediaSource);
		return mediaItem;
	}

	static MediaPlaybackItem CreateTextSource(Windows.Media.Core.MediaSource mediaSource, Uri path)
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

	/*
	MediaPlaybackItem CreateSubIDXWithSource(Windows.Media.Core.MediaSource mediaSource, IMediaElement MediaElement)
	{
		if (MediaElement.SubFile is UriMediaSource uriSub && MediaElement.IdxFile is UriMediaSource idxUri)
		{
			Debug.WriteLine("SubFile is UriMediaSource");
			var timedTextSource = TimedTextSource.CreateFromUriWithIndex(uriSub.Uri, idxUri.Uri);
			mediaSource.ExternalTimedTextSources.Add(timedTextSource);
			MediaPlaybackItem playbackItem = new(mediaSource);
			playbackItem.TimedMetadataTracksChanged += Register;

			for (int index = 0; index < playbackItem.TimedMetadataTracks.Count; index++)
			{
				RegisterMetadataHandlerForImageSubtitles(playbackItem, index);
			}
			return playbackItem;
		}
		if (MediaElement.SubFile is ResourceMediaSource resourceSub && MediaElement.IdxFile is ResourceMediaSource resourceIdx)
		{
			Debug.WriteLine($"SubFile is ResourceMediaSource");
			string path = "ms-appx:///" + resourceSub.Path;
			string idxPath = "ms-appx:///" + resourceIdx.Path;
			if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrEmpty(idxPath))
			{
				timedTextSource = TimedTextSource.CreateFromUriWithIndex(new Uri(path), new Uri(idxPath));
				Debug.WriteLine("timedTextSource created");
				mediaSource.ExternalTimedTextSources.Add(timedTextSource);
				Debug.WriteLine("ExternalTimedTextSources added");
				MediaPlaybackItem playbackItem = new(mediaSource);
				Debug.WriteLine("MediaPlaybackItem created");
				playbackItem.TimedMetadataTracksChanged += Register;
				Debug.WriteLine("TimedMetadataTracksChanged registered");
				for (int index = 0; index < playbackItem.TimedMetadataTracks.Count; index++)
				{
					Debug.WriteLine("RegisterMetadataHandlerForImageSubtitles called");
					RegisterMetadataHandlerForImageSubtitles(playbackItem, index);
				}
				return playbackItem;
			}
		}
		if (MediaElement.SubFile is FileMediaSource fileSub && MediaElement.IdxFile is FileMediaSource fileIdx)
		{
			Debug.WriteLine("SubFile is FileMediaSource");
			var filename = fileSub.Path;
			var idxFilename = fileIdx.Path;
			if (!string.IsNullOrWhiteSpace(filename) && !string.IsNullOrEmpty(idxFilename))
			{
				timedTextSource = TimedTextSource.CreateFromUriWithIndex(new Uri(filename), new Uri(idxFilename));
				mediaSource.ExternalTimedTextSources.Add(timedTextSource);
				MediaPlaybackItem playbackItem = new(mediaSource);
				playbackItem.TimedMetadataTracksChanged += Register;
				playbackItem.TimedMetadataTracksChanged += (item, args) =>
				{
					RegisterMetadataHandlerForImageSubtitles(playbackItem, 0);
				};
				for (int index = 0; index < playbackItem.TimedMetadataTracks.Count; index++)
				{
					RegisterMetadataHandlerForImageSubtitles(playbackItem, index);
				}
				return playbackItem;
			}
		}
		return new MediaPlaybackItem(mediaSource);
	}

	void Register(MediaPlaybackItem sender, IVectorChangedEventArgs args)
	{
		Debug.WriteLine("TimedMetadataTracksChanged");
		if (args.CollectionChange == CollectionChange.ItemInserted)
		{
			RegisterMetadataHandlerForImageSubtitles(sender, (int)args.Index);
		}
		else if (args.CollectionChange == CollectionChange.Reset)
		{
			for (int index = 0; index < sender.TimedMetadataTracks.Count; index++)
			{
				if (sender.TimedMetadataTracks[index].TimedMetadataKind == TimedMetadataKind.ImageSubtitle)
				{
					RegisterMetadataHandlerForImageSubtitles(sender, index);
				}
			}
		}
	}

	void RegisterMetadataHandlerForImageSubtitles(MediaPlaybackItem item, int index)
	{
		Debug.WriteLine("RegisterMetadataHandlerForImageSubtitles");
		var timedTrack = item.TimedMetadataTracks[index];
		timedTrack.CueEntered += metadata_ImageSubtitleCueEntered;
		timedTrack.CueExited += metadata_ImageSubtitleCueExited;
		item.TimedMetadataTracks.SetPresentationMode((uint)index, TimedMetadataTrackPresentationMode.PlatformPresented);
		Debug.WriteLine("RegisterMetadataHandlerForImageSubtitles");
	}

	void metadata_ImageSubtitleCueExited(TimedMetadataTrack sender, MediaCueEventArgs args)
	{
		Debug.WriteLine("Cue Exited");
	}

	async void metadata_ImageSubtitleCueEntered(TimedMetadataTrack timedMetadataTrack, MediaCueEventArgs args)
	{
		Debug.WriteLine("Cue Entered");
		// Check in case there are different tracks and the handler was used for more tracks 
		if (timedMetadataTrack.TimedMetadataKind == TimedMetadataKind.ImageSubtitle)
		{
			var cue = args.Cue as ImageCue;
			if (cue != null)
			{
				var source = new SoftwareBitmapSource();
				var SubtitleImage = new Image();
				if (Application.Current is null)
				{
					return;
				}
				if (Application.Current.Dispatcher.IsDispatchRequired)
				{
					await source.SetBitmapAsync(cue.SoftwareBitmap);
					SubtitleImage.Source = source;
					SubtitleImage.Width = cue.Extent.Width;
					SubtitleImage.Height = cue.Extent.Height;
					SubtitleImage.SetValue(Canvas.LeftProperty, cue.Position.X);
					SubtitleImage.SetValue(Canvas.TopProperty, cue.Position.Y);
				}
				await source.SetBitmapAsync(cue.SoftwareBitmap);
				SubtitleImage.Source = source;
				SubtitleImage.Width = cue.Extent.Width;
				SubtitleImage.Height = cue.Extent.Height;
				SubtitleImage.SetValue(Canvas.LeftProperty, cue.Position.X);
				SubtitleImage.SetValue(Canvas.TopProperty, cue.Position.Y);
			}
		}
	}
	*/
}
