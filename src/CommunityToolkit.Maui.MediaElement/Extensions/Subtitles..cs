namespace CommunityToolkit.Maui.Extensions;
/// <summary>
/// Subtitles class to handle subtitle functionalities for MediaElement.
/// </summary>
public partial class Subtitle
{
	/// <summary>
	/// Source of the subtitles file (URL or local path).
	/// </summary>
	public string Source { get; set; } = string.Empty;

	/// <summary>
	/// Language of the subtitles (e.g., "en" for English, "es" for Spanish).
	/// </summary>
	public string Language { get; set; } = string.Empty;

	/// <summary>
	/// Type of the subtitles file (e.g., SubRip, Text_VTT).
	/// </summary>
	public MimeType Type { get; set; } = MimeType.Text_VTT;
}
