using System.Collections.ObjectModel;

namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Represents a collection of media tracks with change notification support.
/// </summary>
public partial class MediaTrackCollection : ObservableCollection<MediaTrack>
{
    /// <summary>
    /// Gets the currently selected track of the specified type.
    /// </summary>
    /// <param name="trackType">The type of track to get.</param>
    /// <returns>The selected track, or null if no track is selected.</returns>
    public MediaTrack? GetSelectedTrack(MediaTrackType trackType)
    {
        return this.FirstOrDefault(t => t.Type == trackType && t.IsSelected);
    }

    /// <summary>
    /// Gets all tracks of the specified type.
    /// </summary>
    /// <param name="trackType">The type of tracks to get.</param>
    /// <returns>Collection of tracks of the specified type.</returns>
    public IEnumerable<MediaTrack> GetTracksByType(MediaTrackType trackType)
    {
        return this.Where(t => t.Type == trackType);
    }

    /// <summary>
    /// Selects a track and deselects other tracks of the same type.
    /// </summary>
    /// <param name="trackId">The ID of the track to select.</param>
    /// <returns>True if the track was found and selected, false otherwise.</returns>
    public bool SelectTrack(string trackId)
    {
        var trackToSelect = this.FirstOrDefault(t => t.Id == trackId);
        if (trackToSelect == null)
		{
			return false;
		}

		// Deselect other tracks of the same type
		foreach (var track in this.Where(t => t.Type == trackToSelect.Type && t.Id != trackId))
        {
            track.IsSelected = false;
        }

        trackToSelect.IsSelected = true;
        return true;
    }

    /// <summary>
    /// Event raised when a track selection changes.
    /// </summary>
    public event EventHandler<TrackSelectionChangedEventArgs>? TrackSelectionChanged;

    /// <summary>
    /// Raises the track selection changed event.
    /// </summary>
    /// <param name="track">The track that was selected.</param>
    protected virtual void OnTrackSelectionChanged(MediaTrack track)
    {
        TrackSelectionChanged?.Invoke(this, new TrackSelectionChangedEventArgs(track));
    }
}

/// <summary>
/// Event arguments for track selection changes.
/// </summary>
/// <remarks>
/// Initializes a new instance of the TrackSelectionChangedEventArgs class.
/// </remarks>
/// <param name="selectedTrack">The track that was selected.</param>
public class TrackSelectionChangedEventArgs(MediaTrack selectedTrack) : EventArgs
{

	/// <summary>
	/// Gets the track that was selected.
	/// </summary>
	public MediaTrack SelectedTrack { get; } = selectedTrack;
}