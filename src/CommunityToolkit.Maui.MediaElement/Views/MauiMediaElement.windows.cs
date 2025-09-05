using System.Reflection;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using WinRT.Interop;
using Application = Microsoft.Maui.Controls.Application;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Page = Microsoft.Maui.Controls.Page;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on Windows.
/// </summary>
public partial class MauiMediaElement : Grid, IDisposable
{
	static readonly AppWindow appWindow = GetAppWindowForCurrentWindow();
	readonly Popup popup = new();
	readonly Grid fullScreenGrid = new();
	readonly MediaPlayerElement mediaPlayerElement;
	readonly CustomTransportControls? customTransportControls;
	bool doesNavigationBarExistBeforeFullScreen;
	bool isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="mediaPlayerElement"></param>
	public MauiMediaElement(MediaPlayerElement mediaPlayerElement)
	{
		LoadResourceDictionary();
		this.mediaPlayerElement = mediaPlayerElement;
		customTransportControls = SetTransportControls();
		Children.Add(this.mediaPlayerElement);
	}

	/// <summary>
	/// Event that gets raised when track selection is requested
	/// </summary>
	public event EventHandler<MediaTrackType>? TrackSelectionRequested;

	/// <summary>
	/// Event that gets raised when a track is selected
	/// </summary>
	public event Action<CommunityToolkit.Maui.Core.MediaTrack>? OnTrackSelected;

	void LoadResourceDictionary()
	{
		var assembly = Assembly.GetExecutingAssembly();
		using Stream? stream = assembly.GetManifestResourceStream("ResourceDictionary.windows.xaml");
		if (stream is null)
		{
			return;
		}
		using StreamReader reader = new(stream);
		var xaml = reader.ReadToEnd();
		var resourceDictionary = (Microsoft.UI.Xaml.ResourceDictionary)XamlReader.Load(xaml);
		if (resourceDictionary is null)
		{
			return;
		}
		this.Resources.MergedDictionaries.Add(resourceDictionary);
	}
	
	void ApplyCustomStyle()
	{
		if (this.Resources.TryGetValue("customTransportcontrols", out object styleObj) &&
			styleObj is Microsoft.UI.Xaml.Style customStyle && mediaPlayerElement is not null && mediaPlayerElement.TransportControls is not null)
		{
			mediaPlayerElement.TransportControls.Style = customStyle;
		}
	}

	CustomTransportControls SetTransportControls()
	{
		mediaPlayerElement.TransportControls.IsEnabled = false;
		var temp = new CustomTransportControls()
		{
			IsZoomButtonVisible = true,
			IsZoomEnabled = true,
			IsVolumeButtonVisible = true,
			IsVolumeEnabled = true,
			IsSeekBarVisible = true,
			IsSeekEnabled = true,
			IsEnabled = true,
			IsRepeatButtonVisible = true,
			IsRepeatEnabled = true,
			IsNextTrackButtonVisible = true,
			IsPreviousTrackButtonVisible = true,
			IsFastForwardButtonVisible = true,
			IsFastForwardEnabled = true,
			IsFastRewindButtonVisible = true,
			IsFastRewindEnabled = true,
			IsPlaybackRateButtonVisible = true,
			IsPlaybackRateEnabled = true,
			IsCompact = false,
		};
		
		temp.OnTemplateLoaded += (s, e) =>
		{
			if (temp.FullScreenButton is null)
			{
				return;
			}
			temp.FullScreenButton.Click += OnFullScreenButtonClick;
		};

		temp.TrackSelectionRequested += (s, trackType) =>
		{
			TrackSelectionRequested?.Invoke(this, trackType);
		};

		mediaPlayerElement.TransportControls = temp;
		ApplyCustomStyle();
		return temp;
	}

