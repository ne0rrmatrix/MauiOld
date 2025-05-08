using CoreGraphics;
using Foundation;
using UIKit;

namespace CommunityToolkit.Maui.Core;

class RequestHandlerProxy(string metadataArtworkUrl, UIImage defaultUIImage)
{
	public UIImage RequestHandler(CGSize size)
	{
		try
		{
			if (metadataArtworkUrl.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
			{
				return UIImage.LoadFromData(NSData.FromUrl(new NSUrl(metadataArtworkUrl))) ?? defaultUIImage;
			}
			return defaultUIImage;
		}
		catch
		{
			return defaultUIImage;
		}
	}
}