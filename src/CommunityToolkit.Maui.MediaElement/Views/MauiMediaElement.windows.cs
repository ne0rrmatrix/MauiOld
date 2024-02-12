using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using WinRT.Interop;
using Application = Microsoft.Maui.Controls.Application;
using Button = Microsoft.UI.Xaml.Controls.Button;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Image = Microsoft.UI.Xaml.Controls.Image;
using ImageSource = Microsoft.UI.Xaml.Media.ImageSource;
using Page = Microsoft.Maui.Controls.Page;
using SolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
using Dispatcher = Microsoft.Maui.Dispatching.Dispatcher;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on Windows.
/// </summary>
public class MauiMediaElement : Grid, IDisposable
{
	Popup popup { get; set; } = new();
	Button btn { get; set; }
	Grid grid { get; set; } = new();
	Grid imageButton { get; set; }
	bool isFullScreen = false;
	readonly FullScreenExtension? setFullScreenStatus;
	readonly MediaPlayerElement mediaPlayerElement;
	bool isDisposed;
	DispatcherTimer? timer;
	bool navbarExist;

	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="mediaPlayerElement"></param>
	public MauiMediaElement(MediaPlayerElement mediaPlayerElement)
	{
		this.mediaPlayerElement = mediaPlayerElement;
		setFullScreenStatus = new(GetAppWindowForCurrentWindow());

		ImageSource source = new BitmapImage(new Uri("ms-appx:///whitefs.png"));
		btn = new Button
		{
			Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent),
			Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent),
			Width = 40,
			Height = 40
		};
		btn.Click += Btn_Clicked;
		Image image = new()
		{
			Width = 40,
			Height = 40,
			HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right,
			VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top,
			Source = source,
		};
		imageButton = new()
		{
			HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right,
			VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top,
			Width = 40,
			Height = 40
		};
		imageButton.Children.Add(image);
		imageButton.Children.Add(btn);
		Children.Add(this.mediaPlayerElement);
		Children.Add(imageButton);

		mediaPlayerElement.PointerMoved += MediaPlayerElement_PointerMoved;
		InitializeTimer();
	}

	void MediaPlayerElement_PointerMoved(object sender, PointerRoutedEventArgs e)
	{
		mediaPlayerElement.PointerMoved -= MediaPlayerElement_PointerMoved;
		InitializeTimer();
		imageButton.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
		e.Handled = true;

	}

	void InitializeTimer()
	{
		if (timer is not null)
		{
			return;
		}

		timer = new();
		timer.Tick += OnTimer_Tick;
		timer.Interval = TimeSpan.FromSeconds(5);
		timer.Start();
	}

	void OnTimer_Tick(object? sender, object e)
	{
		ClearTimer();
		imageButton.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
		mediaPlayerElement.PointerMoved += MediaPlayerElement_PointerMoved;
	}

	void ClearTimer()
	{
		if (timer is null)
		{
			return;
		}

		timer.Tick -= OnTimer_Tick;
		timer.Stop();
		timer = null;
	}

	void Btn_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
	{
		var currentPage = CurrentPage;
		if (!isFullScreen)
		{
			setFullScreenStatus?.SetFullScreen();
			navbarExist = Shell.GetNavBarIsVisible(currentPage);
			Shell.SetNavBarIsVisible(CurrentPage, false);
			isFullScreen = true;

			var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
			mediaPlayerElement.Width = displayInfo.Width / displayInfo.Density;
			mediaPlayerElement.Height = displayInfo.Height / displayInfo.Density;

			Children.Clear();
			popup.XamlRoot = mediaPlayerElement.XamlRoot;
			grid.Children.Add(mediaPlayerElement);
			grid.Children.Add(imageButton);
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

		Shell.SetNavBarIsVisible(CurrentPage, navbarExist);
		setFullScreenStatus?.SetFullScreen();
		isFullScreen = false;
		if (popup.IsOpen)
		{
			popup.IsOpen = false;
			popup.Child = null;
			grid.Children.Clear();
		}
		Children.Add(this.mediaPlayerElement);
		Children.Add(this.imageButton);
		var parent = mediaPlayerElement.Parent as FrameworkElement;
		mediaPlayerElement.Width = parent?.Width ?? mediaPlayerElement.Width;
		mediaPlayerElement.Height = parent?.Height ?? mediaPlayerElement.Height;
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
		if (isDisposed)
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