	/// <summary>
	/// Updates track availability and shows/hides track selection buttons
	/// </summary>
	/// <param name="tracks">Available tracks</param>
	public void UpdateTrackAvailability(IEnumerable<CommunityToolkit.Maui.Core.MediaTrack> tracks)
	{
		if (customTransportControls is null)
		{
			return;
		}

		var trackList = tracks.ToList();
		var audioTracks = trackList.Where(t => t.Type == MediaTrackType.Audio).ToList();
		var subtitleTracks = trackList.Where(t => t.Type == MediaTrackType.Text).ToList();
		var videoTracks = trackList.Where(t => t.Type == MediaTrackType.Video).ToList();

		// Update track selection UI first
		UpdateTrackSelectionUI(trackList);

		// Show/hide audio track button
		if (customTransportControls.AudioTracksSelectionButton is not null)
		{
			customTransportControls.AudioTracksSelectionButton.Visibility = 
				audioTracks.Count > 1 ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
		}

		// Show/hide subtitle track button
		if (customTransportControls.CCSelectionButton is not null)
		{
			customTransportControls.CCSelectionButton.Visibility = 
				subtitleTracks.Count > 0 ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
		}

		// Show/hide video track button
		if (customTransportControls.VideoTracksSelectionButton is not null)
		{
			customTransportControls.VideoTracksSelectionButton.Visibility = 
				videoTracks.Count > 1 ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
		}
	}

	/// <summary>
	/// Updates the track selection UI with current tracks
	/// </summary>
	/// <param name="tracks">Available tracks</param>
	public void UpdateTrackSelectionUI(IEnumerable<CommunityToolkit.Maui.Core.MediaTrack> tracks)
	{
		if (customTransportControls is null)
		{
			return;
		}

		var trackList = tracks.ToList();

		// Update audio tracks
		var audioTracks = trackList.Where(t => t.Type == MediaTrackType.Audio).ToList();
		SetupTrackSelectionFlyout(
			customTransportControls.AudioTracksSelectionButton,
			audioTracks,
			"AudioTracksSelectionListView");

		// Update subtitle tracks
		var subtitleTracks = trackList.Where(t => t.Type == MediaTrackType.Text).ToList();
		SetupTrackSelectionFlyout(
			customTransportControls.CCSelectionButton,
			subtitleTracks,
			"CCSelectionListView");

		// Update video tracks
		var videoTracks = trackList.Where(t => t.Type == MediaTrackType.Video).ToList();
		SetupTrackSelectionFlyout(
			customTransportControls.VideoTracksSelectionButton,
			videoTracks,
			"VideoTracksSelectionListView");
	}

	void SetupTrackSelectionFlyout(AppBarButton? button, List<CommunityToolkit.Maui.Core.MediaTrack> tracks, string listViewName)
	{
		if (button?.Flyout is not Flyout flyout || flyout.Content is not StackPanel panel)
		{
			return;
		}

		var listView = panel.Children.OfType<Microsoft.UI.Xaml.Controls.ListView>().FirstOrDefault();
		if (listView is null)
		{
			// Try to find by name
			listView = FindChildByName<Microsoft.UI.Xaml.Controls.ListView>(panel, listViewName);
		}

		if (listView is not null)
		{
			// Clear previous event handlers
			listView.SelectionChanged -= OnTrackSelectionChanged;
			
			// Set the new data source
			listView.ItemsSource = tracks;
			
			// Add event handler
			listView.SelectionChanged += OnTrackSelectionChanged;

			// Select the currently active track
			var selectedTrack = tracks.FirstOrDefault(t => t.IsSelected);
			if (selectedTrack is not null)
			{
				listView.SelectedItem = selectedTrack;
			}
		}
	}

	void OnTrackSelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
	{
		if (e.AddedItems.FirstOrDefault() is CommunityToolkit.Maui.Core.MediaTrack selectedTrack)
		{
			OnTrackSelected?.Invoke(selectedTrack);
			
			// Close the flyout
			if (sender is Microsoft.UI.Xaml.Controls.ListView listView)
			{
				var button = FindParentButton(listView);
				if (button?.Flyout is Flyout flyout)
				{
					flyout.Hide();
				}
			}
		}
	}

