#pragma once

// Windows SDK headers
#include <windows.h>
#include <unknwn.h>

// Direct3D 11 + DXGI (swap chain rendering)
// d3d11_4.h is required for ID3D11Multithread (d3d11_2.h does not declare it).
#include <d3d11_4.h>
#include <dxgi1_3.h>

// SwapChainPanel interop (ISwapChainPanelNative). The WinUI 3 header
// (microsoft.ui.xaml.media.dxinterop.h) declares it with the SAME IID as UWP
// ({63AAD0B8-7C24-40FF-85A8-640D944CC325}) — verified against WindowsAppSDK 1.7.
#include <microsoft.ui.xaml.media.dxinterop.h>

// WRL (Windows Runtime C++ Template Library) for COM smart pointers
// Must be included before C++/WinRT headers to avoid ComPtr template conflicts
#include <wrl/client.h>
#include <wrl/implements.h>

// Undefine macros from windows.h BEFORE C++/WinRT generated headers
// to prevent conflicts with COM interface methods.
// GetCurrentTime is a deprecated Win32 API macro that collides with:
//   - IMFMediaEngine::GetCurrentTime
//   - Microsoft.UI.Xaml.Media.Animation.* generated headers
#undef GetCurrentTime

// C++/WinRT
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.System.Threading.h>
#include <winrt/Windows.Web.Http.h>
#include <winrt/Windows.Web.Http.Headers.h>
#include <winrt/Windows.Storage.Streams.h>
#include <winrt/Windows.System.h>

// Audio session types (AudioCategory_Media, etc.)
#include <AudioSessionTypes.h>

// Media Foundation
#include <mfapi.h>
#include <mfmediaengine.h>
#include <mfmp2dlna.h>
#include <mfreadwrite.h>
#include <mfcontentdecryptionmodule.h>
#include <CompPkgSup.h>
#include <Mferror.h>
#include <propsys.h>

#pragma comment(lib, "mfplat")
#pragma comment(lib, "mfreadwrite")
#pragma comment(lib, "mfuuid")
#pragma comment(lib, "mf")
#pragma comment(lib, "dxgi")
#pragma comment(lib, "d3d11")
#pragma comment(lib, "propsys")
#pragma comment(lib, "CompPkgSup")
#pragma comment(lib, "WindowsApp")
