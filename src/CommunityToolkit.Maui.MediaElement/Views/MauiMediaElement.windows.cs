using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using Application = Microsoft.Maui.Controls.Application;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Page = Microsoft.Maui.Controls.Page;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on Windows.
/// </summary>
public partial class MauiMediaElement : Grid, IDisposable
{
	readonly Popup popup = new();
	readonly Grid fullScreenGrid = new();
	readonly MediaPlayerElement mediaPlayerElement;
	readonly CustomTransportControls? customTransportControls;
	UIElement activeMediaView;
	bool doesNavigationBarExistBeforeFullScreen;
	bool isDisposed;
	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="mediaPlayerElement"></param>
	public MauiMediaElement(MediaPlayerElement mediaPlayerElement)
	{
		ArgumentNullException.ThrowIfNull(mediaPlayerElement);
		Trace.WriteLine("[MediaElement.Windows.View] Creating MauiMediaElement");
		LoadResourceDictionary();
		this.mediaPlayerElement = mediaPlayerElement;
		activeMediaView = mediaPlayerElement;
		customTransportControls = SetTransportControls();
		Children.Add(this.mediaPlayerElement);
		Trace.WriteLine($"[MediaElement.Windows.View] Standard MediaPlayerElement attached — Children={Children.Count}, Width={Width}, Height={Height}");
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
	/// Swaps the child view to a SwapChainPanel for PlayReady DRM playback.
	/// WinUI 3's MediaPlayerElement cannot render protected content, so DRM
	/// playback renders through the native engine's swap chain instead.
	/// </summary>
	public void SwapToDrmView(Microsoft.UI.Xaml.Controls.SwapChainPanel drmPanel)
	{
		Trace.WriteLine($"[MediaElement.Windows.View] SwapToDrmView — active={activeMediaView.GetType().Name}, childrenBefore={Children.Count}, fullscreenChildrenBefore={fullScreenGrid.Children.Count}");
		// Ensure the SwapChainPanel stretches to fill the parent grid.
		// This is critical because the DXGI swap chain auto-sizes to
		// match the panel dimensions for video rendering.
		drmPanel.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch;
		drmPanel.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch;
		drmPanel.Width = double.NaN;
		drmPanel.Height = double.NaN;

		Children.Remove(activeMediaView);
		fullScreenGrid.Children.Remove(activeMediaView);
		Children.Add(drmPanel);
		activeMediaView = drmPanel;
		Trace.WriteLine($"[MediaElement.Windows.View] DRM SwapChainPanel attached — childrenAfter={Children.Count}, active={activeMediaView.GetType().Name}, panelSize={drmPanel.ActualWidth}x{drmPanel.ActualHeight}");
	}

	internal void RestoreMediaPlayerView()
	{
		Trace.WriteLine($"[MediaElement.Windows.View] RestoreMediaPlayerView — active={activeMediaView.GetType().Name}, childrenBefore={Children.Count}, fullscreenChildrenBefore={fullScreenGrid.Children.Count}");
		if (ReferenceEquals(activeMediaView, mediaPlayerElement))
		{
			Trace.WriteLine("[MediaElement.Windows.View] Standard MediaPlayerElement is already active");
			return;
		}

		Children.Remove(activeMediaView);
		fullScreenGrid.Children.Remove(activeMediaView);
		Children.Remove(mediaPlayerElement);
		Children.Add(mediaPlayerElement);
		activeMediaView = mediaPlayerElement;
		Trace.WriteLine($"[MediaElement.Windows.View] Standard MediaPlayerElement restored — childrenAfter={Children.Count}");
	}

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
		Trace.WriteLine($"[MediaElement.Windows.View] Fullscreen click — currentPresenter={GetAppWindowForCurrentWindow().Presenter.Kind}, active={activeMediaView.GetType().Name}");
		var currentPage = CurrentPage;
		var appWindow = GetAppWindowForCurrentWindow();

		if (appWindow.Presenter.Kind is AppWindowPresenterKind.FullScreen)
		{
			Trace.WriteLine("[MediaElement.Windows.View] Leaving fullscreen");
			appWindow.SetPresenter(AppWindowPresenterKind.Default);
			Shell.SetNavBarIsVisible(CurrentPage, doesNavigationBarExistBeforeFullScreen);

			if (popup.IsOpen)
			{
				popup.IsOpen = false;
				popup.Child = null;
				fullScreenGrid.Children.Clear();
			}
			Children.Add(activeMediaView);
			Trace.WriteLine($"[MediaElement.Windows.View] Fullscreen view restored — activeParent={(activeMediaView is FrameworkElement { Parent: not null } activeFrameworkElement ? activeFrameworkElement.Parent.GetType().Name : "null")}");

			if (activeMediaView is FrameworkElement activeView)
			{
				activeView.Width = Width;
				activeView.Height = Height;
			}
		}
		else
		{
			Trace.WriteLine("[MediaElement.Windows.View] Entering fullscreen");
			appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
			doesNavigationBarExistBeforeFullScreen = Shell.GetNavBarIsVisible(currentPage);
			Shell.SetNavBarIsVisible(CurrentPage, false);

			var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
			if (activeMediaView is FrameworkElement activeView)
			{
				activeView.Width = displayInfo.Width / displayInfo.Density;
				activeView.Height = displayInfo.Height / displayInfo.Density;
			}

			Children.Clear();
			fullScreenGrid.Children.Add(activeMediaView);
			Trace.WriteLine($"[MediaElement.Windows.View] Active view moved to fullscreen grid — childCount={fullScreenGrid.Children.Count}");

			popup.XamlRoot = XamlRoot;
			popup.HorizontalOffset = 0;
			popup.VerticalOffset = 0;
			popup.ShouldConstrainToRootBounds = false;
			popup.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center;
			popup.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center;
			popup.Child = fullScreenGrid;

			if (!popup.IsOpen)
			{
				popup.IsOpen = true;
			}
		}
	}
}