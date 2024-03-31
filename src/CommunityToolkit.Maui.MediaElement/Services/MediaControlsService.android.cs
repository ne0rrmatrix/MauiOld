﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Media.Session;
using Android.Graphics;
using CommunityToolkit.Maui.Extensions;
using Android.Media;
using Stream = Android.Media.Stream;

namespace CommunityToolkit.Maui.Services;

[Service(Exported = true, Name ="CommunityToolkit.Maui.Services", ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
public class MediaControlsService : Service	
{
	Bitmap? bitmap = null;
	PlaybackStateCompat.Builder? stateBuilder;
	MediaSessionCompat? mediaSession;

	public MediaControlsService()
	{
	}

	public async Task startForegroundServiceAsync(MediaSessionCompat.Token token,string title,string artist,string album,string albumArtUri, long position, long currentTime, long duration)
	{
		mediaSession = new MediaSessionCompat(this, "notification")
		{
			Active = true
		};
		
		await Task.Run(async () => bitmap = await MetaDataExtensions.GetBitmapFromUrl(albumArtUri, Resources));

		var intent = new Intent(this, typeof(MediaControlsService));
		var pendingIntentFlags = Build.VERSION.SdkInt >= BuildVersionCodes.S
			? PendingIntentFlags.UpdateCurrent |
			  PendingIntentFlags.Immutable
			: PendingIntentFlags.UpdateCurrent;
		var pendingIntent = PendingIntent.GetActivity(this, 2, intent, pendingIntentFlags);
		var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
		var audioManager = GetSystemService(Context.AudioService) as AudioManager;

		audioManager?.RequestAudioFocus(null, Stream.Music, AudioFocus.Gain);
		audioManager?.SetParameters("Ducking=true");
		audioManager?.SetStreamVolume(Stream.Music, audioManager.GetStreamVolume(Stream.Music), VolumeNotificationFlags.ShowUi);

		var notification = MetaDataExtensions.SetNotifications(Platform.AppContext, "1", token, title, artist, album, bitmap, pendingIntent, duration);
		var metadataBuilder = MetaDataExtensions.SetMetadata(album, artist, title, bitmap, duration, position);
		stateBuilder = new PlaybackStateCompat.Builder().SetActions(PlaybackStateCompat.ActionPlayPause | PlaybackStateCompat.ActionStop | PlaybackStateCompat.ActionSetPlaybackSpeed | PlaybackStateCompat.ActionFastForward | PlaybackStateCompat.ActionSeekTo);

		stateBuilder?.SetState(PlaybackStateCompat.StatePlaying, position, 1.0f, currentTime);
		mediaSession.SetExtras(intent.Extras);
		mediaSession.SetMetadata(metadataBuilder.Build());
		mediaSession.SetMetadata(metadataBuilder.Build());
		mediaSession.SetPlaybackState(stateBuilder?.Build());
		mediaSession.SetPlaybackToLocal(AudioManager.AudioSessionIdGenerate);
		mediaSession.SetSessionActivity(pendingIntent);
		
		if (Build.VERSION.SdkInt >= BuildVersionCodes.O && notificationManager is not null)
		{
			CreateNotificationChannel(notificationManager);
		}
	
		if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
		{
			StartForeground(1, notification.Build(), ForegroundService.TypeMediaPlayback);
			return;
		}

		if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
		{
			StartForeground(1, notification.Build());
		}
	}

	static void CreateNotificationChannel(NotificationManager notificationMnaManager)
	{
		var channel = new NotificationChannel("1", "notification", NotificationImportance.Low);
		notificationMnaManager.CreateNotificationChannel(channel);
	}

	public override IBinder? OnBind(Intent? intent)
	{
		return null;
	}

	public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
	{
		var token = intent?.GetParcelableExtra("token") as MediaSessionCompat.Token;
		var position = intent?.GetLongExtra("position", 0) ?? 0;
		var currentTime = intent?.GetLongExtra("currentTime", 0) ?? 0;
		var duration = intent?.GetLongExtra("duration", 0) ?? 0;
		var title = intent?.GetStringExtra("title") as string ?? string.Empty;
		var artist = intent?.GetStringExtra("artist") as string ?? string.Empty;
		var album = intent?.GetStringExtra("album") as string ?? string.Empty;
		var albumArtUri = intent?.GetStringExtra("albumArtUri")  as string ?? string.Empty;

		if (token is not null)
		{
			_ = startForegroundServiceAsync(token, title, artist, album, albumArtUri, position, currentTime, duration);
		}
		return StartCommandResult.NotSticky;
	}
}
