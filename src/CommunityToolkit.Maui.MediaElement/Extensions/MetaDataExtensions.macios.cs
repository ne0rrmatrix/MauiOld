﻿
using AVFoundation;
using CommunityToolkit.Maui.Core;
using Foundation;
using MediaPlayer;
using System.Diagnostics;
using UIKit;
using static TagLib.File;
using System.Net.Http;
using File = System.IO.File;
using CoreMedia;

namespace CommunityToolkit.Maui.Extensions;

/// <summary>
/// The class that provides methods to update the system UI for media transport controls to display media metadata.
/// </summary>
public partial class MetaDataExtensions
{
	ID3Tag id3Tag { get; set; } = new();

	/// <summary>
	/// Provides the metadata for the currently playing media.
	/// </summary>
	public MPNowPlayingInfo NowPlayingInfo { get; set; } = new();

	/// <summary>
	/// The media element that is currently playing.
	/// </summary>
	protected IMediaElement? MediaElement { get; set; }

	/// <summary>
	/// The platform-specific media player.
	/// </summary>
	protected PlatformMediaElement? Player { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MetaDataExtensions"/> class.
	/// </summary>
	/// <param name="mediaElement"></param>
	/// <param name="player"></param>
	public MetaDataExtensions(IMediaElement mediaElement, PlatformMediaElement player)
	{
		MediaElement = mediaElement;
		Player = player;
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

	/// <summary>
	/// Remote command to seek to a specific position in the media.
	/// </summary>
	/// <param name="commandEvent"></param>
	/// <returns></returns>
	protected MPRemoteCommandHandlerStatus SeekCommand(MPRemoteCommandEvent commandEvent)
	{
		if (Player is not null)
		{
			var eventArgs = commandEvent as MPChangePlaybackPositionCommandEvent;
			if (eventArgs is not null)
			{
				var seekTime = CMTime.FromSeconds(eventArgs.PositionTime, 1);
				Player.Seek(seekTime);
			}
		}
		return MPRemoteCommandHandlerStatus.Success;
	}
	/// <summary>
	/// Remote command to seek backward 10 seconds.
	/// </summary>
	/// <param name="commandEvent"></param>
	/// <returns></returns>
	protected MPRemoteCommandHandlerStatus SeekBackwardCommand(MPRemoteCommandEvent commandEvent)
	{
		if (Player is not null)
		{
			var seekTime = Player.CurrentTime - CMTime.FromSeconds(10, 1);
			Player.Seek(seekTime);
		}
		return MPRemoteCommandHandlerStatus.Success;
	}

	/// <summary>
	/// Remote command to seek forward 10 seconds.
	/// </summary>
	/// <param name="commandEvent"></param>
	/// <returns></returns>
	protected MPRemoteCommandHandlerStatus SeekForwardCommand(MPRemoteCommandEvent commandEvent)
	{
		if (Player is not null)
		{
			var seekTime = Player.CurrentTime + CMTime.FromSeconds(10, 1);
			Player.Seek(seekTime);
		}
		return MPRemoteCommandHandlerStatus.Success;
	}

	/// <summary>
	/// Remote command to play the media.
	/// </summary>
	/// <param name="commandEvent"></param>
	/// <returns></returns>
	protected MPRemoteCommandHandlerStatus PlayCommand(MPRemoteCommandEvent commandEvent)
	{
		Player?.Play();
		return MPRemoteCommandHandlerStatus.Success;
	}

	/// <summary>
	/// Remote command to pause the media.
	/// </summary>
	/// <param name="commandEvent"></param>
	/// <returns></returns>
	protected MPRemoteCommandHandlerStatus PauseCommand(MPRemoteCommandEvent commandEvent)
	{
		Player?.Pause();
		return MPRemoteCommandHandlerStatus.Success;
	}

	/// <summary>
	/// Remote command to toggle between play and pause.
	/// </summary>
	/// <param name="commandEvent"></param>
	/// <returns></returns>
	protected MPRemoteCommandHandlerStatus ToggleCommand(MPRemoteCommandEvent commandEvent)
	{
		if (Player is not null)
		{
			if (Player.Rate == 0)
			{
				Player?.Play();
			}
			else
			{
				Player?.Pause();
			}
		}
		return MPRemoteCommandHandlerStatus.Success;
	}

	/// <summary>
	/// Clears the metadata for the currently playing media.
	/// </summary>
	public void ClearNowPlaying()
	{
		NowPlayingInfo.AlbumTitle = string.Empty;
		NowPlayingInfo.Title = string.Empty;
		NowPlayingInfo.Artist = string.Empty;
		NowPlayingInfo.AlbumTitle = string.Empty;
		NowPlayingInfo.PlaybackDuration = 0;
		NowPlayingInfo.IsLiveStream = false;
		NowPlayingInfo.PlaybackRate = 0;
		NowPlayingInfo.ElapsedPlaybackTime = 0;
		NowPlayingInfo.Artwork = new MPMediaItemArtwork(new UIImage());
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
	}

	/// <summary>
	/// Sets the metadata for the currently playing media from a file.
	/// </summary>
	public void SetMetaDataFromFile(string url)
	{
		var temp = id3Tag.GetImageDataFromFile(url);
		ProcessData(temp);
	}

	/// <summary>
	/// Sets the metadata for the currently playing media from a URL.
	/// </summary>
	/// <param name="url"></param>
	public async Task SetMetaDataFromUrl(string url)
	{
		var temp = await id3Tag.GetImageDataFromUrl(url);
		ProcessData(temp);
	}

	/// <summary>
	/// Processes the image data and sets the metadata for the currently playing media.
	/// </summary>
	/// <param name="ImageData"></param>
	protected void ProcessData(byte[] ImageData)
	{
		UIImage? image = UIImage.LoadFromData(NSData.FromArray(ImageData));
		NowPlayingInfo.Artwork = new MPMediaItemArtwork(image ?? new UIImage());
		NowPlayingInfo.AlbumTitle = id3Tag.Album;
		NowPlayingInfo.Title = id3Tag.Title;
		NowPlayingInfo.Artist = id3Tag.Artist;
		NowPlayingInfo.AlbumTitle = id3Tag.Album;
		MainThread.BeginInvokeOnMainThread(() =>
		{
			MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
		});
	}

	/// <summary>
	/// Sets the data for the currently playing media from the media element.
	/// </summary>
	/// <param name="PlayerItem"></param>
	/// <param name="MediaElement"></param>
	public void SetMetaDataFromMediaElement(AVPlayerItem? PlayerItem, IMediaElement MediaElement)
	{
		if (MediaElement is null)
		{
			return;
		}
		NowPlayingInfo ??= new();

		NowPlayingInfo.AlbumTitle = MediaElement.Album;
		NowPlayingInfo.Title = MediaElement.Title;
		NowPlayingInfo.Artist = MediaElement.Artist;
		NowPlayingInfo.AlbumTitle = MediaElement.Album;
		NowPlayingInfo.PlaybackDuration = PlayerItem?.Duration.Seconds ?? 0;
		NowPlayingInfo.IsLiveStream = false;
		NowPlayingInfo.PlaybackRate = (float)MediaElement.Speed;
		NowPlayingInfo.ElapsedPlaybackTime = PlayerItem?.CurrentTime.Seconds ?? 0;
		MPMediaItemArtwork? artwork = GetArtwork(MediaElement.Artwork);
		if (artwork is not null)
		{
			NowPlayingInfo.Artwork = artwork;
		}
		else
		{
			NowPlayingInfo.Artwork = new MPMediaItemArtwork(new UIImage());
		}
		MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = NowPlayingInfo;
	}

	static MPMediaItemArtwork? GetArtwork(string? ImageUri)
	{
		if (!string.IsNullOrWhiteSpace(ImageUri))
		{
			UIImage? image = GetImage(ImageUri);
			return new MPMediaItemArtwork(image ?? new UIImage());
		}
		return null;
	}

	static UIImage? GetImage(string? ImageUri)
	{
		object? image = null;
		try
		{
			if (!string.IsNullOrEmpty(ImageUri))
			{
				if (ImageUri.StartsWith("http", StringComparison.CurrentCulture))
				{
					image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(ImageUri)));
				}
				else
				{
					if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
					{
						image = UIImage.FromBundle(ImageUri);
					}
				}
			}
			return image as UIImage;
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex.Message);
		}
		return null;
	}
}
