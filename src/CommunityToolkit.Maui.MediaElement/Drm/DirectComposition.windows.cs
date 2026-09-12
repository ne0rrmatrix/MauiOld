using System.Runtime.InteropServices;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// Minimal DirectComposition interop: hosts a raw DComp surface HANDLE (from the native PlayReady player) into a
/// native child HWND. WinUI3/Windows App SDK's Microsoft.UI.Composition.Interop.ICompositorInterop does not expose
/// CreateCompositionSurfaceForHandle the way UWP's did (verified against current MS docs), so there is no way to
/// drop this handle straight into the XAML compositor tree — DirectComposition targeting a plain Win32 child HWND
/// is the documented fallback and is what's used here.
///
/// Only the vtable slots actually called are given real signatures; every other slot in each interface is still
/// declared (as a same-size placeholder) purely to keep later slot OFFSETS correct — COM vtable order is
/// positional, so skipping a method breaks every call after it. Interface shapes and the exact method order are
/// copied directly from the current Windows SDK's dcomp.h (10.0.26100.0), not reconstructed from memory.
/// </summary>
static unsafe partial class DirectComposition
{
	static readonly Guid iidIDCompositionDevice = new("C37EA93A-E7AA-450D-B16F-9746CB0407F3");

	[LibraryImport("dcomp.dll")]
	private static partial int DCompositionCreateDevice(nint dxgiDevice, in Guid iid, out nint dcompositionDevice);

	static nint GetSlot(nint obj, int index) => *(nint*)(*(nint*)obj + index * sizeof(nint));

	public static nint CreateDevice()
	{
		var hr = DCompositionCreateDevice(0, iidIDCompositionDevice, out var device);
		Marshal.ThrowExceptionForHR(hr);
		return device;
	}

	// IDCompositionDevice vtable (post-IUnknown, 0-based): 0 Commit, 1 WaitForCommitCompletion,
	// 2 GetFrameStatistics, 3 CreateTargetForHwnd, 4 CreateVisual, 5 CreateSurface, 6 CreateVirtualSurface,
	// 7 CreateSurfaceFromHandle, 8 CreateSurfaceFromHwnd, ... (rest unused here).
	const int deviceCommitSlot = 3;
	const int deviceCreateTargetForHwndSlot = 6;
	const int deviceCreateVisualSlot = 7;
	const int deviceCreateSurfaceFromHandleSlot = 10;

