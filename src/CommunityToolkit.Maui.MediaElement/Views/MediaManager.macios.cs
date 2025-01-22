using System.Diagnostics.CodeAnalysis;
using AVFoundation;
using AVKit;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using MediaPlayer;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.PlatformConfiguration.macOSSpecific;
using UIKit;

namespace CommunityToolkit.Maui.Core.Views;

public partial class MediaManager : IDisposable
{
	string pagetitle = string.Empty;
	bool hasNavigationBar = false;
	bool hasTabBar = false;

	UIButton? playPauseButton;
	UIButton? fastForwardButton;
	UIButton? rewindButton;
	UISlider? timelineSlider;
	UIButton? aspectRatioButton;
	UIButton? fullScreenButton;
	UIViewController? viewController;
	UIViewController? fullScreenViewController;
	UIView? originalView = null;
	CGRect? originalFrame;
	MPVolumeView? volumeView;
	bool isFullscreen;
	Metadata? metaData;
	UIView? controlPanel;

	// Media would still start playing when Speed was set although ShouldAutoPlay=False
	// This field was added to overcome that.
	bool isInitialSpeedSet;

	/// <summary>
	/// The default <see cref="NSKeyValueObservingOptions"/> flags used in the iOS and macOS observers.
	/// </summary>
	protected const NSKeyValueObservingOptions valueObserverOptions =
		NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.New;

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
		controlPanel = CreateControlPanel();

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
	
	 UIView CreateControlPanel()
	{
		ArgumentNullException.ThrowIfNull(PlayerViewController?.View);
		controlPanel = new UIView
		{
			Frame = new CGRect(0, PlayerViewController.View.Bounds.Height - 100, PlayerViewController.View.Bounds.Width, 100),
			BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f),
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin
		};
		playPauseButton = CreateButton(UIImage.GetSystemImage("play.fill") ?? throw new InvalidOperationException(), UIImage.GetSystemImage("pause.fill"), PlayPauseButtonClicked);
		fastForwardButton = CreateButton(UIImage.GetSystemImage("goforward.10") ?? throw new InvalidOperationException(), null, FastForwardButtonClicked);
		rewindButton = CreateButton(UIImage.GetSystemImage("gobackward.10") ?? throw new InvalidOperationException(), null, RewindButtonClicked);
		aspectRatioButton = CreateButton(UIImage.GetSystemImage("aspectratio.fill") ?? throw new InvalidOperationException(), null, AspectRatioButtonClicked);
		fullScreenButton = CreateButton(UIImage.GetSystemImage("arrow.up.left.and.arrow.down.right") ?? throw new InvalidOperationException(), UIImage.GetSystemImage("arrow.down.right.and.arrow.up.left"), FullScreenButtonClicked);
		UIButton volumeButton = CreateButton(UIImage.GetSystemImage("speaker.fill") ?? throw new InvalidOperationException(), null,  ShowVolumeSlider);
		
		volumeView = new MPVolumeView();
		var sliderRotation = CGAffineTransform.MakeIdentity();
		sliderRotation = CGAffineTransform.Rotate(sliderRotation, -(nfloat)(Math.PI / 2));
		volumeView.Transform = sliderRotation;
		volumeView.ShowsVolumeSlider = false;
		// Add the MPVolumeView to the view
		MainThread.InvokeOnMainThreadAsync(() => PlayerViewController?.View?.AddSubview(volumeView));
		// Arrange buttons in a horizontal line centered above the volume slider
		nfloat buttonWidth = 40;
		nfloat buttonHeight = 40;
		nfloat buttonSpacing = 10;
		nfloat totalButtonWidth = (buttonWidth * 5) + (buttonSpacing * 4);
		nfloat startX = (controlPanel.Bounds.Width - totalButtonWidth) / 2;
		nfloat buttonY = 10;

		playPauseButton.Frame = new CGRect(startX, buttonY, buttonWidth, buttonHeight);
		rewindButton.Frame = new CGRect(startX + (buttonWidth + buttonSpacing) * 1, buttonY, buttonWidth, buttonHeight);
		fastForwardButton.Frame = new CGRect(startX + (buttonWidth + buttonSpacing) * 2, buttonY, buttonWidth, buttonHeight);
		aspectRatioButton.Frame = new CGRect(startX + (buttonWidth + buttonSpacing) * 3, buttonY, buttonWidth, buttonHeight);
		fullScreenButton.Frame = new CGRect(startX + (buttonWidth + buttonSpacing) * 4, buttonY, buttonWidth, buttonHeight);
		volumeButton.Frame = new CGRect(startX + (buttonWidth + buttonSpacing) * 5, buttonY, buttonWidth, buttonHeight);
		volumeView.Frame = new CGRect(startX + (buttonWidth + buttonSpacing) * 5, buttonY, buttonWidth, buttonHeight);

