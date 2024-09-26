using CommunityToolkit.Maui.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Button = Microsoft.UI.Xaml.Controls.Button;
namespace CommunityToolkit.Maui.Primitives;

/// <summary>
/// A class that provides a way to create custom transport controls for a <see cref="IMediaElement"/>.
/// </summary>
public sealed class CustomTransportControls : MediaTransportControls
{
	/// <summary>
	/// An event that triggers when the template is loaded.
	/// </summary>
	public event EventHandler<EventArgs>? OnTemplateLoaded;

	/// <summary>
	/// A button for Full screen controls.
	/// </summary>
	public Button? FullScreenButton { get; private set; }

	Button? CCSelectionbutton { get; set; }

	/// <summary>
	/// A <see cref="CustomTransportControls"/> for a <see cref="IMediaElement"/>.
	/// </summary>
	public CustomTransportControls()
	{
		this.DefaultStyleKey = typeof(CustomTransportControls);
	}

	/// <summary>
	/// Add full screen button to the template.
	/// </summary>
	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		var cc = GetTemplateChild("CCSelectionButton") as Button;
		if (cc is not null)
		{
			CCSelectionbutton = cc;
			CCSelectionbutton.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
			CCSelectionbutton.IsEnabled = true;
		}
		var temp = GetTemplateChild("FullWindowButton") as Button;
		if(temp is not null)
		{
			FullScreenButton = temp;
			FullScreenButton.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
			OnTemplateLoaded?.Invoke(this, EventArgs.Empty);
		}
	}
}
