#include "pch.h"
#include "WinRTDataTypes.h"
#include "PlayReadyMediaEngine.h"
#include "winrt/Microsoft.UI.Xaml.Controls.h"
#include "PlayReadyProtectionManager.h"
#include "MediaEngineNotify.h"
#include "MemoryByteStream.h"
#include "CrashLog.h"
#include "PlayReadyMediaEngine.g.cpp"

namespace
{
	void NativeLog(std::wstring const& message)
	{
		winrt::CommunityToolkit::Maui::Media::WinRT::implementation::EngineLog(L"[MediaElement.WinRT] " + message);
	}

	void NativeLogHresult(std::wstring const& operation, HRESULT hr)
	{
		wchar_t buffer[256]{};
		swprintf_s(buffer, L"%s — HRESULT=0x%08X", operation.c_str(), static_cast<unsigned>(hr));
		NativeLog(buffer);
	}

	// -----------------------------------------------------------------------
	// CDM probe helpers (diagnostics — TestCdmGenerateRequest)
	// -----------------------------------------------------------------------

	struct CdmProbeCallbacks : winrt::implements<CdmProbeCallbacks, IMFContentDecryptionModuleSessionCallbacks>
	{
		winrt::handle keyMessageEvent{ CreateEventW(nullptr, FALSE, FALSE, nullptr) };
		winrt::handle keyStatusEvent{ CreateEventW(nullptr, FALSE, FALSE, nullptr) };
		std::vector<BYTE> challenge;
		winrt::hstring destinationUrl;

		STDMETHODIMP KeyMessage(MF_MEDIAKEYSESSION_MESSAGETYPE type, const BYTE* message, DWORD messageSize, LPCWSTR url) override
		{
			challenge.assign(message, message + messageSize);
			if (url)
			{
				destinationUrl = url;
			}
			NativeLog(L"CDM KeyMessage — type=" + std::to_wstring(static_cast<int>(type))
				+ L", bytes=" + std::to_wstring(messageSize)
				+ L", url=" + (url ? std::wstring(url) : std::wstring(L"(null)")));
			SetEvent(keyMessageEvent.get());
			return S_OK;
		}

		STDMETHODIMP KeyStatusChanged() override
		{
			NativeLog(L"CDM KeyStatusChanged");
			SetEvent(keyStatusEvent.get());
			return S_OK;
		}
	};

