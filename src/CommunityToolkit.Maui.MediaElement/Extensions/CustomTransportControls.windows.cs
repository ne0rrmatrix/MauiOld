using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Button = Microsoft.UI.Xaml.Controls.Button;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// 
/// </summary>
public sealed class CustomTransportControls : MediaTransportControls
{	
	/// <summary>
	/// 
	/// </summary>
	public event EventHandler<EventArgs>? FullScreen;

	/// <summary>
	/// 
	/// </summary>
	public CustomTransportControls()
	{
		this.DefaultStyleKey = typeof(MediaTransportControls);

	}

	/// <summary>
	/// 
	/// </summary>
	protected override void OnApplyTemplate()
	{
		Button? FullWindowButton = GetTemplateChild("FullWindowButton") as Button;
		if (FullWindowButton is null)
		{
			Debug.WriteLine("fsButton is null!, Template not applied");
		}
		if (FullWindowButton is not null)
		{
			FullWindowButton.Click += FullScreenButton_Click;
		}
		base.OnApplyTemplate();
	}


	void FullScreenButton_Click(object sender, RoutedEventArgs e)
	{
		FullScreen?.Invoke(this, EventArgs.Empty);
	}
}
