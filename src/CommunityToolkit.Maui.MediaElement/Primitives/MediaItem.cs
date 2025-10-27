using System.ComponentModel;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Represents a media item for playback.
/// </summary>
public partial class MediaItem : View
{
	/// <summary>
	/// Backing store for the <see cref="MetadataTitle"/> property.
	/// </summary>
	public static readonly BindableProperty MetadataTitleProperty = BindableProperty.Create(nameof(MetadataTitle), typeof(string), typeof(MediaItem), string.Empty);

	/// <summary>
	/// Backing store for the <see cref="MetadataArtist"/> property.
	/// </summary>
	public static readonly BindableProperty MetadataArtistProperty = BindableProperty.Create(nameof(MetadataArtist), typeof(string), typeof(MediaItem), string.Empty);

	/// <summary>
	/// Backing store for the <see cref="MetadataArtworkUrl"/> property.
	/// </summary>
	public static readonly BindableProperty MetadataArtworkUrlProperty = BindableProperty.Create(nameof(MetadataArtworkUrl), typeof(string), typeof(MediaItem), string.Empty);

	/// <summary>
	/// Backing store for the <see cref="Source"/> property.
	/// </summary>
	public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(MediaSource), typeof(MediaItem), null);

	/// <summary>
	/// Gets or sets the source of the media to play.
	/// This is a bindable property.
	/// </summary>
	[TypeConverter(typeof(MediaSourceConverter))]
	public MediaSource? Source
	{
		get => (MediaSource)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}


	/// <summary>
	/// Gets or sets the Title of the media.
	/// This is a bindable property.
	/// </summary>
	public string MetadataTitle
	{
		get => (string)GetValue(MetadataTitleProperty);
		set => SetValue(MetadataTitleProperty, value);
	}

	/// <summary>
	/// Gets or sets the Artist of the media.
	/// This is a bindable property.
	/// </summary>
	public string MetadataArtist
	{
		get => (string)GetValue(MetadataArtistProperty);
		set => SetValue(MetadataArtistProperty, value);
	}

	/// <summary>
	/// Gets or sets the Artwork Image Url of the media.
	/// This is a bindable property.
	/// </summary>
	public string MetadataArtworkUrl
	{
		get => (string)GetValue(MetadataArtworkUrlProperty);
		set => SetValue(MetadataArtworkUrlProperty, value);
	}
}
