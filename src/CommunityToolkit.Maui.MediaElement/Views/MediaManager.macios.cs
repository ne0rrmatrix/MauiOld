using System.Runtime.InteropServices;
using AVFoundation;
using AVKit;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MediaPlayer;
using Microsoft.Extensions.Logging;
using UIKit;

namespace CommunityToolkit.Maui.Core.Views;

public partial class MediaManager : IDisposable
{
	// P/Invoke to call native Objective-C methods
	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
	static extern void objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1);

	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "sel_registerName")]
	static extern IntPtr sel_registerName(string name);

	AVContentKeySession? contentKeySession;
    FairPlayContentKeySessionDelegate? contentKeyDelegate;
    Metadata? metaData;

    string? certificateUrl;
    string? licenseUrl;
    string? licenseToken;

    // Media would still start playing when Speed was set although ShouldAutoPlay=False
    // This field was added to overcome that.
    bool isInitialSpeedSet;

    /// <summary>
    /// Creates the corresponding platform view of <see cref="MediaElement"/> on iOS and macOS.
    /// </summary>
    /// <returns>The platform native counterpart of <see cref="MediaElement"/>.</returns>
    public (PlatformMediaElement Player, AVPlayerViewController PlayerViewController) CreatePlatformView()
    {
        Player = new();
        PlayerViewController = new()
        {
            Player = Player
        };

        // Pre-initialize Volume and Muted properties to the player object
        Player.Muted = MediaElement.ShouldMute;
        var volumeDiff = Math.Abs(Player.Volume - MediaElement.Volume);
        if (volumeDiff > 0.01)
        {
            Player.Volume = (float)MediaElement.Volume;
        }

        UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();

#if IOS
        PlayerViewController.UpdatesNowPlayingInfoCenter = false;
#else
        PlayerViewController.UpdatesNowPlayingInfoCenter = true;
#endif
        var avSession = AVAudioSession.SharedInstance();
        avSession.SetCategory(AVAudioSessionCategory.Playback);
        avSession.SetActive(true);

        AddStatusObservers();
        AddPlayedToEndObserver();
        AddErrorObservers();
		
        return (Player, PlayerViewController);
    }

    /// <summary>
    /// Releases the managed and unmanaged resources used by the <see cref="MediaManager"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// The default <see cref="NSKeyValueObservingOptions"/> flags used in the iOS and macOS observers.
    /// </summary>
    protected NSKeyValueObservingOptions ValueObserverOptions => NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.New;

    /// <summary>
    /// Observer that tracks when an error has occurred in the playback of the current item.
    /// </summary>
    protected IDisposable? CurrentItemErrorObserver { get; set; }

    /// <summary>
    /// Observer that tracks when an error has occurred with media playback.
    /// </summary>
    protected NSObject? ErrorObserver { get; set; }

    /// <summary>
    /// Observer that tracks when the media has failed to play to the end.
    /// </summary>
    protected NSObject? ItemFailedToPlayToEndTimeObserver { get; set; }

    /// <summary>
    /// Observer that tracks when the playback of media has stalled.
    /// </summary>
    protected NSObject? PlaybackStalledObserver { get; set; }

    /// <summary>
    /// Observer that tracks when the media has played to the end.
    /// </summary>
    protected NSObject? PlayedToEndObserver { get; set; }

    /// <summary>
    /// The current media playback item.
    /// </summary>
    protected AVPlayerItem? PlayerItem { get; set; }

    /// <summary>
    /// The <see cref="AVPlayerViewController"/> that hosts the media Player.
    /// </summary>
    protected AVPlayerViewController? PlayerViewController { get; set; }

    /// <summary>
    /// Observer that tracks the playback rate of the media.
    /// </summary>
    protected IDisposable? RateObserver { get; set; }

    /// <summary>
    /// Observer that tracks the status of the media.
    /// </summary>
    protected IDisposable? StatusObserver { get; set; }

    /// <summary>
    /// Observer that tracks the time control status of the media.
    /// </summary>
    protected IDisposable? TimeControlStatusObserver { get; set; }

    /// <summary>
    /// Observer that tracks the volume of the media playback.
    /// </summary>
    protected IDisposable? VolumeObserver { get; set; }

    /// <summary>
    /// Observer that tracks if the audio is muted.
    /// </summary>
    protected IDisposable? MutedObserver { get; set; }

    protected virtual partial void PlatformPlay()
    {
        if (Player?.CurrentTime == PlayerItem?.Duration)
        {
            return;
        }

        Player?.Play();
    }

    protected virtual partial void PlatformPause()
    {
        Player?.Pause();
    }

    protected virtual async partial Task PlatformSeek(TimeSpan position, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var seekTaskCompletionSource = new TaskCompletionSource();

        if (Player?.CurrentItem is null)
        {
            throw new InvalidOperationException($"{nameof(AVPlayer)}.{nameof(AVPlayer.CurrentItem)} is not yet initialized");
        }

        if (Player.Status is not AVPlayerStatus.ReadyToPlay)
        {
            throw new InvalidOperationException($"{nameof(AVPlayer)}.{nameof(AVPlayer.Status)} must first be set to {AVPlayerStatus.ReadyToPlay}");
        }

        var ranges = Player.CurrentItem.SeekableTimeRanges;
        var seekToTime = new CMTime(Convert.ToInt64(position.TotalMilliseconds), 1000);
        foreach (var range in ranges.Select(r => r.CMTimeRangeValue))
        {
            if (seekToTime >= range.Start && seekToTime < (range.Start + range.Duration))
            {
                Player.Seek(seekToTime, complete =>
                {
                    if (!complete)
                    {
                        throw new InvalidOperationException("Seek Failed");
                    }

                    seekTaskCompletionSource.SetResult();
                });
                break;
            }
        }

        await seekTaskCompletionSource.Task.WaitAsync(token);

        MediaElement.SeekCompleted();
    }

    protected virtual partial void PlatformStop()
    {
        // There's no Stop method so pause the video and reset its position
        Player?.Seek(CMTime.Zero);
        Player?.Pause();

        MediaElement.CurrentStateChanged(MediaElementState.Stopped);
    }

    protected virtual partial void PlatformUpdateAspect()
    {
        if (PlayerViewController is null)
        {
            return;
        }

        PlayerViewController.VideoGravity = MediaElement.Aspect switch
        {
            Aspect.Fill => AVLayerVideoGravity.Resize,
            Aspect.AspectFill => AVLayerVideoGravity.ResizeAspectFill,
            _ => AVLayerVideoGravity.ResizeAspect,
        };
    }

    protected virtual partial ValueTask PlatformUpdateSource()
    {
        MediaElement.CurrentStateChanged(MediaElementState.Opening);

        AVAsset? asset = null;
        if (Player is null)
        {
			return ValueTask.CompletedTask;
        }

        metaData ??= new(Player);
        Metadata.ClearNowPlaying();
        PlayerViewController?.ContentOverlayView?.Subviews.FirstOrDefault()?.RemoveFromSuperview();

        // Initialize DRM settings
        DrmExtensions? drmExtensions = MediaElement.Drmextensions;
        licenseToken = drmExtensions.DrmToken ?? string.Empty;
        certificateUrl = drmExtensions.FairPlayCertificateUrl ?? string.Empty;
        licenseUrl = drmExtensions.FairPlayServerUrl ?? string.Empty;

        SetupFairPlayDRM(certificateUrl, licenseUrl, licenseToken);

        if (MediaElement.Source is UriMediaSource uriMediaSource)
        {
            var uri = uriMediaSource.Uri;
            if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
            {
				asset = LoadAsset(uri.AbsoluteUri) ?? AVAsset.FromUrl(new NSUrl(uri.AbsoluteUri));
			}
        }
        else if (MediaElement.Source is FileMediaSource fileMediaSource)
        {
            var uri = fileMediaSource.Path;
            
			if (!string.IsNullOrWhiteSpace(uri))
            {
				asset = AVAsset.FromUrl(NSUrl.CreateFileUrl(uri));
			}
        }
        else if (MediaElement.Source is ResourceMediaSource resourceMediaSource)
        {
            var path = resourceMediaSource.Path;

			if (!string.IsNullOrWhiteSpace(path) && Path.HasExtension(path))
			{
				string directory = Path.GetDirectoryName(path) ?? "";
				string filename = Path.GetFileNameWithoutExtension(path);
				string extension = Path.GetExtension(path)[1..];
				var url = NSBundle.MainBundle.GetUrlForResource(filename,
					extension, directory);

				asset = AVAsset.FromUrl(url);
			}
			else
			{
				Logger.LogWarning("Invalid file path for ResourceMediaSource.");
			}
        }

        PlayerItem = asset is not null
            ? new AVPlayerItem(asset)
            : null;
       
        metaData.SetMetadata(PlayerItem, MediaElement);
        CurrentItemErrorObserver?.Dispose();

        Player.ReplaceCurrentItemWithPlayerItem(PlayerItem);

        CurrentItemErrorObserver = PlayerItem?.AddObserver("error",
			ValueObserverOptions, (NSObservedChange change) =>
            {
                if (Player.CurrentItem?.Error is null)
                {
                    return;
                }

                var message = $"{Player.CurrentItem?.Error?.LocalizedDescription} - " +
                                $"{Player.CurrentItem?.Error?.LocalizedFailureReason}";

                MediaElement.MediaFailed(
                    new MediaFailedEventArgs(message));

                Logger.LogError("{LogMessage}", message);
            });

        if (PlayerItem is not null && PlayerItem.Error is null)
        {
            MediaElement.MediaOpened();

            (MediaElement.MediaWidth, MediaElement.MediaHeight) = GetVideoDimensions(PlayerItem);

            // Extract and notify about available tracks
            ExtractMediaTracks(asset);

            if (MediaElement.ShouldAutoPlay)
            {
                Player.Play();
            }

            SetPoster();
        }
        else if (PlayerItem is null)
        {
            MediaElement.MediaWidth = MediaElement.MediaHeight = 0;

            MediaElement.CurrentStateChanged(MediaElementState.None);
        }

		return ValueTask.CompletedTask;
	}

    void SetPoster()
    {
        if (PlayerItem is null || metaData is null)
        {
            return;
        }

        var videoTrack = PlayerItem.Asset.TracksWithMediaType(AVMediaTypes.Video.GetConstant()).FirstOrDefault();
        if (videoTrack is not null)
        {
            return;
        }

        if (PlayerItem.Asset.Tracks.Length == 0)
        {
            // No video track found and no tracks found. This is likely an audio file. So we can't set a poster.
            return;
        }

        if (PlayerViewController?.View is not null && PlayerViewController.ContentOverlayView is not null && !string.IsNullOrEmpty(MediaElement.MetadataArtworkUrl))
        {
            var image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(MediaElement.MetadataArtworkUrl))) ?? new UIImage();
            var imageView = new UIImageView(image)
            {
                ContentMode = UIViewContentMode.ScaleAspectFit,
                TranslatesAutoresizingMaskIntoConstraints = false,
                ClipsToBounds = true,
                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions
            };

            PlayerViewController.ContentOverlayView.AddSubview(imageView);
            NSLayoutConstraint.ActivateConstraints(
            [
                imageView.CenterXAnchor.ConstraintEqualTo(PlayerViewController.ContentOverlayView.CenterXAnchor),
                            imageView.CenterYAnchor.ConstraintEqualTo(PlayerViewController.ContentOverlayView.CenterYAnchor),
                            imageView.WidthAnchor.ConstraintLessThanOrEqualTo(PlayerViewController.ContentOverlayView.WidthAnchor),
                            imageView.HeightAnchor.ConstraintLessThanOrEqualTo(PlayerViewController.ContentOverlayView.HeightAnchor),

                            // Maintain the aspect ratio
                            imageView.WidthAnchor.ConstraintEqualTo(imageView.HeightAnchor, image.Size.Width / image.Size.Height)
            ]);
        }
    }

    protected virtual partial void PlatformUpdateSpeed()
    {
        if (PlayerViewController?.Player is null)
        {
            return;
        }

        // First time we're getting a playback speed and should NOT auto play, do nothing.
        if (!isInitialSpeedSet && !MediaElement.ShouldAutoPlay)
        {
            isInitialSpeedSet = true;
            return;
        }

        PlayerViewController.Player.Rate = (float)MediaElement.Speed;
    }

    protected virtual partial void PlatformUpdateShouldShowPlaybackControls()
    {
        if (PlayerViewController is null)
        {
            return;
        }

		PlayerViewController.ShowsPlaybackControls =
			MediaElement.ShouldShowPlaybackControls;
    }

    protected virtual partial void PlatformUpdatePosition()
    {
        if (Player is null)
        {
            return;
        }

        if (PlayerItem is not null)
        {
            if (PlayerItem.Duration == CMTime.Indefinite)
            {
                var range = PlayerItem.SeekableTimeRanges?.LastOrDefault();

                if (range?.CMTimeRangeValue is not null)
                {
                    MediaElement.Duration = ConvertTime(range.CMTimeRangeValue.Duration);
                    MediaElement.Position = ConvertTime(PlayerItem.CurrentTime);
                }
            }
            else
            {
                MediaElement.Duration = ConvertTime(PlayerItem.Duration);
                MediaElement.Position = ConvertTime(PlayerItem.CurrentTime);
            }
        }
        else
        {
            Player.Pause();
            MediaElement.Duration = MediaElement.Position = TimeSpan.Zero;
        }
    }

    protected virtual partial void PlatformUpdateVolume()
    {
        if (Player is null)
        {
            return;
        }

        var volumeDiff = Math.Abs(Player.Volume - MediaElement.Volume);
        if (volumeDiff > 0.01)
        {
            Player.Volume = (float)MediaElement.Volume;
        }
    }


    protected virtual partial void PlatformUpdateShouldKeepScreenOn()
    {
        if (Player is null)
        {
            return;
        }

        UIApplication.SharedApplication.IdleTimerDisabled = MediaElement.ShouldKeepScreenOn;
    }


    protected virtual partial void PlatformUpdateShouldMute()
    {
        if (Player is null)
        {
            return;
        }

        Player.Muted = MediaElement.ShouldMute;
    }

    protected virtual partial void PlatformUpdateShouldLoopPlayback()
    {
        // no-op we loop through using the PlayedToEndObserver
    }

    protected virtual partial void PlatformSelectTrack(string trackId)
    {
        if (Player?.CurrentItem?.Asset is null)
		{
			return;
		}

		try
        {
			if (MediaElement.Handler?.VirtualView is not MediaElement mediaElement)
			{
				System.Diagnostics.Debug.WriteLine("MediaManager: PlatformSelectTrack - MediaElement is not available.");
				return;
			}
            // Get the track from the MediaElement's track collection
				var track = mediaElement.AvailableTracks.FirstOrDefault(t => t.Id == trackId);
            if (track is null)
			{
				return;
			}

			switch (track.Type)
            {
                case MediaTrackType.Audio:
                    SelectAudioTrack(track);
                    break;
                case MediaTrackType.Text:
                    SelectSubtitleTrack(track);
                    break;
                case MediaTrackType.Video:
                    // Video track selection is not directly supported in AVFoundation
                    // For HLS, this would typically control bitrate selection
                    SelectVideoTrack(track);
                    break;
            }
			
            // Notify that track selection changed
            mediaElement.OnTrackSelectionChanged(track);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error selecting track");
        }
    }

	void SetupFairPlayDRM(string certificateUrl, string licenseUrl, string? licenseToken)
	{
		if (string.IsNullOrWhiteSpace(certificateUrl) || string.IsNullOrWhiteSpace(licenseUrl) || string.IsNullOrWhiteSpace(licenseToken))
		{
			// Don't throw an exception, just log a warning
			Logger.LogWarning("FairPlay DRM not configured - missing certificate URL, license URL, or token");
			return;
		}

		try
		{
			// Initialize the content key session delegate
			contentKeyDelegate = new FairPlayContentKeySessionDelegate(certificateUrl, licenseUrl, licenseToken);

			// Create the content key session with the correct string parameter (modern approach)
			contentKeySession = AVContentKeySession.Create("FairPlayStreaming");
			contentKeySession.SetDelegate(contentKeyDelegate, CoreFoundation.DispatchQueue.MainQueue);

			Console.WriteLine("FairPlay content key session created and configured");
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Failed to set up FairPlay DRM");
		}
	}

	/// <summary>
	/// Selects an audio track
	/// </summary>
	void SelectAudioTrack(MediaTrack track)
    {
        var asset = Player?.CurrentItem?.Asset;
        if (asset is null)
		{
			return;
		}

		// Check if this is an HLS audio option
		if (track.Metadata is not null && 
            track.Metadata.TryGetValue("AVMediaSelectionOption", out var optionObj) && 
            optionObj is AVMediaSelectionOption option)
        {
            // This is an HLS audio option
            var audioGroup = asset.MediaSelectionGroupForMediaCharacteristic(AVMediaCharacteristics.Audible.GetConstant());
            if (audioGroup is not null && Player is not null)
            {
                Player.CurrentItem?.SelectMediaOption(option, audioGroup);
                return;
            }
        }

        // Try to select by track ID for non-HLS content
        if (int.TryParse(track.Id, out var trackId))
        {
            // For non-HLS content, we would need to recreate the player item with selected tracks
            // This is a simplified approach and might not work for all scenarios
            var audioTracks = asset.TracksWithMediaType(AVMediaTypes.Audio.GetConstant());
            var selectedTrack = audioTracks?.FirstOrDefault(t => t.TrackID == trackId);
            
            if (selectedTrack is not null)
            {
                // This approach is not ideal as it requires recreating the player item
                // A better approach would be to use AVPlayerItem.SelectMediaOption for HLS content
                Logger.LogWarning("Audio track selection for non-HLS content is not fully supported");
            }
        }
    }

    /// <summary>
    /// Selects a subtitle track
    /// </summary>
    void SelectSubtitleTrack(MediaTrack track)
    {
        var asset = Player?.CurrentItem?.Asset;
        if (asset is null)
		{
			return;
		}

		if (track.Id == "subtitle-none" || track.Name == "Off")
        {
            var subtitleGroup = asset.MediaSelectionGroupForMediaCharacteristic(AVMediaCharacteristics.Legible.GetConstant());
            if (subtitleGroup is not null)
            {
                Player?.CurrentItem?.SelectMediaOption(null, subtitleGroup);
            }
            return;
        }

        if (track.Metadata is not null && 
            track.Metadata.TryGetValue("AVMediaSelectionOption", out var optionObj) && 
            optionObj is AVMediaSelectionOption option)
        {
            var subtitleGroup = asset.MediaSelectionGroupForMediaCharacteristic(AVMediaCharacteristics.Legible.GetConstant());
            if (subtitleGroup is not null)
            {
                Player?.CurrentItem?.SelectMediaOption(option, subtitleGroup);
                return;
            }
        }

        // Try to select by track ID for non-HLS content
        if (int.TryParse(track.Id, out var trackId))
        {
            // For non-HLS content with embedded subtitles
            var subtitleTracks = asset.TracksWithMediaType(AVMediaTypes.Subtitle.GetConstant());
            var selectedTrack = subtitleTracks?.FirstOrDefault(t => t.TrackID == trackId);
            
            if (selectedTrack is not null)
            {
                // This approach is not ideal as it requires recreating the player item
                Logger.LogWarning("Subtitle track selection for non-HLS content is not fully supported");
            }
        }
    }

    /// <summary>
    /// Selects a video track (typically for bitrate selection in HLS)
    /// </summary>
    void SelectVideoTrack(MediaTrack track)
    {
        // For HLS content, video track selection is typically handled through bitrate preferences
        if (track.Bitrate > 0 && Player?.CurrentItem is not null)
        {
            // Set preferred maximum bitrate
            Player.CurrentItem.PreferredPeakBitRate = track.Bitrate;
        }
    }


	/// <summary>
	/// Extracts available audio, video, and subtitle tracks from the asset
	/// </summary>
	void ExtractMediaTracks(AVAsset? asset)
	{
		if (asset is null)
		{
			return;
		}

		var tracks = new List<MediaTrack>();

		// Extract audio tracks
		var audioTracks = asset.TracksWithMediaType(AVMediaTypes.Audio.GetConstant());
		if (audioTracks is not null)
		{
			foreach (var track in audioTracks)
			{
				var audioTrack = new MediaTrack
				{
					Id = track.TrackID.ToString(),
					Type = MediaTrackType.Audio,
					Name = GetTrackDisplayName(track),
					Language = GetTrackLanguage(track),
					IsDefault = IsDefaultTrack(track),
					Codec = GetTrackCodec(track),
					Bitrate = (int)GetTrackBitrate(track)
				};
				tracks.Add(audioTrack);
			}
		}

		// Extract video tracks (for multi-bitrate streams)
		var videoTracks = asset.TracksWithMediaType(AVMediaTypes.Video.GetConstant());
		if (videoTracks is not null)
		{
			foreach (var track in videoTracks)
			{
				var size = track.NaturalSize;
				var videoTrack = new MediaTrack
				{
					Id = track.TrackID.ToString(),
					Type = MediaTrackType.Video,
					Name = GetTrackDisplayName(track),
					Language = GetTrackLanguage(track),
					IsDefault = IsDefaultTrack(track),
					Codec = GetTrackCodec(track),
					Bitrate = (int)GetTrackBitrate(track),
					Width = (int)size.Width,
					Height = (int)size.Height,
					FrameRate = GetTrackFrameRate(track)
				};
				tracks.Add(videoTrack);
			}
		}

		// Extract subtitle tracks
		var subtitleTracks = asset.TracksWithMediaType(AVMediaTypes.Subtitle.GetConstant());
		if (subtitleTracks is not null)
		{
			foreach (var track in subtitleTracks)
			{
				var subtitleTrack = new MediaTrack
				{
					Id = track.TrackID.ToString(),
					Type = MediaTrackType.Text,
					Name = GetTrackDisplayName(track),
					Language = GetTrackLanguage(track),
					IsDefault = IsDefaultTrack(track)
				};
				tracks.Add(subtitleTrack);
			}
		}

		// For HLS streams, also check for variant streams and text tracks
		if (asset is AVUrlAsset urlAsset)
		{
			// For HLS, we need to extract variant streams from the master playlist
			// This is a simplified approach - in a real implementation, you would parse the HLS manifest
			ExtractHlsVariantTracks(urlAsset, tracks);
		}

		// Mark the first track of each type as selected
		var audioSelected = false;
		var videoSelected = false;
		var textSelected = false;

		foreach (var track in tracks)
		{
			if (track.Type == MediaTrackType.Audio && !audioSelected)
			{
				track.IsSelected = true;
				audioSelected = true;
			}
			else if (track.Type == MediaTrackType.Video && !videoSelected)
			{
				track.IsSelected = true;
				videoSelected = true;
			}
			else if (track.Type == MediaTrackType.Text && !textSelected && track.IsDefault)
			{
				track.IsSelected = true;
				textSelected = true;
			}
		}

		if (MediaElement.Handler?.VirtualView is MediaElement mediaElement)
		{
			System.Diagnostics.Debug.WriteLine($"MediaManager: Extracted {tracks.Count} tracks from the asset.");
			mediaElement.OnTracksChanged(tracks);
		}
		else
		{
			System.Diagnostics.Debug.WriteLine("MediaManager: No valid MediaElement found.");
		}
	}

	/// <summary>
	/// Extracts HLS variant streams from the master playlist
	/// </summary>
	void ExtractHlsVariantTracks(AVUrlAsset urlAsset, List<MediaTrack> tracks)
	{
		try
		{
			// For HLS streams, we can try to get variant information
			// This is a simplified approach - in a real implementation, you would parse the HLS manifest

			// Check if this is an HLS stream
			var url = urlAsset.Url?.AbsoluteString;
			if (url is null || (!url.Contains(".m3u8") && !url.Contains("playlist")))
			{
				return;
			}

			// For HLS streams with multiple audio tracks, they might be available as media option groups
			var audioGroup = urlAsset.MediaSelectionGroupForMediaCharacteristic(AVMediaCharacteristics.Audible.GetConstant());
			if (audioGroup is not null)
			{
				foreach (var option in audioGroup.Options)
				{
					var locale = option.Locale;
					var language = locale?.LanguageCode ?? "unknown";
					var displayName = option.DisplayName ?? language;

					var audioTrack = new MediaTrack
					{
						Id = $"audio-option-{tracks.Count(t => t.Type == MediaTrackType.Audio)}",
						Type = MediaTrackType.Audio,
						Name = displayName,
						Language = language,
						IsDefault = option.HasMediaCharacteristic("AVMediaCharacteristicDefault"),
						Metadata = new Dictionary<string, object>
						{
							{ "AVMediaSelectionOption", option }
						}
					};

					// Check if we already have this track (avoid duplicates)
					if (!tracks.Any(t => t.Type == MediaTrackType.Audio &&
										t.Name == audioTrack.Name &&
										t.Language == audioTrack.Language))
					{
						tracks.Add(audioTrack);
					}
				}
			}

			// For HLS streams with subtitles
			var subtitleGroup = urlAsset.MediaSelectionGroupForMediaCharacteristic(AVMediaCharacteristics.Legible.GetConstant());
			if (subtitleGroup is not null)
			{
				foreach (var option in subtitleGroup.Options)
				{
					var locale = option.Locale;
					var language = locale?.LanguageCode ?? "unknown";
					var displayName = option.DisplayName ?? language;

					var subtitleTrack = new MediaTrack
					{
						Id = $"subtitle-option-{tracks.Count(t => t.Type == MediaTrackType.Text)}",
						Type = MediaTrackType.Text,
						Name = displayName,
						Language = language,
						IsDefault = option.HasMediaCharacteristic("AVMediaCharacteristicDefault"),
						Metadata = new Dictionary<string, object>
						{
							{ "AVMediaSelectionOption", option }
						}
					};

					// Check if we already have this track (avoid duplicates)
					if (!tracks.Any(t => t.Type == MediaTrackType.Text &&
										t.Name == subtitleTrack.Name &&
										t.Language == subtitleTrack.Language))
					{
						tracks.Add(subtitleTrack);
					}
				}

				// Add a "None" option for subtitles
				tracks.Add(new MediaTrack
				{
					Id = "subtitle-none",
					Type = MediaTrackType.Text,
					Name = "Off",
					Language = "",
					IsDefault = false
				});
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error extracting HLS variant tracks");
		}
	}

	/// <summary>
	/// Gets the display name for a track
	/// </summary>
	string GetTrackDisplayName(AVAssetTrack track)
	{
		var language = GetTrackLanguage(track);

		if (!string.IsNullOrEmpty(language))
		{
			try
			{
				// Normalize language code (e.g. "en", "en-US", "fr")
				var culture = System.Globalization.CultureInfo.GetCultureInfo(language);

				// This gives you a localized, human-readable name
				return culture.DisplayName;
			}
			catch (System.Globalization.CultureNotFoundException)
			{
				// If .NET doesn't recognize the code, just return it raw
				return language;
			}
		}

		// Fallback to track type and number
		return $"{track.MediaType} Track {track.TrackID}";
	}

	/// <summary>
	/// Gets the language code for a track
	/// </summary>
	string GetTrackLanguage(AVAssetTrack track)
	{
		// Try to get language from format descriptions
		var formatDescriptions = track.FormatDescriptions;
		if (formatDescriptions != null && formatDescriptions.Length > 0)
		{
			foreach (var desc in formatDescriptions)
			{
				if (desc is CMFormatDescription formatDesc)
				{
					var extensions = formatDesc.GetExtensions();
					if (extensions != null && extensions.ContainsKey(new NSString("Language")))
					{
						var lang = extensions[new NSString("Language")]?.ToString();
						if (!string.IsNullOrEmpty(lang))
						{
							return lang;
						}
					}
				}
			}
		}

		// Try to get from extended language tag
		var extendedLanguageTag = track.ExtendedLanguageTag;
		if (!string.IsNullOrEmpty(extendedLanguageTag))
		{
			return extendedLanguageTag;
		}

		// Fallback to language code
		return track.LanguageCode ?? "und";
	}

	/// <summary>
	/// Checks if a track is marked as default
	/// </summary>
	static bool IsDefaultTrack(AVAssetTrack track)
	{
		// Check if this is the default track
		// For audio, usually the first track is default
		// For subtitles, check for forced or default flags

		if (track.MediaType == AVMediaTypes.Subtitle.GetConstant())
		{
			// Check for forced subtitles
			var formatDescriptions = track.FormatDescriptions;
			if (formatDescriptions != null && formatDescriptions.Length > 0)
			{
				foreach (var desc in formatDescriptions)
				{
					if (desc is CMFormatDescription formatDesc)
					{
						var extensions = formatDesc.GetExtensions();
						if (extensions is not null)
						{
							if (extensions.ContainsKey(new NSString("Forced")))
							{
								return true;
							}

							if (extensions.ContainsKey(new NSString("Default")))
							{
								return true;
							}
						}
					}
				}
			}
		}

		// For audio, usually the first track is default
		if (track.MediaType == AVMediaTypes.Audio.GetConstant())
		{
			var audioTracks = track.Asset?.TracksWithMediaType(AVMediaTypes.Audio.GetConstant());
			if (audioTracks is not null && audioTracks.Length > 0 && audioTracks[0].TrackID == track.TrackID)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Gets the codec information for a track
	/// </summary>
	static string GetTrackCodec(AVAssetTrack track)
	{
		var formatDescriptions = track.FormatDescriptions;
		if (formatDescriptions is not null && formatDescriptions.Length > 0)
		{
			foreach (var desc in formatDescriptions)
			{
				if (desc is CMFormatDescription formatDesc)
				{
					var mediaSubType = formatDesc.MediaSubType;
					return mediaSubType.ToString();
				}
			}
		}

		return string.Empty;
	}

	/// <summary>
	/// Gets the estimated bitrate for a track
	/// </summary>
	static double GetTrackBitrate(AVAssetTrack track)
	{
		return track.EstimatedDataRate;
	}

	/// <summary>
	/// Gets the frame rate for a video track
	/// </summary>
	static float GetTrackFrameRate(AVAssetTrack track)
	{
		return track.NominalFrameRate;
	}

	static string GetContentType(string url)
	{
		var extension = Path.GetExtension(url).ToLower();
		return extension switch
		{
			".mp4" => "video/mp4",
			".mp3" => "audio/mpeg",
			".wav" => "audio/wav",
			".webm" => "video/webm",
			_ => "video/mp4"
		};
	}

	string GetUrl()
	{
		if (MediaElement.Source is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri;
			if (string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
			{
				return string.Empty;
			}
			return uri.AbsoluteUri;
		}
		else if (MediaElement.Source is FileMediaSource fileMediaSource)
		{
			var uri = fileMediaSource.Path;
			if (string.IsNullOrWhiteSpace(uri))
			{
				return string.Empty;
			}
			return uri;
		}
		else if (MediaElement.Source is ResourceMediaSource resourceMediaSource)
		{
			var path = resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path) && Path.HasExtension(path))
			{
				string directory = Path.GetDirectoryName(path) ?? "";
				string filename = Path.GetFileNameWithoutExtension(path);
				string extension = Path.GetExtension(path)[1..];
				var url = NSBundle.MainBundle.GetUrlForResource(filename,
					extension, directory);
				if (string.IsNullOrWhiteSpace(url.AbsoluteString))
				{
					Logger.LogWarning("Invalid file path for ResourceMediaSource.");
					return string.Empty;
				}

				return url.AbsoluteString;
			}
		}
		return string.Empty;
	}

	/// <summary>
	/// Loads the asset from the provided HLS URL.
	/// </summary>
	/// <param name="hlsUrl">The HLS URL to load the asset from.</param>
	/// <returns>The loaded AVUrlAsset or null if loading failed.</returns>
	AVUrlAsset? LoadAsset(string hlsUrl)
	{
		try
		{
			// Create the URL and AVURLAsset
			var url = new NSUrl(hlsUrl);

			// Create asset with options that may help with FairPlay detection
			var options = new NSMutableDictionary();
			// Add option that might help with content key session detection
			options.SetValueForKey(NSNumber.FromBoolean(true), new NSString("AVURLAssetAllowsCellularAccessKey"));

			var asset = new AVUrlAsset(url, options);

			// Make the content key session available to the asset by trying different approaches
			try
			{
				// Approach 1: Try using P/Invoke to call addContentKeyRecipient directly
				try
				{
					var selector = sel_registerName("addContentKeyRecipient:");
					if (contentKeySession is null)
					{
						throw new InvalidOperationException("Content key session is not initialized.");
					}
					objc_msgSend(contentKeySession.Handle, selector, asset.Handle);
				}
				catch (Exception pinvokeEx)
				{
					if (contentKeySession is null)
					{
						throw new InvalidOperationException($"Content key session is not initialized. {pinvokeEx.Message}");
					}
					// Approach 2: Try using reflection to call addContentKeyRecipient if available
					var addContentKeyRecipientMethod = contentKeySession.GetType().GetMethod("AddContentKeyRecipient");
					if (addContentKeyRecipientMethod is not null)
					{
						addContentKeyRecipientMethod.Invoke(contentKeySession, [asset]);
					}
					else
					{
						// Approach 4: Pre-load the asset which might trigger content key requests
						asset.LoadValuesAsynchronously(["duration", "playable"], () =>
						{
							Console.WriteLine("Asset values loaded asynchronously");
						});
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to associate content key session with asset.");
				return null;
			}
			return asset;
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error loading video");
			return null;
		}
	}

	/// <summary>
	/// Releases the unmanaged resources used by the <see cref="MediaManager"/> and optionally releases the managed resources.
	/// </summary>
	/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Player is not null)
            {
                Player.Pause();
                Player.InvokeOnMainThread(() => { UIApplication.SharedApplication.EndReceivingRemoteControlEvents(); });
                // disable the idle timer so screen turns off when media is not playing
                UIApplication.SharedApplication.IdleTimerDisabled = false;
                var audioSession = AVAudioSession.SharedInstance();
                audioSession.SetActive(false);

                DestroyErrorObservers();
                DestroyPlayedToEndObserver();

                RateObserver?.Dispose();
                RateObserver = null;

                CurrentItemErrorObserver?.Dispose();
                CurrentItemErrorObserver = null;

                Player.ReplaceCurrentItemWithPlayerItem(null);

                MutedObserver?.Dispose();
                MutedObserver = null;

                VolumeObserver?.Dispose();
                VolumeObserver = null;

                StatusObserver?.Dispose();
                StatusObserver = null;

                TimeControlStatusObserver?.Dispose();
                TimeControlStatusObserver = null;

                Player.Dispose();
                Player = null;
            }

            PlayerViewController?.Dispose();
            PlayerViewController = null;
        }
    }

    static TimeSpan ConvertTime(CMTime cmTime) => TimeSpan.FromSeconds(double.IsNaN(cmTime.Seconds) ? 0 : cmTime.Seconds);

    static (int Width, int Height) GetVideoDimensions(AVPlayerItem avPlayerItem)
    {
        // Create an AVAsset instance with the video file URL
        var asset = avPlayerItem.Asset;
        
        // Retrieve the video track
        var videoTrack = asset.TracksWithMediaType(AVMediaTypes.Video.GetConstant()).FirstOrDefault();
        
        if (videoTrack is not null)
        {
            // Get the natural size of the video
            var size = videoTrack.NaturalSize;
            var preferredTransform = videoTrack.PreferredTransform;

            // Apply the preferred transform to get the correct dimensions
            var transformedSize = CGAffineTransform.CGRectApplyAffineTransform(new CGRect(CGPoint.Empty, size), preferredTransform);
            var width = Math.Abs(transformedSize.Width);
            var height = Math.Abs(transformedSize.Height);

            return ((int)width, (int)height);
        }
        else
        {
            // HLS doesn't have tracks, try to get the dimensions this way
            if (!avPlayerItem.PresentationSize.IsEmpty)
            {
                return ((int)avPlayerItem.PresentationSize.Width, (int)avPlayerItem.PresentationSize.Height);
            }

            // If all else fails, just return 0, 0
            return (0, 0);
        }
    }

    
    void AddStatusObservers()
    {
        if (Player is null)
        {
            return;
        }
		
		MutedObserver = Player.AddObserver("muted", ValueObserverOptions, MutedChanged);
        VolumeObserver = Player.AddObserver("volume", ValueObserverOptions, VolumeChanged);
        StatusObserver = Player.AddObserver("status", ValueObserverOptions, StatusChanged);
        TimeControlStatusObserver = Player.AddObserver("timeControlStatus", ValueObserverOptions, TimeControlStatusChanged);
        RateObserver = AVPlayer.Notifications.ObserveRateDidChange(RateChanged);
    }


    void VolumeChanged(NSObservedChange e)
    {
        if (Player is null)
        {
            return;
        }

        var volumeDiff = Math.Abs(Player.Volume - MediaElement.Volume);
        if (volumeDiff > 0.01)
        {
            MediaElement.Volume = Player.Volume;
        }
    }


    void MutedChanged(NSObservedChange e)
    {
        if (Player is null)
        {
            return;
        }

        MediaElement.ShouldMute = Player.Muted;
    }

    void AddErrorObservers()
    {
        DestroyErrorObservers();

        ItemFailedToPlayToEndTimeObserver = AVPlayerItem.Notifications.ObserveItemFailedToPlayToEndTime(ErrorOccurred);
        PlaybackStalledObserver = AVPlayerItem.Notifications.ObservePlaybackStalled(ErrorOccurred);
        ErrorObserver = AVPlayerItem.Notifications.ObserveNewErrorLogEntry(ErrorOccurred);
    }

    void AddPlayedToEndObserver()
    {
        DestroyPlayedToEndObserver();

        PlayedToEndObserver = AVPlayerItem.Notifications.ObserveDidPlayToEndTime(PlayedToEnd);
    }

    void DestroyErrorObservers()
    {
        ItemFailedToPlayToEndTimeObserver?.Dispose();
        PlaybackStalledObserver?.Dispose();
        ErrorObserver?.Dispose();
    }

    void DestroyPlayedToEndObserver()
    {
        PlayedToEndObserver?.Dispose();
    }

    void StatusChanged(NSObservedChange obj)
    {
        if (Player is null)
        {
            return;
        }

        var newState = Player.Status switch
        {
            AVPlayerStatus.Unknown => MediaElementState.Stopped,
            AVPlayerStatus.ReadyToPlay => MediaElementState.Paused,
            AVPlayerStatus.Failed => MediaElementState.Failed,
            _ => MediaElement.CurrentState
        };

        MediaElement.CurrentStateChanged(newState);
    }

    void TimeControlStatusChanged(NSObservedChange obj)
    {
        if (Player is null || Player.Status is AVPlayerStatus.Unknown
                           || Player.CurrentItem?.Error is not null)
        {
            return;
        }

        var newState = Player.TimeControlStatus switch
        {
            AVPlayerTimeControlStatus.Paused => MediaElementState.Paused,
            AVPlayerTimeControlStatus.Playing => MediaElementState.Playing,
            AVPlayerTimeControlStatus.WaitingToPlayAtSpecifiedRate => MediaElementState.Buffering,
            _ => MediaElement.CurrentState
        };

        metaData?.SetMetadata(PlayerItem, MediaElement);

        MediaElement.CurrentStateChanged(newState);
    }

    void ErrorOccurred(object? sender, NSNotificationEventArgs args)
    {
        string message;

        var error = Player?.CurrentItem?.Error;
        if (error is not null)
        {
            message = error.LocalizedDescription;

            MediaElement.MediaFailed(new MediaFailedEventArgs(message));
            Logger.LogError("{LogMessage}", message);
        }
        else
        {
            // Non-fatal error, just log
            message = args.Notification?.ToString() ??
                      "Media playback failed for an unknown reason.";

            Logger?.LogWarning("{LogMessage}", message);
        }
    }

    void PlayedToEnd(object? sender, NSNotificationEventArgs args)
    {
        if (args.Notification.Object != PlayerViewController?.Player?.CurrentItem || Player is null)
        {
            return;
        }

        if (MediaElement.ShouldLoopPlayback)
        {
            PlayerViewController?.Player?.Seek(CMTime.Zero);
            Player.Play();
        }
        else
        {
            try
            {
                DispatchQueue.MainQueue.DispatchAsync(MediaElement.MediaEnded);
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, "{LogMessage}", $"Failed to play media to end.");
            }
        }
    }

    void RateChanged(object? sender, NSNotificationEventArgs args)
    {
        if (Player is null)
        {
            return;
        }

        if (!AreFloatingPointNumbersEqual(MediaElement.Speed, Player.Rate))
        {
            MediaElement.Speed = Player.Rate;
            if (metaData is not null)
            {
                metaData.NowPlayingInfo.PlaybackRate = (float)MediaElement.Speed;
                MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = metaData.NowPlayingInfo;
            }
        }
    }
}