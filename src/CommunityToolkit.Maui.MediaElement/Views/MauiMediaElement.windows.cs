using System.Reflection;
using System.Runtime.InteropServices;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Application = Microsoft.Maui.Controls.Application;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Page = Microsoft.Maui.Controls.Page;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on Windows.
/// </summary>
public partial class MauiMediaElement : Grid, IDisposable
{
	readonly Grid fullScreenGrid = new();
	readonly MediaPlayerElement mediaPlayerElement;
	readonly CustomTransportControls? customTransportControls;
	WebView2? drmWebView;
	WebView2TransportOverlay? drmOverlay;
	bool doesNavigationBarExistBeforeFullScreen;
	bool isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="mediaPlayerElement"></param>
	public MauiMediaElement(MediaPlayerElement mediaPlayerElement)
	{
		ArgumentNullException.ThrowIfNull(mediaPlayerElement);
		LoadResourceDictionary();
		this.mediaPlayerElement = mediaPlayerElement;
		customTransportControls = SetTransportControls();
		Children.Add(this.mediaPlayerElement);
	}

	/// <summary>
	/// Finalizer
	/// </summary>
	~MauiMediaElement() => Dispose(false);

	/// <summary>
	/// Gets the presented page.
	/// </summary>
	protected static Page CurrentPage =>
		PageExtensions.GetCurrentPage(Application.Current?.Windows[0].Page ?? throw new InvalidOperationException($"{nameof(Page)} cannot be null."));

	/// <summary>
	/// Swaps the child view to a WebView2 control for PlayReady DRM playback.
	/// WinUI 3's MediaPlayerElement cannot render protected content, so DRM
	/// playback renders through Edge's rendering engine via WebView2.
	/// A transport controls overlay is added on top for play/pause, seek, volume, etc.
	/// </summary>
	public void SwapToWebView2(WebView2 webView2, WebView2TransportOverlay transportOverlay)
	{
		this.drmWebView = webView2;
		this.drmOverlay = transportOverlay;

		webView2.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch;
		webView2.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch;

		// Apply the custom transport controls style from the resource dictionary
		if (this.Resources.TryGetValue("customTransportcontrols", out object styleObj) &&
			styleObj is Microsoft.UI.Xaml.Style customStyle)
		{
			transportOverlay.Style = customStyle;
		}

		Children.Remove(mediaPlayerElement);
		fullScreenGrid.Children.Remove(mediaPlayerElement);
		Children.Add(drmWebView);
		Children.Add(drmOverlay);
	}

	/// <summary>
	/// Toggles fullscreen mode. Called by the WebView2 transport overlay's fullscreen button.
	/// </summary>
	internal void ToggleFullScreen() => OnFullScreenButtonClick(this, new RoutedEventArgs());

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MauiMediaElement"/>.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	[LibraryImport("user32.dll")]
	internal static partial IntPtr GetForegroundWindow();

	/// <summary>
	/// Safely gets the foreground window handle, returning null if no foreground window exists.
	/// </summary>
	internal static IntPtr? TryGetForegroundWindow()
	{
		var hwnd = GetForegroundWindow();
		return hwnd == IntPtr.Zero ? null : hwnd;
	}

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MauiMediaElement"/>.
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if (isDisposed)
		{
			return;
		}
		if (customTransportControls?.FullScreenButton is not null)
		{
			customTransportControls.FullScreenButton.Click -= OnFullScreenButtonClick;
		}

		if (disposing)
		{
			mediaPlayerElement.MediaPlayer.Pause();

			if (mediaPlayerElement.MediaPlayer.Source is Windows.Media.Core.MediaSource mediaSource)
			{
				// Dispose the MediaSource to release the resources
				// https://learn.microsoft.com/en-us/windows/uwp/audio-video-camera/play-audio-and-video-with-mediaplayer Shows how to dispose the MediaSource
				mediaSource.Dispose();
			}
			mediaPlayerElement.MediaPlayer.Source = null;
			mediaPlayerElement.MediaPlayer.Dispose();
			mediaPlayerElement.SetMediaPlayer(null);
		}

