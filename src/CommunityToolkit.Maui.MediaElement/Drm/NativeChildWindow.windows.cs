using System.Runtime.InteropServices;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>A plain Win32 child HWND (the built-in "STATIC" class — no custom WndProc needed) used purely as a
/// DirectComposition render target positioned over a XAML element's on-screen rectangle. WinUI3's own compositor
/// can't host a raw DComp surface handle directly (see <see cref="DirectComposition"/>), so this sits alongside the
/// XAML tree instead of inside it.</summary>
static partial class NativeChildWindow
{
	const int wsChild = 0x40000000;
	const int wsVisible = 0x10000000;

	// SetWindowPos flags/handle used to keep the surface on top of its sibling(s) in Z-order. MoveWindow (the more
	// obvious API) only ever changes position/size — it never touches Z-order — so a surface that ends up behind
	// the WinUI3 window's own DirectComposition-rendered content (an "airspace" quirk of mixing a plain HWND with
	// a DComp-composited XAML tree) stays hidden through any number of Move calls; only something that forces a
	// full window recomposite (e.g. toggling the AppWindow presenter) made it reappear. Re-asserting HWND_TOP on
	// every reposition — not just at creation — fixes that instead of relying on an incidental redraw trigger.
	const nint hwndTop = 0;
	const uint swpNoActivate = 0x0010;
	const uint swpShowWindow = 0x0040;

	// RGN_DIFF: the combined region is (src1 - src2) — used to cut the control-bar-shaped hole out of the video
	// surface's full client rect.
	const int rgnDiff = 4;

	[LibraryImport("user32.dll", EntryPoint = "CreateWindowExW", StringMarshalling = StringMarshalling.Utf16)]
	private static partial nint CreateWindowExW(
		int dwExStyle, string lpClassName, string lpWindowName, int dwStyle,
		int x, int y, int nWidth, int nHeight,
		nint hWndParent, nint hMenu, nint hInstance, nint lpParam);

	[LibraryImport("user32.dll")]
	private static partial int SetWindowPos(nint hWnd, nint hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

	[LibraryImport("user32.dll")]
	private static partial int DestroyWindow(nint hWnd);

	[LibraryImport("gdi32.dll")]
	private static partial nint CreateRectRgn(int left, int top, int right, int bottom);

	[LibraryImport("gdi32.dll")]
	private static partial int CombineRgn(nint dest, nint src1, nint src2, int combineMode);

	[LibraryImport("gdi32.dll")]
	private static partial int DeleteObject(nint gdiObject);

	[LibraryImport("user32.dll")]
	private static partial int SetWindowRgn(nint hWnd, nint region, [MarshalAs(UnmanagedType.Bool)] bool redraw);

	public static nint Create(nint parentHwnd, int x, int y, int width, int height)
	{
		return CreateWindowExW(0, "STATIC", "", wsChild | wsVisible,
			x, y, width, height, parentHwnd, 0, 0, 0);
	}

	public static void Move(nint hwnd, int x, int y, int width, int height) =>
		SetWindowPos(hwnd, hwndTop, x, y, width, height, swpNoActivate | swpShowWindow);

	/// <summary>
	/// Clips the window (in its OWN client coordinates, i.e. 0,0 is this window's own top-left) to its full
	/// <paramref name="width"/> x <paramref name="height"/> rectangle minus <paramref name="hole"/>, or to the
	/// full rectangle unclipped if <paramref name="hole"/> is <see langword="null"/>. Used to cut a hole matching
	/// the transport bar's own rectangle out of the native video surface so the bar's XAML rendering — which this
	/// sibling HWND would otherwise always paint over, a Win32 "airspace" limitation with no simpler fix — shows
	/// through where it's actually visible.
	/// </summary>
	public static void SetRegion(nint hwnd, int width, int height, (int X, int Y, int Width, int Height)? hole)
	{
		var full = CreateRectRgn(0, 0, width, height);
		if (hole is { Width: > 0, Height: > 0 } h)
		{
			var holeRgn = CreateRectRgn(h.X, h.Y, h.X + h.Width, h.Y + h.Height);
			CombineRgn(full, full, holeRgn, rgnDiff);
			// holeRgn was only ever an input to CombineRgn (never itself handed to SetWindowRgn), so it's ours to
			// free — unlike `full` below, which the system takes ownership of once SetWindowRgn succeeds.
			DeleteObject(holeRgn);
		}

		SetWindowRgn(hwnd, full, true);
	}

	public static void Destroy(nint hwnd) => DestroyWindow(hwnd);
}
