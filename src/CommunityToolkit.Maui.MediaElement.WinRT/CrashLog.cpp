#include "pch.h"
#include "CrashLog.h"

#include <mutex>
#include <string>
#include <winrt/Windows.Storage.h>

namespace
{
	std::wstring g_logPath;
	std::mutex g_logMutex;

	std::wstring ResolveLogPath()
	{
		try
		{
			// Packaged app: LocalState is writable inside the AppContainer.
			auto folder = winrt::Windows::Storage::ApplicationData::Current().LocalFolder();
			return std::wstring(folder.Path()) + L"\\playready-engine.log";
		}
		catch (...)
		{
			wchar_t tempPath[MAX_PATH]{};
			DWORD len = GetTempPathW(MAX_PATH, tempPath);
			return len ? (std::wstring(tempPath) + L"playready-engine.log") : L"playready-engine.log";
		}
	}

	void AppendToFile(std::wstring const& line)
	{
		if (g_logPath.empty())
		{
			return;
		}

		HANDLE file = CreateFileW(
			g_logPath.c_str(),
			FILE_APPEND_DATA,
			FILE_SHARE_READ,
			nullptr,
			OPEN_ALWAYS,
			FILE_ATTRIBUTE_NORMAL,
			nullptr);

		if (file == INVALID_HANDLE_VALUE)
		{
			return;
		}

		// UTF-8 output for easy reading.
		int utf8Len = WideCharToMultiByte(CP_UTF8, 0, line.c_str(), -1, nullptr, 0, nullptr, nullptr);
		if (utf8Len > 1)
		{
			std::string utf8(static_cast<size_t>(utf8Len) - 1, '\0');
			WideCharToMultiByte(CP_UTF8, 0, line.c_str(), -1, utf8.data(), utf8Len, nullptr, nullptr);
			DWORD written = 0;
			WriteFile(file, utf8.data(), static_cast<DWORD>(utf8.size()), &written, nullptr);
		}

		CloseHandle(file);
	}
}

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	void EngineLogInit()
	{
		g_logPath = ResolveLogPath();

		SYSTEMTIME now{};
		GetLocalTime(&now);
		wchar_t stamp[128]{};
		swprintf_s(stamp, L"=== engine log start %04d-%02d-%02d %02d:%02d:%02d ===",
			now.wYear, now.wMonth, now.wDay, now.wHour, now.wMinute, now.wSecond);
		EngineLog(stamp);
		EngineLog(L"log path: " + g_logPath);
	}

	void EngineLog(std::wstring const& message)
	{
		std::lock_guard<std::mutex> lock(g_logMutex);
		OutputDebugStringW((message + L"\n").c_str());
		AppendToFile(message + L"\r\n");
	}

	void EngineLogHresult(std::wstring const& operation, HRESULT hr)
	{
		wchar_t buffer[512]{};
		swprintf_s(buffer, L"%s — HRESULT=0x%08X", operation.c_str(), static_cast<unsigned>(hr));
		EngineLog(buffer);
	}

	int EngineLogSeh(unsigned int exceptionCode, char const* site)
	{
		wchar_t buffer[512]{};
		swprintf_s(buffer, L"!!! STRUCTURED EXCEPTION 0x%08X at %S", exceptionCode, site ? site : "unknown");
		EngineLog(buffer);
		return EXCEPTION_EXECUTE_HANDLER;
	}
}
