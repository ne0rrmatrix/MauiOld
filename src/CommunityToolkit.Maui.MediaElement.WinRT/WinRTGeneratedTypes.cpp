#include "pch.h"
#include "WinRTGeneratedTypes.h"

#include "PlayReadyDrmConfiguration.g.cpp"
#include "MediaEngineErrorEventArgs.g.cpp"
#include "MediaEngineSizeChangedEventArgs.g.cpp"
#include "MediaEngineStateChangedEventArgs.g.cpp"
#include "MediaEngineTimeChangedEventArgs.g.cpp"
#include "DrmLicenseAcquiredEventArgs.g.cpp"

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	// -----------------------------------------------------------------------
	// PlayReadyDrmConfiguration
	// -----------------------------------------------------------------------

	PlayReadyDrmConfiguration::PlayReadyDrmConfiguration()
	{
		m_customHeaders = winrt::single_threaded_map<hstring, hstring>();
	}

	hstring PlayReadyDrmConfiguration::LicenseServerUrl()
	{
		return m_licenseServerUrl;
	}

	void PlayReadyDrmConfiguration::LicenseServerUrl(hstring const& value)
	{
		m_licenseServerUrl = value;
	}

	winrt::Windows::Foundation::Collections::IMap<hstring, hstring> PlayReadyDrmConfiguration::CustomHeaders()
	{
		return m_customHeaders;
	}

	void PlayReadyDrmConfiguration::CustomHeaders(winrt::Windows::Foundation::Collections::IMap<hstring, hstring> const& value)
	{
		m_customHeaders = value;
	}

	// -----------------------------------------------------------------------
	// MediaEngineErrorEventArgs
	// -----------------------------------------------------------------------

	MediaEngineErrorEventArgs::MediaEngineErrorEventArgs(std::int32_t errorCode, hstring const& errorMessage)
		: m_errorCode(errorCode)
		, m_errorMessage(errorMessage)
	{
	}

	std::int32_t MediaEngineErrorEventArgs::ErrorCode()
	{
		return m_errorCode;
	}

	hstring MediaEngineErrorEventArgs::ErrorMessage()
	{
		return m_errorMessage;
	}

	// -----------------------------------------------------------------------
	// MediaEngineSizeChangedEventArgs
	// -----------------------------------------------------------------------

	MediaEngineSizeChangedEventArgs::MediaEngineSizeChangedEventArgs(std::uint32_t width, std::uint32_t height)
		: m_width(width)
		, m_height(height)
	{
	}

	std::uint32_t MediaEngineSizeChangedEventArgs::Width()
	{
		return m_width;
	}

	std::uint32_t MediaEngineSizeChangedEventArgs::Height()
	{
		return m_height;
	}

	// -----------------------------------------------------------------------
	// MediaEngineStateChangedEventArgs
	// -----------------------------------------------------------------------

	MediaEngineStateChangedEventArgs::MediaEngineStateChangedEventArgs(std::int32_t oldState, std::int32_t newState)
		: m_oldState(oldState)
		, m_newState(newState)
	{
	}

	std::int32_t MediaEngineStateChangedEventArgs::OldState()
	{
		return m_oldState;
	}

	std::int32_t MediaEngineStateChangedEventArgs::NewState()
	{
		return m_newState;
	}

	// -----------------------------------------------------------------------
	// MediaEngineTimeChangedEventArgs
	// -----------------------------------------------------------------------

	MediaEngineTimeChangedEventArgs::MediaEngineTimeChangedEventArgs(double positionSeconds, double durationSeconds)
		: m_positionSeconds(positionSeconds)
		, m_durationSeconds(durationSeconds)
	{
	}

	double MediaEngineTimeChangedEventArgs::PositionSeconds()
	{
		return m_positionSeconds;
	}

	double MediaEngineTimeChangedEventArgs::DurationSeconds()
	{
		return m_durationSeconds;
	}

	// -----------------------------------------------------------------------
	// DrmLicenseAcquiredEventArgs
	// -----------------------------------------------------------------------

	DrmLicenseAcquiredEventArgs::DrmLicenseAcquiredEventArgs(bool success, hstring const& errorMessage)
		: m_success(success)
		, m_errorMessage(errorMessage)
	{
	}

	bool DrmLicenseAcquiredEventArgs::Success()
	{
		return m_success;
	}

	hstring DrmLicenseAcquiredEventArgs::ErrorMessage()
	{
		return m_errorMessage;
	}
}