	public static void DeviceCommit(nint device)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, int>)GetSlot(device, deviceCommitSlot);
		Marshal.ThrowExceptionForHR(fn(device));
	}

	public static nint CreateTargetForHwnd(nint device, nint hwnd, bool topmost)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, nint, int, nint*, int>)GetSlot(device, deviceCreateTargetForHwndSlot);
		nint target;
		Marshal.ThrowExceptionForHR(fn(device, hwnd, topmost ? 1 : 0, &target));
		return target;
	}

	public static nint CreateVisual(nint device)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, nint*, int>)GetSlot(device, deviceCreateVisualSlot);
		nint visual;
		Marshal.ThrowExceptionForHR(fn(device, &visual));
		return visual;
	}

	public static nint CreateSurfaceFromHandle(nint device, nint handle)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, nint, nint*, int>)GetSlot(device, deviceCreateSurfaceFromHandleSlot);
		nint surface;
		Marshal.ThrowExceptionForHR(fn(device, handle, &surface));
		return surface;
	}

	// IDCompositionTarget vtable (post-IUnknown): 0 SetRoot. Absolute slot = 3 (past QueryInterface/AddRef/Release).
	const int targetSetRootSlot = 3;

	public static void TargetSetRoot(nint target, nint visual)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, nint, int>)GetSlot(target, targetSetRootSlot);
		Marshal.ThrowExceptionForHR(fn(target, visual));
	}

	// IDCompositionVisual vtable (post-IUnknown). dcomp.h's TEXTUAL declaration order for each overloaded pair
	// (SetOffsetX, SetOffsetY, SetTransform, SetClip) is NOT the real binary vtable order — this bit both
	// windows-rs (github.com/microsoft/windows-rs issue #1017) and, initially, this file: for every one of
	// those pairs the object/animation-taking overload is actually FIRST in the vtable and the
	// primitive-taking overload SECOND, the reverse of how the header lists them. Verified against winapi-rs's
	// dcomp.rs bindings (which get this right) rather than trusted from the header a second time:
	// 0 SetOffsetX(anim), 1 SetOffsetX(float), 2 SetOffsetY(anim), 3 SetOffsetY(float), 4 SetTransform(ptr),
	// 5 SetTransform(matrix), 6 SetTransformParent, 7 SetEffect, 8 SetBitmapInterpolationMode, 9 SetBorderMode,
	// 10 SetClip(ptr), 11 SetClip(rect), 12 SetContent, ... Absolute slots = 0-based index + 3 (IUnknown).
	const int visualSetOffsetXFloatSlot = 4;
	const int visualSetOffsetYFloatSlot = 6;
	const int visualSetTransformMatrixSlot = 8;
	const int visualSetContentSlot = 15;

	public static void VisualSetOffsetX(nint visual, float x)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, float, int>)GetSlot(visual, visualSetOffsetXFloatSlot);
		Marshal.ThrowExceptionForHR(fn(visual, x));
	}

	public static void VisualSetOffsetY(nint visual, float y)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, float, int>)GetSlot(visual, visualSetOffsetYFloatSlot);
		Marshal.ThrowExceptionForHR(fn(visual, y));
	}

	/// <summary>D2D_MATRIX_3X2_F — the 2D affine transform IDCompositionVisual::SetTransform takes by pointer.</summary>
	[StructLayout(LayoutKind.Sequential)]
	struct Matrix3x2F
	{
		public float M11, M12, M21, M22, Dx, Dy;
	}

	/// <summary>
	/// Scales the visual's content (the native decoder's own-resolution surface, e.g. 512x288) up to fill
	/// whatever physical-pixel rectangle the hosting <c>HWND</c> was sized to. Content bound via
	/// <see cref="VisualSetContent"/> renders at ITS OWN pixel size by default — sizing/positioning the HWND and
	/// DComp target does nothing to the content's rendered size on its own; this transform is what actually
	/// stretches it. Non-uniform scaleX/scaleY is intentional — it lets <see cref="Aspect.Fill"/> stretch to an
	/// arbitrary rect, while the AspectFit/AspectFill callers already pre-shape their destination rect so
	/// scaleX == scaleY there.
	/// </summary>
	public static void VisualSetScaleTransform(nint visual, float scaleX, float scaleY)
	{
		var matrix = new Matrix3x2F { M11 = scaleX, M22 = scaleY };
		var slotAddress = GetSlot(visual, visualSetTransformMatrixSlot);
		var fn = (delegate* unmanaged[Stdcall]<nint, Matrix3x2F*, int>)slotAddress;
		var hr = fn(visual, &matrix);
		System.Diagnostics.Trace.WriteLine(
			$"[DirectComposition] SetTransform visual=0x{visual:X} slotIndex={visualSetTransformMatrixSlot} " +
			$"slotFnPtr=0x{slotAddress:X} matrix=({matrix.M11},{matrix.M12},{matrix.M21},{matrix.M22},{matrix.Dx},{matrix.Dy}) " +
			$"hr=0x{unchecked((uint)hr):X8}");
		Marshal.ThrowExceptionForHR(hr);
	}

	public static void VisualSetContent(nint visual, nint content)
	{
		var fn = (delegate* unmanaged[Stdcall]<nint, nint, int>)GetSlot(visual, visualSetContentSlot);
		Marshal.ThrowExceptionForHR(fn(visual, content));
	}

	public static void Release(nint obj)
	{
		if (obj == 0)
		{
			return;
		}

		var fn = (delegate* unmanaged[Stdcall]<nint, int>)GetSlot(obj, 2);
		fn(obj);
	}
}
