#include "pch.h"
#include "MemoryByteStream.h"
#include "CrashLog.h"
#include <Mferror.h>

using namespace Microsoft::WRL;

namespace
{
	void ByteStreamLog(std::wstring const& message)
	{
		winrt::CommunityToolkit::Maui::Media::WinRT::implementation::EngineLog(L"[MediaElement.WinRT.ByteStream] " + message);
	}

	// Read logging is capped — MF can issue thousands of reads once playback runs.
	int g_readsLogged = 0;
	constexpr int kMaxReadLogs = 40;
}

MemoryByteStream::MemoryByteStream(const BYTE* data, DWORD size)
	: m_data(data, data + size)
{
}

MemoryByteStream::~MemoryByteStream()
{
}

// ---------------------------------------------------------------------------
// IUnknown
// ---------------------------------------------------------------------------

STDMETHODIMP MemoryByteStream::QueryInterface(REFIID riid, void** ppv)
{
	if (!ppv) return E_POINTER;
	if (riid == __uuidof(IUnknown) || riid == __uuidof(IMFByteStream))
	{
		*ppv = static_cast<IMFByteStream*>(this);
	}
	else if (riid == __uuidof(IMFAttributes))
	{
		*ppv = static_cast<IMFAttributes*>(this);
	}
	else
	{
		*ppv = nullptr;
		return E_NOINTERFACE;
	}
	AddRef();
	return S_OK;
}

STDMETHODIMP_(ULONG) MemoryByteStream::AddRef()
{
	return InterlockedIncrement(&m_refCount);
}

STDMETHODIMP_(ULONG) MemoryByteStream::Release()
{
	ULONG count = InterlockedDecrement(&m_refCount);
	if (count == 0)
	{
		delete this;
	}
	return count;
}

// ---------------------------------------------------------------------------
// IMFByteStream
// ---------------------------------------------------------------------------

STDMETHODIMP MemoryByteStream::GetCapabilities(DWORD* pdwCapabilities)
{
	if (!pdwCapabilities) return E_POINTER;
	*pdwCapabilities = MFBYTESTREAM_IS_READABLE | MFBYTESTREAM_IS_SEEKABLE | MFBYTESTREAM_DOES_NOT_USE_NETWORK;
	return S_OK;
}

STDMETHODIMP MemoryByteStream::GetLength(QWORD* pqwLength)
{
	if (!pqwLength) return E_POINTER;
	*pqwLength = m_data.size();
	return S_OK;
}

STDMETHODIMP MemoryByteStream::SetLength(QWORD)
{
	return E_NOTIMPL;
}

STDMETHODIMP MemoryByteStream::GetCurrentPosition(QWORD* pqwPosition)
{
	if (!pqwPosition) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	*pqwPosition = m_position;
	return S_OK;
}

STDMETHODIMP MemoryByteStream::SetCurrentPosition(QWORD qwPosition)
{
	std::lock_guard<std::mutex> lock(m_mutex);
	if (qwPosition > m_data.size())
	{
		ByteStreamLog(L"SetCurrentPosition REJECTED — pos=" + std::to_wstring(qwPosition) + L", size=" + std::to_wstring(m_data.size()));
		return E_INVALIDARG;
	}
	if (g_readsLogged < kMaxReadLogs)
	{
		g_readsLogged++;
		ByteStreamLog(L"SetCurrentPosition — pos=" + std::to_wstring(qwPosition));
	}
	m_position = qwPosition;
	return S_OK;
}

STDMETHODIMP MemoryByteStream::IsEndOfStream(BOOL* pfEndOfStream)
{
	if (!pfEndOfStream) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	*pfEndOfStream = (m_position >= m_data.size()) ? TRUE : FALSE;
	return S_OK;
}

