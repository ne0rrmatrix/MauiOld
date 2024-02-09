using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using WinRT.Interop;
using Button = Microsoft.UI.Xaml.Controls.Button;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Page = Microsoft.Maui.Controls.Page;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on Windows.
/// </summary>
public class MauiMediaElement : Grid, IDisposable
{
	bool isFullScreen = false;
	CustomBindings? CBinding { get; set; }
	readonly MediaPlayerElement mediaPlayerElement;
	bool isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="mediaPlayerElement"></param>
	public MauiMediaElement(MediaPlayerElement mediaPlayerElement)
	{
		this.mediaPlayerElement = mediaPlayerElement;
		CBinding = new(GetAppWindowForCurrentWindow());
		Button btn = new()
		{
			Content = "Full Screen",
			Width = 120,
			Height = 40,
			HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right,
			VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Top
		};
		btn.Click += Btn_Clicked;
		
		Children.Add(this.mediaPlayerElement);
		Children.Add(btn);
	}

	void Btn_Clicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
	{
		if (!isFullScreen)
		{
			Shell.SetNavBarIsVisible(CurrentPage, false);
			CBinding?.SetFullScreen(mediaPlayerElement);
			isFullScreen = true;
			return;
		}
		Shell.SetNavBarIsVisible(CurrentPage, true);
		CBinding?.SetFullScreen(mediaPlayerElement);
		isFullScreen = false;
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