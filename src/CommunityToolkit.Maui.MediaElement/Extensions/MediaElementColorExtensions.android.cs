using CommunityToolkit.Maui.Primitives;
using Color = Android.Graphics.Color;

namespace CommunityToolkit.Maui.Extensions;

static class MediaElementColorExtensions
{
	/// <summary>
	/// A <see cref="Android.Graphics.Color"/> extension method that converts a <see cref="MediaElementColor"/> to a <see cref="Android.Graphics.Color"/>.
	/// </summary>
	/// <param name="mediaElementColor"></param>
	/// <returns><see cref="Android.Graphics.Color"/></returns>
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
