using CommunityToolkit.Maui.Core;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Views.MediaElement;

public class DrmConfigurationTests
{
	[Fact]
	public void DrmConfiguration_DefaultValues()
	{
		var config = new DrmConfiguration();

		Assert.Equal(DrmScheme.Unknown, config.Scheme);
		Assert.Null(config.LicenseServerUrl);
		Assert.Empty(config.LicenseRequestHeaders);
		Assert.False(config.RequiresHardwareSecurity);
	}

	[Fact]
	public void DrmConfiguration_CanSetLicenseServerUrl()
	{
		var config = new DrmConfiguration
		{
			LicenseServerUrl = new Uri("https://license.example.com/wv/acquire")
		};

		Assert.NotNull(config.LicenseServerUrl);
		Assert.Equal("https://license.example.com/wv/acquire", config.LicenseServerUrl.AbsoluteUri);
	}

	[Fact]
	public void DrmConfiguration_RequiresHardwareSecurityDefaultsFalse()
	{
		var config = new DrmConfiguration();

		Assert.False(config.RequiresHardwareSecurity);
	}

	[Fact]
	public void DrmConfiguration_CanSetRequiresHardwareSecurity()
	{
		var config = new DrmConfiguration
		{
			RequiresHardwareSecurity = true
		};

		Assert.True(config.RequiresHardwareSecurity);
	}

	[Theory]
	[InlineData(DrmScheme.Widevine)]
	[InlineData(DrmScheme.PlayReady)]
	[InlineData(DrmScheme.FairPlay)]
	public void DrmConfiguration_SchemeRoundTrip(DrmScheme scheme)
	{
		var config = new DrmConfiguration { Scheme = scheme };

		Assert.Equal(scheme, config.Scheme);
	}

	[Fact]
	public void DrmConfiguration_LicenseRequestHeadersCanBeAdded()
	{
		var config = new DrmConfiguration();

		config.LicenseRequestHeaders["Authorization"] = "Bearer test-token";
		config.LicenseRequestHeaders["X-Custom-Header"] = "custom-value";

		Assert.Equal(2, config.LicenseRequestHeaders.Count);
		Assert.Equal("Bearer test-token", config.LicenseRequestHeaders["Authorization"]);
		Assert.Equal("custom-value", config.LicenseRequestHeaders["X-Custom-Header"]);
	}

	[Fact]
	public void DrmConfiguration_LicenseRequestHeadersIsEmptyByDefault()
	{
		var config = new DrmConfiguration();

		Assert.Empty(config.LicenseRequestHeaders);
	}
}