		timelineSlider = new UISlider
		{
			Frame = new CGRect(20, controlPanel.Bounds.Height - 40, controlPanel.Bounds.Width - 60, 20), // Adjusted Y coordinate and margins
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
			MinValue = 0,
			MaxValue = 1,
			Value = 0 // Initial value
		};
		timelineSlider.ValueChanged += TimelineSlider_ValueChanged;

		controlPanel.AddSubviews(rewindButton, playPauseButton, fastForwardButton, volumeButton, volumeView, aspectRatioButton, fullScreenButton, timelineSlider);
		PlayerViewController.View.AddSubview(controlPanel);

		// Start a timer to update the timeline slider periodically
		NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(1), (timer) => UpdateTimelineSlider());

		return controlPanel;
	}

	void TimelineSlider_ValueChanged(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(sender);

		if (sender is UISlider slider && MediaElement.Duration > TimeSpan.Zero)
		{
			var newValue = slider.Value * MediaElement.Duration.TotalSeconds;
			MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None).ContinueWith(_ =>
			{
				MediaElement.Play();
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}

	void UpdateTimelineSlider()
	{
		if (timelineSlider != null && MediaElement.Duration > TimeSpan.Zero)
		{
			timelineSlider.Value = (float)(MediaElement.Position.TotalSeconds / MediaElement.Duration.TotalSeconds);
		}
	}

	void ShowVolumeSlider(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(volumeView);
		  // Create and configure the MPVolumeView
		if(volumeView.ShowsVolumeSlider == true){
			volumeView.ShowsVolumeSlider = false;
		}
		else
		{
			volumeView.ShowsVolumeSlider = true;
		}
	}


	static UIButton CreateButton(UIImage icon, UIImage? selectedIcon, EventHandler action)
	{
		UIButton button = new(UIButtonType.System)
		{
			Frame = new CGRect(0, 40, 44, 44),
			TintColor = UIColor.White,
			BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f), // Make the button translucent
			Layer = 
			{
				CornerRadius = 22,
				BorderWidth = 1,
				BorderColor = UIColor.White.CGColor,
				ShadowColor = UIColor.Black.CGColor,
				ShadowOffset = new CGSize(0, 2),
				ShadowOpacity = 0.5f,
				ShadowRadius = 2
			}
		};
		button.SetImage(icon, UIControlState.Normal);
		if (selectedIcon != null)
		{
			button.SetImage(selectedIcon, UIControlState.Selected);
		}
		button.TouchUpInside += action;
		return button;
	}

	void PlayPauseButtonClicked(object? sender, EventArgs e)
	{
		if (sender is null)
		{
			return;
		}
		UIButton button = (UIButton)sender;
		if (Player?.Rate > 0)
		{
			Player?.Pause();
			button.Selected = false;
			button.SetImage(UIImage.GetSystemImage("play.fill"), UIControlState.Normal); // Show play icon
		}
		else
		{
			Player?.Play();
			button.SetImage(UIImage.GetSystemImage("pause.fill"), UIControlState.Normal); // Show pause icon
		}
	}



	void FastForwardButtonClicked(object? sender, EventArgs e)
	{
		if(sender is null)
		{
			return;
		}
		if (Player?.CurrentTime is not null)
		{
			var newTime = TimeSpan.FromSeconds(Player.CurrentTime.Seconds + 10);
			PlatformSeek(newTime, CancellationToken.None);
		}
	}

	void RewindButtonClicked(object? sender, EventArgs e)
	{
		if (Player?.CurrentTime is not null)
		{
			var newTime = TimeSpan.FromSeconds(Math.Max(0, Player.CurrentTime.Seconds - 10));
			PlatformSeek(newTime, CancellationToken.None);
		}
	}

	void AspectRatioButtonClicked(object? sender, EventArgs e)
	{
		if (PlayerViewController is null)
		{
			return;
		}
		PlatformUpdateAspect();
	}

	void FullScreenButtonClicked(object? sender, EventArgs e)
	{
		if(sender is null)
		{
			return;
		}
		EnterFullScreen();
	}
	void EnterFullScreen()
	{
		if(PlayerViewController is null)
		{
			return;
		}
		viewController = ViewControllerExtensions.GetViewController(PlayerViewController, (MediaElement)MediaElement);
		if (viewController is null)
		{
			return;
		}
		var page = Microsoft.Maui.Controls.Application.Current?.Windows[0].Page ?? throw new InvalidOperationException();
		if (isFullscreen && PlayerViewController.View is not null && viewController.View is not null && originalFrame is not null)
		{
			 isFullscreen = false;
			 Shell.Current.CurrentPage.Title = pagetitle;

			controlPanel?.RemoveFromSuperview();
			PlayerViewController.View.RemoveFromSuperview();
			PlayerViewController.RemoveFromParentViewController();

			ArgumentNullException.ThrowIfNull(originalView);
			Shell.SetNavBarIsVisible(Shell.Current.CurrentPage, hasNavigationBar);
			Shell.SetTabBarIsVisible(Shell.Current.CurrentPage, hasTabBar);
		
			UIApplication.SharedApplication.SetStatusBarHidden(false, UIKit.UIStatusBarAnimation.None);
			Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(page, false);

			viewController.View.AddSubview(originalView);
			
			UIEdgeInsets insets = viewController.View.SafeAreaInsets;
			PlayerViewController.AdditionalSafeAreaInsets =
				new UIEdgeInsets(insets.Top * -1, insets.Left, insets.Bottom * -1, insets.Right);
			PlayerViewController.View.Frame = (CGRect)originalFrame;
			PlayerViewController.ShowsPlaybackControls = false;
			ArgumentNullException.ThrowIfNull(controlPanel);
			PlayerViewController.View.AddSubview(controlPanel);

			controlPanel.Frame = new CGRect(0, PlayerViewController.View.Bounds.Height - 100, PlayerViewController.View.Bounds.Width, 100);
			originalView = null;
		}
		else
		{
			ArgumentNullException.ThrowIfNull(PlayerViewController.View);
			originalView = PlayerViewController.View;
			originalFrame = PlayerViewController.View.Frame;
			pagetitle = Shell.Current.CurrentPage.Title;
			Shell.Current.CurrentPage.Title = string.Empty;
			isFullscreen = true;
			PlayerViewController.ShowsPlaybackControls = false;

			hasNavigationBar = Shell.GetNavBarIsVisible(Shell.Current.CurrentPage);
			hasTabBar = Shell.GetTabBarIsVisible(Shell.Current.CurrentPage);
			
			controlPanel?.RemoveFromSuperview();
			PlayerViewController.View.RemoveFromSuperview();
			PlayerViewController.RemoveFromParentViewController();
			
			fullScreenViewController = [];
			ArgumentNullException.ThrowIfNull(fullScreenViewController.View);
			fullScreenViewController.View.Frame = UIScreen.MainScreen.Bounds;
			fullScreenViewController.View.AddSubview(PlayerViewController.View);
			
			PlayerViewController.View.Frame = fullScreenViewController.View.Bounds;
			ArgumentNullException.ThrowIfNull(controlPanel);
			PlayerViewController.View.AddSubview(controlPanel);
			
			viewController?.AddChildViewController(fullScreenViewController);
			viewController?.View?.AddSubview(fullScreenViewController.View);
			
			fullScreenViewController.DidMoveToParentViewController(viewController);
			controlPanel.Frame = new CGRect(0, PlayerViewController.View.Bounds.Height - 200, PlayerViewController.View.Bounds.Width, 100);
			
			UIApplication.SharedApplication.SetStatusBarHidden(true, UIKit.UIStatusBarAnimation.Fade);
			Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(page, true);
			Shell.SetTabBarIsVisible(Shell.Current.CurrentPage, false);
			Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(Shell.Current.CurrentPage,false);
			Shell.SetNavBarIsVisible(Shell.Current.CurrentPage, false);
		}
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

		if (MediaElement.Source is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri;
			if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
			{
				asset = AVAsset.FromUrl(new NSUrl(uri.AbsoluteUri));
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
				playPauseButton?.SetImage(UIImage.GetSystemImage("pause.fill"), UIControlState.Normal);
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