using CommunityToolkit.Maui.Primitives;
using Color = Android.Graphics.Color;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// 
/// </summary>
public static class MediaElementColorExtensions
{
	public static Color ToAndroidColor(this MediaElementColor mediaElementColor)
	{
		switch (mediaElementColor)
		{
			case MediaElementColor.Black:
				return Color.Black;
			case MediaElementColor.White:
				return Color.White;
			case MediaElementColor.Red:
				return Color.Red;
			case MediaElementColor.Green:
				return Color.Green;
			case MediaElementColor.Blue:
				return Color.Blue;
			case MediaElementColor.Yellow:
				return Color.Yellow;
			case MediaElementColor.Magenta:
				return Color.Magenta;
			case MediaElementColor.Cyan:
				return Color.Cyan;
			case MediaElementColor.Transparent:
				return Color.Transparent;
			default:
				return Color.Black;
		}
	}
}
