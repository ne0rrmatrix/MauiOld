#pragma once

#include "pch.h"
#include "EventShim.h"
#include "PlayReadyMediaEngine.g.h"
#include "PlayReadyProtectionManager.h"
#include "MediaEngineNotify.h"
#include "winrt/Microsoft.UI.Xaml.Controls.h"
#include <winrt/Microsoft.UI.Dispatching.h>

#include <mfapi.h>
#include <mfmediaengine.h>
#include <mfmp2dlna.h>
#include <d3d11.h>
#include <d3d11_1.h>
#include <dxgi1_2.h>
#include <functional>
#include <memory>
#include <mutex>

using namespace Microsoft::WRL;

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{

	/// <summary>
	/// Wraps IMFMediaEngine and IMFMediaEngineEx for low-level PlayReady-protected
	/// video playback with DirectX 11 swapchain rendering.
	///
	/// Architecture:
	///   IMFMediaEngineClassFactory -> IMFMediaEngine (+ IMFMediaEngineEx)
	///     -> DirectX 11 SwapChain (video output bound to SwapChainPanel)
	///     -> IMFContentProtectionManager (PlayReady license acquisition)
	///     -> Audio sink (default audio endpoint)
	/// </summary>
	struct PlayReadyMediaEngine : PlayReadyMediaEngineT<PlayReadyMediaEngine>
	{
		PlayReadyMediaEngine() = default;

		// IPlayReadyMediaEngine
		void Initialize(int64_t swapChainPanelHandle);
		void SetSource(hstring const& url, CommunityToolkit::Maui::Media::WinRT::PlayReadyDrmConfiguration const& drmConfig);
		void SetSourceFromManifestBytes(Windows::Foundation::Collections::IVectorView<uint8_t> const& manifestData, hstring const& originalUrl, CommunityToolkit::Maui::Media::WinRT::PlayReadyDrmConfiguration const& drmConfig);
		hstring TestCdmGenerateRequest(hstring const& mp4Path, hstring const& licenseUrl, hstring const& licenseHeaderName, hstring const& licenseHeaderValue);
		void Play();
		void Pause();
		void Seek(double positionSeconds);
		void SetVolume(double volume);
		void SetMuted(bool muted);
		void SetPlaybackRate(double rate);
		void SetLooping(bool looping);
		void SetStretch(int32_t stretchMode);
		void ResizeSwapChain(uint32_t width, uint32_t height);
		void Close();

		double CurrentPositionSeconds();
		double DurationSeconds();
		uint32_t NaturalVideoWidth();
		uint32_t NaturalVideoHeight();
		bool IsBuffering();

		/// <summary>
		/// Called by MediaEngineNotify on the UI thread (after marshaling from
		/// the MF work queue) to handle media engine events.
		/// </summary>
		void OnMediaEngineEvent(ULONG eventCode, ULONG_PTR param1, DWORD param2);
		// WinRT events — must be named exactly as in the IDL.
		// The C++/WinRT produce<> code accesses these by name to implement
		// the ABI-level event add/remove methods.
		// WinRT events — must be named exactly as in the IDL.
		// Uses event_shim<> (not winrt::event<>) to bridge the v2.0+/v3.0+ API change
		// where add()/remove() replaced operator() for subscription management.
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, winrt::Windows::Foundation::IInspectable>> MediaOpened;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, CommunityToolkit::Maui::Media::WinRT::MediaEngineErrorEventArgs>> MediaFailed;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, winrt::Windows::Foundation::IInspectable>> MediaEnded;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, CommunityToolkit::Maui::Media::WinRT::MediaEngineTimeChangedEventArgs>> TimeChanged;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, bool>> BufferingChanged;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, CommunityToolkit::Maui::Media::WinRT::MediaEngineSizeChangedEventArgs>> SizeChanged;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, CommunityToolkit::Maui::Media::WinRT::DrmLicenseAcquiredEventArgs>> LicenseAcquired;
		event_shim<winrt::Windows::Foundation::TypedEventHandler<CommunityToolkit::Maui::Media::WinRT::PlayReadyMediaEngine, CommunityToolkit::Maui::Media::WinRT::MediaEngineStateChangedEventArgs>> StateChanged;

	private:
		// Media Foundation initialization
		HRESULT CreateMediaEngine();
		HRESULT CreateD3D11DeviceAndSwapChain(ISwapChainPanelNative* panelNative);

		// IMFMediaEngineNotify callback methods (called on UI thread via DispatcherQueue)
		void FireMediaOpened();
		void FireMediaFailed(int32_t errorCode, hstring const& message);
		void FireMediaEnded();
		void FireTimeChanged();
		void FireStateChanged(int32_t oldState, int32_t newState);
		void FireLicenseAcquired(bool success, hstring const& errorMessage);
		void FireSizeChanged(uint32_t width, uint32_t height);
		void FireBufferingChanged(bool isBuffering);

		// Render loop
		void StartRenderLoop();
		void StopRenderLoop();
		void RenderFrame();
		void RenderFrameImpl();

		// SEH-wrapped impls — the wrappers contain no RAII objects (C2712),
		// so all C++ scoped types live in the *Impl functions.
		void ResizeSwapChainImpl(uint32_t width, uint32_t height);
		void OnMediaEngineEventImpl(ULONG eventCode, ULONG_PTR param1, DWORD param2);

		// Helper to get current swap chain back buffer dimensions
		void GetSwapChainDimensions(UINT& width, UINT& height);

		// COM pointers — core Media Foundation
		ComPtr<IMFMediaEngine> m_mediaEngine;
		ComPtr<IMFMediaEngineEx> m_mediaEngineEx;
		ComPtr<IMFMediaEngineClassFactory> m_classFactory;

		// Protected-content interface — required for TransferVideoFrame on
		// DRM-protected streams (IMFMediaEngineEx::TransferVideoFrame is not
		// usable with protected content).
		ComPtr<IMFMediaEngineProtectedContent> m_protectedContent;

		// COM pointers — Direct3D 11 + DXGI swap chain
		ComPtr<ID3D11Device> m_d3dDevice;
		ComPtr<ID3D11DeviceContext> m_d3dContext;
		ComPtr<IDXGISwapChain1> m_swapChain;

		// Protects m_swapChain (and its GetBuffer/Present/ResizeBuffers calls)
		// from concurrent access between the background render-loop timer
		// thread (RenderFrame) and the UI thread (ResizeSwapChain), which
		// otherwise race and can crash inside DXGI/D3D11 with an access
		// violation when a resize happens while a frame is being presented.
		std::mutex m_swapChainMutex;

		// COM pointers — D3D11-MF interop
		ComPtr<IMFDXGIDeviceManager> m_dxgiManager;

		// PlayReady protection manager (implements IMFContentProtectionManager)
		winrt::com_ptr<PlayReadyProtectionManager> m_protectionManager;

		// Media engine event callback (implements IMFMediaEngineNotify)
		winrt::com_ptr<MediaEngineNotify> m_notify;

		// State
		bool m_isInitialized = false;
		bool m_isPlaying = false;
		bool m_isLooping = false;
		double m_volume = 1.0;
		bool m_isMuted = false;
		double m_playbackRate = 1.0;
		int32_t m_stretchMode = 2; // Uniform

		// DRM configuration (projected WinRT types, not implementation types)
		winrt::hstring m_licenseServerUrl;
		CommunityToolkit::Maui::Media::WinRT::PlayReadyDrmConfiguration m_drmConfig{ nullptr };

		// Rendering
		winrt::Windows::System::Threading::ThreadPoolTimer m_renderTimer{ nullptr };

		// UI thread dispatcher for event marshaling from MF work queue
		winrt::Microsoft::UI::Dispatching::DispatcherQueue m_dispatcherQueue{ nullptr };

		// SwapChainPanel used for rendering; stored via IInspectable to avoid a public
		// Microsoft.UI.Xaml.Controls dependency in the WinRT ABI.
		winrt::Windows::Foundation::IInspectable m_swapChainPanelIInspectable{ nullptr };

		// COM interface for binding the swap chain to the panel (ISwapChainPanelNative).
		::Microsoft::WRL::ComPtr<ISwapChainPanelNative> m_panelNative;

		// Byte stream for manifest data — kept alive for async engine processing.
		::Microsoft::WRL::ComPtr<IMFByteStream> m_sourceByteStream;
	};
}

namespace winrt::CommunityToolkit::Maui::Media::WinRT::factory_implementation
{
	struct PlayReadyMediaEngine : PlayReadyMediaEngineT<PlayReadyMediaEngine, implementation::PlayReadyMediaEngine>
	{
	};
}
