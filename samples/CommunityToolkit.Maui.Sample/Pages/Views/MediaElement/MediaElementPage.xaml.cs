using System.ComponentModel;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Sample.ViewModels.Views;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;
using LayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;

namespace CommunityToolkit.Maui.Sample.Pages.Views;

public partial class MediaElementPage : BasePage<MediaElementViewModel>
{
	readonly ILogger logger;
	const string loadOnlineMp4 = "Load Online MP4";
	const string loadHls = "Load HTTP Live Stream (HLS)";
	const string loadLocalResource = "Load Local Resource";
	const string resetSource = "Reset Source to null";
	const string loadMusic = "Load Music";
	const string loadLocalArtwork = "Load Local Artwork";

	public MediaElementPage(MediaElementViewModel viewModel, ILogger<MediaElementPage> logger) : base(viewModel)
	{
		InitializeComponent();

		this.logger = logger;
		MediaElement.PropertyChanged += MediaElement_PropertyChanged;
	}

	void MediaElement_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
		{
			logger.LogInformation("Duration: {newDuration}", MediaElement.Duration);
			PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
		}
	}

	void OnMediaOpened(object? sender, EventArgs e) => logger.LogInformation("Media opened.");

	void OnStateChanged(object? sender, MediaStateChangedEventArgs e) =>
		logger.LogInformation("Media State Changed. Old State: {PreviousState}, New State: {NewState}", e.PreviousState, e.NewState);

	void OnMediaFailed(object? sender, MediaFailedEventArgs e) => logger.LogInformation("Media failed. Error: {ErrorMessage}", e.ErrorMessage);

	void OnMediaEnded(object? sender, EventArgs e) => logger.LogInformation("Media ended.");

	void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
	{
		logger.LogInformation("Position changed to {position}", e.Position);
		PositionSlider.Value = e.Position.TotalSeconds;
	}

	void OnSeekCompleted(object? sender, EventArgs e) => logger.LogInformation("Seek completed.");

	void OnSpeedMinusClicked(object? sender, EventArgs e)
	{
		if (MediaElement.Speed >= 1)
		{
			MediaElement.Speed -= 1;
		}
	}

	void OnSpeedPlusClicked(object? sender, EventArgs e)
	{
		if (MediaElement.Speed < 10)
		{
			MediaElement.Speed += 1;
		}
	}

	void OnVolumeMinusClicked(object? sender, EventArgs e)
	{
		if (MediaElement.Volume >= 0)
		{
			if (MediaElement.Volume < .1)
			{
				MediaElement.Volume = 0;

				return;
			}

			MediaElement.Volume -= .1;
		}
	}

	void OnVolumePlusClicked(object? sender, EventArgs e)
	{
		if (MediaElement.Volume < 1)
		{
			if (MediaElement.Volume > .9)
			{
				MediaElement.Volume = 1;

				return;
			}

			MediaElement.Volume += .1;
		}
	}

	void OnPlayClicked(object? sender, EventArgs e)
	{
		MediaElement.Play();
	}

	void OnPauseClicked(object? sender, EventArgs e)
	{
		MediaElement.Pause();
	}

	void OnStopClicked(object? sender, EventArgs e)
	{
		MediaElement.Stop();
	}

	void OnMuteClicked(object? sender, EventArgs e)
	{
		MediaElement.ShouldMute = !MediaElement.ShouldMute;
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
		MediaElement.Stop();
		MediaElement.Handler?.DisconnectHandler();
	}

	async void Slider_DragCompleted(object? sender, EventArgs e)
	{
		ArgumentNullException.ThrowIfNull(sender);

		var newValue = ((Slider)sender).Value;
		await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

		MediaElement.Play();
	}

	void Slider_DragStarted(object sender, EventArgs e)
	{
		MediaElement.Pause();
	}

	void Button_Clicked(object? sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(CustomSourceEntry.Text))
		{
			DisplayAlert("Error Loading URL Source", "No value was found to load as a media source. " +
				"When you do enter a value, make sure it's a valid URL. No additional validation is done.",
				"OK");

			return;
		}

		MediaElement.Source = MediaSource.FromUri(CustomSourceEntry.Text);
	}

	async void ChangeSourceClicked(Object sender, EventArgs e)
	{
		var result = await DisplayActionSheet("Choose a source", "Cancel", null,
			loadOnlineMp4, loadHls, loadLocalResource, resetSource, loadMusic, loadLocalArtwork);

		switch (result)
		{
			case loadOnlineMp4:
				MediaElement.MetadataTitle = "Big Buck Bunny";
				MediaElement.MetadataArtworkUrl = "https://lh3.googleusercontent.com/pw/AP1GczNRrebWCJvfdIau1EbsyyYiwAfwHS0JXjbioXvHqEwYIIdCzuLodQCZmA57GADIo5iB3yMMx3t_vsefbfoHwSg0jfUjIXaI83xpiih6d-oT7qD_slR0VgNtfAwJhDBU09kS5V2T5ZML-WWZn8IrjD4J-g=w1792-h1024-s-no-gm";
				MediaElement.MetadataArtist = "Big Buck Bunny Album";
				MediaElement.Source =
					MediaSource.FromUri(
						"https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
				return;

			case loadHls:
				MediaElement.MetadataArtist = "HLS Album";
				MediaElement.MetadataArtworkUrl = "https://lh3.googleusercontent.com/pw/AP1GczNRrebWCJvfdIau1EbsyyYiwAfwHS0JXjbioXvHqEwYIIdCzuLodQCZmA57GADIo5iB3yMMx3t_vsefbfoHwSg0jfUjIXaI83xpiih6d-oT7qD_slR0VgNtfAwJhDBU09kS5V2T5ZML-WWZn8IrjD4J-g=w1792-h1024-s-no-gm";
				MediaElement.MetadataTitle = "HLS Title";
				MediaElement.Source
					= MediaSource.FromUri(
						"https://mtoczko.github.io/hls-test-streams/test-gap/playlist.m3u8");
				return;

			case resetSource:
				MediaElement.MetadataArtworkUrl = string.Empty;
				MediaElement.MetadataTitle = string.Empty;
				MediaElement.MetadataArtist = string.Empty;
				MediaElement.Source = null;
				return;

			case loadLocalResource:
				MediaElement.MetadataArtworkUrl = MediaSource.FromResource("robot.jpg");
				MediaElement.MetadataTitle = "Local Resource Title";
				MediaElement.MetadataArtist = "Local Resource Album";

				if (DeviceInfo.Platform == DevicePlatform.MacCatalyst
					|| DeviceInfo.Platform == DevicePlatform.iOS)
				{
					MediaElement.Source = MediaSource.FromResource("AppleVideo.mp4");
				}
				else if (DeviceInfo.Platform == DevicePlatform.Android)
				{
					MediaElement.Source = MediaSource.FromResource("AndroidVideo.mp4");
				}
				else if (DeviceInfo.Platform == DevicePlatform.WinUI)
				{
					MediaElement.Source = MediaSource.FromResource("WindowsVideo.mp4");
				}
				return;

			case loadMusic:
				MediaElement.MetadataTitle = "HAL 9000";
				MediaElement.MetadataArtist = "HAL 9000 Album";
				MediaElement.MetadataArtworkUrl = "https://lh3.googleusercontent.com/pw/AP1GczNRrebWCJvfdIau1EbsyyYiwAfwHS0JXjbioXvHqEwYIIdCzuLodQCZmA57GADIo5iB3yMMx3t_vsefbfoHwSg0jfUjIXaI83xpiih6d-oT7qD_slR0VgNtfAwJhDBU09kS5V2T5ZML-WWZn8IrjD4J-g=w1792-h1024-s-no-gm";
				MediaElement.Source = MediaSource.FromUri("https://github.com/prof3ssorSt3v3/media-sample-files/raw/master/hal-9000.mp3");
				return;

			case loadLocalArtwork:
				await SetupDownload().ConfigureAwait(true);
				MediaElement.MetadataTitle = "Local Video Title";
				MediaElement.MetadataArtist = "Local Video Album";
				
				MediaElement.MetadataArtworkUrl = await GetArtwork(new PickOptions
				{
					FileTypes = FilePickerFileType.Images,		
					PickerTitle = "Please select a Metadata Artwork file"
				}).ConfigureAwait(true);
				MediaElement.Source =
					MediaSource.FromUri(
						"https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
				return;
		}
	}
	async Task SetupDownload()
	{
		var fileStream = await GetFileStream("https://lh3.googleusercontent.com/pw/AP1GczNRrebWCJvfdIau1EbsyyYiwAfwHS0JXjbioXvHqEwYIIdCzuLodQCZmA57GADIo5iB3yMMx3t_vsefbfoHwSg0jfUjIXaI83xpiih6d-oT7qD_slR0VgNtfAwJhDBU09kS5V2T5ZML-WWZn8IrjD4J-g=w1792-h1024-s-no-gm").ConfigureAwait(true);
		if (fileStream is not null)
		{
			MemoryStream memoryStream = new();
			await fileStream.CopyToAsync(memoryStream, CancellationToken.None).ConfigureAwait(true);
			memoryStream.Seek(0, SeekOrigin.Begin);
			await FileSaver.Default.SaveAsync("robot.jpg", memoryStream, CancellationToken.None).ConfigureAwait(true);
		}
	}
	async Task<string> GetArtwork(PickOptions options)
	{
		try
		{
			var result = await FilePicker.Default.PickAsync(options).ConfigureAwait(true);
			if (result is not null && (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
					result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase)))
			{
				return result.FullPath;
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.TraceError(Title + " Error: " + ex.Message);
			return string.Empty;
		}
	}
	async Task<Stream> GetFileStream(string fileUrl)
	{
		HttpClient httpClient = new();
		try
		{
			return await httpClient.GetStreamAsync(fileUrl).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.TraceError(Title + " Error: " + ex.Message);
			return Stream.Null;
		}
	}

	async void ChangeAspectClicked(object? sender, EventArgs e)
	{
		var resultAspect = await DisplayActionSheet("Choose aspect ratio",
			"Cancel", null, Aspect.AspectFit.ToString(),
			Aspect.AspectFill.ToString(), Aspect.Fill.ToString());

		if (resultAspect is null || resultAspect.Equals("Cancel"))
		{
			return;
		}

		if (!Enum.TryParse(typeof(Aspect), resultAspect, true, out var aspectEnum))
		{
			await DisplayAlert("Error", "There was an error determining the selected aspect", "OK");

			return;
		}

		MediaElement.Aspect = (Aspect)aspectEnum;
	}

	void DisplayPopup(object sender, EventArgs e)
	{
		MediaElement.Pause();
		MediaElement popupMediaElement = new MediaElement
		{
			Source = MediaSource.FromResource("AppleVideo.mp4"),
			HeightRequest = 600,
			WidthRequest = 600,
			ShouldAutoPlay = true,
			ShouldShowPlaybackControls = true,
		};
		var popup = new Popup
		{
			VerticalOptions = LayoutAlignment.Center,
			HorizontalOptions = LayoutAlignment.Center,
		};
		popup.Content = new StackLayout
		{
			Children =
			{
				popupMediaElement,
			}
		};

		this.ShowPopup(popup);
		popup.Closed += (s, e) =>
		{
			popupMediaElement.Stop();
			popupMediaElement.Handler?.DisconnectHandler();
		};
	}
}