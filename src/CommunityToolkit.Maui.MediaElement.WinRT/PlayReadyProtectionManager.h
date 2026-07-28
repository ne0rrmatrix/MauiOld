#pragma once

#include "pch.h"

// Media Foundation IDL — defines IMFContentProtectionManager, IMFContentEnabler
#include <mfapi.h>
#include <mfidl.h>

#include <functional>
#include <vector>

using namespace Microsoft::WRL;

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	/// <summary>
	/// Implements IMFContentProtectionManager for PlayReady DRM license acquisition.
	///
	/// When the media engine encounters PlayReady-encrypted content, it calls
	/// BeginEnableContent with an IMFActivate that can produce an IMFContentEnabler.
	/// The enabler provides the PlayReady challenge data and license server URL.
	/// We HTTP POST the challenge and signal completion via the IMFAsyncCallback.
	///
	/// PlayReady System ID: {9A04F079-9840-4286-AB92-E65BE0885F95}
	///
	/// SDK 10.0.26100.0+ change: IMFMediaProtectionManager and
	/// IMFMediaProtectionServiceRequest were removed. The replacement flow is:
	///   1. Activate pEnablerActivate → IMFContentEnabler
	///   2. GetEnableData() → PlayReady challenge bytes
	///   3. GetEnableURL() → license server URL
	///   4. HTTP POST challenge to URL (with custom headers)
	///   5. MFCreateAsyncResult + pCallback->Invoke() → signal completion
	/// </summary>
	struct PlayReadyProtectionManager : public winrt::implements<
		PlayReadyProtectionManager,
		IMFContentProtectionManager
	>
	{
		PlayReadyProtectionManager(
			winrt::hstring const& licenseServerUrl);

		// IMFContentProtectionManager methods
		STDMETHOD(BeginEnableContent)(
			IMFActivate* pEnablerActivate,
			IMFTopology* pTopo,
			IMFAsyncCallback* pCallback,
			IUnknown* punkState) override;

		// SEH-wrapped impl — the STDMETHOD override stays RAII-free (C2712).
		HRESULT BeginEnableContentImpl(
			IMFActivate* pEnablerActivate,
			IMFTopology* pTopo,
			IMFAsyncCallback* pCallback,
			IUnknown* punkState);

		STDMETHOD(EndEnableContent)(
			IMFAsyncResult* pResult) override;

		// Configuration
		void SetLicenseServerUrl(winrt::hstring const& url);
		void SetCustomHeaders(
			winrt::Windows::Foundation::Collections::IMap<winrt::hstring, winrt::hstring> const& headers);

		/// <summary>
		/// Sets a callback invoked when license acquisition completes.
		/// The callback receives (success, errorMessage) and fires
		/// the PlayReadyMediaEngine::LicenseAcquired WinRT event.
		/// </summary>
		void SetLicenseResultCallback(
			std::function<void(bool success, winrt::hstring const& errorMessage)> callback);

		/// <summary>
		/// Fires the license result callback with the given success status and message.
		/// </summary>
		void FireResult(bool success, winrt::hstring const& errorMessage);

	private:
		/// <summary>
		/// Creates an error async result and invokes the callback.
		/// </summary>
		HRESULT CompleteWithError(
			IMFAsyncCallback* pCallback,
			IUnknown* punkState,
			HRESULT errorCode);

		/// <summary>
		/// Extracts challenge from IMFContentEnabler, POSTs to license server,
		/// and signals completion via async callback.
		/// </summary>
		HRESULT PostChallengeAndComplete(
			IMFContentEnabler* pEnabler,
			const std::vector<BYTE>& challenge,
			const std::wstring& url,
			IMFAsyncCallback* pCallback,
			IUnknown* punkState);

		winrt::hstring m_licenseServerUrl;
		winrt::Windows::Foundation::Collections::IMap<winrt::hstring, winrt::hstring> m_customHeaders{ nullptr };

		// Callback to notify the engine of license acquisition results
		std::function<void(bool success, winrt::hstring const& errorMessage)> m_licenseResultCallback;
	};
}
