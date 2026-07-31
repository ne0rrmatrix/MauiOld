using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Views.MediaElement;

public class UriMediaSourceDrmTests
{
	[Fact]
	public void UriMediaSource_DrmConfigurationDefaultsToNull()
	{
		var source = new UriMediaSource();

		Assert.Null(source.DrmConfiguration);
	}

	[Fact]
	public void UriMediaSource_CanSetDrmConfiguration()
	{
		var source = new UriMediaSource
		{
			Uri = new Uri("https://example.com/video.mp4"),
			DrmConfiguration = new DrmConfiguration
			{
				Scheme = DrmScheme.Widevine,
				LicenseServerUrl = new Uri("https://license.example.com/wv"),
			}
		};

		Assert.NotNull(source.DrmConfiguration);
		Assert.Equal(DrmScheme.Widevine, source.DrmConfiguration.Scheme);
		Assert.Equal("https://license.example.com/wv", source.DrmConfiguration.LicenseServerUrl?.AbsoluteUri);
	}

	[Fact]
	public void UriMediaSource_DrmConfigurationCanIncludeCustomHeaders()
	{
		var source = new UriMediaSource
		{
			Uri = new Uri("https://example.com/encrypted.mpd"),
			DrmConfiguration = new DrmConfiguration
			{
				Scheme = DrmScheme.PlayReady,
				LicenseServerUrl = new Uri("https://license.example.com/pr"),
			}
		};
		source.DrmConfiguration.LicenseRequestHeaders["Authorization"] = "Bearer token123";

		Assert.Single(source.DrmConfiguration.LicenseRequestHeaders);
		Assert.Equal("Bearer token123", source.DrmConfiguration.LicenseRequestHeaders["Authorization"]);
	}

	[Fact]
	public void UriMediaSource_DrmConfigurationTriggersSourceChanged()
	{
		var source = new UriMediaSource
		{
			Uri = new Uri("https://example.com/video.mp4"),
		};

		var sourceChangedCount = 0;
		source.SourceChanged += (_, _) => sourceChangedCount++;

		// Setting DrmConfiguration should trigger OnSourceChanged
		source.DrmConfiguration = new DrmConfiguration
		{
			Scheme = DrmScheme.PlayReady,
			LicenseServerUrl = new Uri("https://license.example.com/pr"),
		};

		Assert.Equal(1, sourceChangedCount);
	}

	[Fact]
	public void UriMediaSource_DrmConfigurationSettingToNullTriggersSourceChanged()
	{
		var source = new UriMediaSource
		{
			Uri = new Uri("https://example.com/video.mp4"),
			DrmConfiguration = new DrmConfiguration
			{
				Scheme = DrmScheme.Widevine,
			}
		};

		var sourceChangedCount = 0;
		source.SourceChanged += (_, _) => sourceChangedCount++;

		// Clearing DrmConfiguration should also trigger source change
		source.DrmConfiguration = null;

		Assert.Equal(1, sourceChangedCount);
	}
}