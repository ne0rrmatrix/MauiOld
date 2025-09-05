namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Event arguments for when available tracks change.
/// </summary>
/// <remarks>
/// Initializes a new instance of the TracksChangedEventArgs class.
/// </remarks>
/// <param name="tracks">The collection of available tracks.</param>
public class TracksChangedEventArgs(IEnumerable<MediaTrack> tracks) : EventArgs
{

	/// <summary>
	/// Gets the collection of available tracks.
	/// </summary>
	public IReadOnlyList<MediaTrack> Tracks { get; } = tracks.ToList().AsReadOnly();
}