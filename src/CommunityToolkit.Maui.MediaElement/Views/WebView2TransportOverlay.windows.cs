using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using WinSlider = Microsoft.UI.Xaml.Controls.Slider;
using WinVisualStateManager = Microsoft.UI.Xaml.VisualStateManager;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// Transport controls overlay for WebView2-based DRM playback.
/// Subclasses <see cref="MediaTransportControls"/> and reuses the
/// <c>customTransportcontrols</c> style/template from
/// <c>ResourceDictionary.windows.xaml</c> so the controls look identical
/// to the standard MediaPlayerElement transport bar.
/// Button clicks and slider changes are routed to the JS bridge
/// instead of driving a native <see cref="Windows.Media.Playback.MediaPlayer"/>.
/// </summary>
public sealed partial class WebView2TransportOverlay : MediaTransportControls
{
	readonly DispatcherTimer autoHideTimer;

	AppBarButton? playPauseButton;
	SymbolIcon? playPauseSymbol;
	WinSlider? progressSlider;
	TextBlock? timeElapsedElement;
	TextBlock? timeRemainingElement;
	AppBarButton? volumeMuteButton;
	SymbolIcon? volumeMuteSymbol;
	AppBarButton? audioMuteButton;
	SymbolIcon? audioMuteSymbol;
	WinSlider? volumeSlider;
	AppBarButton? fullWindowButton;

	bool isSeeking;
	bool isPlaying;
	bool isMuted;
	double currentVolume = 1.0;
	TimeSpan duration;

	/// <summary>Raised when the user requests playback to start.</summary>
	public event EventHandler? PlayRequested;

	/// <summary>Raised when the user requests playback to pause.</summary>
	public event EventHandler? PauseRequested;

	/// <summary>Raised when the user seeks to a new position. The value is the target time in seconds.</summary>
	public event EventHandler<double>? SeekRequested;

	/// <summary>Raised when the user changes the volume. The value is 0.0–1.0.</summary>
	public event EventHandler<double>? VolumeChanged;

	/// <summary>Raised when the user toggles mute.</summary>
	public event EventHandler<bool>? MuteChanged;

	/// <summary>Raised when the user requests fullscreen toggle.</summary>
	public event EventHandler? FullScreenRequested;

	/// <summary>
	/// Initializes a new instance of the <see cref="WebView2TransportOverlay"/> class.
	/// The <c>customTransportcontrols</c> style is applied externally by
	/// <see cref="MauiMediaElement.SwapToWebView2"/> from the merged resource dictionary.
	/// </summary>
	public WebView2TransportOverlay()
	{
		// Show only the controls relevant to WebView2 DRM playback
		IsVolumeButtonVisible = true;
		IsSeekBarVisible = true;
		IsZoomButtonVisible = false;
		IsRepeatButtonVisible = false;
		IsNextTrackButtonVisible = false;
		IsPreviousTrackButtonVisible = false;
		IsFastForwardButtonVisible = false;
		IsFastRewindButtonVisible = false;
		IsPlaybackRateButtonVisible = false;
		IsStopButtonVisible = false;
		IsSkipBackwardButtonVisible = false;
		IsSkipForwardButtonVisible = false;
		IsCompact = false;

		// Auto-hide timer (3 seconds of inactivity while playing)
		autoHideTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
		autoHideTimer.Tick += (s, e) =>
		{
			if (isPlaying && !isSeeking)
			{
				WinVisualStateManager.GoToState(this, "ControlPanelFadeOut", true);
			}
		};

		// Show controls on pointer interaction
		PointerMoved += (s, e) =>
		{
			WinVisualStateManager.GoToState(this, "ControlPanelFadeIn", true);
			ResetAutoHide();
		};

		PointerPressed += (s, e) =>
		{
			WinVisualStateManager.GoToState(this, "ControlPanelFadeIn", true);
			ResetAutoHide();
		};
	}

	/// <inheritdoc/>
	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		UnhookTemplateParts();

		playPauseButton = GetTemplateChild("PlayPauseButton") as AppBarButton;
		playPauseSymbol = GetTemplateChild("PlayPauseSymbol") as SymbolIcon;
		progressSlider = GetTemplateChild("ProgressSlider") as WinSlider;
		timeElapsedElement = GetTemplateChild("TimeElapsedElement") as TextBlock;
		timeRemainingElement = GetTemplateChild("TimeRemainingElement") as TextBlock;
		volumeMuteButton = GetTemplateChild("VolumeMuteButton") as AppBarButton;
		volumeMuteSymbol = GetTemplateChild("VolumeMuteSymbol") as SymbolIcon;
		audioMuteButton = GetTemplateChild("AudioMuteButton") as AppBarButton;
		audioMuteSymbol = GetTemplateChild("AudioMuteSymbol") as SymbolIcon;
		volumeSlider = GetTemplateChild("VolumeSlider") as WinSlider;
		fullWindowButton = GetTemplateChild("FullWindowButton") as AppBarButton;

		if (playPauseButton is not null)
		{
			playPauseButton.Click += OnPlayPauseClick;
		}

		if (progressSlider is not null)
		{
			progressSlider.AddHandler(PointerPressedEvent, new PointerEventHandler((s, e) => isSeeking = true), true);
			progressSlider.AddHandler(PointerReleasedEvent, new PointerEventHandler(OnProgressSliderPointerReleased), true);
			progressSlider.ValueChanged += OnProgressSliderValueChanged;
		}

		if (volumeMuteButton is not null)
		{
			volumeMuteButton.Click += OnMuteToggleClick;
		}

		if (audioMuteButton is not null)
		{
			audioMuteButton.Click += OnMuteToggleClick;
		}

