#pragma once

#include "pch.h"

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	/// <summary>
	/// Initializes the file logger (resolves the app's LocalState path).
	/// Call once during engine Initialize.
	/// </summary>
	void EngineLogInit();

	/// <summary>
	/// Appends a message to playready-engine.log in the app's LocalState folder
	/// and mirrors it to OutputDebugStringW.
	/// </summary>
	void EngineLog(std::wstring const& message);

	/// <summary>Logs operation name + HRESULT in hex.</summary>
	void EngineLogHresult(std::wstring const& operation, HRESULT hr);

	/// <summary>
	/// SEH filter: logs the structured exception code and faulting site name,
	/// then returns EXCEPTION_EXECUTE_HANDLER.
	/// </summary>
	int EngineLogSeh(unsigned int exceptionCode, char const* site);
}
