﻿
using AVFoundation;
using CommunityToolkit.Maui.Core;
using Foundation;
using MediaPlayer;
using UIKit;
using CoreMedia;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// The class that provides methods to update the system UI for media transport controls to display media metadata.
/// </summary>
public class MetaDataExtensions : IDisposable
{
	/// <summary>
	/// The metadata for the currently playing media.
	/// </summary>
	public readonly MPNowPlayingInfo NowPlayingInfo;
	static readonly UIImage defaultUIImage =  new();
	static readonly MPNowPlayingInfo nowPlayingInfoDefault = InitNowPlayingInfo();
	readonly PlatformMediaElement? player;
	static readonly MPMediaItemArtwork defaultImage = InitItemArtWork();

	bool disposedValue;

	/// <summary>
	/// Initializes a new instance of the <see cref="MetaDataExtensions"/> class.
	/// </summary>
	/// <param name="player"></param>
	public MetaDataExtensions(PlatformMediaElement player)
	{
		this.player = player;
		NowPlayingInfo = new();
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;

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

	static MPNowPlayingInfo InitNowPlayingInfo()
	{
		return new()
		{
			AlbumTitle = string.Empty,
			Title = string.Empty,
			Artist = string.Empty,
			PlaybackDuration = 0,
			IsLiveStream = false,
			PlaybackRate = 0,
			ElapsedPlaybackTime = 0,
			Artwork = defaultImage
		};
	}

	static MPMediaItemArtwork InitItemArtWork()
	{
		return new MPMediaItemArtwork(new UIImage());
	}

	MPRemoteCommandHandlerStatus SeekCommand(MPRemoteCommandEvent commandEvent)
	{
		if (player is null || commandEvent is not MPChangePlaybackPositionCommandEvent eventArgs)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		var seekTime = CMTime.FromSeconds(eventArgs.PositionTime, 1);
		player.Seek(seekTime);
		return MPRemoteCommandHandlerStatus.Success;
	}
	
	MPRemoteCommandHandlerStatus SeekBackwardCommand(MPRemoteCommandEvent commandEvent)
	{
		if (commandEvent is null || player is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		var seekTime = player.CurrentTime - CMTime.FromSeconds(10, 1);
		player.Seek(seekTime);
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus SeekForwardCommand(MPRemoteCommandEvent commandEvent)
	{
		if (player is null || commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		var seekTime = player.CurrentTime + CMTime.FromSeconds(10, 1);
		player.Seek(seekTime);
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus PlayCommand(MPRemoteCommandEvent commandEvent)
	{
		if (player is null || commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		player.Play();
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus PauseCommand(MPRemoteCommandEvent commandEvent)
	{
		if (player is null || commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		player.Pause();
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus ToggleCommand(MPRemoteCommandEvent commandEvent)
	{
		if (player is null || commandEvent is not null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		if (player.Rate == 0)
		{
			player.Play();
		}
		else
		{
			player.Pause();
		}
		return MPRemoteCommandHandlerStatus.Success;
	}

	/// <summary>
	/// Clears the metadata for the currently playing media.
	/// </summary>
	public void ClearNowPlaying() => MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = nowPlayingInfoDefault;

	/// <summary>
	/// Sets the data for the currently playing media from the media element.
	/// </summary>
	/// <param name="PlayerItem"></param>
	/// <param name="MediaElement"></param>
	public void SetMetaData(AVPlayerItem? PlayerItem, IMediaElement MediaElement)
	{
		if (MediaElement is null)
		{
			ClearNowPlaying();
			return;
		}
		NowPlayingInfo.Title = MediaElement.MetaDataTitle;
		NowPlayingInfo.Artist = MediaElement.MetaDataArtist;
		NowPlayingInfo.PlaybackDuration = PlayerItem?.Duration.Seconds ?? 0;
		NowPlayingInfo.IsLiveStream = false;
		NowPlayingInfo.PlaybackRate = MediaElement.Speed;
		NowPlayingInfo.ElapsedPlaybackTime = PlayerItem?.CurrentTime.Seconds ?? 0;
		NowPlayingInfo.Artwork = GetArtwork(MediaElement.MetaDataArtworkUrl);
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
	}

	static MPMediaItemArtwork GetArtwork(string? ImageUri)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(ImageUri))
			{
				UIImage? image = GetImage(ImageUri);
				if (image is not null)
				{
					return new MPMediaItemArtwork(image);
				}
				return defaultImage;
			}
			return defaultImage;
		}
		catch
		{
			return defaultImage;
		}
		
	}

	static UIImage GetImage(string ImageUri)
	{
		return ImageUri switch
		{
			_ when ImageUri.StartsWith("http", StringComparison.CurrentCulture) 
			=> (UIImage.LoadFromData(NSData.FromUrl(new NSUrl(ImageUri))) ?? defaultUIImage),
			_ => defaultUIImage
		};
	}

	/// <summary>
	/// Cleans up the resources used by the <see cref="MetaDataExtensions"/>.
	/// </summary>
	/// <param name="disposing"></param>
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				defaultImage.Dispose();
			}
			disposedValue = true;
		}
	}

	/// <summary>
	/// Cleans up the resources used by the <see cref="MetaDataExtensions"/>.
	/// </summary>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