	static T? FindChildByName<T>(DependencyObject parent, string name) where T : FrameworkElement
	{
		if (parent is null)
		{
			return null;
		}

		var childCount = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
		for (int i = 0; i < childCount; i++)
		{
			var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
			if (child is T element && element.Name == name)
			{
				return element;
			}

			var result = FindChildByName<T>(child, name);
			if (result is not null)
			{
				return result;
			}
		}

		return null;
	}

	static AppBarButton? FindParentButton(DependencyObject child)
	{
		var parent = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetParent(child);
		while (parent is not null)
		{
			if (parent is AppBarButton button)
			{
				return button;
			}
			parent = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetParent(parent);
		}
		return null;
	}

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
	/// Gets the presented page.
	/// </summary>
	protected static Page CurrentPage =>
		PageExtensions.GetCurrentPage(Application.Current?.Windows[0].Page ?? throw new InvalidOperationException($"{nameof(Page)} cannot be null."));

	/// <summary>
	/// Releases the managed and unmanaged resources used by the <see cref="MauiMediaElement"/>.
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if (isDisposed)
		{
			return;
		}
		if (customTransportControls?.FullScreenButton is not null)
		{
			customTransportControls.FullScreenButton.Click -= OnFullScreenButtonClick;
		}

		if (disposing)
		{
			mediaPlayerElement.MediaPlayer.Dispose();
		}

		isDisposed = true;
	}

	static AppWindow GetAppWindowForCurrentWindow()
	{
		// let's cache the CurrentPage here, since the user can navigate or background the app
		// while this method is running
		var currentPage = CurrentPage;

		if (currentPage?.GetParentWindow().Handler.PlatformView is not MauiWinUIWindow window)
		{
			throw new InvalidOperationException($"{nameof(window)} cannot be null.");
		}

		var handle = WindowNative.GetWindowHandle(window);
		var id = Win32Interop.GetWindowIdFromWindow(handle);

		return AppWindow.GetFromWindowId(id);
	}

	void OnFullScreenButtonClick(object sender, RoutedEventArgs e)
	{
		var currentPage = CurrentPage;
		if (appWindow.Presenter.Kind is AppWindowPresenterKind.FullScreen)
		{
			appWindow.SetPresenter(AppWindowPresenterKind.Default);
			Shell.SetNavBarIsVisible(CurrentPage, doesNavigationBarExistBeforeFullScreen);

			if (popup.IsOpen)
			{
				popup.IsOpen = false;
				popup.Child = null;
				fullScreenGrid.Children.Clear();
			}
			Children.Add(mediaPlayerElement);

			var parent = mediaPlayerElement.Parent as FrameworkElement;
			mediaPlayerElement.Width = parent?.Width ?? mediaPlayerElement.Width;
			mediaPlayerElement.Height = parent?.Height ?? mediaPlayerElement.Height;
		}
		else
		{
			appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
			doesNavigationBarExistBeforeFullScreen = Shell.GetNavBarIsVisible(currentPage);
			Shell.SetNavBarIsVisible(CurrentPage, false);

			var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
			mediaPlayerElement.Width = displayInfo.Width / displayInfo.Density;
			mediaPlayerElement.Height = displayInfo.Height / displayInfo.Density;

			Children.Clear();
			fullScreenGrid.Children.Add(mediaPlayerElement);

			popup.XamlRoot = mediaPlayerElement.XamlRoot;
			popup.HorizontalOffset = 0;
			popup.VerticalOffset = 0;
			popup.ShouldConstrainToRootBounds = false;
			popup.VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center;
			popup.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center;
			popup.Child = fullScreenGrid;

			if (!popup.IsOpen)
			{
				popup.IsOpen = true;
			}
		}
	}
}