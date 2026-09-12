using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using WinRT.Interop;
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
	DrmTransportOverlay? drmOverlay;
	nint nativeSurfaceHwnd;
	nint dcompDevice;
	nint dcompTarget;
	nint dcompVisual;
	ulong boundSurfaceHandle;
	bool controlPanelVisible = true;
	Aspect currentAspect = Aspect.AspectFit;
	int naturalVideoWidth;
	int naturalVideoHeight;
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
	/// Swaps the child view to the native PlayReady DRM surface. WinUI 3's MediaPlayerElement cannot render
	/// protected content and there is no way to host a raw DirectComposition surface handle directly inside the
	/// XAML compositor tree (see <see cref="DirectComposition"/>), so the decoded video is instead rendered by a
	/// same-process native child <c>HWND</c> kept geometrically aligned with this element's on-screen bounds via
	/// <see cref="RepositionNativeSurface"/>. A transport controls overlay is added to the XAML tree on top of it
	/// for play/pause, seek, volume, etc.
	/// </summary>
	public void SwapToNativePlayReady(DrmTransportOverlay transportOverlay)
	{
		drmOverlay = transportOverlay;
		drmOverlay.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch;
		drmOverlay.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch;

		// Apply the custom transport controls style from the resource dictionary
		if (Resources.TryGetValue("customTransportcontrols", out object styleObj) &&
			styleObj is Microsoft.UI.Xaml.Style customStyle)
		{
			transportOverlay.Style = customStyle;
		}

		Children.Remove(mediaPlayerElement);
		fullScreenGrid.Children.Remove(mediaPlayerElement);
		Children.Add(drmOverlay);

		drmOverlay.SizeChanged += OnDrmOverlaySizeChanged;
		drmOverlay.Loaded += OnDrmOverlaySizeChanged;
		drmOverlay.ControlPanelVisibilityChanged += OnControlPanelVisibilityChanged;
		controlPanelVisible = true;
		RepositionNativeSurface();
	}

	void OnControlPanelVisibilityChanged(object? sender, bool visible)
	{
		controlPanelVisible = visible;
		RepositionNativeSurface();
	}

	/// <summary>
	/// Updates the DirectComposition visual's content to the native backend's latest shared surface handle.
	/// A no-op when the handle hasn't changed since the last bind, or before the surface has been created.
	/// </summary>
	internal void BindNativeSurface(ulong handle)
	{
		if (handle == 0 || handle == boundSurfaceHandle || dcompDevice == 0)
		{
			return;
		}

		var surface = DirectComposition.CreateSurfaceFromHandle(dcompDevice, (nint)handle);
		DirectComposition.VisualSetContent(dcompVisual, surface);
		DirectComposition.DeviceCommit(dcompDevice);
		DirectComposition.Release(surface);
		boundSurfaceHandle = handle;
	}

	/// <summary>
	/// Tears down the native child window, DirectComposition objects, and transport overlay, and restores the
	/// standard <see cref="MediaPlayerElement"/> to the visual tree.
	/// </summary>
	internal void CleanupNativePlayReady()
	{
		if (drmOverlay is not null)
		{
			drmOverlay.SizeChanged -= OnDrmOverlaySizeChanged;
			drmOverlay.Loaded -= OnDrmOverlaySizeChanged;
			drmOverlay.ControlPanelVisibilityChanged -= OnControlPanelVisibilityChanged;
			Children.Remove(drmOverlay);
			fullScreenGrid.Children.Remove(drmOverlay);
			drmOverlay = null;
		}

		if (nativeSurfaceHwnd != 0)
		{
			NativeChildWindow.Destroy(nativeSurfaceHwnd);
			nativeSurfaceHwnd = 0;
		}

		DirectComposition.Release(dcompVisual);
		DirectComposition.Release(dcompTarget);
		DirectComposition.Release(dcompDevice);
		dcompVisual = dcompTarget = dcompDevice = 0;
		boundSurfaceHandle = 0;

		if (!Children.Contains(mediaPlayerElement) && !fullScreenGrid.Children.Contains(mediaPlayerElement))
		{
			Children.Add(mediaPlayerElement);
		}
	}

	void OnDrmOverlaySizeChanged(object sender, object args) => RepositionNativeSurface();

	/// <summary>Updates how the native video surface is scaled/cropped within <see cref="drmOverlay"/>'s bounds.
	/// There is no native aspect/stretch export (see <c>Native/PlayReadyNative</c>'s P/Invoke surface) — this is
	/// implemented entirely in managed code by resizing the destination rectangle the native child window covers.</summary>
	internal void UpdateNativeAspect(Aspect aspect)
	{
		currentAspect = aspect;
		RepositionNativeSurface();
	}

	/// <summary>Records the native backend's reported natural video size so <see cref="Aspect.AspectFit"/> /
	/// <see cref="Aspect.AspectFill"/> can be honored; a no-op until the first frame reports a real size.</summary>
	internal void UpdateNativeVideoNaturalSize(int width, int height)
	{
		if (width <= 0 || height <= 0 || (naturalVideoWidth == width && naturalVideoHeight == height))
		{
			return;
		}

		naturalVideoWidth = width;
		naturalVideoHeight = height;
		RepositionNativeSurface();
	}

	/// <summary>
	/// Creates (on first use) or repositions the native child window hosting the DirectComposition-bound video
	/// surface so it covers <see cref="drmOverlay"/>'s current on-screen rectangle (scaled per <see cref="currentAspect"/>)
	/// — including after a fullscreen toggle, since <see cref="drmOverlay"/> is what actually moves between
	/// <c>Children</c> and <see cref="fullScreenGrid"/>.
	/// </summary>
	void RepositionNativeSurface()
	{
		if (drmOverlay is not { XamlRoot.Content: not null } overlay)
		{
			return;
		}

		// drmOverlay serves double duty: it IS the transport bar (styled via "customTransportcontrols", which sets
		// a MaxWidth — ~720 DIPs — a sensible cap for a button bar but wrong for "the whole video area") and it's
		// also what we've been using as the sizing reference for that video area. In fullscreen that MaxWidth caps
		// drmOverlay.ActualWidth to ~720 regardless of the real display width, leaving the video pinned to a
		// narrow column instead of filling the screen. fullScreenGrid (drmOverlay's parent there) has no such cap,
		// so use IT as the frame whenever drmOverlay is hosted inside it; windowed mode is unaffected (drmOverlay
		// is still the right frame there — its own MaxWidth already shaped what "windowed" sizing looked like
		// before this fix, which is the sizing already confirmed correct).
		FrameworkElement frame = overlay.Parent == fullScreenGrid ? fullScreenGrid : overlay;

		// Force any pending layout (e.g. the parent swap this same call stack just made when entering/exiting
		// fullscreen) to be applied NOW: XAML layout is otherwise asynchronous, so ActualWidth/ActualHeight read
		// immediately after moving drmOverlay to fullScreenGrid would still reflect its old, small windowed size.
		frame.UpdateLayout();

		if (frame.ActualWidth <= 0 || frame.ActualHeight <= 0)
		{
			Trace.WriteLine($"[MauiMediaElement.Reposition] bailing: frame={frame.GetType().Name} ActualWidth={frame.ActualWidth} ActualHeight={frame.ActualHeight}");
			return;
		}

		var transform = frame.TransformToVisual(overlay.XamlRoot.Content);
		var origin = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

		// The video now covers the FULL frame — it's allowed to overlap the transport bar's own rectangle. A
		// same-process native video surface is a sibling HWND, not a XAML element, so Win32 always paints it OVER
		// whatever XAML the bar draws underneath; instead of avoiding that by reserving space for the bar (which
		// looked like a separate strip below the video, not "on top of" it the way every other player does this),
		// the region cut below carves the bar's own rectangle back out of the video surface whenever it's visible,
		// so the bar's real XAML rendering shows through there instead.
		var (dipX, dipY, dipWidth, dipHeight) = ComputeVideoRect(origin.X, origin.Y, frame.ActualWidth, frame.ActualHeight);

		// TransformToVisual/ActualWidth are in DIPs; CreateWindowExW/MoveWindow want physical pixels. Skipping this
		// conversion renders the native surface at the right DIP-sized rectangle only at 100% scaling — at any other
		// scale factor (150% etc., the common case) it comes out too small and offset from where it should be.
		var scale = overlay.XamlRoot.RasterizationScale;
		var x = (int)Math.Round(dipX * scale);
		var y = (int)Math.Round(dipY * scale);
		var width = (int)Math.Round(dipWidth * scale);
		var height = (int)Math.Round(dipHeight * scale);

		Trace.WriteLine($"[MauiMediaElement.Reposition] frame={frame.GetType().Name} {frame.ActualWidth}x{frame.ActualHeight} " +
			$"overlay.ActualWidth={overlay.ActualWidth} origin=({origin.X},{origin.Y}) " +
			$"naturalVideo={naturalVideoWidth}x{naturalVideoHeight} aspect={currentAspect} " +
			$"dipRect=({dipX},{dipY},{dipWidth},{dipHeight}) scale={scale} -> physical=({x},{y},{width},{height}) " +
			$"hwndExists={nativeSurfaceHwnd != 0}");

		if (nativeSurfaceHwnd == 0)
		{
			nativeSurfaceHwnd = NativeChildWindow.Create(GetHostWindowHandle(), x, y, width, height);
			dcompDevice = DirectComposition.CreateDevice();
			dcompTarget = DirectComposition.CreateTargetForHwnd(dcompDevice, nativeSurfaceHwnd, false);
			dcompVisual = DirectComposition.CreateVisual(dcompDevice);
			DirectComposition.TargetSetRoot(dcompTarget, dcompVisual);
		}
		else
		{
			NativeChildWindow.Move(nativeSurfaceHwnd, x, y, width, height);
		}

		ApplyControlPanelRegion(overlay, scale, x, y, width, height);

		// Content bound via VisualSetContent (see BindNativeSurface) renders at the native decoder's OWN pixel
		// size (e.g. 512x288) by default — sizing/positioning the HWND and DComp target above does nothing to
		// stretch it. This is what actually scales the video up (or down) to fill the destination rectangle.
		if (naturalVideoWidth > 0 && naturalVideoHeight > 0)
		{
			var scaleX = (float)(width / (double)naturalVideoWidth);
			var scaleY = (float)(height / (double)naturalVideoHeight);
			DirectComposition.VisualSetScaleTransform(dcompVisual, scaleX, scaleY);
			Trace.WriteLine($"[MauiMediaElement.Reposition] visual scale=({scaleX:F3},{scaleY:F3}) " +
				$"native={naturalVideoWidth}x{naturalVideoHeight} -> physical {width}x{height}");
		}

		DirectComposition.DeviceCommit(dcompDevice);
	}

	/// <summary>
	/// Cuts the transport bar's own on-screen rectangle back out of the video surface (in the surface's own local,
	/// physical-pixel client coordinates) whenever the bar is visible, so its real XAML rendering shows through
	/// there instead of being permanently painted over by the video's sibling HWND. Passing a <see langword="null"/>
	/// hole (bar hidden, or no rect available yet) restores the surface to its full, unclipped rectangle.
	/// </summary>
	void ApplyControlPanelRegion(DrmTransportOverlay overlay, double scale, int videoX, int videoY, int videoWidth, int videoHeight)
	{
		(int X, int Y, int Width, int Height)? hole = null;

		if (controlPanelVisible && overlay.GetControlPanelRect(overlay.XamlRoot.Content) is { } panelRect)
		{
			var panelX = (int)Math.Round(panelRect.X * scale);
			var panelY = (int)Math.Round(panelRect.Y * scale);
			var panelRight = (int)Math.Round((panelRect.X + panelRect.Width) * scale);
			var panelBottom = (int)Math.Round((panelRect.Y + panelRect.Height) * scale);

			// Intersect with the video's own rect, then translate into the video HWND's local coordinates (its own
			// top-left is (0,0), not (videoX,videoY)) — SetWindowRgn's region is always in the window's own client
			// space, never in the parent/screen space TransformToVisual reports.
			var left = Math.Max(panelX, videoX) - videoX;
			var top = Math.Max(panelY, videoY) - videoY;
			var right = Math.Min(panelRight, videoX + videoWidth) - videoX;
			var bottom = Math.Min(panelBottom, videoY + videoHeight) - videoY;

			if (right > left && bottom > top)
			{
				hole = (left, top, right - left, bottom - top);
			}
		}

		Trace.WriteLine($"[MauiMediaElement.Reposition] controlPanelVisible={controlPanelVisible} hole={hole}");
		NativeChildWindow.SetRegion(nativeSurfaceHwnd, videoWidth, videoHeight, hole);
	}

	/// <summary>
	/// Resolves the HWND of the window actually hosting this element, via MAUI's own window handler — NOT
	/// <see cref="GetForegroundWindow"/>, which only reliably names this app's window when called synchronously
	/// from a user-initiated event (like the fullscreen button click below). <see cref="RepositionNativeSurface"/>
	/// runs from layout/timer callbacks where a debugger or another app can hold OS input focus, so parenting the
	/// native child window to "whatever is in the foreground" silently attaches it to the wrong top-level window —
	/// it never crashes, it just renders somewhere the user never sees.
	/// </summary>
	static nint GetHostWindowHandle()
	{
		if (CurrentPage.GetParentWindow()?.Handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
		{
			throw new InvalidOperationException("Could not resolve the WinUI Window hosting this page.");
		}

		return WindowNative.GetWindowHandle(window);
	}

	(double X, double Y, double Width, double Height) ComputeVideoRect(double overlayX, double overlayY, double overlayWidth, double overlayHeight)
	{
		if (naturalVideoWidth <= 0 || naturalVideoHeight <= 0 || currentAspect == Aspect.Fill)
		{
			return (overlayX, overlayY, overlayWidth, overlayHeight);
		}

		var scale = currentAspect == Aspect.AspectFill
			? Math.Max(overlayWidth / naturalVideoWidth, overlayHeight / naturalVideoHeight)
			: Math.Min(overlayWidth / naturalVideoWidth, overlayHeight / naturalVideoHeight);

		var destWidth = naturalVideoWidth * scale;
		var destHeight = naturalVideoHeight * scale;
		var destX = overlayX + ((overlayWidth - destWidth) / 2);
		var destY = overlayY + ((overlayHeight - destHeight) / 2);
		return (destX, destY, destWidth, destHeight);
	}

	/// <summary>
	/// Toggles fullscreen mode. Called by the DRM transport overlay's fullscreen button.
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
			CleanupNativePlayReady();

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
		Resources.MergedDictionaries.Add(resourceDictionary);
	}
	void ApplyCustomStyle()
	{
		if (Resources.TryGetValue("customTransportcontrols", out object styleObj) &&
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

	/// <summary>
	/// Toggles fullscreen for whichever surface is currently active (the native-DRM <see cref="drmOverlay"/> if
	/// PlayReady is playing, else the standard <see cref="mediaPlayerElement"/>). Ported from the upstream
	/// (non-DRM) implementation: a plain <c>rootPanel.Children.Add(fullScreenGrid)</c> tree-walk approach was
	/// tried first here and proved unreliable for this element's on-screen bounds — a <see cref="Popup"/> with
	/// <c>ShouldConstrainToRootBounds = false</c> positions <see cref="fullScreenGrid"/> at absolute screen
	/// coordinates regardless of how deep this element sits in a MAUI Shell/NavigationView tree.
	/// </summary>
	void OnFullScreenButtonClick(object sender, RoutedEventArgs e)
	{
		var currentPage = CurrentPage;
		var appWindow = GetAppWindowForCurrentWindow();
		FrameworkElement activeSurface = (FrameworkElement?)drmOverlay ?? mediaPlayerElement;

		Trace.WriteLine($"[MauiMediaElement.FullScreen] click: presenterKind={appWindow.Presenter.Kind} " +
			$"activeSurface={activeSurface.GetType().Name} currentWidth={activeSurface.Width} currentHeight={activeSurface.Height}");

		if (appWindow.Presenter.Kind is AppWindowPresenterKind.FullScreen)
		{
			appWindow.SetPresenter(AppWindowPresenterKind.Default);
			Shell.SetNavBarIsVisible(CurrentPage, doesNavigationBarExistBeforeFullScreen);

			if (popup.IsOpen)
			{
				popup.IsOpen = false;
				popup.Child = null;
				fullScreenGrid.Children.Clear();
				fullScreenGrid.ClearValue(WidthProperty);
				fullScreenGrid.ClearValue(HeightProperty);
			}
			Children.Add(activeSurface);

			// NOT parent?.Width/Height: MAUI's Windows layout arranges native views via its own Arrange-rect
			// logic — it never sets FrameworkElement.Width/Height on them — so `this.Width` here is NaN, not the
			// on-screen size. `parent?.Width` is a boxed `double?` wrapping that NaN, which is NOT null, so `??`
			// never falls through: activeSurface.Width silently becomes NaN-the-literal-value assigned (which is
			// actually equivalent to Auto — but relying on that by accident, rather than clearing explicitly, is
			// what left this fragile). ClearValue is the same technique this element used before the Popup port,
			// and unambiguously returns to Stretch/Auto sizing regardless of what the parent's CLR properties read.
			activeSurface.ClearValue(WidthProperty);
			activeSurface.ClearValue(HeightProperty);

			Trace.WriteLine($"[MauiMediaElement.FullScreen] exited: activeSurface.Width={activeSurface.Width} " +
				$"activeSurface.Height={activeSurface.Height} (should read NaN/Auto here)");
		}
		else
		{
			appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
			doesNavigationBarExistBeforeFullScreen = Shell.GetNavBarIsVisible(currentPage);
			Shell.SetNavBarIsVisible(CurrentPage, false);

			var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
			var fullScreenWidth = displayInfo.Width / displayInfo.Density;
			var fullScreenHeight = displayInfo.Height / displayInfo.Density;
			activeSurface.Width = fullScreenWidth;
			activeSurface.Height = fullScreenHeight;

			// fullScreenGrid itself ALSO needs an explicit size, distinct from activeSurface's: a Popup gives its
			// Child effectively unconstrained space to measure in, so a plain auto-sized Grid just shrinks to fit
			// whatever its content naturally wants — and activeSurface (drmOverlay) is styled with a MaxWidth (the
			// "customTransportcontrols" style, ~720 DIPs — a sensible cap for a transport BAR) which caps ITS
			// width well short of the display. Without this, that cap silently propagates up through fullScreenGrid's
			// auto-sizing even though fullScreenGrid has no MaxWidth of its own — measured directly, this is exactly
			// what kept coming back as "frame=Grid 720x1090" instead of the real display size.
			fullScreenGrid.Width = fullScreenWidth;
			fullScreenGrid.Height = fullScreenHeight;

			Trace.WriteLine($"[MauiMediaElement.FullScreen] entering: displayInfo={displayInfo.Width}x{displayInfo.Height} " +
				$"density={displayInfo.Density} -> activeSurface/fullScreenGrid set to {fullScreenWidth}x{fullScreenHeight} DIPs");

			Children.Clear();
			fullScreenGrid.Children.Add(activeSurface);

			// Left/Top, NOT Center: Center alignment positions the popup based on how much bigger the "available"
			// area is than fullScreenGrid — a computation TransformToVisual doesn't see, so RepositionNativeSurface
			// computed the native video surface's screen position as if the popup were pinned at (0,0) while it
			// was actually rendered centered somewhere else. That mismatch is what showed up as both a margin
			// around the whole fullscreen content AND the video/control-bar looking misaligned with each other —
			// the video (native HWND, positioned from the stale (0,0)-relative math) and the bar (rendered natively
			// by the popup at its true, offset position) were simply computed against two different origins.
			// Pinning here at Left/Top with zero offset removes the ambiguity: fullScreenGrid IS the full display
			// size (set above), so anchoring its top-left at the root's top-left exactly covers the screen.
			popup.XamlRoot = activeSurface.XamlRoot;
			popup.HorizontalOffset = 0;
			popup.VerticalOffset = 0;
			popup.ShouldConstrainToRootBounds = false;
			popup.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top;
			popup.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Left;
			popup.Child = fullScreenGrid;

			if (!popup.IsOpen)
			{
				popup.IsOpen = true;
			}
		}

		// The native surface is a sibling HWND, not a XAML element — nudge it to the overlay's new on-screen rect
		// now that the parent switch above has taken effect (SizeChanged will keep it in sync afterwards).
		RepositionNativeSurface();
	}
}
