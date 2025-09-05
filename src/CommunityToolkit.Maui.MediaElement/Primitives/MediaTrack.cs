namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Represents a media track (audio, video, or text) in a multi-track media source.
/// </summary>
public class MediaTrack
{
    /// <summary>
    /// Gets or sets the unique identifier for this track.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the track.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the language code of the track (e.g., "en", "es", "fr").
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of media track.
    /// </summary>
    public MediaTrackType Type { get; set; }

    /// <summary>
    /// Gets or sets whether this track is currently selected.
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets whether this track is the default track for its type.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets the bitrate of the track (for audio/video tracks).
    /// </summary>
    public int Bitrate { get; set; }

    /// <summary>
    /// Gets or sets the width of the video track (video tracks only).
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the video track (video tracks only).
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the frame rate of the video track (video tracks only).
    /// </summary>
    public float FrameRate { get; set; }

    /// <summary>
    /// Gets or sets the codec used for this track.
    /// </summary>
    public string Codec { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional metadata for the track.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = [];
}

/// <summary>
/// Defines the types of media tracks.
/// </summary>
public enum MediaTrackType
{
    /// <summary>
    /// Unknown track type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Audio track.
    /// </summary>
    Audio = 1,

    /// <summary>
    /// Video track.
    /// </summary>
    Video = 2,

    /// <summary>
    /// Text/subtitle track.
    /// </summary>
    Text = 3
}