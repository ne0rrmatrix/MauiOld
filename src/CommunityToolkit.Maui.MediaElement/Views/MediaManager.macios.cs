﻿using AVFoundation;
using AVKit;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Primitives;
using CommunityToolkit.Maui.Views;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MediaPlayer;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;
using UIKit;
using System.Linq;

namespace CommunityToolkit.Maui.Core.Views;

public partial class MediaManager : IDisposable
{
	Metadata? metaData;

	// Media would still start playing when Speed was set although ShouldAutoPlay=False
	// This field was added to overcome that.
	bool isInitialSpeedSet;

	/// <summary>
	/// The default <see cref="NSKeyValueObservingOptions"/> flags used in the iOS and macOS observers.
	/// </summary>
	protected const NSKeyValueObservingOptions valueObserverOptions =
		NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.New;

	/// <summary>
	/// Observer that tracks when an error has occurred in the playback of the current source.
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
	/// The current media playback source.
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

	protected virtual partial void PlatformUpdateSource()
	{
		if (Player is null)
		{
			return;
		}
		if(MediaElement.Source is null && MediaElement.Sources is null)
		{
			Player.ReplaceCurrentItemWithPlayerItem(null);
			MediaElement.CurrentStateChanged(MediaElementState.Stopped);
		}
		if(MediaElement.Source is null)
		{
			return;
		}
		MediaElement.CurrentStateChanged(MediaElementState.Opening);

		metaData ??= new(Player);
		Metadata.ClearNowPlaying();
		PlayerViewController?.ContentOverlayView?.Subviews?.FirstOrDefault()?.RemoveFromSuperview();

		NSUrl? videoURL = null;
		videoURL = VideoSource(MediaElement.Source);

		AVPlayerItem? playerItem = null;
		if (videoURL is not null)
		{
			playerItem = new AVPlayerItem(AVAsset.FromUrl(videoURL));
		}

		PlayerItem = videoURL is not null
			? playerItem
			: null;

		metaData.SetMetadata(PlayerItem, MediaElement);
		CurrentItemErrorObserver?.Dispose();

		Player.ReplaceCurrentItemWithPlayerItem(PlayerItem);
		
		CurrentItemErrorObserver = PlayerItem?.AddObserver("error",
			valueObserverOptions, (NSObservedChange change) =>
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

			if (MediaElement.ShouldAutoPlay)
			{
				Player.Play();
			}
		}
		else if (PlayerItem is null)
		{
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;

			MediaElement.CurrentStateChanged(MediaElementState.None);
		}
	}

	protected virtual partial void PlatformUpdateSources()
	{
		if (Player is null)
		{
			return;
		}
		
		if (MediaElement.Sources is null || MediaElement.Sources is null)
		{
			return;
		}
		MediaElement.CurrentStateChanged(MediaElementState.Opening);
		CurrentItemErrorObserver?.Dispose();

		metaData ??= new(Player);
		Metadata.ClearNowPlaying();

		PlayerViewController?.ContentOverlayView?.Subviews?.FirstOrDefault()?.RemoveFromSuperview();
		Player.RemoveAllItems();
		
		foreach (var source in MediaElement.Sources)
		{
			NSUrl? videoURL = null;
			
			var subtitleUrl = source?.SubtitleUrl;
			if (source?.Source is not null)
			{
				videoURL = VideoSource(source.Source);
			}
			
			AVPlayerItem? playerItem = null;
			if (videoURL is not null && subtitleUrl is null)
			{
				playerItem = new AVPlayerItem(AVAsset.FromUrl(videoURL));
			}
			if (playerItem is not null)
			{
				Player.InsertItem(playerItem, null);
				continue;
			}

			var composition = new AVMutableComposition();
			if (videoURL is not null)
			{
				var videoAsset = AVAsset.FromUrl(videoURL);
				var videoTrack = videoAsset.TracksWithMediaType(mediaType: AVMediaTypesExtensions.GetConstant(AVMediaTypes.Video))[0];
				var audioTrack = videoAsset.TracksWithMediaType(mediaType: AVMediaTypesExtensions.GetConstant(AVMediaTypes.Audio))[0];
				var videoCompositionTrack = composition.AddMutableTrack(mediaType: AVMediaTypesExtensions.GetConstant(AVMediaTypes.Video), 0);
				var audioCompositionTrack = composition.AddMutableTrack(mediaType: AVMediaTypesExtensions.GetConstant(AVMediaTypes.Audio), 0);
				videoCompositionTrack?.InsertTimeRange(new CMTimeRange { Start = CMTime.Zero, Duration = videoAsset.Duration }, videoTrack, CMTime.Zero, out _);
				audioCompositionTrack?.InsertTimeRange(new CMTimeRange { Start = CMTime.Zero, Duration = videoAsset.Duration }, audioTrack, CMTime.Zero, out _);
			}
			
			var uri = VideoSource(subtitleUrl);
			if (uri is not null)
			{
				var subtitleAsset = AVAsset.FromUrl(uri);
				var subtitleTrack = subtitleAsset.TracksWithMediaType(mediaType: AVMediaTypesExtensions.GetConstant(AVMediaTypes.Text))[0];
				var subtitleCompositionTrack = composition.AddMutableTrack(mediaType: AVMediaTypesExtensions.GetConstant(AVMediaTypes.Text), 0);
				subtitleCompositionTrack?.InsertTimeRange(new CMTimeRange { Start = CMTime.Zero, Duration = subtitleAsset.Duration }, subtitleTrack, CMTime.Zero, out _);
			}
			playerItem = new AVPlayerItem(composition);
			if(source?.SubtitleFont is not null && source?.SubtitleFontSize is not null)
			{
				playerItem.TextStyleRules = [CreateTextStyleRule(UIColor.White, UIColor.Black, source.SubtitleFontSize, source.SubtitleFont)];
			}
			Player.InsertItem(playerItem, null);
		}
		
		PlayerItem = Player.Items.FirstOrDefault();
		CurrentItemErrorObserver = PlayerItem?.AddObserver("error",
			valueObserverOptions, (NSObservedChange change) =>
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
			metaData.SetMetadata(Player.CurrentItem, MediaElement);
			(MediaElement.MediaWidth, MediaElement.MediaHeight) = GetVideoDimensions(PlayerItem);

			if (MediaElement.ShouldAutoPlay)
			{
				Player.Play();
			}
		}
		
		else if (PlayerItem is null)
		{
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;

			MediaElement.CurrentStateChanged(MediaElementState.None);
		}
	}