		if (volumeSlider is not null)
		{
			volumeSlider.ValueChanged += OnVolumeSliderChanged;
		}

		if (fullWindowButton is not null)
		{
			fullWindowButton.Click += OnFullWindowClick;
		}

		// Apply current state to the freshly loaded template parts
		UpdatePlayPauseVisual();
		UpdateVolumeVisual();
	}

	// ─── Public state update methods (called by MediaManager) ────────

	/// <summary>Updates the current playback position displayed in the overlay.</summary>
	public void UpdatePosition(TimeSpan pos)
	{
		if (timeElapsedElement is not null)
		{
			timeElapsedElement.Text = FormatTime(pos);
		}

		if (!isSeeking && progressSlider is not null && duration.TotalSeconds > 0)
		{
			progressSlider.Value = pos.TotalSeconds / duration.TotalSeconds * 100;
		}
	}

	/// <summary>Updates the total duration displayed in the overlay.</summary>
	public void UpdateDuration(TimeSpan dur)
	{
		duration = dur;
		if (timeRemainingElement is not null)
		{
			timeRemainingElement.Text = FormatTime(dur);
		}
	}

	/// <summary>Updates the play/pause button state and control panel visibility.</summary>
	public void UpdateIsPlaying(bool playing)
	{
		isPlaying = playing;
		UpdatePlayPauseVisual();

		if (playing)
		{
			ResetAutoHide();
		}
		else
		{
			WinVisualStateManager.GoToState(this, "ControlPanelFadeIn", true);
			autoHideTimer.Stop();
		}
	}

	/// <summary>Updates the volume slider position.</summary>
	public void UpdateVolume(double volume)
	{
		currentVolume = volume;
		if (volumeSlider is not null)
		{
			volumeSlider.Value = volume * 100;
		}

		UpdateVolumeVisual();
	}

	/// <summary>Updates the mute state icon.</summary>
	public void UpdateIsMuted(bool muted)
	{
		isMuted = muted;
		UpdateVolumeVisual();
	}

	// ─── Event handlers ──────────────────────────────────────────────

	void OnPlayPauseClick(object sender, RoutedEventArgs e)
	{
		if (isPlaying)
		{
			PauseRequested?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			PlayRequested?.Invoke(this, EventArgs.Empty);
		}

		ResetAutoHide();
	}

	void OnProgressSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
	{
		if (isSeeking && duration.TotalSeconds > 0 && timeElapsedElement is not null)
		{
			var targetSeconds = e.NewValue / 100 * duration.TotalSeconds;
			timeElapsedElement.Text = FormatTime(TimeSpan.FromSeconds(targetSeconds));
		}
	}

	void OnProgressSliderPointerReleased(object sender, PointerRoutedEventArgs e)
	{
		if (isSeeking)
		{
			isSeeking = false;
			if (progressSlider is not null && duration.TotalSeconds > 0)
			{
				var targetSeconds = progressSlider.Value / 100 * duration.TotalSeconds;
				SeekRequested?.Invoke(this, targetSeconds);
			}
		}

		ResetAutoHide();
	}

	void OnMuteToggleClick(object sender, RoutedEventArgs e)
	{
		MuteChanged?.Invoke(this, !isMuted);
		ResetAutoHide();
	}

	void OnVolumeSliderChanged(object sender, RangeBaseValueChangedEventArgs e)
	{
		VolumeChanged?.Invoke(this, e.NewValue / 100);
		ResetAutoHide();
	}

	void OnFullWindowClick(object sender, RoutedEventArgs e)
	{
		FullScreenRequested?.Invoke(this, EventArgs.Empty);
	}

	// ─── Visual state helpers ───────────────────────────────────────

	void UpdatePlayPauseVisual()
	{
		if (playPauseSymbol is not null)
		{
			playPauseSymbol.Symbol = isPlaying ? Symbol.Pause : Symbol.Play;
		}

		WinVisualStateManager.GoToState(this, isPlaying ? "PauseState" : "PlayState", true);
	}

	void UpdateVolumeVisual()
	{
		var muted = isMuted || currentVolume == 0;

		if (audioMuteSymbol is not null)
		{
			audioMuteSymbol.Symbol = muted ? Symbol.Mute : Symbol.Volume;
		}

		if (volumeMuteSymbol is not null)
		{
			volumeMuteSymbol.Symbol = muted ? Symbol.Mute : Symbol.Volume;
		}

		WinVisualStateManager.GoToState(this, muted ? "MuteState" : "VolumeState", true);
	}

	void UnhookTemplateParts()
	{
		if (playPauseButton is not null)
		{
			playPauseButton.Click -= OnPlayPauseClick;
		}

		if (progressSlider is not null)
		{
			progressSlider.ValueChanged -= OnProgressSliderValueChanged;
		}

		if (volumeMuteButton is not null)
		{
			volumeMuteButton.Click -= OnMuteToggleClick;
		}

		if (audioMuteButton is not null)
		{
			audioMuteButton.Click -= OnMuteToggleClick;
		}

		if (volumeSlider is not null)
		{
			volumeSlider.ValueChanged -= OnVolumeSliderChanged;
		}

		if (fullWindowButton is not null)
		{
			fullWindowButton.Click -= OnFullWindowClick;
		}
	}

	// ─── Auto-hide logic ─────────────────────────────────────────────

	void ResetAutoHide()
	{
		autoHideTimer.Stop();
		if (isPlaying)
		{
			autoHideTimer.Start();
		}
	}

	// ─── Helpers ─────────────────────────────────────────────────────

	static string FormatTime(TimeSpan time)
	{
		if (time.TotalHours >= 1)
		{
			return $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
		}

		return $"{time.Minutes:D2}:{time.Seconds:D2}";
	}
}
