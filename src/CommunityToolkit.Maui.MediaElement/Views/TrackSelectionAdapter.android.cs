using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Views;

public class TrackSelectionAdapter : RecyclerView.Adapter
{
    List<MediaTrack> tracks = [];
    int selectedPosition = -1;
    public event Action<MediaTrack>? TrackSelected;

    public TrackSelectionAdapter(IEnumerable<MediaTrack> tracks)
    {
        this.tracks = [.. tracks];
        
        // Find the currently selected track
        for (int i = 0; i < this.tracks.Count; i++)
        {
            if (this.tracks[i].IsSelected)
            {
                selectedPosition = i;
                break;
            }
        }
    }

    public override int ItemCount => tracks.Count;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        if (holder is TrackViewHolder trackHolder)
        {
            var track = tracks[position];
            trackHolder.Bind(track, position == selectedPosition);
        }
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var inflater = LayoutInflater.From(parent.Context);
        var layoutResourceId = parent.Context?.Resources?.GetIdentifier("track_selection_item", "layout", parent.Context.PackageName) ?? 0;
        var view = inflater?.Inflate(layoutResourceId, parent, false);
        return new TrackViewHolder(view!, OnTrackClick);
    }

    void OnTrackClick(int position)
    {
        if (position < 0 || position >= tracks.Count)
		{
			return;
		}

		var previousSelected = selectedPosition;
        selectedPosition = position;

        // Notify changes
        if (previousSelected != -1)
		{
			NotifyItemChanged(previousSelected);
		}

		NotifyItemChanged(selectedPosition);

        TrackSelected?.Invoke(tracks[position]);
    }

    public MediaTrack? GetSelectedTrack()
    {
        return selectedPosition >= 0 && selectedPosition < tracks.Count ? tracks[selectedPosition] : null;
    }
}

public class TrackViewHolder : RecyclerView.ViewHolder
{
    readonly Android.Widget.RadioButton radioButton;
    readonly TextView trackName;
    readonly TextView trackDetails;
    readonly Action<int> onItemClick;

    public TrackViewHolder(Android.Views.View itemView, Action<int> onItemClick) : base(itemView)
    {
        this.onItemClick = onItemClick;
        var radioButtonId = itemView.Context?.Resources?.GetIdentifier("track_radio_button", "id", itemView.Context.PackageName) ?? 0;
        var trackNameId = itemView.Context?.Resources?.GetIdentifier("track_name", "id", itemView.Context.PackageName) ?? 0;
        var trackDetailsId = itemView.Context?.Resources?.GetIdentifier("track_details", "id", itemView.Context.PackageName) ?? 0;
        
        radioButton = itemView.FindViewById<Android.Widget.RadioButton>(radioButtonId) ?? throw new InvalidOperationException("RadioButton not found");
        trackName = itemView.FindViewById<TextView>(trackNameId) ?? throw new InvalidOperationException("Track name TextView not found");
        trackDetails = itemView.FindViewById<TextView>(trackDetailsId) ?? throw new InvalidOperationException("Track details TextView not found");

        itemView.Click += (s, e) => this.onItemClick(BindingAdapterPosition);
        radioButton.Click += (s, e) => this.onItemClick(BindingAdapterPosition);
    }

    public void Bind(MediaTrack track, bool isSelected)
    {
        radioButton.Checked = isSelected;
        trackName.Text = string.IsNullOrEmpty(track.Name) ? GetTrackDisplayName(track) : track.Name;
        trackDetails.Text = GetTrackDetails(track);
    }

    static string GetTrackDisplayName(MediaTrack track)
    {
        return track.Type switch
        {
            MediaTrackType.Audio => $"Audio {(!string.IsNullOrEmpty(track.Language) ? $"({track.Language})" : "")}",
            MediaTrackType.Video => $"Video {track.Width}x{track.Height}",
            MediaTrackType.Text => $"Subtitle {(!string.IsNullOrEmpty(track.Language) ? $"({track.Language})" : "")}",
            _ => "Unknown Track"
        };
    }

    static string GetTrackDetails(MediaTrack track)
    {
        var details = new List<string>();
        
        if (!string.IsNullOrEmpty(track.Language))
		{
			details.Add(track.Language.ToUpperInvariant());
		}

		if (!string.IsNullOrEmpty(track.Codec))
		{
			details.Add(track.Codec.ToUpperInvariant());
		}

		if (track.Bitrate > 0)
		{
			details.Add($"{track.Bitrate / 1000} kbps");
		}

		if (track.Type == MediaTrackType.Video && track.Width > 0 && track.Height > 0)
		{
			details.Add($"{track.Width}x{track.Height}");
		}

		if (track.Type == MediaTrackType.Video && track.FrameRate > 0)
		{
			details.Add($"{track.FrameRate:F1} fps");
		}

		return string.Join(" • ", details);
    }
}
