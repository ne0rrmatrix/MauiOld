using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using FluentAssertions;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Views;

public class MediaElementTests : BaseViewTest
{
	public MediaElementTests()
	{
		Assert.IsType<IMediaElement>(new MediaElement(), exactMatch: false);
	}

	[Fact]
	public void PosterIsNotStringEmptyOrNull()
	{
		MediaElement mediaElement = new();
		MediaItem mediaItem = new();
		mediaItem.MetadataArtworkUrl = "https://www.example.com/image.jpg";
		mediaElement.Source = mediaItem;
		Assert.False(string.IsNullOrEmpty(mediaElement.Source.MetadataArtworkUrl));
	}

	[Fact]
	public void PosterIsStringEmptyDoesNotThrow()
	{
		MediaElement mediaElement = new();
		MediaItem mediaItem = new();
		mediaItem.MetadataArtworkUrl = string.Empty;
		mediaElement.Source = mediaItem;
		Assert.True(string.IsNullOrEmpty(mediaElement.Source.MetadataArtworkUrl));
		Assert.True(mediaElement.Source.MetadataArtworkUrl == string.Empty);
	}

	[Fact]
	public void BindingContextPropagation()
	{
		object context = new();
		MediaElement mediaElement = new();
		FileMediaSource mediaSource = new();
		MediaItem mediaItem = new();
		mediaItem.Source = mediaSource;
		mediaElement.Source = mediaItem;

		mediaElement.BindingContext = context;

		mediaSource.BindingContext.Should().Be(context);
	}

	[Fact]
	public void CorrectDimensionsForVideoTest()
	{
		MediaElement mediaElement = new();
		MediaItem mediaItem = new();
		var mediaSource = MediaSource.FromUri("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
		mediaItem.Source = mediaSource;

		mediaElement.MediaOpened += (_, _) =>
		{
			mediaElement.MediaWidth.Should().Be(1280);
			mediaElement.MediaHeight.Should().Be(720);
		};

		mediaElement.Source = mediaItem;
	}

	[Fact]
	public void CorrectDimensionsForNullTest()
	{
		object context = new();
		MediaElement mediaElement = new();
		MediaItem mediaItem = new();
		var mediaSource = MediaSource.FromUri("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
		mediaItem.Source = mediaSource;

		mediaElement.MediaOpened += (_, _) =>
		{
			// We set it to an actual media source first, when the is opened, set the source to null
			if (mediaSource is not null)
			{
				mediaElement.MediaWidth.Should().Be(1280);
				mediaElement.MediaHeight.Should().Be(720);

				mediaElement.Source = null;
				return;
			}

			// When the source is null, the dimensions should be 0
			if (mediaElement.Source is null)
			{
				mediaElement.MediaWidth.Should().Be(0);
				mediaElement.MediaHeight.Should().Be(0);
			}
		};

		// Set the first (actual) media source, which will trigger the above event
		mediaElement.Source = mediaItem;
	}

	[Fact]
	public void MediaElementShouldBeAssignedToIMediaElement()
	{
		new MediaElement().Should().BeAssignableTo<IMediaElement>();
	}

	[Fact]
	public void MediaElementVolumeShouldNotBeMoreThan1()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			MediaElement mediaElement = new()
			{
				Volume = 1 + Math.Pow(10, -15)
			};
		});
	}

	[Fact]
	public void MediaElementVolumeShouldNotBeLessThan0()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			MediaElement mediaElement = new()
			{
				Volume = -double.Epsilon
			};
		});
	}
}