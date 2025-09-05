using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Primitives;

sealed partial class CustomTransportControls : MediaTransportControls
{
	public event EventHandler<EventArgs>? OnTemplateLoaded;
	public event EventHandler<MediaTrackType>? TrackSelectionRequested;
	
	public AppBarButton FullScreenButton = new();
	public AppBarButton? AudioTracksSelectionButton;
	public AppBarButton? CCSelectionButton;
	public AppBarButton? VideoTracksSelectionButton;
	
	bool isFullScreen = false;

	public CustomTransportControls()
	{
		this.DefaultStyleKey = typeof(CustomTransportControls);
	}

	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		if (GetTemplateChild("FullWindowButton") is AppBarButton fullScreenButton)
		{
			FullScreenButton = fullScreenButton;
			FullScreenButton.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
			FullScreenButton.Click += FullScreenButton_Click;
		}

		if (GetTemplateChild("AudioTracksSelectionButton") is AppBarButton audioButton)
		{
			AudioTracksSelectionButton = audioButton;
			AudioTracksSelectionButton.Click += AudioTracksSelectionButton_Click;
		}

		if (GetTemplateChild("CCSelectionButton") is AppBarButton ccButton)
		{
			CCSelectionButton = ccButton;
			CCSelectionButton.Click += CCSelectionButton_Click;
		}

		if (GetTemplateChild("VideoTracksSelectionButton") is AppBarButton videoButton)
		{
			VideoTracksSelectionButton = videoButton;
			VideoTracksSelectionButton.Click += VideoTracksSelectionButton_Click;
		}

		OnTemplateLoaded?.Invoke(this, EventArgs.Empty);
	}

	void FullScreenButton_Click(object sender, RoutedEventArgs e)
	{
		if (isFullScreen)
		{
			FullScreenButton.Icon = new FontIcon { Glyph = "\uE740" };
			isFullScreen = false;
		}
		else
		{
			FullScreenButton.Icon = new SymbolIcon(Symbol.BackToWindow);
			isFullScreen = true;
		}
	}

	void AudioTracksSelectionButton_Click(object sender, RoutedEventArgs e)
	{
		TrackSelectionRequested?.Invoke(this, MediaTrackType.Audio);
	}

	void CCSelectionButton_Click(object sender, RoutedEventArgs e)
	{
		TrackSelectionRequested?.Invoke(this, MediaTrackType.Text);
	}

	void VideoTracksSelectionButton_Click(object sender, RoutedEventArgs e)
	{
		TrackSelectionRequested?.Invoke(this, MediaTrackType.Video);
	}
}