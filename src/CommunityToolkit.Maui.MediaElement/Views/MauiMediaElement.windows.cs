using System.Diagnostics;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using WinRT.Interop;
using Application = Microsoft.Maui.Controls.Application;
using Button = Microsoft.UI.Xaml.Controls.Button;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Page = Microsoft.Maui.Controls.Page;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on Windows.
/// </summary>
public class MauiMediaElement : Grid, IDisposable
{
	Popup popup { get; set; } = new();
	Button btn { get; set; } = new();
	Grid grid { get; set; } = new();
	CustomTransportControls transportControls { get; set; }
	bool isFullScreen = false;
	readonly FullScreenExtension? setFullScreenStatus;
	readonly MediaPlayerElement mediaPlayerElement;
	bool isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="mediaPlayerElement"></param>
	public MauiMediaElement(MediaPlayerElement mediaPlayerElement)
	{
		this.mediaPlayerElement = mediaPlayerElement;
		setFullScreenStatus = new(GetAppWindowForCurrentWindow());
		transportControls = new CustomTransportControls()
		{
			IsEnabled = true,
			IsStopButtonVisible = true,
			IsStopEnabled = true,
		};
		transportControls.FullScreen += TransportControls_FullScreen;
		mediaPlayerElement.TransportControls.IsEnabled = false;
		this.mediaPlayerElement.TransportControls = transportControls;

		btn.Content = "Full Screen";
		btn.Width = 120;
		btn.Height = 40;
		btn.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Black);
		btn.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Black);
		btn.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
		btn.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top;
		btn.Click += Btn_Clicked;

		Children.Add(this.mediaPlayerElement);
		Children.Add(btn);
	}

	void TransportControls_FullScreen(object? sender, EventArgs e)
	{
		Debug.WriteLine("Full Screen toggled");
	}

	void Btn_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
	{
		if (!isFullScreen)
		{
			setFullScreenStatus?.SetFullScreen();
			Shell.SetNavBarIsVisible(CurrentPage, false);
			isFullScreen = true;

			var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
			mediaPlayerElement.Width = displayInfo.Width / displayInfo.Density;
			mediaPlayerElement.Height = displayInfo.Height / displayInfo.Density;


			Children.Remove(this.mediaPlayerElement);
			Children.Remove(this.btn);

			popup.XamlRoot = mediaPlayerElement.XamlRoot;
			grid.Children.Add(mediaPlayerElement);
			grid.Children.Add(btn);

			popup.HorizontalOffset = 0;
			popup.VerticalOffset = 0;
			popup.ShouldConstrainToRootBounds = false;
			popup.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center;
			popup.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center;
			popup.Child = grid;

			if (!popup.IsOpen)
			{
				popup.IsOpen = true;
			}
			return;
		}

		Shell.SetNavBarIsVisible(CurrentPage, true);
		setFullScreenStatus?.SetFullScreen();
		isFullScreen = false;
		if (popup.IsOpen)
		{
			popup.IsOpen = false;
			popup.Child = null;
			grid.Children.Clear();
		}
		Children.Add(this.mediaPlayerElement);
		Children.Add(this.btn);
		var parent = mediaPlayerElement.Parent as FrameworkElement;
		if (parent != null)
		{
			mediaPlayerElement.Width = parent.Width;
			mediaPlayerElement.Height = parent.Height;
		}
	}

	static AppWindow GetAppWindowForCurrentWindow()
	{
		// let's cache the CurrentPage here, since the user can navigate or background the app
		// while this method is running
		var currentPage = CurrentPage;

		if (currentPage?.GetParentWindow().Handler.PlatformView is not MauiWinUIWindow window)
		{
			throw new InvalidOperationException();
		}
		var handle = WindowNative.GetWindowHandle(window);
		var id = Win32Interop.GetWindowIdFromWindow(handle);
		return AppWindow.GetFromWindowId(id);
	}

	/// <summary>
	/// Gets the presented page.
	/// </summary>
	protected static Page CurrentPage =>
		PageExtensions.GetCurrentPage(Application.Current?.MainPage ?? throw new InvalidOperationException($"{nameof(Application.Current.MainPage)} cannot be null."));

	/// <summary>
	/// Finalizer
	/// </summary>
	~MauiMediaElement() => Dispose(false);

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MauiMediaElement"/>.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MauiMediaElement"/>.
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if(isDisposed)
		{
			return;
		}

		if (disposing)
		{
			mediaPlayerElement.MediaPlayer.Dispose();
		}

		isDisposed = true;
	}
}