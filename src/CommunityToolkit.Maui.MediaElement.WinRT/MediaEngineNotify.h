#pragma once

#include "pch.h"
#include <winrt/Microsoft.UI.Dispatching.h>
#include <mfmediaengine.h>

using namespace Microsoft::WRL;

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	// Forward declaration
	struct PlayReadyMediaEngine;

	/// <summary>
	/// Implements IMFMediaEngineNotify to receive callbacks from the media engine.
	/// Media Foundation delivers events on its own work queue threads; this class
	/// marshals them to the UI thread via DispatcherQueue before forwarding to
	/// PlayReadyMediaEngine for WinRT event dispatch.
	/// </summary>
	struct MediaEngineNotify : public winrt::implements<
		MediaEngineNotify,
		IMFMediaEngineNotify
	>
	{
		MediaEngineNotify(
			PlayReadyMediaEngine* owner,
			winrt::Microsoft::UI::Dispatching::DispatcherQueue const& dispatcherQueue);

		// IMFMediaEngineNotify
		STDMETHOD(EventNotify)(DWORD eventCode, ULONG_PTR param1, DWORD param2) override;

	private:
		/// <summary>
		/// Maps MF media engine event codes to state transitions and dispatches events.
		/// Called on the UI thread after marshaling from the MF work queue.
		/// </summary>
		HRESULT HandleMediaEngineEvent(DWORD eventCode, ULONG_PTR param1, DWORD param2);

		PlayReadyMediaEngine* m_owner;

		/// <summary>
		/// The SwapChainPanel's DispatcherQueue, used to marshal MF callbacks
		/// from Media Foundation work queue threads to the UI thread.
		/// </summary>
		winrt::Microsoft::UI::Dispatching::DispatcherQueue m_dispatcherQueue{ nullptr };
	};
}
