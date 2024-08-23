using CommunityToolkit.Maui.Primitives;
using UIKit;

namespace CommunityToolkit.Maui.Extensions;

static class MediaElementColorExtensions
{
	/// <summary>
	/// A <see cref="UIColor"/> extension method that converts a <see cref="MediaElementColor"/> to a <see cref="UIColor"/>.
	/// </summary>
	/// <param name="mediaElementColor"></param>
	/// <returns><see cref="UIColor"/></returns>
	public static UIColor ToUIColor(this MediaElementColor mediaElementColor)
	{
		switch (mediaElementColor)
		{
			case MediaElementColor.Black:
				return UIColor.Black;
			case MediaElementColor.White:
				return UIColor.White;
			case MediaElementColor.Red:
				return UIColor.Red;
			case MediaElementColor.Green:
				return UIColor.Green;
			case MediaElementColor.Blue:
				return UIColor.Blue;
			case MediaElementColor.Yellow:
				return UIColor.Yellow;
			case MediaElementColor.Magenta:
				return UIColor.Magenta;
			case MediaElementColor.Cyan:
				return UIColor.Cyan;
			case MediaElementColor.Transparent:
				return UIColor.Clear;
			default:
				return UIColor.Black;
		}
	}
}
