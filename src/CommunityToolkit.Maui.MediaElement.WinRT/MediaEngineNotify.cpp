#include "pch.h"
#include "MediaEngineNotify.h"
#include "PlayReadyMediaEngine.h"

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	MediaEngineNotify::MediaEngineNotify(
		PlayReadyMediaEngine* owner,
		winrt::Microsoft::UI::Dispatching::DispatcherQueue const& dispatcherQueue)
		: m_owner(owner)
		, m_dispatcherQueue(dispatcherQueue)
	{
	}

	//
	// EventNotify: Called by IMFMediaEngine on a Media Foundation work queue thread.
	// We marshal the event to the UI thread via DispatcherQueue before delegating
	// to PlayReadyMediaEngine, because WinRT events must fire on the UI thread
	// (the SwapChainPanel's owning thread).
	//
	STDMETHODIMP MediaEngineNotify::EventNotify(DWORD eventCode, ULONG_PTR param1, DWORD param2)
	{
		if (!m_owner)
		{
			return E_POINTER;
		}

		return HandleMediaEngineEvent(eventCode, param1, param2);
	}

	HRESULT MediaEngineNotify::HandleMediaEngineEvent(
		DWORD eventCode, ULONG_PTR param1, DWORD param2)
	{
		// Marshal the event from the MF work queue thread to the UI thread.
		// WinRT events on PlayReadyMediaEngine must be fired on the same thread
		// as the SwapChainPanel (the UI/dispatcher thread).
		if (m_dispatcherQueue)
		{
			auto owner = m_owner;

			// Capture event data by value — the MF work queue thread may be reused
			// immediately after TryEnqueue returns, so don't capture references.
			BOOL queued = m_dispatcherQueue.TryEnqueue(
				[owner, eventCode, param1, param2]()
				{
					// owner is valid here because MediaEngineNotify outlives the
					// queued work items (the engine's Close() shuts down the MF
					// pipeline before releasing the notify).
					owner->OnMediaEngineEvent(eventCode, param1, param2);
				});

			if (!queued)
			{
				// DispatcherQueue is shutting down — deliver directly.
				// This should be rare; only during application exit.
				owner->OnMediaEngineEvent(eventCode, param1, param2);
			}
		}
		else
		{
			// No dispatcher queue available — deliver on the current thread.
			// The WinRT event subsystem handles cross-thread marshaling
			// for the event handlers, but this is suboptimal.
			m_owner->OnMediaEngineEvent(eventCode, param1, param2);
		}

		return S_OK;
	}
}