	// Extracts the PlayReady pssh PAYLOAD (PlayReady data / WRM header) from an
	// MP4 file's moov. v0 pssh layout: [size][type][ver+flags][systemID][dataSize][data]
	std::vector<BYTE> ExtractPlayReadyPsshPayload(const wchar_t* path)
	{
		std::vector<BYTE> result;
		winrt::handle file(CreateFileW(path, GENERIC_READ, FILE_SHARE_READ, nullptr, OPEN_EXISTING, 0, nullptr));
		if (!file)
		{
			return result;
		}

		DWORD size = GetFileSize(file.get(), nullptr);
		std::vector<BYTE> data(size);
		DWORD bytesRead = 0;
		ReadFile(file.get(), data.data(), size, &bytesRead, nullptr);

		static const BYTE sysId[16] = { 0x9A,0x04,0xF0,0x79,0x98,0x40,0x42,0x86,0xAB,0x92,0xE6,0x5B,0xE0,0x88,0x5F,0x95 };
		for (DWORD i = 12; i + 28 < size; i++)
		{
			if (memcmp(data.data() + i, sysId, 16) == 0)
			{
				DWORD boxStart = i - 12;
				uint32_t boxSize = (uint32_t(data[boxStart]) << 24) | (uint32_t(data[boxStart + 1]) << 16)
					| (uint32_t(data[boxStart + 2]) << 8) | data[boxStart + 3];
				if (boxSize >= 28 && boxStart + boxSize <= size && memcmp(data.data() + boxStart + 4, "pssh", 4) == 0)
				{
					result.assign(data.begin() + boxStart + 28, data.begin() + boxStart + boxSize);
					return result;
				}
			}
		}
		return result;
	}
}

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	hstring PlayReadyMediaEngine::TestCdmGenerateRequest(
		hstring const& mp4Path,
		hstring const& licenseUrl,
		hstring const& licenseHeaderName,
		hstring const& licenseHeaderValue)
	{
		std::wstring summary;
		auto log = [&summary](std::wstring const& step, HRESULT hr)
		{
			wchar_t buffer[512]{};
			swprintf_s(buffer, L"%s — 0x%08X", step.c_str(), static_cast<unsigned>(hr));
			NativeLog(L"CDM probe: " + std::wstring(buffer));
			if (!summary.empty())
			{
				summary += L" | ";
			}
			summary += buffer;
		};

		NativeLog(L"TestCdmGenerateRequest — path=" + std::wstring(mp4Path));
		HRESULT hr = S_OK;

		ComPtr<IMFContentDecryptionModuleFactory> cdmFactory;
		hr = GetDefaultContentDecryptionModuleFactory(L"com.microsoft.playready", &cdmFactory);
		log(L"GetDefaultContentDecryptionModuleFactory", hr);
		if (FAILED(hr)) { return hstring(summary); }

		ComPtr<IPropertyStore> config;
		hr = PSCreateMemoryPropertyStore(IID_PPV_ARGS(&config));

		// The CDM requires a writable store path for its license/state store.
		std::wstring storeDir;
		try
		{
			storeDir = std::wstring(winrt::Windows::Storage::ApplicationData::Current().LocalFolder().Path()) + L"\\CdmStore";
		}
		catch (...)
		{
			wchar_t tempPath[MAX_PATH]{};
			GetTempPathW(MAX_PATH, tempPath);
			storeDir = std::wstring(tempPath) + L"CdmStore";
		}
		CreateDirectoryW(storeDir.c_str(), nullptr);
		{
			PROPVARIANT pv;
			PropVariantInit(&pv);
			pv.vt = VT_BSTR;
			pv.bstrVal = SysAllocString(storeDir.c_str());
			config->SetValue(MF_EME_CDM_STOREPATH, pv);
			SysFreeString(pv.bstrVal);
		}

		IPropertyStore* configs[] = { config.Get() };
		ComPtr<IMFContentDecryptionModuleAccess> cdmAccess;
		hr = cdmFactory->CreateContentDecryptionModuleAccess(L"com.microsoft.playready", configs, 1, &cdmAccess);
		log(L"CreateContentDecryptionModuleAccess", hr);
		if (FAILED(hr)) { return hstring(summary); }

		ComPtr<IMFContentDecryptionModule> cdm;
		hr = cdmAccess->CreateContentDecryptionModule(config.Get(), &cdm);
		log(L"CreateContentDecryptionModule", hr);
		if (FAILED(hr)) { return hstring(summary); }

		auto callbacks = winrt::make_self<CdmProbeCallbacks>();
		ComPtr<IMFContentDecryptionModuleSession> session;
		hr = cdm->CreateSession(MF_MEDIAKEYSESSION_TYPE_TEMPORARY, callbacks.get(), &session);
		log(L"CreateSession", hr);
		if (FAILED(hr)) { return hstring(summary); }

		auto pssh = ExtractPlayReadyPsshPayload(mp4Path.c_str());
		NativeLog(L"CDM probe: PSSH payload — " + std::to_wstring(pssh.size()) + L" bytes");
		if (pssh.empty())
		{
			log(L"ExtractPlayReadyPsshPayload (empty)", E_FAIL);
			return hstring(summary);
		}

		// Try the payload, then the bare WRMHEADER inside it.
		for (int attempt = 0; attempt < 2; attempt++)
		{
			const BYTE* initData = pssh.data();
			DWORD initSize = static_cast<DWORD>(pssh.size());

			if (attempt == 1)
			{
				static const BYTE marker[] = { 0x3C,0x00,0x57,0x00,0x52,0x00,0x4D,0x00,0x48,0x00 };
				size_t offset = SIZE_MAX;
				for (size_t i = 0; i + sizeof(marker) <= pssh.size(); i++)
				{
					if (memcmp(pssh.data() + i, marker, sizeof(marker)) == 0)
					{
						offset = i;
						break;
					}
				}
				if (offset == SIZE_MAX)
				{
					NativeLog(L"CDM probe: WRMHEADER marker not found");
					break;
				}
				initData = pssh.data() + offset;
				initSize = static_cast<DWORD>(pssh.size() - offset);
			}

			hr = session->GenerateRequest(L"cenc", initData, initSize);
			log(L"GenerateRequest attempt " + std::to_wstring(attempt) + L" (" + std::to_wstring(initSize) + L" bytes)", hr);
			if (SUCCEEDED(hr))
			{
				break;
			}
		}
		if (FAILED(hr)) { return hstring(summary); }

		if (WaitForSingleObject(callbacks->keyMessageEvent.get(), 15000) != WAIT_OBJECT_0)
		{
			log(L"KeyMessage wait (timeout)", HRESULT_FROM_WIN32(ERROR_TIMEOUT));
			return hstring(summary);
		}

		// License POST with the caller's custom header.
		std::vector<BYTE> licenseResponse;
		try
		{
			winrt::Windows::Web::Http::HttpClient client;
			if (!licenseHeaderName.empty())
			{
				client.DefaultRequestHeaders().TryAppendWithoutValidation(licenseHeaderName, licenseHeaderValue);
			}

			winrt::Windows::Storage::Streams::Buffer buffer(static_cast<uint32_t>(callbacks->challenge.size()));
			memcpy(buffer.data(), callbacks->challenge.data(), callbacks->challenge.size());
			buffer.Length(static_cast<uint32_t>(callbacks->challenge.size()));

			winrt::Windows::Web::Http::HttpBufferContent content(buffer);
			content.Headers().ContentType(winrt::Windows::Web::Http::Headers::HttpMediaTypeHeaderValue(L"text/xml"));

			hstring url = callbacks->destinationUrl.empty() ? licenseUrl : callbacks->destinationUrl;
			winrt::Windows::Foundation::Uri uri(url);
			auto response = client.PostAsync(uri, content).get();
			response.EnsureSuccessStatusCode();

			auto buf = response.Content().ReadAsBufferAsync().get();
			licenseResponse.assign(buf.data(), buf.data() + buf.Length());
		}
		catch (winrt::hresult_error const& ex)
		{
			log(L"license POST", ex.to_abi());
			return hstring(summary);
		}

		NativeLog(L"CDM probe: license response — " + std::to_wstring(licenseResponse.size()) + L" bytes");
		if (licenseResponse.empty())
		{
			log(L"license response (empty)", E_FAIL);
			return hstring(summary);
		}

		hr = session->Update(licenseResponse.data(), static_cast<DWORD>(licenseResponse.size()));
		log(L"session Update", hr);
		if (FAILED(hr)) { return hstring(summary); }

		WaitForSingleObject(callbacks->keyStatusEvent.get(), 15000);
		log(L"probe complete", S_OK);
		return hstring(summary);
	}

	// -----------------------------------------------------------------------
	// Initialize
	// -----------------------------------------------------------------------

	void PlayReadyMediaEngine::Initialize(int64_t swapChainPanelNativePtr)
	{
		EngineLogInit();
		NativeLog(L"Initialize started");

		// Capture the UI thread's DispatcherQueue FIRST. Initialize() is always
		// called on the UI thread (managed side dispatches here). Without this,
		// MediaEngineNotify falls back to delivering MF work-queue callbacks on
		// their native threads, and the resulting WinRT events reach managed
		// handlers off the UI thread — a fatal wrong-thread XAML access.
		m_dispatcherQueue = winrt::Microsoft::UI::Dispatching::DispatcherQueue::GetForCurrentThread();
		NativeLog(m_dispatcherQueue ? L"DispatcherQueue captured" : L"WARNING: no DispatcherQueue on this thread");

		if (swapChainPanelNativePtr == 0)
		{
			throw winrt::hresult_invalid_argument(L"swapChainPanelNativePtr");
		}

		// The managed side has already done the QI to ISwapChainPanelNative
		// via Marshal.GetComInterfaceForObject. We receive the raw COM pointer
		// with an AddRef already applied — attach() takes ownership.
		auto rawPtr = reinterpret_cast<ISwapChainPanelNative*>(swapChainPanelNativePtr);
		m_panelNative.Attach(rawPtr);
		NativeLog(L"ISwapChainPanelNative attached");

		// Step 1: Create Direct3D 11 device and DXGI swap chain
		HRESULT hr = CreateD3D11DeviceAndSwapChain(m_panelNative.Get());
		if (FAILED(hr))
		{
			NativeLogHresult(L"CreateD3D11DeviceAndSwapChain failed", hr);
			winrt::check_hresult(hr);
		}
		NativeLog(L"D3D11 device, swap chain, and DXGI manager created");

		// Step 2: Create IMFMediaEngine with protection manager
		hr = CreateMediaEngine();
		if (FAILED(hr))
		{
			NativeLogHresult(L"CreateMediaEngine failed", hr);
			winrt::check_hresult(hr);
		}

		m_isInitialized = true;
		NativeLog(L"Initialize completed successfully");
	}

	// -----------------------------------------------------------------------
	// CreateD3D11DeviceAndSwapChain
	// -----------------------------------------------------------------------

	HRESULT PlayReadyMediaEngine::CreateD3D11DeviceAndSwapChain(
		ISwapChainPanelNative* panelNative)
	{
		NativeLog(L"CreateD3D11DeviceAndSwapChain started");
		HRESULT hr = S_OK;

		// --- Create D3D11 device with BGRA and video support ---
		D3D_FEATURE_LEVEL featureLevels[] =
		{
			D3D_FEATURE_LEVEL_11_1,
			D3D_FEATURE_LEVEL_11_0,
		};

		D3D_FEATURE_LEVEL featureLevel;
		UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT
		                   | D3D11_CREATE_DEVICE_VIDEO_SUPPORT;

		hr = D3D11CreateDevice(
			nullptr,                          // default adapter
			D3D_DRIVER_TYPE_HARDWARE,
			nullptr,                          // no software rasterizer
			creationFlags,
			featureLevels,
			ARRAYSIZE(featureLevels),
			D3D11_SDK_VERSION,
			&m_d3dDevice,
			&featureLevel,
			&m_d3dContext);

		if (FAILED(hr))
		{
			return hr;
		}

		// --- Enable multithread protection on the D3D11 device ---
		// The IMFMediaEngine pipeline processes decoded video frames on its
		// own internal Media Foundation work-queue threads and issues GPU
		// commands against this device concurrently with our render-loop
		// thread (RenderFrame/TransferVideoFrame/Present) and the UI thread
		// (ResizeSwapChain/ResizeBuffers). The D3D11 immediate context is not
		// thread-safe by default, so without this the concurrent access can
		// corrupt internal state and crash with an access violation.
		ComPtr<ID3D11Multithread> multithread;
		hr = m_d3dDevice.As(&multithread);
		if (SUCCEEDED(hr))
		{
			multithread->SetMultithreadProtected(TRUE);
		}
		else
		{
			NativeLogHresult(L"Failed to query ID3D11Multithread", hr);
		}

		// --- Get DXGI adapter and factory for swap chain creation ---
		ComPtr<IDXGIDevice2> dxgiDevice;
		hr = m_d3dDevice.As(&dxgiDevice);
		if (FAILED(hr))
		{
			return hr;
		}

		ComPtr<IDXGIAdapter> dxgiAdapter;
		hr = dxgiDevice->GetAdapter(&dxgiAdapter);
		if (FAILED(hr))
		{
			return hr;
		}

		ComPtr<IDXGIFactory2> dxgiFactory;
		hr = dxgiAdapter->GetParent(IID_PPV_ARGS(&dxgiFactory));
		if (FAILED(hr))
		{
			return hr;
		}

		// --- Create swap chain for composition (SwapChainPanel) ---
		DXGI_SWAP_CHAIN_DESC1 swapChainDesc = {};
		swapChainDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
		swapChainDesc.SampleDesc.Count = 1;
		swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		swapChainDesc.BufferCount = 2;
		swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;
		swapChainDesc.AlphaMode = DXGI_ALPHA_MODE_IGNORE;
		// DXGI_SCALING_STRETCH makes XAML scale the swap chain content to the
		// panel automatically. This lets us keep a FIXED-size swap chain and
		// never call ResizeBuffers — which crashes the XAML compositor on a
		// panel-bound composition swap chain (see ResizeSwapChainImpl).
		swapChainDesc.Scaling = DXGI_SCALING_STRETCH;
		swapChainDesc.Width = 1280;
		swapChainDesc.Height = 720;

		hr = dxgiFactory->CreateSwapChainForComposition(
			m_d3dDevice.Get(),
			&swapChainDesc,
			nullptr,
			&m_swapChain);

		if (FAILED(hr))
		{
			return hr;
		}

		// --- Bind swap chain to SwapChainPanel ---
		hr = panelNative->SetSwapChain(m_swapChain.Get());
		if (FAILED(hr))
		{
			return hr;
		}

		// --- Create IMFDXGIDeviceManager for D3D11-MF interop ---
		UINT resetToken = 0;
		hr = MFCreateDXGIDeviceManager(&resetToken, &m_dxgiManager);
		if (FAILED(hr))
		{
			return hr;
		}

		hr = m_dxgiManager->ResetDevice(m_d3dDevice.Get(), resetToken);
		if (FAILED(hr))
		{
			return hr;
		}

		return S_OK;
	}

	// -----------------------------------------------------------------------
	// CreateMediaEngine
	// -----------------------------------------------------------------------

	HRESULT PlayReadyMediaEngine::CreateMediaEngine()
	{
		NativeLog(L"CreateMediaEngine started");
		HRESULT hr = S_OK;

		// --- Initialize Media Foundation with full platform (network, etc.) ---
		hr = MFStartup(MF_VERSION, MFSTARTUP_FULL);
		if (FAILED(hr))
		{
			NativeLogHresult(L"MFStartup(MFSTARTUP_FULL) failed", hr);
			// Try without flags as fallback — may already be initialized
			hr = MFStartup(MF_VERSION);
			if (FAILED(hr) && hr != MF_E_ALREADY_INITIALIZED)
			{
				NativeLogHresult(L"MFStartup fallback also failed", hr);
			}
			else
			{
				NativeLog(L"Media Foundation initialized (fallback)");
			}
		}
		else
		{
			NativeLog(L"Media Foundation initialized with FULL platform");
		}

		// --- Create the PlayReady protection manager ---
		m_protectionManager = winrt::make_self<PlayReadyProtectionManager>(
			m_licenseServerUrl);

		// Wire the license result callback
		m_protectionManager->SetLicenseResultCallback(
			[this](bool success, winrt::hstring const& errorMessage)
			{
					NativeLog(std::wstring(L"License result callback — success=") + (success ? L"true" : L"false") + L", error=" + std::wstring(errorMessage));
				// Marshal to UI thread for WinRT event dispatch
				if (m_dispatcherQueue)
				{
					m_dispatcherQueue.TryEnqueue(
						[this, success, errorMessage]()
						{
							FireLicenseAcquired(success, errorMessage);
						});
				}
				else
				{
					FireLicenseAcquired(success, errorMessage);
				}
			});

		// --- Create the media engine notify (callback bridge) ---
		m_notify = winrt::make_self<MediaEngineNotify>(this, m_dispatcherQueue);

		// --- Build IMFAttributes for the media engine ---
		ComPtr<IMFAttributes> attributes;
		hr = MFCreateAttributes(&attributes, 8);
		if (FAILED(hr))
		{
			NativeLogHresult(L"MFCreateAttributes failed", hr);
			return hr;
		}

		// Callback (IMFMediaEngineNotify)
		hr = attributes->SetUnknown(
			MF_MEDIA_ENGINE_CALLBACK,
			static_cast<IMFMediaEngineNotify*>(m_notify.get()));
		if (FAILED(hr))
		{
			NativeLogHresult(L"CreateInstance(MFMediaEngine) failed", hr);
			return hr;
		}

		// Video output format (must match swap chain format)
		hr = attributes->SetUINT32(
			MF_MEDIA_ENGINE_VIDEO_OUTPUT_FORMAT,
			DXGI_FORMAT_B8G8R8A8_UNORM);
		if (FAILED(hr))
		{
			return hr;
		}

		// DXGI device manager (D3D11 interop)
		hr = attributes->SetUnknown(
			MF_MEDIA_ENGINE_DXGI_MANAGER,
			m_dxgiManager.Get());
		if (FAILED(hr))
		{
			return hr;
		}

		// Content protection manager (PlayReady license acquisition)
		hr = attributes->SetUnknown(
			MF_MEDIA_ENGINE_CONTENT_PROTECTION_MANAGER,
			static_cast<IMFContentProtectionManager*>(m_protectionManager.get()));
		if (FAILED(hr))
		{
			return hr;
		}

		// Content protection flags — enable PlayReady
		hr = attributes->SetUINT32(
			MF_MEDIA_ENGINE_CONTENT_PROTECTION_FLAGS,
			MF_MEDIA_ENGINE_ENABLE_PROTECTED_CONTENT);
		if (FAILED(hr))
		{
			return hr;
		}

		// Audio category
		hr = attributes->SetUINT32(
			MF_MEDIA_ENGINE_AUDIO_CATEGORY,
			AudioCategory_Media);
		if (FAILED(hr))
		{
			return hr;
		}

		// --- Create the media engine class factory ---
		hr = CoCreateInstance(
			CLSID_MFMediaEngineClassFactory,
			nullptr,
			CLSCTX_INPROC_SERVER,
			IID_PPV_ARGS(&m_classFactory));
		if (FAILED(hr))
		{
			return hr;
		}

		// --- Create the IMFMediaEngine instance ---
		DWORD creationFlags = MF_MEDIA_ENGINE_REAL_TIME_MODE
		                    | MF_MEDIA_ENGINE_ENABLE_PROTECTED_CONTENT;

		hr = m_classFactory->CreateInstance(
			creationFlags,
			attributes.Get(),
			&m_mediaEngine);
		if (FAILED(hr))
		{
			return hr;
		}

		// --- Get IMFMediaEngineEx for advanced operations ---
		hr = m_mediaEngine.As(&m_mediaEngineEx);
		if (FAILED(hr))
		{
			return hr;
		}

		// --- Get IMFMediaEngineProtectedContent for DRM frame transfer ---
		// TransferVideoFrame on IMFMediaEngineEx must not be used with protected
		// content; the protected-content variant is the supported path (and the
		// engine exposes it when created with MF_MEDIA_ENGINE_ENABLE_PROTECTED_CONTENT).
		hr = m_mediaEngine.As(&m_protectedContent);
		if (FAILED(hr))
		{
			NativeLogHresult(L"WARNING: IMFMediaEngineProtectedContent unavailable", hr);
			// Not fatal — clear content can still use IMFMediaEngineEx::TransferVideoFrame.
			hr = S_OK;
		}
		else
		{
			// Log what frame protections the pipeline requires. If a protected
			// surface is required (hardware DRM), a plain swap chain back buffer
			// cannot receive frames — software DRM (SL2000) must not require it.
			DWORD protectionFlags = 0;
			HRESULT hrProt = m_protectedContent->GetRequiredProtections(&protectionFlags);
			if (SUCCEEDED(hrProt))
			{
				NativeLog(std::wstring(L"GetRequiredProtections — flags=0x") + winrt::to_hstring(static_cast<uint32_t>(protectionFlags)).c_str());
			}
			else
			{
				NativeLogHresult(L"GetRequiredProtections failed", hrProt);
			}
		}

		NativeLog(L"CreateMediaEngine completed successfully");
		return S_OK;
	}

	// -----------------------------------------------------------------------
	// SetSource
	// -----------------------------------------------------------------------

	void PlayReadyMediaEngine::SetSource(
		hstring const& url,
		CommunityToolkit::Maui::Media::WinRT::PlayReadyDrmConfiguration const& drmConfig)
	{
		NativeLog(std::wstring(L"SetSource started — URL=") + std::wstring(url) + L", initialized=" + (m_isInitialized ? L"true" : L"false") + L", engine=" + (m_mediaEngine ? L"true" : L"false"));
		if (!m_isInitialized || !m_mediaEngine)
		{
			NativeLog(L"SetSource skipped because the engine is not initialized");
			return;
		}

		m_licenseServerUrl = drmConfig.LicenseServerUrl();
		m_drmConfig = drmConfig;
		NativeLog(std::wstring(L"SetSource DRM configuration — licenseServer=") + std::wstring(m_licenseServerUrl) + L", customHeaderCount=" + std::to_wstring(drmConfig.CustomHeaders().Size()));

		// Configure the protection manager with license server URL and custom headers
		if (m_protectionManager)
		{
			m_protectionManager->SetLicenseServerUrl(m_licenseServerUrl);

			if (auto headers = drmConfig.CustomHeaders())
			{
				m_protectionManager->SetCustomHeaders(headers);
			}
		}

		// Set the source URL on the media engine.
		// When the engine detects PlayReady-encrypted content, it triggers
		// the IMFContentProtectionManager callbacks for license acquisition
		// before playback begins.
		BSTR bstrUrl = SysAllocString(url.c_str());
		if (bstrUrl)
		{
			HRESULT hr = m_mediaEngine->SetSource(bstrUrl);
			SysFreeString(bstrUrl);
			NativeLogHresult(L"IMFMediaEngine::SetSource returned", hr);

			if (FAILED(hr))
			{
				FireMediaFailed(static_cast<int32_t>(hr),
					L"Failed to set media source URL");
			}
			else
			{
				NativeLog(L"SetSource accepted; waiting for Media Foundation events");
			}
		}
		else
		{
			NativeLog(L"SetSource failed: SysAllocString returned null");
		}
	}

	void PlayReadyMediaEngine::SetSourceFromManifestBytes(
		Windows::Foundation::Collections::IVectorView<uint8_t> const& manifestData,
		hstring const& originalUrl,
		CommunityToolkit::Maui::Media::WinRT::PlayReadyDrmConfiguration const& drmConfig)
	{
		if (!m_isInitialized || !m_mediaEngine)
		{
			return;
		}

		if (manifestData.Size() == 0)
		{
			FireMediaFailed(E_INVALIDARG, L"Manifest data is empty");
			return;
		}

		m_licenseServerUrl = drmConfig.LicenseServerUrl();
		m_drmConfig = drmConfig;

		if (m_protectionManager)
		{
			m_protectionManager->SetLicenseServerUrl(m_licenseServerUrl);
			if (auto headers = drmConfig.CustomHeaders())
			{
				m_protectionManager->SetCustomHeaders(headers);
			}
		}

		// Use custom MemoryByteStream which properly implements IMFAttributes.
		// The standard MFCreateMFByteStreamOnStream creates a stream whose
		// IMFAttributes implementation doesn't reliably expose the
		// MF_BYTESTREAM_CONTENT_TYPE we set on it, which sends the source
		// resolver into format probing (crash-prone — see below).
		uint32_t dataSize = manifestData.Size();
		std::vector<uint8_t> buffer(dataSize);
		manifestData.GetMany(0, buffer);

		MemoryByteStream* pStream = new MemoryByteStream(buffer.data(), dataSize);
		// The managed side delivers a stitched fragmented-MP4 stream (init +
		// media segments), NOT a DASH manifest: Windows has no
		// application/dash+xml byte-stream handler, and the source resolver's
		// format probing crashes in mfmkvsrcsnk.dll when handed an .mpd.
		// video/mp4 binds to the in-box MP4 source, which parses fMP4 and
		// reports the CENC protection to our IMFContentProtectionManager.
		pStream->SetString(MF_BYTESTREAM_CONTENT_TYPE, L"video/mp4");

		NativeLog(std::wstring(L"MemoryByteStream created: ") + std::to_wstring(dataSize) + L" bytes");

		// Store the stream so the engine can use it asynchronously.
		// SetSourceFromByteStream processes on MF work queues — the
		// engine needs the stream kept alive beyond this call.
		m_sourceByteStream = pStream;

		BSTR bstrUrl = SysAllocString(originalUrl.c_str());
		if (bstrUrl)
		{
			HRESULT hr = m_mediaEngineEx->SetSourceFromByteStream(pStream, bstrUrl);
			SysFreeString(bstrUrl);
			pStream->Release(); // engine took its own reference; release ours
			NativeLogHresult(L"SetSourceFromByteStream(custom) returned", hr);
			if (FAILED(hr))
			{
				m_sourceByteStream.Reset();
				FireMediaFailed(static_cast<int32_t>(hr),
					L"SetSourceFromByteStream failed");
			}
		}
		else
		{
			pStream->Release();
			m_sourceByteStream.Reset();
		}
	}

	// -----------------------------------------------------------------------
	// Playback Control
	// -----------------------------------------------------------------------

	void PlayReadyMediaEngine::Play()
	{
		NativeLog(std::wstring(L"Play requested — engine=") + (m_mediaEngine ? L"true" : L"false"));
		if (m_mediaEngine)
		{
			HRESULT hr = m_mediaEngine->Play();
			NativeLogHresult(L"IMFMediaEngine::Play returned", hr);
			m_isPlaying = SUCCEEDED(hr);
			// The render loop is NOT started here — the protected pipeline is still
			// being constructed at this point and touching it via OnVideoStreamTick /
			// TransferVideoFrame is unsafe. It starts on MF_MEDIA_ENGINE_EVENT_CANPLAY.
		}
	}

	void PlayReadyMediaEngine::Pause()
	{
		NativeLog(L"Pause requested");
		if (m_mediaEngine)
		{
			m_isPlaying = false;
			HRESULT hr = m_mediaEngine->Pause();
			NativeLogHresult(L"IMFMediaEngine::Pause returned", hr);
			// Keep render loop running for potential last-frame display
		}
	}

	void PlayReadyMediaEngine::Seek(double positionSeconds)
	{
		if (m_mediaEngine)
		{
			m_mediaEngine->SetCurrentTime(positionSeconds);
		}
	}

	void PlayReadyMediaEngine::SetVolume(double volume)
	{
		m_volume = volume;
		if (m_mediaEngine && !m_isMuted)
		{
			m_mediaEngine->SetVolume(static_cast<float>(volume));
		}
	}

	void PlayReadyMediaEngine::SetMuted(bool muted)
	{
		m_isMuted = muted;
		if (m_mediaEngine)
		{
			m_mediaEngine->SetMuted(muted);
		}
	}

	void PlayReadyMediaEngine::SetPlaybackRate(double rate)
	{
		m_playbackRate = rate;
		if (m_mediaEngineEx)
		{
			m_mediaEngineEx->SetPlaybackRate(static_cast<float>(rate));
		}
	}

	void PlayReadyMediaEngine::SetLooping(bool looping)
	{
		m_isLooping = looping;
		if (m_mediaEngine)
		{
			m_mediaEngine->SetLoop(looping);
		}
	}

	void PlayReadyMediaEngine::SetStretch(int32_t stretchMode)
	{
		// stretchMode values:
		//   0 = None (no scaling)
		//   1 = UniformToFill (zoom/crop to fill)
		//   2 = Uniform (fit within bounds, maintain aspect ratio)
		m_stretchMode = stretchMode;
	}

	void PlayReadyMediaEngine::ResizeSwapChain(uint32_t width, uint32_t height)
	{
		// No RAII objects in this function — C2712 forbids __try with unwinding.
		__try
		{
			ResizeSwapChainImpl(width, height);
		}
		__except (EngineLogSeh(GetExceptionCode(), "ResizeSwapChain"))
		{
		}
	}

	void PlayReadyMediaEngine::ResizeSwapChainImpl(uint32_t width, uint32_t height)
	{
		// INTENTIONALLY A NO-OP. Calling IDXGISwapChain1::ResizeBuffers on a swap
		// chain bound to a SwapChainPanel crashes the process with 0xc0000005
		// immediately after ResizeBuffers returns S_OK (verified via engine log:
		// resize returned success, then the compositor died before the next
		// render tick). Because the swap chain uses DXGI_SCALING_STRETCH, XAML
		// already scales the fixed-size (1280x720) content to the panel, so
		// resizing is purely cosmetic. If pixel-perfect sizing is ever needed,
		// recreate the swap chain and re-SetSwapChain instead of ResizeBuffers.
		NativeLog(L"ResizeSwapChain ignored (fixed-size swap chain, XAML stretches) — requested "
			+ std::to_wstring(width) + L"x" + std::to_wstring(height));
	}

	void PlayReadyMediaEngine::Close()
	{
		NativeLog(std::wstring(L"Close requested — initialized=") + (m_isInitialized ? L"true" : L"false") + L", playing=" + (m_isPlaying ? L"true" : L"false"));
		StopRenderLoop();

		// Remove the license callback to break circular reference
		if (m_protectionManager)
		{
			m_protectionManager->SetLicenseResultCallback(nullptr);
		}

		// Shut down the media engine — this stops the MF pipeline
		// and releases the protection manager and notify references
		if (m_mediaEngine)
		{
			m_mediaEngine->Shutdown();
			m_mediaEngine.Reset();
		}

		// Release COM pointers in correct order. m_swapChain is guarded
		// because ThreadPoolTimer::Cancel() does not guarantee an
		// in-flight RenderFrame callback has finished running.
		m_mediaEngineEx.Reset();
		m_protectedContent.Reset();
		m_classFactory.Reset();
		m_notify = nullptr;
		m_protectionManager = nullptr;
		m_dxgiManager.Reset();
		{
			std::lock_guard<std::mutex> lock(m_swapChainMutex);
			m_swapChain.Reset();
		}
		m_d3dContext.Reset();
		m_d3dDevice.Reset();

		m_panelNative.Reset();
		m_sourceByteStream.Reset();
		m_swapChainPanelIInspectable = nullptr;
		m_dispatcherQueue = nullptr;
		m_isInitialized = false;

		MFShutdown();
		NativeLog(L"Media Foundation shut down");
	}

	// -----------------------------------------------------------------------
	// Properties
	// -----------------------------------------------------------------------

	double PlayReadyMediaEngine::CurrentPositionSeconds()
	{
		if (m_mediaEngine)
		{
			// SDK 10.0.26100.0+: GetCurrentTime() takes 0 args and returns double directly.
			// (Previously it was HRESULT GetCurrentTime(double*).)
			return m_mediaEngine->GetCurrentTime();
		}
		return 0.0;
	}

	double PlayReadyMediaEngine::DurationSeconds()
	{
		if (m_mediaEngine)
		{
			// SDK 10.0.26100.0+: GetDuration() takes 0 args and returns double directly.
			double duration = m_mediaEngine->GetDuration();
			return (duration == std::numeric_limits<double>::infinity())
				? 0.0 : duration;
		}
		return 0.0;
	}

	uint32_t PlayReadyMediaEngine::NaturalVideoWidth()
	{
		if (m_mediaEngineEx)
		{
			DWORD width = 0, height = 0;
			if (SUCCEEDED(m_mediaEngineEx->GetNativeVideoSize(&width, &height)))
			{
				return width;
			}
		}
		return 0;
	}

	uint32_t PlayReadyMediaEngine::NaturalVideoHeight()
	{
		if (m_mediaEngineEx)
		{
			DWORD width = 0, height = 0;
			if (SUCCEEDED(m_mediaEngineEx->GetNativeVideoSize(&width, &height)))
			{
				return height;
			}
		}
		return 0;
	}

	bool PlayReadyMediaEngine::IsBuffering()
	{
		if (m_mediaEngine)
		{
			// SDK 10.0.26100.0+: GetNetworkState() takes 0 args and returns enum directly.
			return m_mediaEngine->GetNetworkState() == MF_MEDIA_ENGINE_NETWORK_LOADING;
		}
		return false;
	}

	// -----------------------------------------------------------------------
	// Render Loop
	// -----------------------------------------------------------------------

	void PlayReadyMediaEngine::StartRenderLoop()
	{
		if (m_renderTimer)
		{
			return; // Already running
		}

		using namespace winrt::Windows::System::Threading;

		m_renderTimer = ThreadPoolTimer::CreatePeriodicTimer(
			[this](ThreadPoolTimer const&)
			{
				RenderFrame();
			},
			std::chrono::milliseconds(16)); // ~60 fps
	}

	void PlayReadyMediaEngine::StopRenderLoop()
	{
		if (m_renderTimer)
		{
			m_renderTimer.Cancel();
			m_renderTimer = nullptr;
		}
	}

	void PlayReadyMediaEngine::RenderFrame()
	{
		// No RAII objects in this function — C2712 forbids __try with unwinding.
		__try
		{
			RenderFrameImpl();
		}
		__except (EngineLogSeh(GetExceptionCode(), "RenderFrame"))
		{
			StopRenderLoop();
		}
	}

	void PlayReadyMediaEngine::RenderFrameImpl()
	{
		// Log the first few frame milestones (and all failures, capped) so a
		// crash in this window leaves a precise trail in the engine log.
		static int s_framesLogged = 0;
		static int s_failuresLogged = 0;

		if (s_framesLogged < 3)
		{
			NativeLog(L"RenderFrame entry — engine=" + std::to_wstring(m_mediaEngine ? 1 : 0)
				+ L", ex=" + std::to_wstring(m_mediaEngineEx ? 1 : 0)
				+ L", protectedContent=" + std::to_wstring(m_protectedContent ? 1 : 0)
				+ L", swapChain=" + std::to_wstring(m_swapChain ? 1 : 0));
		}

		if (!m_mediaEngine || !m_mediaEngineEx || !m_swapChain)
		{
			return;
		}

		// Serialize against ResizeSwapChain (called on the UI thread) —
		// GetBuffer/Present must not run concurrently with ResizeBuffers.
		std::lock_guard<std::mutex> lock(m_swapChainMutex);

		if (!m_swapChain)
		{
			return; // Swap chain may have been torn down while waiting for the lock
		}

		// Skip rendering if the swap chain hasn't been sized yet
		// (panel hasn't received its layout)
		DXGI_SWAP_CHAIN_DESC1 swapDesc;
		if (SUCCEEDED(m_swapChain->GetDesc1(&swapDesc))
			&& (swapDesc.Width == 0 || swapDesc.Height == 0))
		{
			return;
		}

		// Check if a new video frame is available.
		// SDK 10.0.26100.0+: OnVideoStreamTick() takes LONGLONG* (presentation timestamp),
		// not BOOL*. The return value indicates success/failure.
		LONGLONG pts = 0;
		if (FAILED(m_mediaEngine->OnVideoStreamTick(&pts)))
		{
			return;
		}

		if (s_framesLogged < 3)
		{
			NativeLog(L"OnVideoStreamTick — frame available, pts=" + std::to_wstring(pts));
		}

		// Get the swap chain back buffer
		ComPtr<ID3D11Texture2D> backBuffer;
		HRESULT hr = m_swapChain->GetBuffer(0, IID_PPV_ARGS(&backBuffer));
		if (FAILED(hr) || !backBuffer)
		{
			if (s_failuresLogged < 5)
			{
				NativeLogHresult(L"GetBuffer(0) failed", hr);
				s_failuresLogged++;
			}
			return;
		}

		D3D11_TEXTURE2D_DESC backBufferDesc;
		backBuffer->GetDesc(&backBufferDesc);

		// Compute destination rectangle based on stretch mode
		RECT dstRect;
		DWORD videoWidth = 0, videoHeight = 0;
		m_mediaEngineEx->GetNativeVideoSize(&videoWidth, &videoHeight);

		if (videoWidth == 0 || videoHeight == 0)
		{
			// Video size not yet known — fill entire swap chain
			dstRect = { 0, 0,
				static_cast<LONG>(backBufferDesc.Width),
				static_cast<LONG>(backBufferDesc.Height) };
		}
		else
		{
			switch (m_stretchMode)
			{
			case 0: // None — render at native size, top-left aligned
				dstRect = { 0, 0,
					static_cast<LONG>(videoWidth),
					static_cast<LONG>(videoHeight) };
				break;

			case 1: // UniformToFill — zoom/crop to fill panel
			{
				float panelAspect = static_cast<float>(backBufferDesc.Width)
				                  / static_cast<float>(backBufferDesc.Height);
				float videoAspect = static_cast<float>(videoWidth)
				                  / static_cast<float>(videoHeight);

				if (videoAspect > panelAspect)
				{
					// Video is wider — fit to height, crop width
					LONG h = static_cast<LONG>(backBufferDesc.Height);
					LONG w = static_cast<LONG>(h * videoAspect);
					LONG x = (static_cast<LONG>(backBufferDesc.Width) - w) / 2;
					dstRect = { x, 0, x + w, h };
				}
				else
				{
					// Video is taller — fit to width, crop height
					LONG w = static_cast<LONG>(backBufferDesc.Width);
					LONG h = static_cast<LONG>(w / videoAspect);
					LONG y = (static_cast<LONG>(backBufferDesc.Height) - h) / 2;
					dstRect = { 0, y, w, y + h };
				}
				break;
			}

			default: // 2 = Uniform — fit within bounds, letterbox/pillarbox
			{
				float panelAspect = static_cast<float>(backBufferDesc.Width)
				                  / static_cast<float>(backBufferDesc.Height);
				float videoAspect = static_cast<float>(videoWidth)
				                  / static_cast<float>(videoHeight);

				if (videoAspect > panelAspect)
				{
					// Video is wider — fit to width, letterbox top/bottom
					LONG w = static_cast<LONG>(backBufferDesc.Width);
					LONG h = static_cast<LONG>(w / videoAspect);
					LONG y = (static_cast<LONG>(backBufferDesc.Height) - h) / 2;
					dstRect = { 0, y, w, y + h };
				}
				else
				{
					// Video is taller — fit to height, pillarbox left/right
					LONG h = static_cast<LONG>(backBufferDesc.Height);
					LONG w = static_cast<LONG>(h * videoAspect);
					LONG x = (static_cast<LONG>(backBufferDesc.Width) - w) / 2;
					dstRect = { x, 0, x + w, h };
				}
				break;
			}
			}
		}

		// Source rectangle = full normalized video frame
		MFVideoNormalizedRect srcRect = { 0.0f, 0.0f, 1.0f, 1.0f };

		// Background color = opaque black
		MFARGB bgColor = { 0, 0, 0, 255 };

		// Transfer the video frame from the MF engine to the swap chain back buffer.
		// Protected content MUST use IMFMediaEngineProtectedContent::TransferVideoFrame —
		// the IMFMediaEngineEx variant is not usable with protected content.
		DWORD frameProtectionFlags = 0;
		if (m_protectedContent)
		{
			hr = m_protectedContent->TransferVideoFrame(
				backBuffer.Get(),
				&srcRect,
				&dstRect,
				&bgColor,
				&frameProtectionFlags);
		}
		else
		{
			hr = m_mediaEngineEx->TransferVideoFrame(
				backBuffer.Get(),
				&srcRect,
				&dstRect,
				&bgColor);
		}

		if (SUCCEEDED(hr))
		{
			if (s_framesLogged < 3)
			{
				NativeLog(std::wstring(L"TransferVideoFrame OK — frameProtectionFlags=0x") + winrt::to_hstring(static_cast<uint32_t>(frameProtectionFlags)).c_str() + L"; presenting");
				s_framesLogged++;
			}

			// Present the frame (sync interval 1 = VSync, 0 = immediate)
			HRESULT hrPresent = m_swapChain->Present(1, 0);
			if (FAILED(hrPresent) && s_failuresLogged < 5)
			{
				NativeLogHresult(L"Present failed", hrPresent);
				s_failuresLogged++;
			}
		}
		else if (s_failuresLogged < 5)
		{
			NativeLogHresult(L"TransferVideoFrame failed", hr);
			s_failuresLogged++;
		}
	}

	void PlayReadyMediaEngine::GetSwapChainDimensions(UINT& width, UINT& height)
	{
		width = 0;
		height = 0;

		if (m_swapChain)
		{
			DXGI_SWAP_CHAIN_DESC1 desc;
			if (SUCCEEDED(m_swapChain->GetDesc1(&desc)))
			{
				width = desc.Width;
				height = desc.Height;
			}
		}
	}

	// -----------------------------------------------------------------------
	// Media Engine Event Handlers
	// Called on the UI thread after marshaling by MediaEngineNotify.
	// -----------------------------------------------------------------------

	void PlayReadyMediaEngine::OnMediaEngineEvent(
		ULONG eventCode, ULONG_PTR param1, DWORD param2)
	{
		// No RAII objects in this function — C2712 forbids __try with unwinding.
		__try
		{
			OnMediaEngineEventImpl(eventCode, param1, param2);
		}
		__except (EngineLogSeh(GetExceptionCode(), "OnMediaEngineEvent"))
		{
		}
	}

	void PlayReadyMediaEngine::OnMediaEngineEventImpl(
		ULONG eventCode, ULONG_PTR param1, DWORD param2)
	{
		NativeLog(L"Media Foundation event — code=" + std::to_wstring(eventCode) + L", param1=" + std::to_wstring(param1) + L", param2=" + std::to_wstring(param2));
		switch (eventCode)
		{
		case MF_MEDIA_ENGINE_EVENT_LOADEDMETADATA:
			FireMediaOpened();
			break;

		case MF_MEDIA_ENGINE_EVENT_CANPLAY:
			// Media is ready to play. The protected pipeline is fully constructed
			// at this point, so it is now safe to start the render loop (first
			// playback only — StartRenderLoop is a no-op if already running).
			StartRenderLoop();
			FireStateChanged(
				MF_MEDIA_ENGINE_READY_HAVE_NOTHING,
				MF_MEDIA_ENGINE_READY_HAVE_ENOUGH_DATA);
			break;

		case MF_MEDIA_ENGINE_EVENT_PLAY:
			m_isPlaying = true;
			break;

		case MF_MEDIA_ENGINE_EVENT_PAUSE:
			m_isPlaying = false;
			break;

		case MF_MEDIA_ENGINE_EVENT_ENDED:
			StopRenderLoop();
			m_isPlaying = false;
			FireMediaEnded();
			break;

		case MF_MEDIA_ENGINE_EVENT_TIMEUPDATE:
			FireTimeChanged();
			break;

		case MF_MEDIA_ENGINE_EVENT_ERROR:
			{
				// SDK 10.0.26100.0+: GetError() takes IMFMediaError** (not DWORD*).
				// IMFMediaError has GetErrorCode() and GetExtendedErrorCode() methods.
				// SDK 10.0.26100.0+: GetExtendedErrorCode() returns HRESULT directly.
				HRESULT extendedCode = 0;
				if (m_mediaEngineEx)
				{
					ComPtr<IMFMediaError> spError;
					if (SUCCEEDED(m_mediaEngineEx->GetError(&spError)))
					{
						extendedCode = spError->GetExtendedErrorCode();
					}
				NativeLogHresult(L"Media Foundation media error extended code", extendedCode);
				}

				// Map error to readable message
				hstring message = L"Media engine error";
				switch (static_cast<MF_MEDIA_ENGINE_ERR>(param1))
				{
				case MF_MEDIA_ENGINE_ERR_NOERROR:
					message = L"No error";
					break;
				case MF_MEDIA_ENGINE_ERR_ABORTED:
					message = L"Operation aborted";
					break;
				case MF_MEDIA_ENGINE_ERR_NETWORK:
					message = L"Network error";
					break;
				case MF_MEDIA_ENGINE_ERR_DECODE:
					message = L"Decode error";
					break;
				case MF_MEDIA_ENGINE_ERR_ENCRYPTED:
					message = L"Encrypted content error (DRM failure)";
					break;
				case MF_MEDIA_ENGINE_ERR_SRC_NOT_SUPPORTED:
					message = L"Source format not supported";
					break;
				default:
					message = L"Unknown media engine error";
					break;
				}

				FireMediaFailed(
					static_cast<int32_t>(param1),
					message + L" (extended: 0x" +
					winrt::to_hstring(extendedCode) + L")");
			}
			break;

		case MF_MEDIA_ENGINE_EVENT_FORMATCHANGE:
			{
				DWORD width = 0, height = 0;
				if (m_mediaEngineEx)
				{
					m_mediaEngineEx->GetNativeVideoSize(&width, &height);
				}
				FireSizeChanged(width, height);
			}
			break;

		case MF_MEDIA_ENGINE_EVENT_WAITING:
			// Buffering has started
			FireBufferingChanged(true);
			break;

		case MF_MEDIA_ENGINE_EVENT_CANPLAYTHROUGH:
			// Buffering complete — content can play through without stalling
			FireBufferingChanged(false);
			break;

		case MF_MEDIA_ENGINE_EVENT_SEEKED:
			// Seek completed
			break;

		case MF_MEDIA_ENGINE_EVENT_PROGRESS:
			// Download progress update
			break;

		default:
			break;
		}
	}

	// -----------------------------------------------------------------------
	// WinRT Event Firing Helpers
	// -----------------------------------------------------------------------

	void PlayReadyMediaEngine::FireMediaOpened()
	{
		MediaOpened(*this, nullptr);
	}

	void PlayReadyMediaEngine::FireMediaFailed(
		int32_t errorCode, hstring const& message)
	{
		auto args = winrt::make<MediaEngineErrorEventArgs>(errorCode, message);
		MediaFailed(*this, args);
	}

	void PlayReadyMediaEngine::FireMediaEnded()
	{
		MediaEnded(*this, nullptr);
	}

	void PlayReadyMediaEngine::FireTimeChanged()
	{
		auto args = winrt::make<MediaEngineTimeChangedEventArgs>(
			CurrentPositionSeconds(), DurationSeconds());
		TimeChanged(*this, args);
	}

	void PlayReadyMediaEngine::FireStateChanged(int32_t oldState, int32_t newState)
	{
		auto args = winrt::make<MediaEngineStateChangedEventArgs>(oldState, newState);
		StateChanged(*this, args);
	}

	void PlayReadyMediaEngine::FireLicenseAcquired(
		bool success, hstring const& errorMessage)
	{
		auto args = winrt::make<DrmLicenseAcquiredEventArgs>(success, errorMessage);
		LicenseAcquired(*this, args);
	}

	void PlayReadyMediaEngine::FireSizeChanged(uint32_t width, uint32_t height)
	{
		auto args = winrt::make<MediaEngineSizeChangedEventArgs>(width, height);
		SizeChanged(*this, args);
	}

	void PlayReadyMediaEngine::FireBufferingChanged(bool isBuffering)
	{
		BufferingChanged(*this, isBuffering);
	}
}
