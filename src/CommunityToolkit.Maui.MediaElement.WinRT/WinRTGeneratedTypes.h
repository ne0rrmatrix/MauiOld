#pragma once

#include "PlayReadyDrmConfiguration.g.h"
#include "MediaEngineErrorEventArgs.g.h"
#include "MediaEngineSizeChangedEventArgs.g.h"
#include "MediaEngineStateChangedEventArgs.g.h"
#include "MediaEngineTimeChangedEventArgs.g.h"
#include "DrmLicenseAcquiredEventArgs.g.h"

namespace winrt::CommunityToolkit::Maui::Media::WinRT::implementation
{
	struct PlayReadyDrmConfiguration : PlayReadyDrmConfigurationT<PlayReadyDrmConfiguration>
	{
		PlayReadyDrmConfiguration();

		hstring LicenseServerUrl();
		void LicenseServerUrl(hstring const& value);
		winrt::Windows::Foundation::Collections::IMap<hstring, hstring> CustomHeaders();
		void CustomHeaders(winrt::Windows::Foundation::Collections::IMap<hstring, hstring> const& value);

	private:
		hstring m_licenseServerUrl;
		winrt::Windows::Foundation::Collections::IMap<hstring, hstring> m_customHeaders{ nullptr };
	};

	struct MediaEngineErrorEventArgs : MediaEngineErrorEventArgsT<MediaEngineErrorEventArgs>
	{
		MediaEngineErrorEventArgs() = default;

		MediaEngineErrorEventArgs(std::int32_t errorCode, hstring const& errorMessage);
		std::int32_t ErrorCode();
		hstring ErrorMessage();

	private:
		std::int32_t m_errorCode{};
		hstring m_errorMessage;
	};

	struct MediaEngineSizeChangedEventArgs : MediaEngineSizeChangedEventArgsT<MediaEngineSizeChangedEventArgs>
	{
		MediaEngineSizeChangedEventArgs() = default;

		MediaEngineSizeChangedEventArgs(std::uint32_t width, std::uint32_t height);
		std::uint32_t Width();
		std::uint32_t Height();

	private:
		std::uint32_t m_width{};
		std::uint32_t m_height{};
	};

	struct MediaEngineStateChangedEventArgs : MediaEngineStateChangedEventArgsT<MediaEngineStateChangedEventArgs>
	{
		MediaEngineStateChangedEventArgs() = default;

		MediaEngineStateChangedEventArgs(std::int32_t oldState, std::int32_t newState);
		std::int32_t OldState();
		std::int32_t NewState();

	private:
		std::int32_t m_oldState{};
		std::int32_t m_newState{};
	};

	struct MediaEngineTimeChangedEventArgs : MediaEngineTimeChangedEventArgsT<MediaEngineTimeChangedEventArgs>
	{
		MediaEngineTimeChangedEventArgs() = default;

		MediaEngineTimeChangedEventArgs(double positionSeconds, double durationSeconds);
		double PositionSeconds();
		double DurationSeconds();

	private:
		double m_positionSeconds{};
		double m_durationSeconds{};
	};

	struct DrmLicenseAcquiredEventArgs : DrmLicenseAcquiredEventArgsT<DrmLicenseAcquiredEventArgs>
	{
		DrmLicenseAcquiredEventArgs() = default;

		DrmLicenseAcquiredEventArgs(bool success, hstring const& errorMessage);
		bool Success();
		hstring ErrorMessage();

	private:
		bool m_success{};
		hstring m_errorMessage;
	};
}

namespace winrt::CommunityToolkit::Maui::Media::WinRT::factory_implementation
{
	struct PlayReadyDrmConfiguration : PlayReadyDrmConfigurationT<PlayReadyDrmConfiguration, implementation::PlayReadyDrmConfiguration>
	{
	};

	struct MediaEngineErrorEventArgs : MediaEngineErrorEventArgsT<MediaEngineErrorEventArgs, implementation::MediaEngineErrorEventArgs>
	{
	};

	struct MediaEngineSizeChangedEventArgs : MediaEngineSizeChangedEventArgsT<MediaEngineSizeChangedEventArgs, implementation::MediaEngineSizeChangedEventArgs>
	{
	};

	struct MediaEngineStateChangedEventArgs : MediaEngineStateChangedEventArgsT<MediaEngineStateChangedEventArgs, implementation::MediaEngineStateChangedEventArgs>
	{
	};

	struct MediaEngineTimeChangedEventArgs : MediaEngineTimeChangedEventArgsT<MediaEngineTimeChangedEventArgs, implementation::MediaEngineTimeChangedEventArgs>
	{
	};

	struct DrmLicenseAcquiredEventArgs : DrmLicenseAcquiredEventArgsT<DrmLicenseAcquiredEventArgs, implementation::DrmLicenseAcquiredEventArgs>
	{
	};
}
