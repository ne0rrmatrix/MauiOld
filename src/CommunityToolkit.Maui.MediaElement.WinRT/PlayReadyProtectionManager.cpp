#include "pch.h"
#include "PlayReadyProtectionManager.h"
#include "CrashLog.h"

namespace
{
	void ProtectionLog(std::wstring const& message)
	{
		winrt::CommunityToolkit::Maui::Media::WinRT::implementation::EngineLog(L"[MediaElement.WinRT.PlayReady] " + message);
	}

	void ProtectionLogHresult(std::wstring const& operation, HRESULT hr)
	{
		wchar_t buffer[256]{};
		swprintf_s(buffer, L"%s — HRESULT=0x%08X", operation.c_str(), static_cast<unsigned>(hr));
		ProtectionLog(buffer);
	}
}

// PlayReady System ID GUID
// {9A04F079-9840-4286-AB92-E65BE0885F95}
static const GUID PLAYREADY_CONTENT_PROTECTION_SYSTEM_ID =
	{ 0x9A04F079, 0x9840, 0x4286, { 0xAB, 0x92, 0xE6, 0x5B, 0xE0, 0x88, 0x5F, 0x95 } };

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	PlayReadyProtectionManager::PlayReadyProtectionManager(
		winrt::hstring const& licenseServerUrl)
		: m_licenseServerUrl(licenseServerUrl)
	{
		ProtectionLog(L"Protection manager created — configuredLicenseServer=" + std::wstring(licenseServerUrl));
	}

	//
	// BeginEnableContent: Called by the media engine when encrypted content is detected.
	//
	// SDK 10.0.26100.0+ flow:
	//   1. Activate pEnablerActivate → IMFContentEnabler
	//   2. GetEnableData() → PlayReady challenge bytes
	//   3. GetEnableURL() → license server URL (with configured URL as fallback)
	//   4. HTTP POST challenge to license server via WinRT HttpClient
	//   5. MFCreateAsyncResult → pCallback->Invoke() to signal completion
	//
	STDMETHODIMP PlayReadyProtectionManager::BeginEnableContent(
		IMFActivate* pEnablerActivate,
		IMFTopology* pTopo,
		IMFAsyncCallback* pCallback,
		IUnknown* punkState)
	{
		// No RAII objects in this function — C2712 forbids __try with unwinding.
		__try
		{
			return BeginEnableContentImpl(pEnablerActivate, pTopo, pCallback, punkState);
		}
		__except (winrt::CommunityToolkit::Maui::Media::WinRT::implementation::EngineLogSeh(GetExceptionCode(), "BeginEnableContent"))
		{
			return E_FAIL;
		}
	}

	HRESULT PlayReadyProtectionManager::BeginEnableContentImpl(
		IMFActivate* pEnablerActivate,
		IMFTopology* /*pTopo*/,
		IMFAsyncCallback* pCallback,
		IUnknown* punkState)
	{
		ProtectionLog(std::wstring(L"BeginEnableContent — enabler=") + (pEnablerActivate ? L"true" : L"false") + L", callback=" + (pCallback ? L"true" : L"false"));
		if (!pEnablerActivate || !pCallback)
		{
			return E_POINTER;
		}

		HRESULT hr = S_OK;

		// Step 1: Activate the IMFContentEnabler from the IMFActivate
		ComPtr<IMFContentEnabler> spEnabler;
		hr = pEnablerActivate->ActivateObject(IID_PPV_ARGS(&spEnabler));
		ProtectionLogHresult(L"ActivateObject(IMFContentEnabler) returned", hr);
		if (FAILED(hr))
		{
			FireResult(false, L"Failed to activate IMFContentEnabler");
			return CompleteWithError(pCallback, punkState, hr);
		}

		// IMFContentEnabler does not expose a method for submitting a manually
		// acquired license response in the Windows 10.0.26100 SDK. Use the
		// platform-provided automatic flow so PlayReady can complete its own
		// challenge/response handshake.
		BOOL automaticSupported = FALSE;
		hr = spEnabler->IsAutomaticSupported(&automaticSupported);
		ProtectionLogHresult(L"IMFContentEnabler::IsAutomaticSupported returned", hr);
		ProtectionLog(std::wstring(L"Automatic license acquisition supported=") + (automaticSupported ? L"true" : L"false") + L", configuredLicenseServer=" + std::wstring(m_licenseServerUrl));
		if (SUCCEEDED(hr) && automaticSupported)
		{
			hr = spEnabler->AutomaticEnable();
			ProtectionLogHresult(L"IMFContentEnabler::AutomaticEnable returned", hr);
		}
		else if (SUCCEEDED(hr))
		{
			ProtectionLog(L"Automatic license acquisition is not supported; no manual response path is available in this Windows SDK");
		}

		if (FAILED(hr))
		{
			FireResult(false, L"PlayReady automatic license acquisition failed");
			return CompleteWithError(pCallback, punkState, hr);
		}

		FireResult(true, L"");
		ProtectionLog(L"PlayReady enablement completed successfully; completing Media Foundation callback");
		return CompleteWithError(pCallback, punkState, S_OK);
	}

	STDMETHODIMP PlayReadyProtectionManager::EndEnableContent(
		IMFAsyncResult* pResult)
	{
		if (!pResult)
		{
			return E_POINTER;
		}

		// SDK 10.0.26100.0+: GetStatus() returns HRESULT directly.
		HRESULT hr = pResult->GetStatus();
		ProtectionLogHresult(L"EndEnableContent status", hr);
		return hr;
	}

	void PlayReadyProtectionManager::SetLicenseServerUrl(winrt::hstring const& url)
	{
		m_licenseServerUrl = url;
		ProtectionLog(L"License server URL updated — valuePresent=" + std::wstring(url.empty() ? L"false" : L"true"));
	}

	void PlayReadyProtectionManager::SetCustomHeaders(
		winrt::Windows::Foundation::Collections::IMap<winrt::hstring, winrt::hstring> const& headers)
	{
		m_customHeaders = headers;
		ProtectionLog(L"Custom license headers updated — count=" + std::to_wstring(headers ? headers.Size() : 0));
	}

	void PlayReadyProtectionManager::SetLicenseResultCallback(
		std::function<void(bool, winrt::hstring const&)> callback)
	{
		m_licenseResultCallback = std::move(callback);
	}

	HRESULT PlayReadyProtectionManager::CompleteWithError(
		IMFAsyncCallback* pCallback,
		IUnknown* punkState,
		HRESULT errorCode)
	{
		ComPtr<IMFAsyncResult> spResult;
		HRESULT hr = MFCreateAsyncResult(
			nullptr,
			pCallback,
			punkState,
			&spResult);

		if (SUCCEEDED(hr))
		{
			ProtectionLogHresult(L"MFCreateAsyncResult returned", hr);
			spResult->SetStatus(errorCode);
			pCallback->Invoke(spResult.Get());
		}
		else
		{
			ProtectionLogHresult(L"MFCreateAsyncResult failed", hr);
		}
		ProtectionLogHresult(L"Completing protection callback with status", errorCode);

		return errorCode;
	}

	//
	// PostChallengeAndComplete: HTTP POST the PlayReady challenge to the license server.
	//
	// Uses WinRT HttpClient to POST the binary challenge data to the configured
	// license server URL. Custom headers (e.g. Authorization tokens) are applied
	// to the request. The response body contains the license/key data that
	// completes the content protection handshake.
	//
	// Blocks on .get() because this runs on an MF work queue thread, not the UI thread.
	//
	HRESULT PlayReadyProtectionManager::PostChallengeAndComplete(
		IMFContentEnabler* pEnabler,
		const std::vector<BYTE>& challenge,
		const std::wstring& url,
		IMFAsyncCallback* pCallback,
		IUnknown* punkState)
	{
		HRESULT operationHr = S_OK;
		bool success = false;

		try
		{
			// Create WinRT HttpClient
			winrt::Windows::Web::Http::HttpClient httpClient;

			// Wrap challenge bytes as an IBuffer for HttpBufferContent
			auto buffer = winrt::Windows::Storage::Streams::Buffer(
				static_cast<uint32_t>(challenge.size()));
			uint8_t* bufferData = buffer.data();
			memcpy(bufferData, challenge.data(), challenge.size());
			buffer.Length(static_cast<uint32_t>(challenge.size()));

			// Create HTTP content with octet-stream content type
			auto content = winrt::Windows::Web::Http::HttpBufferContent(buffer);
			content.Headers().ContentType(
				winrt::Windows::Web::Http::Headers::HttpMediaTypeHeaderValue(
					L"application/octet-stream"));

			// POST challenge to the license server
			auto uri = winrt::Windows::Foundation::Uri(winrt::hstring(url));
		auto request = winrt::Windows::Web::Http::HttpRequestMessage(
			winrt::Windows::Web::Http::HttpMethod::Post(), uri);
		request.Content(content);

		// Apply custom license request headers to the HTTP request.
		if (m_customHeaders)
		{
			for (auto const& pair : m_customHeaders)
			{
				request.Headers().TryAppendWithoutValidation(pair.Key(), pair.Value());
			}
		}

			// Block on the async call — runs on MF work queue thread
		auto response = httpClient.SendRequestAsync(request).get();

			// Check HTTP status
			response.EnsureSuccessStatusCode();

			success = true;

			OutputDebugStringW(L"[PlayReadyProtectionManager] License acquired successfully\n");
			FireResult(true, L"");
		}
		catch (winrt::hresult_error const& ex)
		{
			// WinRT error (network failure, HTTP error, etc.)
			OutputDebugStringW(L"[PlayReadyProtectionManager] HTTP POST failed: ");
			OutputDebugStringW(ex.message().c_str());
			OutputDebugStringW(L"\n");
			operationHr = ex.to_abi();
			FireResult(false, ex.message());
		}
		catch (...)
		{
			OutputDebugStringW(L"[PlayReadyProtectionManager] HTTP POST failed: unknown exception\n");
			operationHr = E_FAIL;
			FireResult(false, L"Unknown exception during license acquisition");
		}

		// Signal completion to the media engine via the async callback
		ComPtr<IMFAsyncResult> spResult;
		HRESULT createHr = MFCreateAsyncResult(
			nullptr,
			pCallback,
			punkState,
			&spResult);

		if (SUCCEEDED(createHr))
		{
			spResult->SetStatus(SUCCEEDED(operationHr) ? S_OK : operationHr);
			pCallback->Invoke(spResult.Get());
		}

		return SUCCEEDED(operationHr) ? S_OK : operationHr;
	}

	void PlayReadyProtectionManager::FireResult(bool success, winrt::hstring const& errorMessage)
	{
		if (m_licenseResultCallback)
		{
			m_licenseResultCallback(success, errorMessage);
		}
	}
}