		isDisposed = true;
	}

	static AppWindow GetAppWindowForCurrentWindow()
	{
		var windowHandle = TryGetForegroundWindow() ?? throw new InvalidOperationException("No foreground window found.");
		var id = Win32Interop.GetWindowIdFromWindow(windowHandle);
		return AppWindow.GetFromWindowId(id);
	}

	void LoadResourceDictionary()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using Stream? stream = assembly.GetManifestResourceStream("ResourceDictionary.windows.xaml");
		if (stream is null)
		{
			return;
		}
		using StreamReader reader = new(stream);
		var xaml = reader.ReadToEnd();
		var resourceDictionary = (Microsoft.UI.Xaml.ResourceDictionary)XamlReader.Load(xaml);
		if (resourceDictionary is null)
		{
			return;
		}
		this.Resources.MergedDictionaries.Add(resourceDictionary);
	}
	void ApplyCustomStyle()
	{
		if (this.Resources.TryGetValue("customTransportcontrols", out object styleObj) &&
			styleObj is Microsoft.UI.Xaml.Style customStyle && mediaPlayerElement is not null && mediaPlayerElement.TransportControls is not null)
		{
			mediaPlayerElement.TransportControls.Style = customStyle;
		}
	}

	CustomTransportControls SetTransportControls()
	{
		mediaPlayerElement.TransportControls.IsEnabled = false;
		var temp = new CustomTransportControls()
		{
			IsZoomButtonVisible = true,
			IsZoomEnabled = true,
			IsVolumeButtonVisible = true,
			IsVolumeEnabled = true,
			IsSeekBarVisible = true,
			IsSeekEnabled = true,
			IsEnabled = true,
			IsRepeatButtonVisible = true,
			IsRepeatEnabled = true,
			IsNextTrackButtonVisible = true,
			IsPreviousTrackButtonVisible = true,
			IsFastForwardButtonVisible = true,
			IsFastForwardEnabled = true,
			IsFastRewindButtonVisible = true,
			IsFastRewindEnabled = true,
			IsPlaybackRateButtonVisible = true,
			IsPlaybackRateEnabled = true,
			IsCompact = false,
		};
		temp.OnTemplateLoaded += (s, e) =>
		{
			if (temp.FullScreenButton is null)
			{
				return;
			}
			temp.FullScreenButton.Click += OnFullScreenButtonClick;
		};
		mediaPlayerElement.TransportControls = temp;
		ApplyCustomStyle();
		return temp;
	}

	void OnFullScreenButtonClick(object sender, RoutedEventArgs e)
	{
		var currentPage = CurrentPage;
		var appWindow = GetAppWindowForCurrentWindow();
		var rootPanel = GetRootPanel() ?? throw new InvalidOperationException("Could not locate the window's root layout panel.");
		if (appWindow.Presenter.Kind is AppWindowPresenterKind.FullScreen)
		{
			appWindow.SetPresenter(AppWindowPresenterKind.Default);
			Shell.SetNavBarIsVisible(CurrentPage, doesNavigationBarExistBeforeFullScreen);

			// Remove the overlay grid from the window's root tree
			if (rootPanel.Children.Contains(fullScreenGrid))
			{
				rootPanel.Children.Remove(fullScreenGrid);
			}
			fullScreenGrid.Children.Clear();

			if (drmWebView is not null && drmOverlay is not null)
			{
				Children.Add(drmWebView);
				Children.Add(drmOverlay);

				// Clear explicit fullscreen dimensions so it conforms back to its original layout
				drmWebView.ClearValue(FrameworkElement.WidthProperty);
				drmWebView.ClearValue(FrameworkElement.HeightProperty);
			}
			else
			{
				Children.Add(mediaPlayerElement);

				mediaPlayerElement.ClearValue(FrameworkElement.WidthProperty);
				mediaPlayerElement.ClearValue(FrameworkElement.HeightProperty);
			}
		}
		else
		{
			appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
			doesNavigationBarExistBeforeFullScreen = Shell.GetNavBarIsVisible(currentPage);
			Shell.SetNavBarIsVisible(CurrentPage, false);

			// Remove from the standard layout structure
			Children.Clear();

			// Configure the overlay grid to fill the entire application space automatically
			fullScreenGrid.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch;
			fullScreenGrid.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch;

			if (drmWebView is not null && drmOverlay is not null)
			{
				drmWebView.ClearValue(FrameworkElement.WidthProperty);
				drmWebView.ClearValue(FrameworkElement.HeightProperty);

				fullScreenGrid.Children.Add(drmWebView);
				fullScreenGrid.Children.Add(drmOverlay);
			}
			else
			{
				mediaPlayerElement.ClearValue(FrameworkElement.WidthProperty);
				mediaPlayerElement.ClearValue(FrameworkElement.HeightProperty);

				fullScreenGrid.Children.Add(mediaPlayerElement);
			}

			rootPanel.Children.Add(fullScreenGrid);

			// Nudge WebView2 to redraw inside its new tree context
			drmWebView?.UpdateLayout();
		}
	}

	/// <summary>
	/// Safely traverses up to locate the absolute topmost layout Panel within the window.
	/// </summary>
	Panel? GetRootPanel()
	{
		if (this.XamlRoot?.Content is Panel panel)
		{
			return panel;
		}

		// Fallback tree walk for complex MAUI layouts
		DependencyObject current = this;
		Panel? topmostPanel = null;

		while (current is not null)
		{
			if (current is Panel p)
			{
				topmostPanel = p;
			}
			current = VisualTreeHelper.GetParent(current);
		}

		return topmostPanel;
	}
}