	static NSUrl? VideoSource(MediaSource? mediaSource)
	{
		if (mediaSource is null)
		{
			return null;
		}
		if (mediaSource is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri;
			if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
			{
				return new NSUrl(uri.AbsoluteUri);
			}
		}
		else if (mediaSource is FileMediaSource fileMediaSource)
		{
			var uri = fileMediaSource.Path;

			if (!string.IsNullOrWhiteSpace(uri))
			{
				return NSUrl.CreateFileUrl(uri);
			}
		}
		else if (mediaSource is ResourceMediaSource resourceMediaSource)
		{
			var path = resourceMediaSource.Path;

			if (!string.IsNullOrWhiteSpace(path) && Path.HasExtension(path))
			{
				string directory = Path.GetDirectoryName(path) ?? "";
				string filename = Path.GetFileNameWithoutExtension(path);
				string extension = Path.GetExtension(path)[1..];
				var url = NSBundle.MainBundle.GetUrlForResource(filename,
					extension, directory);

				return NSUrl.CreateFileUrl(url?.Path ?? "");
			}
		}
		return null;
	}

	static (float red, float green, float blue, float alpha) GetColorValues(UIColor color)
	{
		float green = color.ToColor()?.Green ?? 1;
		float red = color.ToColor()?.Red ?? 1;
		float blue = color.ToColor()?.Blue ?? 1;
		float alpha = color.ToColor()?.Alpha ?? 1;
		return (red, green, blue, alpha);
	}
	static AVTextStyleRule CreateTextStyleRule(UIColor foregorund, UIColor background, float fontSize, string fontFamily)
	{
		var cmTextMarkupAtrributes = new CMTextMarkupAttributes();
		if (foregorund is not null)
		{
			var (red, green, blue, alpha) = GetColorValues(foregorund);
			var foreGroundColor = new TextMarkupColor(red, green, blue, alpha);
			cmTextMarkupAtrributes.ForegroundColor = foreGroundColor;
		}

		if (background is not null)
		{
			var (red, green, blue, alpha) = GetColorValues(background);
			var backgroundColor = new TextMarkupColor(red, green, blue, alpha);
			cmTextMarkupAtrributes.BackgroundColor = backgroundColor;
		}

		cmTextMarkupAtrributes.FontFamilyName = GetFontFamily(fontFamily, fontSize).FamilyName;

		return new AVTextStyleRule(cmTextMarkupAtrributes);
	}
	static UIFont GetFontFamily(string fontFamily, float fontSize) => UIFont.FromName(new Core.FontExtensions.FontFamily(fontFamily).MacIOS, fontSize);

	void SetPoster()
	{
		if (PlayerItem is null || metaData is null)
		{
			return;
		}
		Metadata.ClearNowPlaying();
		PlayerViewController?.ContentOverlayView?.Subviews?.FirstOrDefault()?.RemoveFromSuperview();
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
				Player.InvokeOnMainThread(() =>
				{
					UIApplication.SharedApplication.EndReceivingRemoteControlEvents();
				});
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

		MutedObserver = Player.AddObserver("muted", valueObserverOptions, MutedChanged);
		VolumeObserver = Player.AddObserver("volume", valueObserverOptions, VolumeChanged);
		StatusObserver = Player.AddObserver("status", valueObserverOptions, StatusChanged);
		TimeControlStatusObserver = Player.AddObserver("timeControlStatus", valueObserverOptions, TimeControlStatusChanged);
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

		MediaElement.CurrentStateChanged(newState);
		if (MediaElement.CurrentState == MediaElementState.Playing)
		{
			PlayerItem = Player.CurrentItem;

			if (PlayerItem is not null && PlayerItem.Error is null)
			{
				metaData?.SetMetadata(Player.CurrentItem, MediaElement);
				(MediaElement.MediaWidth, MediaElement.MediaHeight) = GetVideoDimensions(PlayerItem);
				SetPoster();
			}
		}
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