using Android.App;
using Android.Content;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Views;

public static class TrackSelectionDialog
{
    public static void Show(Context context, IEnumerable<MediaTrack> tracks, MediaTrackType trackType, Action<MediaTrack> onTrackSelected)
    {
        var tracksOfType = tracks.Where(t => t.Type == trackType).ToArray();
        if (tracksOfType.Length == 0)
        {
            return;
        }

        var items = tracksOfType.Select(GetTrackDisplayName).ToArray();
        var selectedIndex = Array.FindIndex(tracksOfType, t => t.IsSelected);

        var builder = new AlertDialog.Builder(context);
        builder.SetTitle(GetDialogTitle(trackType));
        builder.SetSingleChoiceItems(items, selectedIndex, (s, e) =>
        {
            if (e.Which >= 0 && e.Which < tracksOfType.Length)
            {
                onTrackSelected(tracksOfType[e.Which]);
            }
            ((AlertDialog)s!).Dismiss();
        });
        builder.SetNegativeButton("Cancel", (s, e) => { });
        builder.Show();
    }

	static string GetTrackDisplayName(MediaTrack track)
	{
		return track.Type switch
		{
			MediaTrackType.Audio => !string.IsNullOrEmpty(track.Name) ? track.Name : $"Audio {(!string.IsNullOrEmpty(track.Language) ? $"({track.Language})" : "")}",
			MediaTrackType.Video => !string.IsNullOrEmpty(track.Name) ? track.Name : $"Video {track.Width}x{track.Height}",
			MediaTrackType.Text => !string.IsNullOrEmpty(track.Name) ? track.Name : $"Subtitle {(!string.IsNullOrEmpty(track.Language) ? $"({track.Language})" : "")}",
			_ => track.Name ?? "Unknown Track"
		};
	}

	static string GetDialogTitle(MediaTrackType trackType)
    {
        return trackType switch
        {
            MediaTrackType.Audio => "Select Audio Track",
            MediaTrackType.Video => "Select Video Quality",
            MediaTrackType.Text => "Select Subtitle",
            _ => "Select Track"
        };
    }
}
