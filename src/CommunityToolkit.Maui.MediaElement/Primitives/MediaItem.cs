using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui.Primitives;

/// <summary>
/// Represents a media item.
/// </summary>
public partial class MediaItem : Element
{
	readonly WeakEventManager weakEventManager = new();

	internal event EventHandler? SourcesChanged
	{
		add => weakEventManager.AddEventHandler(value);
		remove => weakEventManager.RemoveEventHandler(value);
	}

	/// <summary>
	/// Gets or sets the title of the media.
	/// </summary>
	public MediaSource? Source { get; set; }

	/// <summary>
	/// Gets or sets the SubtitleUrl of the media.
	/// </summary>
	public MediaSource? SubtitleUrl { get; set; }

	/// <summary>
	/// Gets or sets the SubtitleLanguage.
	/// </summary>
	public string? SubtitleLanguage { get; set; }

	/// <summary>
	/// Gets or sets the SubtitleUrlDictionary.
	/// </summary>
	public Dictionary<string,string> SubtitleUrlDictionary { get; set; } = new Dictionary<string, string>();
	
	/// <summary>
	/// Gets or sets the SubtitleFont.
	/// </summary>
	public string? SubtitleFont { get; set; }

	/// <summary>
	/// Gets or sets the SubtitleFontSize.
	/// </summary>
	public int SubtitleFontSize { get; set; } = 16;

	/// <summary>
	/// Triggers the <see cref="SourcesChanged"/> event.
	/// </summary>
	protected void OnSourcesChanged() => weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(SourcesChanged));

}
