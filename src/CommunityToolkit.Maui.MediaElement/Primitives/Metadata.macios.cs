using AVFoundation;
using CoreMedia;
using Foundation;
using MediaPlayer;
using UIKit;

namespace CommunityToolkit.Maui.Core.Primitives;

sealed class Metadata
{
	List<AVPlayerItem> items = [];
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
	readonly IMediaElement mediaElement;

	/// <summary>
	/// Initializes a new instance of the <see cref="Metadata"/> class.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="mediaElement"></param>
	public Metadata(PlatformMediaElement player, IMediaElement mediaElement)
	{
		this.player = player;
		this.mediaElement = mediaElement;
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = nowPlayingInfoDefault;

		var commandCenter = MPRemoteCommandCenter.Shared;

		commandCenter.TogglePlayPauseCommand.Enabled = true;
		commandCenter.TogglePlayPauseCommand.AddTarget(ToggleCommand);
	
		commandCenter.ChangePlaybackPositionCommand.Enabled = true;
		commandCenter.ChangePlaybackPositionCommand.AddTarget(SeekCommand);

		commandCenter.NextTrackCommand.Enabled = true;
		commandCenter.NextTrackCommand.AddTarget(NextCommand);

		commandCenter.PreviousTrackCommand.Enabled = true;
		commandCenter.PreviousTrackCommand.AddTarget(PreviousCommand);
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
	/// <param name="playerItems"></param>
	public void SetMetadata(List<AVPlayerItem> playerItems, AVPlayerItem? playerItem)
	{
		if (playerItem is null)
		{
			Metadata.ClearNowPlaying();
			return;
		}
		items = playerItems;
		// Extract metadata from AVPlayerItem
		if (playerItem?.ExternalMetadata != null)
		{
			foreach (var metadataItem in playerItem.ExternalMetadata)
			{
				if (metadataItem.Key as NSString ==  AVMetadata.CommonKeyTitle)
				{
					NowPlayingInfo.Title = metadataItem.StringValue;
				}
				else if (metadataItem.Key as NSString == AVMetadata.CommonKeyArtist)
				{
					NowPlayingInfo.Artist = metadataItem.StringValue;
				}
				else if (metadataItem.Key as NSString == AVMetadata.CommonKeyArtwork)
				{
					NowPlayingInfo.Artwork = new(boundsSize: new(320, 240), requestHandler: _ => GetImage(metadataItem.StringValue));
				}
			}
			MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
		}
		
		NowPlayingInfo.PlaybackDuration = playerItem?.Duration.Seconds ?? 0;
		NowPlayingInfo.IsLiveStream = false;
		NowPlayingInfo.PlaybackRate = mediaElement.Speed;
		NowPlayingInfo.ElapsedPlaybackTime = playerItem?.CurrentTime.Seconds ?? 0;
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
	}

	static UIImage GetImage(string? imageUri)
	{
		if (imageUri is null)
		{
			return defaultUIImage;
		}
		try
		{
			if (imageUri.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
			{
				return UIImage.LoadFromData(NSData.FromUrl(new NSUrl(imageUri))) ?? defaultUIImage;
			}
			return defaultUIImage;
		}
		catch
		{
			return defaultUIImage;
		}
	}

	MPRemoteCommandHandlerStatus NextCommand(MPRemoteCommandEvent commandEvent)
	{
		if(commandEvent is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}
		player.AdvanceToNextItem();
		return MPRemoteCommandHandlerStatus.Success;
	}

	MPRemoteCommandHandlerStatus PreviousCommand(MPRemoteCommandEvent commandEvent)
	{
		if (commandEvent is null || player.CurrentItem is null)
		{
			return MPRemoteCommandHandlerStatus.CommandFailed;
		}

		if(items.IndexOf(player.CurrentItem) is int currentIndex)
		{
			if (currentIndex <= 0)
			{
				return MPRemoteCommandHandlerStatus.CommandFailed;
			}
			player.RemoveAllItems();
			items.ForEach(item => player.InsertItem(item, null));
			foreach (var item in items)
			{
				if (item == items[currentIndex - 1])
				{
					break;
				}
				player.AdvanceToNextItem();
			}
			player.Play();
		}
		return MPRemoteCommandHandlerStatus.Success;
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