STDMETHODIMP MemoryByteStream::Read(BYTE* pb, ULONG cb, ULONG* pcbRead)
{
	if (!pb || !pcbRead) return E_POINTER;

	std::lock_guard<std::mutex> lock(m_mutex);

	ULONG available = static_cast<ULONG>(m_data.size() - m_position);
	ULONG toRead = (cb < available) ? cb : available;

	memcpy(pb, m_data.data() + m_position, toRead);

	if (g_readsLogged < kMaxReadLogs)
	{
		g_readsLogged++;
		ByteStreamLog(L"Read — pos=" + std::to_wstring(m_position) + L", cb=" + std::to_wstring(cb) + L", returned=" + std::to_wstring(toRead));
	}

	m_position += toRead;
	*pcbRead = toRead;

	return S_OK;
}

STDMETHODIMP MemoryByteStream::BeginRead(BYTE* pb, ULONG cb, IMFAsyncCallback* pCallback, IUnknown* punkState)
{
	if (!pb || !pCallback) return E_POINTER;

	// Synchronous read wrapped in async completion
	ULONG cbRead = 0;
	HRESULT hr = Read(pb, cb, &cbRead);

	if (g_readsLogged < kMaxReadLogs)
	{
		g_readsLogged++;
		ByteStreamLog(L"BeginRead — cb=" + std::to_wstring(cb) + L", returned=" + std::to_wstring(cbRead));
	}

	ComPtr<IMFAsyncResult> spResult;
	HRESULT hrCreate = MFCreateAsyncResult(nullptr, pCallback, punkState, &spResult);
	if (SUCCEEDED(hrCreate))
	{
		spResult->SetStatus(hr);

		// Record the count BEFORE invoking — the callback may call EndRead
		// synchronously on this thread before BeginRead returns.
		{
			std::lock_guard<std::mutex> lock(m_mutex);
			m_pendingReads[spResult.Get()] = cbRead;
		}

		pCallback->Invoke(spResult.Get());
	}

	return hr;
}

STDMETHODIMP MemoryByteStream::EndRead(IMFAsyncResult* pResult, ULONG* pcbRead)
{
	if (!pResult || !pcbRead) return E_POINTER;

	// Report the count captured by BeginRead. Under the IMFByteStream async
	// contract MF learns the bytes actually read here, not from BeginRead —
	// always returning 0 starves the pipeline (the WAITING-forever stall).
	{
		std::lock_guard<std::mutex> lock(m_mutex);
		auto it = m_pendingReads.find(pResult);
		if (it == m_pendingReads.end())
		{
			return E_INVALIDARG;
		}
		*pcbRead = it->second;
		m_pendingReads.erase(it);
	}

	if (g_readsLogged < kMaxReadLogs)
	{
		g_readsLogged++;
		ByteStreamLog(L"EndRead — reported=" + std::to_wstring(*pcbRead));
	}

	return pResult->GetStatus();
}

STDMETHODIMP MemoryByteStream::Write(const BYTE*, ULONG, ULONG* pcbWritten)
{
	if (pcbWritten) *pcbWritten = 0;
	return E_NOTIMPL;
}

STDMETHODIMP MemoryByteStream::BeginWrite(const BYTE*, ULONG, IMFAsyncCallback*, IUnknown*)
{
	return E_NOTIMPL;
}

STDMETHODIMP MemoryByteStream::EndWrite(IMFAsyncResult*, ULONG* pcbWritten)
{
	if (pcbWritten) *pcbWritten = 0;
	return E_NOTIMPL;
}

STDMETHODIMP MemoryByteStream::Seek(MFBYTESTREAM_SEEK_ORIGIN SeekOrigin, LONGLONG llSeekOffset, DWORD /*dwSeekFlags*/, QWORD* pqwCurrentPosition)
{
	std::lock_guard<std::mutex> lock(m_mutex);

	LONGLONG newPos = 0;
	switch (SeekOrigin)
	{
	case msoBegin:
		newPos = llSeekOffset;
		break;
	case msoCurrent:
		newPos = static_cast<LONGLONG>(m_position) + llSeekOffset;
		break;
	default:
		return E_INVALIDARG;
	}

	if (newPos < 0 || static_cast<QWORD>(newPos) > static_cast<QWORD>(m_data.size()))
	{
		ByteStreamLog(L"Seek REJECTED — origin=" + std::to_wstring(static_cast<int>(SeekOrigin)) + L", offset=" + std::to_wstring(llSeekOffset) + L", size=" + std::to_wstring(m_data.size()));
		return E_INVALIDARG;
	}

	m_position = static_cast<QWORD>(newPos);

	if (g_readsLogged < kMaxReadLogs)
	{
		g_readsLogged++;
		ByteStreamLog(L"Seek — origin=" + std::to_wstring(static_cast<int>(SeekOrigin)) + L", offset=" + std::to_wstring(llSeekOffset) + L", newPos=" + std::to_wstring(m_position));
	}

	if (pqwCurrentPosition)
	{
		*pqwCurrentPosition = m_position;
	}

	return S_OK;
}

