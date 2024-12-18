﻿using CommunityToolkit.Maui.Converters;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Converters;

public class ColorToRgbaStringConverterTests : BaseConverterTest<ColorToRgbaStringConverter>
{
	public static readonly TheoryData<float, float, float, float, string> ValidInputData = new()
	{
		{
			int.MinValue, int.MinValue, int.MinValue, int.MinValue, "RGBA(0,0,0,0)"
		},
		{
			int.MinValue, int.MinValue, int.MinValue, int.MaxValue, "RGBA(0,0,0,1)"
		},
		{
			0, 0, 0, int.MinValue, "RGBA(0,0,0,0)"
		},
		{
			0, 0, 0, -0.5f, "RGBA(0,0,0,0)"
		},
		{
			0, 0, 0, 0, "RGBA(0,0,0,0)"
		},
		{
			0, 0, 0, 0.5f, "RGBA(0,0,0,0.5)"
		},
		{
			0, 0, 0, 1, "RGBA(0,0,0,1)"
		},
		{
			0, 0, 0, int.MaxValue, "RGBA(0,0,0,1)"
		},
		{
			int.MaxValue, int.MaxValue, int.MaxValue, int.MinValue, "RGBA(255,255,255,0)"
		},
		{
			int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, "RGBA(255,255,255,1)"
		},
		{
			0, 0, 1, 0, "RGBA(0,0,255,0)"
		},
		{
			0, 0, 1, 1, "RGBA(0,0,255,1)"
		},
		{
			0, 1, 0, 0, "RGBA(0,255,0,0)"
		},
		{
			0, 1, 0, 1, "RGBA(0,255,0,1)"
		},
		{
			0, 1, 1, 0, "RGBA(0,255,255,0)"
		},
		{
			0, 1, 1, 1, "RGBA(0,255,255,1)"
		},
		{
			1, 0, 0, 0, "RGBA(255,0,0,0)"
		},
		{
			1, 0, 0, 1, "RGBA(255,0,0,1)"
		},
		{
			1, 0, 1, 0, "RGBA(255,0,255,0)"
		},
		{
			1, 0, 1, 1, "RGBA(255,0,255,1)"
		},
		{
			1, 1, 0, 0, "RGBA(255,255,0,0)"
		},
		{
			1, 1, 0, 1, "RGBA(255,255,0,1)"
		},
		{
			1, 1, 1, 0, "RGBA(255,255,255,0)"
		},
		{
			1, 1, 1, 1, "RGBA(255,255,255,1)"
		},
		{
			0.5f, 0, 0, 1, "RGBA(128,0,0,1)"
		},
		{
			0.5f, 0, 0, 0, "RGBA(128,0,0,0)"
		},
		{
			0, 0.5f, 0, 1, "RGBA(0,128,0,1)"
		},
		{
			0, 0.5f, 0, 0, "RGBA(0,128,0,0)"
		},
		{
			0, 0, 0.5f, 1, "RGBA(0,0,128,1)"
		},
		{
			0, 0, 0.5f, 0, "RGBA(0,0,128,0)"
		},
		{
			0.5f, 0.5f, 0.5f, 1, "RGBA(128,128,128,1)"
		},
		{
			0.5f, 0.5f, 0.5f, 0, "RGBA(128,128,128,0)"
		},
		{
			0.25f, 0.25f, 0.25f, 1, "RGBA(64,64,64,1)"
		},
		{
			0.25f, 0.25f, 0.25f, 0, "RGBA(64,64,64,0)"
		},
		{
			0.25f, 0.25f, 1, 1, "RGBA(64,64,255,1)"
		},
		{
			0.25f, 0.25f, 1, 0, "RGBA(64,64,255,0)"
		},
		{
			0.25f, 1, 0.25f, 1, "RGBA(64,255,64,1)"
		},
		{
			0.25f, 1, 0.25f, 0, "RGBA(64,255,64,0)"
		},
		{
			0.75f, 1, 0.25f, 1, "RGBA(191,255,64,1)"
		},
		{
			0.75f, 1, 0.25f, 0, "RGBA(191,255,64,0)"
		},
		{
			0.75f, 0, 1, 1, "RGBA(191,0,255,1)"
		},
		{
			0.75f, 0, 1, 0, "RGBA(191,0,255,0)"
		},
	};

	[Theory]
	[MemberData(nameof(ValidInputData))]
	public void ColorToRgbStringConverterValidInputTest(float red, float green, float blue, float alpha, string expectedResult)
	{
		var converter = new ColorToRgbaStringConverter();
		var color = new Color(red, green, blue, alpha);

		var resultConvert = ((ICommunityToolkitValueConverter)converter).Convert(color, typeof(string), null, new System.Globalization.CultureInfo("en-US"));
		var resultConvertFrom = converter.ConvertFrom(color, new System.Globalization.CultureInfo("en-US"));

		Assert.Equal(expectedResult, resultConvert);
		Assert.Equal(expectedResult, resultConvertFrom);
	}

	[Theory]
	[MemberData(nameof(ValidInputData))]
	public void ColorToRgbStringConverterConvertBackValidInputTest(float red, float green, float blue, float alpha, string colorString)
	{
		var converter = new ColorToRgbaStringConverter();
		var expectedResult = new Color(red, green, blue, alpha);

		var resultConvertBack = ((ICommunityToolkitValueConverter)converter).ConvertBack(colorString, typeof(Color), null, null);
		var resultConvertBackTo = converter.ConvertBackTo(colorString, System.Globalization.CultureInfo.InvariantCulture);

		AssertColorComparison(expectedResult, resultConvertBack as Color);
		AssertColorComparison(expectedResult, resultConvertBackTo);
	}

	[Fact]
	public void ColorToRgbStringConverterCultureTest()
	{
		var expectedResult = "RGBA(0,0,0,0,5)";
		var converter = new ColorToRgbaStringConverter();
		var color = new Color(0, 0, 0, 0.5f);

		var resultConvert = ((ICommunityToolkitValueConverter)converter).Convert(color, typeof(string), null, new System.Globalization.CultureInfo("uk-UA"));
		var resultConvertFrom = converter.ConvertFrom(color, new System.Globalization.CultureInfo("uk-UA"));

		Assert.Equal(expectedResult, resultConvert);
		Assert.Equal(expectedResult, resultConvertFrom);
	}

	[Fact]
	public void ColorToRgbStringConverterNullInputTest()
	{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		Assert.Throws<ArgumentNullException>(() => new ColorToRgbaStringConverter().ConvertFrom(null));
		Assert.Throws<ArgumentNullException>(() => ((ICommunityToolkitValueConverter)new ColorToRgbaStringConverter()).Convert(null, typeof(string), null, null));
		Assert.Throws<ArgumentNullException>(() => ((ICommunityToolkitValueConverter)new ColorToRgbaStringConverter()).Convert(new Color(), null, null, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
	}

	static void AssertColorComparison(Color expectedResult, Color? result)
	{
		const int precision = 2;

		Assert.NotNull(result);
		Assert.Equal(expectedResult.Red, result.Red, precision);
		Assert.Equal(expectedResult.Green, result.Green, precision);
		Assert.Equal(expectedResult.Blue, result.Blue, precision);
	}
}