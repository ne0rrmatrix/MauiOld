using AVFoundation;
using AVKit;
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
	Metadata? metaData;

	// Media would still start playing when Speed was set although ShouldAutoPlay=False
	// This field was added to overcome that.
	bool isInitialSpeedSet;

	/// <summary>
	/// A reference to the native iOS/macOS media player.
	/// </summary>
	protected IDisposable? CurrentItemObserver { get; set; }

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

		// Observe currentItem changes to keep metadata/dimensions in sync
		CurrentItemObserver?.Dispose();
		CurrentItemObserver = Player.AddObserver("currentItem", ValueObserverOptions, (NSObservedChange _) =>
		{
			if (Player?.CurrentItem is AVPlayerItem curr)
			{
				PlayerItem = curr;
				metaData?.SetMetadata(curr, MediaElement);
				(MediaElement.MediaWidth, MediaElement.MediaHeight) = GetVideoDimensions(curr);
				SetPoster();
			}
		});

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

		asset = CreatePlayerItem(MediaElement.Source);

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

	protected virtual partial ValueTask PlatformUpdatePlaylist()
	{
		MediaElement.CurrentStateChanged(MediaElementState.Opening);

		if (Player is null)
		{
			return ValueTask.CompletedTask;
		}

		metaData ??= new(Player);
		Metadata.ClearNowPlaying();
		PlayerViewController?.ContentOverlayView?.Subviews.FirstOrDefault()?.RemoveFromSuperview();

		List<AVPlayerItem> items = [];
		foreach (var source in MediaElement.Playlist)
		{
			var asset = CreatePlayerItem(source);
			if (asset is null)
			{
				continue;
			}
			items.Add(new AVPlayerItem(asset));
		}

		if (items.Count == 0)
		{
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;
			MediaElement.CurrentStateChanged(MediaElementState.None);
			return ValueTask.CompletedTask;
		}

		// All queue mutations on main thread
		DispatchQueue.MainQueue.DispatchSync(() =>
		{
			// Clear any previous queue contents to prevent mixed queues
			Player.RemoveAllItems();

			// Make the first item the current item
			var first = items[0];
			Player.ReplaceCurrentItemWithPlayerItem(first);
			PlayerItem = first;

			// Append the rest in order, anchored after the last inserted
			var last = first;
			for (int i = 1; i < items.Count; i++)
			{
				var next = items[i];
				if (Player.CanInsert(next, last))
				{
					Player.InsertItem(next, last);
				}
				else
				{
					// Fallback: append at end
					Player.InsertItem(next, afterItem: null);
				}
				last = next;
			}
		});

		// Setup error observer for the current item
		CurrentItemErrorObserver?.Dispose();
		if (PlayerItem is not null)
		{
			metaData.SetMetadata(PlayerItem, MediaElement);

			CurrentItemErrorObserver = PlayerItem.AddObserver("error",
				ValueObserverOptions, (NSObservedChange change) =>
				{
					if (Player?.CurrentItem?.Error is null)
					{
						return;
					}

					var message = $"{Player.CurrentItem?.Error?.LocalizedDescription} - {Player.CurrentItem?.Error?.LocalizedFailureReason}";
					MediaElement.MediaFailed(new MediaFailedEventArgs(message));
					Logger.LogError("{LogMessage}", message);
				});

			MediaElement.MediaOpened();
			(MediaElement.MediaWidth, MediaElement.MediaHeight) = GetVideoDimensions(PlayerItem);

			if (MediaElement.ShouldAutoPlay)
			{
				Player.Play();
			}

			SetPoster();
		}
		else
		{
			MediaElement.MediaWidth = MediaElement.MediaHeight = 0;
			MediaElement.CurrentStateChanged(MediaElementState.None);
		}

		return ValueTask.CompletedTask;
	}

	void PlayedToEnd(object? sender, NSNotificationEventArgs args)
	{
		if (Player is null)
		{
			return;
		}

		// AVQueuePlayer auto-advances. When the last item finishes:
		// - CurrentItem is null
		// - Items is empty
		var queueEmpty = Player.CurrentItem is null && (Player.Items?.Length ?? 0) == 0;

		if (!queueEmpty)
		{
			// Still more items in the queue; nothing to do.
			return;
		}

		if (MediaElement.ShouldLoopPlayback)
		{
			// Re-enqueue the playlist and continue playing
			var sources = MediaElement.Playlist;
			if (sources is null || sources.Count == 0)
			{
				// Single-source fallback
				PlayerViewController?.Player?.Seek(CMTime.Zero);
				Player.Play();
				return;
			}

			List<AVPlayerItem> newItems = [];
			foreach (var source in sources)
			{
				var asset = CreatePlayerItem(source);
				if (asset is null)
				{
					continue;
				}
				newItems.Add(new AVPlayerItem(asset));
			}

			if (newItems.Count == 0)
			{
				return;
			}

			DispatchQueue.MainQueue.DispatchSync(() =>
			{
				Player.RemoveAllItems();
				var first = newItems[0];
				Player.ReplaceCurrentItemWithPlayerItem(first);
				var last = first;
				for (int i = 1; i < newItems.Count; i++)
				{
					var next = newItems[i];
					if (Player.CanInsert(next, last))
					{
						Player.InsertItem(next, last);
					}
					else
					{
						Player.InsertItem(next, afterItem: null);
					}
					last = next;
				}
				Player.Play();
			});
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

	AVAsset? CreatePlayerItem(MediaSource? source)
	{
		if (source is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri;
			if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
			{
				return AVAsset.FromUrl(new NSUrl(uri.AbsoluteUri));
			}
		}
		else if (source is FileMediaSource fileMediaSource)
		{
			var uri = fileMediaSource.Path;

			if (!string.IsNullOrWhiteSpace(uri))
			{
				return AVAsset.FromUrl(NSUrl.CreateFileUrl(uri));
			}
		}
		else if (source is ResourceMediaSource resourceMediaSource)
		{
			var path = resourceMediaSource.Path;

			if (!string.IsNullOrWhiteSpace(path) && Path.HasExtension(path))
			{
				string directory = Path.GetDirectoryName(path) ?? "";
				string filename = Path.GetFileNameWithoutExtension(path);
				string extension = Path.GetExtension(path)[1..];
				var url = NSBundle.MainBundle.GetUrlForResource(filename,
					extension, directory);

				return AVAsset.FromUrl(url);
			}
			else
			{
				Logger.LogWarning("Invalid file path for ResourceMediaSource.");
			}
		}
		return null;
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

	static (int Width, int Height) GetVideoDimensions(AVPlayerItem avPlayerItem)
	{
		// Create an AVAsset instance with the video file URL
		var asset = avPlayerItem.Asset;

		// Retrieve the video track
		var videoTrack = asset.TracksWithMediaType(AVMediaTypes.Video.GetConstant() ?? "0").FirstOrDefault();

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

	void SetPoster()
	{
		if (PlayerItem is null || metaData is null)
		{
			return;
		}

		var videoTrack = PlayerItem.Asset.TracksWithMediaType(AVMediaTypes.Video.GetConstant() ?? "0").FirstOrDefault();
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