STDMETHODIMP MemoryByteStream::Flush()
{
	return S_OK;
}

STDMETHODIMP MemoryByteStream::Close()
{
	return S_OK;
}

// ---------------------------------------------------------------------------
// IMFAttributes — minimal implementation storing string values by GUID key
// ---------------------------------------------------------------------------

STDMETHODIMP MemoryByteStream::SetString(REFGUID guidKey, LPCWSTR wszValue)
{
	std::lock_guard<std::mutex> lock(m_mutex);
	// Replace existing or add new
	for (auto& attr : m_attributes)
	{
		if (attr.key == guidKey)
		{
			attr.stringValue = wszValue ? wszValue : L"";
			return S_OK;
		}
	}
	m_attributes.push_back({ guidKey, wszValue ? wszValue : L"" });
	return S_OK;
}

STDMETHODIMP MemoryByteStream::GetString(REFGUID guidKey, LPWSTR pwszValue, UINT32 cchBufSize, UINT32* pcchLength)
{
	std::lock_guard<std::mutex> lock(m_mutex);
	for (auto& attr : m_attributes)
	{
		if (attr.key == guidKey)
		{
			UINT32 len = static_cast<UINT32>(attr.stringValue.length()) + 1;
			if (pcchLength) *pcchLength = len;
			if (pwszValue && cchBufSize >= len)
			{
				wcscpy_s(pwszValue, cchBufSize, attr.stringValue.c_str());
				return S_OK;
			}
			return pwszValue ? MF_E_BUFFERTOOSMALL : S_OK;
		}
	}
	return MF_E_ATTRIBUTENOTFOUND;
}

STDMETHODIMP MemoryByteStream::GetAllocatedString(REFGUID guidKey, LPWSTR* ppwszValue, UINT32* pcchLength)
{
	if (!ppwszValue) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	for (auto& attr : m_attributes)
	{
		if (attr.key == guidKey)
		{
			UINT32 len = static_cast<UINT32>(attr.stringValue.length()) + 1;
			if (pcchLength) *pcchLength = len;
			*ppwszValue = static_cast<LPWSTR>(CoTaskMemAlloc(len * sizeof(wchar_t)));
			if (*ppwszValue)
			{
				wcscpy_s(*ppwszValue, len, attr.stringValue.c_str());
				return S_OK;
			}
			return E_OUTOFMEMORY;
		}
	}
	return MF_E_ATTRIBUTENOTFOUND;
}

STDMETHODIMP MemoryByteStream::GetStringLength(REFGUID guidKey, UINT32* pcchLength)
{
	if (!pcchLength) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	for (auto& attr : m_attributes)
	{
		if (attr.key == guidKey)
		{
			*pcchLength = static_cast<UINT32>(attr.stringValue.length()) + 1;
			return S_OK;
		}
	}
	return MF_E_ATTRIBUTENOTFOUND;
}

STDMETHODIMP MemoryByteStream::GetItemType(REFGUID guidKey, MF_ATTRIBUTE_TYPE* pType)
{
	if (!pType) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	for (auto& attr : m_attributes)
	{
		if (attr.key == guidKey)
		{
			*pType = MF_ATTRIBUTE_STRING;
			return S_OK;
		}
	}
	return MF_E_ATTRIBUTENOTFOUND;
}

