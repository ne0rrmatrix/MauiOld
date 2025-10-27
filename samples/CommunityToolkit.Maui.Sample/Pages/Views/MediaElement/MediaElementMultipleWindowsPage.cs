using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Sample.ViewModels.Views;
#if WINDOWS || MACCATALYST
using CommunityToolkit.Maui.Sample.Constants;
using CommunityToolkit.Maui.Views;
#else
using CommunityToolkit.Maui.Markup;
#endif

namespace CommunityToolkit.Maui.Sample.Pages.Views;

public partial class MediaElementMultipleWindowsPage : BasePage<MediaElementMultipleWindowsViewModel>
{
#if WINDOWS || MACCATALYST
	readonly Window secondWindow;
#endif

	public MediaElementMultipleWindowsPage(MediaElementMultipleWindowsViewModel viewModel) : base(viewModel)
	{
#if WINDOWS || MACCATALYST
		var mediaItem = new MediaItem
				{
					MetadataTitle = "Big Buck Bunny",
					MetadataArtist = "Blender Foundation",
					MetadataArtworkUrl = "https://peach.blender.org/wp-content/uploads/title_anouncement.jpg?x11217",
					Source = StreamingVideoUrls.BuckBunny,
				};
		secondWindow = new Window(new ContentPage
		{
			Content = new MediaElement
			{
				AndroidViewType= AndroidViewType.SurfaceView,
				Source = mediaItem,
				ShouldAutoPlay = true
			}
		});

		Content = new MediaElement
		{
			AndroidViewType= AndroidViewType.SurfaceView,
			Source = mediaItem,
			ShouldAutoPlay = true
		};
#else
		Content = new Label()
			.Text("This sample is only testable on MacCatalyst and Windows")
			.TextCenter();
#endif
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
#if WINDOWS || MACCATALYST
		Application.Current?.OpenWindow(secondWindow);
#endif
	}
}