using AVFoundation;
using CommunityToolkit.Maui.Views;
using CoreMedia;
using Foundation;
using MediaPlayer;
using UIKit;

namespace CommunityToolkit.Maui.Core.Primitives;

class Metadata
{
	static readonly UIImage defaultUIImage = new();
	static readonly MPNowPlayingInfo nowPlayingInfoDefault = new()
	{
		AlbumTitle = string.Empty,
		Title = string.Empty,
		Artist = string.Empty,
		PlaybackDuration = 0,
		IsLiveStream = false,
		PlaybackRate = 0,
		ElapsedPlaybackTime = 0,
		Artwork = new(boundsSize: new(0, 0), requestHandler: _ => defaultUIImage)
	};

	readonly PlatformMediaElement player;

	/// <summary>
	/// Initializes a new instance of the <see cref="Metadata"/> class.
	/// </summary>
	/// <param name="player"></param>
	public Metadata(PlatformMediaElement player)
	{
		this.player = player;
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = nowPlayingInfoDefault;

		var commandCenter = MPRemoteCommandCenter.Shared;

		commandCenter.TogglePlayPauseCommand.Enabled = true;
		commandCenter.TogglePlayPauseCommand.AddTarget(ToggleCommand);

		commandCenter.PlayCommand.Enabled = true;
		commandCenter.PlayCommand.AddTarget(PlayCommand);

		commandCenter.PauseCommand.Enabled = true;
		commandCenter.PauseCommand.AddTarget(PauseCommand);

		commandCenter.ChangePlaybackPositionCommand.Enabled = true;
		commandCenter.ChangePlaybackPositionCommand.AddTarget(SeekCommand);

		commandCenter.SeekBackwardCommand.Enabled = true;
		commandCenter.SeekBackwardCommand.AddTarget(SeekBackwardCommand);

		commandCenter.SeekForwardCommand.Enabled = false;
		commandCenter.SeekForwardCommand.AddTarget(SeekForwardCommand);
	}

	/// <summary>
	/// The metadata for the currently playing media.
	/// </summary>
	public MPNowPlayingInfo NowPlayingInfo { get; } = new();


	/// <summary>
	/// Clears the metadata for the currently playing media.
	/// </summary>
	public static void ClearNowPlaying() => MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = nowPlayingInfoDefault;

	/// <summary>
	/// Sets the data for the currently playing media from the media element.
	/// </summary>
	/// <param name="playerItem"></param>
	/// <param name="mediaElement"></param>
	public void SetMetadata(AVPlayerItem? playerItem, IMediaElement? mediaElement)
	{
		if (mediaElement is null)
		{
			Metadata.ClearNowPlaying();
			return;
		}

		NowPlayingInfo.Title = mediaElement.MetadataTitle;
		NowPlayingInfo.Artist = mediaElement.MetadataArtist;
		NowPlayingInfo.PlaybackDuration = playerItem?.Duration.Seconds ?? 0;
		NowPlayingInfo.IsLiveStream = false;
		NowPlayingInfo.PlaybackRate = mediaElement.Speed;
		NowPlayingInfo.ElapsedPlaybackTime = playerItem?.CurrentTime.Seconds ?? 0;
		NowPlayingInfo.Artwork = new(boundsSize: new(320, 240), requestHandler: _ => GetImage(mediaElement.MetadataArtworkUrl));
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
	}

	static UIImage GetImage(MediaSource? imageUri)
	{
		try
		{
			if (imageUri is FileMediaSource fileMediaSource)
			{
				return UIImage.LoadFromData(NSData.FromFile(fileMediaSource.Path ?? string.Empty)) ?? defaultUIImage;
			}
			if (imageUri is ResourceMediaSource resourceMediaSource)
			{
				var path = resourceMediaSource.Path;

				if (!string.IsNullOrWhiteSpace(path) && Path.HasExtension(path))
				{
					string directory = Path.GetDirectoryName(path) ?? string.Empty;
					string filename = Path.GetFileNameWithoutExtension(path);
					string extension = Path.GetExtension(path)[1..];
					var url = NSBundle.MainBundle.GetUrlForResource(filename,
						extension, directory);

					return UIImage.LoadFromData(NSData.FromUrl(url)) ?? defaultUIImage;
				}
				else
				{
					System.Diagnostics.Trace.TraceError("Invalid file path for ResourceMediaSource.");
				}
			}
			if (imageUri is UriMediaSource uriMediaSource)
			{
				return UIImage.LoadFromData(NSData.FromUrl(new NSUrl(uriMediaSource.Uri?.ToString() ?? string.Empty))) ?? defaultUIImage;
			}
			System.Diagnostics.Trace.TraceError("Invalid MediaSource type or location for artwork.");
			return defaultUIImage;
		}
		catch
		{
			System.Diagnostics.Trace.TraceError("Invalid MediaSource type or location for artwork.");
			return defaultUIImage;
		}
	}

	MPRemoteCommandHandlerStatus SeekCommand(MPRemoteCommandEvent? commandEvent)
	{
		if (commandEvent is not MPChangePlaybackPositionCommandEvent eventArgs)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		var seekTime = CMTime.FromSeconds(eventArgs.PositionTime, 1);
		player.Seek(seekTime);
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus SeekBackwardCommand(MPRemoteCommandEvent? commandEvent)
	{
		if (commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		var seekTime = player.CurrentTime - CMTime.FromSeconds(10, 1);
		player.Seek(seekTime);
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus SeekForwardCommand(MPRemoteCommandEvent? commandEvent)
	{
		if (commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		var seekTime = player.CurrentTime + CMTime.FromSeconds(10, 1);
		player.Seek(seekTime);
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus PlayCommand(MPRemoteCommandEvent? commandEvent)
	{
		if (commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		player.Play();
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus PauseCommand(MPRemoteCommandEvent? commandEvent)
	{
		if (commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		player.Pause();
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus ToggleCommand(MPRemoteCommandEvent? commandEvent)
	{
		if (commandEvent is not null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		if (player.Rate is 0)
		{
			player.Play();
		}
		else
		{
			player.Pause();
		}

		return MPRemoteCommandHandlerStatus.Success;
	}
}