STDMETHODIMP MemoryByteStream::GetItem(REFGUID guidKey, PROPVARIANT* pValue)
{
	if (!pValue) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	for (auto& attr : m_attributes)
	{
		if (attr.key == guidKey)
		{
			PropVariantClear(pValue);
			pValue->vt = VT_LPWSTR;
			pValue->pwszVal = static_cast<LPWSTR>(CoTaskMemAlloc((attr.stringValue.length() + 1) * sizeof(wchar_t)));
			if (pValue->pwszVal)
			{
				wcscpy_s(pValue->pwszVal, attr.stringValue.length() + 1, attr.stringValue.c_str());
				return S_OK;
			}
			return E_OUTOFMEMORY;
		}
	}
	return MF_E_ATTRIBUTENOTFOUND;
}

STDMETHODIMP MemoryByteStream::GetCount(UINT32* pcItems)
{
	if (!pcItems) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	*pcItems = static_cast<UINT32>(m_attributes.size());
	return S_OK;
}

STDMETHODIMP MemoryByteStream::GetItemByIndex(UINT32 unIndex, GUID* pguidKey, PROPVARIANT* pValue)
{
	std::lock_guard<std::mutex> lock(m_mutex);
	if (unIndex >= m_attributes.size()) return E_INVALIDARG;
	if (pguidKey) *pguidKey = m_attributes[unIndex].key;
	if (pValue)
	{
		PropVariantClear(pValue);
		pValue->vt = VT_LPWSTR;
		pValue->pwszVal = static_cast<LPWSTR>(CoTaskMemAlloc(
			(m_attributes[unIndex].stringValue.length() + 1) * sizeof(wchar_t)));
		if (pValue->pwszVal)
		{
			wcscpy_s(pValue->pwszVal,
				m_attributes[unIndex].stringValue.length() + 1,
				m_attributes[unIndex].stringValue.c_str());
		}
	}
	return S_OK;
}

STDMETHODIMP MemoryByteStream::CopyAllItems(IMFAttributes* pDest)
{
	if (!pDest) return E_POINTER;
	std::lock_guard<std::mutex> lock(m_mutex);
	for (auto& attr : m_attributes)
	{
		pDest->SetString(attr.key, attr.stringValue.c_str());
	}
	return S_OK;
}

STDMETHODIMP MemoryByteStream::LockStore() { return S_OK; }
STDMETHODIMP MemoryByteStream::UnlockStore() { return S_OK; }
STDMETHODIMP MemoryByteStream::DeleteItem(REFGUID) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::DeleteAllItems() { m_attributes.clear(); return S_OK; }

// Unsupported IMFAttributes methods
STDMETHODIMP MemoryByteStream::CompareItem(REFGUID, REFPROPVARIANT, BOOL* pbResult) { if (pbResult) *pbResult = FALSE; return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::Compare(IMFAttributes*, MF_ATTRIBUTES_MATCH_TYPE, BOOL* pbResult) { if (pbResult) *pbResult = FALSE; return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::GetUINT32(REFGUID, UINT32*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetUINT64(REFGUID, UINT64*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetDouble(REFGUID, double*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetGUID(REFGUID, GUID*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetBlobSize(REFGUID, UINT32*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetBlob(REFGUID, UINT8*, UINT32, UINT32*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetAllocatedBlob(REFGUID, UINT8**, UINT32*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::GetUnknown(REFGUID, REFIID, LPVOID*) { return MF_E_ATTRIBUTENOTFOUND; }
STDMETHODIMP MemoryByteStream::SetItem(REFGUID, REFPROPVARIANT) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::SetUINT32(REFGUID, UINT32) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::SetUINT64(REFGUID, UINT64) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::SetDouble(REFGUID, double) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::SetGUID(REFGUID, REFGUID) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::SetBlob(REFGUID, const UINT8*, UINT32) { return E_NOTIMPL; }
STDMETHODIMP MemoryByteStream::SetUnknown(REFGUID, IUnknown*) { return E_NOTIMPL; }
