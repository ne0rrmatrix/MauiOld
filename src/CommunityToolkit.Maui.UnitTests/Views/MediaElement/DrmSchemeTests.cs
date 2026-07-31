using CommunityToolkit.Maui.Core;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Views.MediaElement;

public class DrmSchemeTests
{
	[Fact]
	public void DrmScheme_UnknownIsZero()
	{
		Assert.Equal(0, (int)DrmScheme.Unknown);
	}

	[Fact]
	public void DrmScheme_ExplicitValues()
	{
		Assert.Equal(0, (int)DrmScheme.Unknown);
		Assert.Equal(1, (int)DrmScheme.Widevine);
		Assert.Equal(2, (int)DrmScheme.PlayReady);
		Assert.Equal(3, (int)DrmScheme.FairPlay);
	}

	[Fact]
	public void DrmScheme_CountIsFour()
	{
		var values = Enum.GetValues<DrmScheme>();
		Assert.Equal(4, values.Length);
	}

	[Fact]
	public void DrmScheme_Names()
	{
		Assert.Equal("Unknown", DrmScheme.Unknown.ToString());
		Assert.Equal("Widevine", DrmScheme.Widevine.ToString());
		Assert.Equal("PlayReady", DrmScheme.PlayReady.ToString());
		Assert.Equal("FairPlay", DrmScheme.FairPlay.ToString());
	}
}