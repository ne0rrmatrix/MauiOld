using Microsoft.UI.Xaml;
using Thickness = Microsoft.UI.Xaml.Thickness;

namespace CommunityToolkit.Maui.Views;
class ElementData
{
	public FrameworkElement? Element { get; set; }
	public double Width { get; set; }
	public double Height { get; set; }
	public Thickness Thickness { get; set; }
}
