#pragma once

#include "pch.h"
#include <mfapi.h>
#include <mfobjects.h>
#include <mutex>
#include <unordered_map>

/// <summary>
/// Minimal IMFByteStream + IMFAttributes implementation that serves data
/// from an in-memory buffer. Feeds the stitched fragmented-MP4 stream to the
/// Media Foundation source resolver without requiring file system or
/// network access (which are blocked/unreliable in AppX containers).
/// </summary>
class MemoryByteStream : public IMFByteStream, public IMFAttributes
{
public:
	MemoryByteStream(const BYTE* data, DWORD size);
	virtual ~MemoryByteStream();

	// IUnknown
	STDMETHODIMP QueryInterface(REFIID riid, void** ppv) override;
	STDMETHODIMP_(ULONG) AddRef() override;
	STDMETHODIMP_(ULONG) Release() override;

	// IMFByteStream
	STDMETHODIMP GetCapabilities(DWORD* pdwCapabilities) override;
	STDMETHODIMP GetLength(QWORD* pqwLength) override;
	STDMETHODIMP SetLength(QWORD qwLength) override;
	STDMETHODIMP GetCurrentPosition(QWORD* pqwPosition) override;
	STDMETHODIMP SetCurrentPosition(QWORD qwPosition) override;
	STDMETHODIMP IsEndOfStream(BOOL* pfEndOfStream) override;
	STDMETHODIMP Read(BYTE* pb, ULONG cb, ULONG* pcbRead) override;
	STDMETHODIMP BeginRead(BYTE* pb, ULONG cb, IMFAsyncCallback* pCallback, IUnknown* punkState) override;
	STDMETHODIMP EndRead(IMFAsyncResult* pResult, ULONG* pcbRead) override;
	STDMETHODIMP Write(const BYTE* pb, ULONG cb, ULONG* pcbWritten) override;
	STDMETHODIMP BeginWrite(const BYTE* pb, ULONG cb, IMFAsyncCallback* pCallback, IUnknown* punkState) override;
	STDMETHODIMP EndWrite(IMFAsyncResult* pResult, ULONG* pcbWritten) override;
	STDMETHODIMP Seek(MFBYTESTREAM_SEEK_ORIGIN SeekOrigin, LONGLONG llSeekOffset, DWORD dwSeekFlags, QWORD* pqwCurrentPosition) override;
	STDMETHODIMP Flush() override;
	STDMETHODIMP Close() override;

	// IMFAttributes
	STDMETHODIMP GetItem(REFGUID guidKey, PROPVARIANT* pValue) override;
	STDMETHODIMP GetItemType(REFGUID guidKey, MF_ATTRIBUTE_TYPE* pType) override;
	STDMETHODIMP CompareItem(REFGUID guidKey, REFPROPVARIANT Value, BOOL* pbResult) override;
	STDMETHODIMP Compare(IMFAttributes* pTheirs, MF_ATTRIBUTES_MATCH_TYPE MatchType, BOOL* pbResult) override;
	STDMETHODIMP GetUINT32(REFGUID guidKey, UINT32* punValue) override;
	STDMETHODIMP GetUINT64(REFGUID guidKey, UINT64* punValue) override;
	STDMETHODIMP GetDouble(REFGUID guidKey, double* pfValue) override;
	STDMETHODIMP GetGUID(REFGUID guidKey, GUID* pguidValue) override;
	STDMETHODIMP GetStringLength(REFGUID guidKey, UINT32* pcchLength) override;
	STDMETHODIMP GetString(REFGUID guidKey, LPWSTR pwszValue, UINT32 cchBufSize, UINT32* pcchLength) override;
	STDMETHODIMP GetAllocatedString(REFGUID guidKey, LPWSTR* ppwszValue, UINT32* pcchLength) override;
	STDMETHODIMP GetBlobSize(REFGUID guidKey, UINT32* pcbBlobSize) override;
	STDMETHODIMP GetBlob(REFGUID guidKey, UINT8* pBuf, UINT32 cbBufSize, UINT32* pcbBlobSize) override;
	STDMETHODIMP GetAllocatedBlob(REFGUID guidKey, UINT8** ppBuf, UINT32* pcbSize) override;
	STDMETHODIMP GetUnknown(REFGUID guidKey, REFIID riid, LPVOID* ppv) override;
	STDMETHODIMP SetItem(REFGUID guidKey, REFPROPVARIANT Value) override;
	STDMETHODIMP DeleteItem(REFGUID guidKey) override;
	STDMETHODIMP DeleteAllItems() override;
	STDMETHODIMP SetUINT32(REFGUID guidKey, UINT32 unValue) override;
	STDMETHODIMP SetUINT64(REFGUID guidKey, UINT64 unValue) override;
	STDMETHODIMP SetDouble(REFGUID guidKey, double fValue) override;
	STDMETHODIMP SetGUID(REFGUID guidKey, REFGUID guidValue) override;
	STDMETHODIMP SetString(REFGUID guidKey, LPCWSTR wszValue) override;
	STDMETHODIMP SetBlob(REFGUID guidKey, const UINT8* pBuf, UINT32 cbBufSize) override;
	STDMETHODIMP SetUnknown(REFGUID guidKey, IUnknown* pUnknown) override;
	STDMETHODIMP LockStore() override;
	STDMETHODIMP UnlockStore() override;
	STDMETHODIMP GetCount(UINT32* pcItems) override;
	STDMETHODIMP GetItemByIndex(UINT32 unIndex, GUID* pguidKey, PROPVARIANT* pValue) override;
	STDMETHODIMP CopyAllItems(IMFAttributes* pDest) override;

private:
	ULONG m_refCount = 1;
	std::vector<BYTE> m_data;   // Owned copy — the source buffer may be stack-local
	QWORD m_position = 0;

	// Byte counts of reads completed synchronously inside BeginRead, keyed by the
	// IMFAsyncResult handed to the caller. MF learns the actual count from EndRead,
	// not BeginRead — reporting 0 there starves the pipeline (the WAITING stall).
	std::unordered_map<IMFAsyncResult*, ULONG> m_pendingReads;

	// Simple attribute storage for MF_BYTESTREAM_CONTENT_TYPE etc.
	struct StoredAttribute
	{
		GUID key;
		std::wstring stringValue;
	};
	std::vector<StoredAttribute> m_attributes;
	mutable std::mutex m_mutex;
};
