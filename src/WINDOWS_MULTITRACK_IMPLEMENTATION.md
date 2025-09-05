# Windows Multi-Track Support for MediaElement

This implementation adds multi-track support for audio and subtitle tracks on Windows in the CommunityToolkit.Maui.MediaElement project.

## Features Added

### 1. Custom Transport Controls Enhancement
- **File**: `CommunityToolkit.Maui.MediaElement\Primitives\CustomTransportControls.windows.cs`
- Added support for audio track selection button (`AudioTracksSelectionButton`)
- Added support for subtitle/CC selection button (`CCSelectionButton`)
- Added `TrackSelectionRequested` event to handle track selection UI interactions

### 2. UI Resource Dictionary Updates
- **File**: `CommunityToolkit.Maui.MediaElement\ResourceDictionary.windows.xaml`
- Added flyout controls for audio track selection
- Added flyout controls for subtitle track selection
- Styled the flyouts with proper ListView controls and selection indicators
- Track selection buttons only appear when multiple tracks are available

### 3. MediaManager Track Extraction and Selection
- **File**: `CommunityToolkit.Maui.MediaElement\Views\MediaManager.windows.cs`
- Implemented `ExtractAndUpdateTracks()` method to extract available tracks from MediaPlaybackItem
- Added `PlatformSelectTrack()` method to handle track selection
- Support for audio track switching via `MediaPlaybackItem.AudioTracks.SelectedIndex`
- Basic subtitle track detection (more advanced subtitle control requires additional Windows API work)
- Automatic track extraction when media opens or track collections change

### 4. MauiMediaElement UI Integration
- **File**: `CommunityToolkit.Maui.MediaElement\Views\MauiMediaElement.windows.cs`
- Added `UpdateTrackAvailability()` method to show/hide track selection buttons
- Added `UpdateTrackSelectionUI()` method to populate flyout ListViews with available tracks
- Connected track selection events between UI and MediaManager
- Proper button visibility management based on available tracks

## How It Works

1. **Track Detection**: When media is loaded, the MediaManager automatically extracts available audio, video, and subtitle tracks from the MediaPlaybackItem.

2. **UI Updates**: The transport controls are updated to show track selection buttons only when multiple tracks are available.

3. **Track Selection**: Users can click on the audio or subtitle track buttons to open flyouts with available options.

4. **Track Switching**: When a user selects a different track, the selection is passed to the MediaManager which updates the MediaPlaybackItem accordingly.

## Track Types Supported

- **Audio Tracks**: Full support for switching between multiple audio tracks
- **Video Tracks**: Detection and basic support (primarily for adaptive bitrate scenarios)
- **Subtitle Tracks**: Detection and basic support (Windows subtitle control has API limitations)

## Usage Example

The track selection functionality is automatically available when:
1. Loading media with multiple audio tracks (e.g., different languages)
2. Loading media with subtitle tracks
3. Using adaptive streaming content with multiple quality levels

The UI will automatically show the appropriate track selection buttons in the transport controls.

## Limitations

- Subtitle track control is limited by Windows MediaPlaybackItem API capabilities
- Advanced subtitle features like styling and positioning require additional implementation
- Video track selection is primarily for adaptive streams and quality selection

## Integration Notes

This implementation maintains compatibility with the existing MediaElement API and extends it with Windows-specific multi-track functionality. The track selection UI follows Windows design guidelines and integrates seamlessly with the existing transport controls.