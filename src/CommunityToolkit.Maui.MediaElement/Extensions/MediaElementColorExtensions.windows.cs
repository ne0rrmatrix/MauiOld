using CommunityToolkit.Maui.Primitives;
using Colors = Microsoft.UI.Colors;

namespace CommunityToolkit.Maui.Extensions;

static class MediaElementColorExtensions
{
	/// <summary>
	/// A <see cref="Windows.UI.Color"/> extension method that converts a <see cref="MediaElementColor"/> to a <see cref="Windows.UI.Color"/>.
	/// </summary>
	/// <param name="mediaElementColor"></param>
	/// <returns><see cref="Colors"/></returns>
	public static Windows.UI.Color ToWindowsColor(this MediaElementColor mediaElementColor)
	{
		
		switch (mediaElementColor)
		{
			case MediaElementColor.Black:
				return Colors.Black;
			case MediaElementColor.White:
				return Colors.White;
			case MediaElementColor.Red:
				return Colors.Red;
			case MediaElementColor.Green:
				return Colors.Green;
			case MediaElementColor.Blue:
				return Colors.Blue;
			case MediaElementColor.Yellow:
				return Colors.Yellow;
			case MediaElementColor.Magenta:
				return Colors.Magenta;
			case MediaElementColor.Cyan:
				return Colors.Cyan;
			case MediaElementColor.Transparent:
				return Colors.Transparent;
			default:
				return Colors.Black;
		}
	}
}
