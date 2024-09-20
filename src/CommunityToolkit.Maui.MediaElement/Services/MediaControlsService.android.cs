﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Media3.ExoPlayer;
using AndroidX.Media3.Session;
using AndroidX.Media3.UI;
using CommunityToolkit.Maui.Services;
using Microsoft.Win32.SafeHandles;
using AudioAttributes = AndroidX.Media3.Common.AudioAttributes;
using Resource = Microsoft.Maui.Controls.Resource;

namespace CommunityToolkit.Maui.Media.Services;

[Service(Exported = false, Enabled = true, Name = "communityToolkit.maui.media.services", ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
class MediaControlsService : MediaSessionService
{
	bool isDisposed;
	readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

    public NotificationCompat.Builder? Notification;
    public MediaSession? Session;
	public AndroidX.Media3.ExoPlayer.IExoPlayer? Player;
	PlayerNotificationManager? playerNotificationManager;
	NotificationManager? notificationManager;

	public PlayerView? PlayerView { get; set; }

    public BoundServiceBinder? Binder = null;
    public override IBinder? OnBind(Intent? intent)
	{
		Binder ??= new BoundServiceBinder(this);
       
        return Binder;
    }

    public override StartCommandResult OnStartCommand([NotNull] Intent? intent, StartCommandFlags flags, int startId)
	{
		ArgumentNullException.ThrowIfNull(intent);
       
        StartForegroundServices(intent);
		return StartCommandResult.NotSticky;
	}

	public override void OnCreate()
	{
		base.OnCreate();	
	}

	public override void OnTaskRemoved(Intent? rootIntent)
	{
		base.OnTaskRemoved(rootIntent);
        if (Player?.PlayWhenReady == true)
        {
            Player.Stop();
        }
        StopSelf();
    }
	public override void OnDestroy()
    {
		playerNotificationManager?.SetPlayer(null);
		notificationManager?.CancelAll();
		Player?.Pause();
		Session?.Release();
		Player?.Release();

		Player?.Dispose();
		notificationManager?.Dispose();
		playerNotificationManager?.Dispose();
		Session?.Dispose();

		Session = null;
		notificationManager = null;
		playerNotificationManager = null;
		Player = null;
        Platform.CurrentActivity?.StopService(new Intent(Platform.AppContext, typeof(MediaControlsService)));
        base.OnDestroy();
    }

    protected override void Dispose(bool disposing)
	{
		if (!isDisposed)
		{
			if (disposing)
			{
				safeHandle.Dispose();
			}
			isDisposed = true;
		}
		base.Dispose(disposing);
	}
    static void CreateNotificationChannel(NotificationManager notificationMnaManager)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            var channel = new NotificationChannel("1", "notification", NotificationImportance.Low);
            notificationMnaManager.CreateNotificationChannel(channel);
        }    
    }

	[MemberNotNull(nameof(Session))]
    [MemberNotNull(nameof(Player))]
    void StartForegroundServices(Intent mediaManagerIntent)
	{
		Player = new ExoPlayerBuilder(Platform.AppContext).Build() ?? throw new InvalidOperationException("Player cannot be null");
		Player.SetHandleAudioBecomingNoisy(true);
		Player.SetAudioAttributes(AudioAttributes.Default, true);

		PlayerView = new PlayerView(Platform.AppContext)
		{
			Player = Player,
			UseController = false,
			ControllerAutoShow = false,
			LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
		};
		notificationManager = GetSystemService(NotificationService) as NotificationManager;
		Notification = new NotificationCompat.Builder(Platform.AppContext, "1");

		string RandomId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..8];
		var mediaSessionWRandomId = new AndroidX.Media3.Session.MediaSession.Builder(Platform.AppContext, Player);
		mediaSessionWRandomId.SetId(RandomId);
		Session ??= mediaSessionWRandomId.Build();
		ArgumentNullException.ThrowIfNull(mediaManagerIntent);
		ArgumentNullException.ThrowIfNull(Session);
		ArgumentNullException.ThrowIfNull(Player);

		var style = new MediaStyleNotificationHelper.MediaStyle(Session);
		
		Notification.SetSmallIcon(Resource.Drawable.media3_notification_small_icon);
		Notification.SetAutoCancel(false);
		Notification.SetVisibility(NotificationCompat.VisibilityPublic);
		if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
			style.SetShowActionsInCompactView(0, 1, 2, 3, 4);
		}
		else
		{
			LegacyNotifications();
		}
		Notification.SetStyle(style);
		if (OperatingSystem.IsAndroidVersionAtLeast(26))
        {
			 ArgumentNullException.ThrowIfNull(notificationManager);
            CreateNotificationChannel(notificationManager);
        }

        if (OperatingSystem.IsAndroidVersionAtLeast(29))
        {
			ArgumentNullException.ThrowIfNull(Notification);
            StartForeground(1, Notification.Build(), ForegroundService.TypeMediaPlayback);
            return;
        }

        if (OperatingSystem.IsAndroidVersionAtLeast(26))
        {
			ArgumentNullException.ThrowIfNull(Notification);
			StartForeground(1, Notification.Build());
        }
    }
	void LegacyNotifications()
	{
		ArgumentNullException.ThrowIfNull(Player);
		ArgumentNullException.ThrowIfNull(Session);
		playerNotificationManager = new PlayerNotificationManager.Builder(Platform.AppContext, 1, "1").Build();
		
		ArgumentNullException.ThrowIfNull(playerNotificationManager);
		playerNotificationManager.SetUseFastForwardAction(true);
		playerNotificationManager.SetUseFastForwardActionInCompactView(true);
		playerNotificationManager.SetUseRewindAction(true);
		playerNotificationManager.SetUseRewindActionInCompactView(true);
		playerNotificationManager.SetUseNextAction(true);
		playerNotificationManager.SetUseNextActionInCompactView(true);
		playerNotificationManager.SetUsePlayPauseActions(true);
		playerNotificationManager.SetUsePreviousAction(true);
		playerNotificationManager.SetColor(Resource.Color.abc_primary_text_material_dark);
		playerNotificationManager.SetUsePreviousActionInCompactView(true);
		playerNotificationManager.SetVisibility(NotificationCompat.VisibilityPublic);
		playerNotificationManager.SetMediaSessionToken(Session.SessionCompatToken);
		playerNotificationManager.SetPlayer(Player);
		playerNotificationManager.SetColorized(true);
		playerNotificationManager.SetShowPlayButtonIfPlaybackIsSuppressed(true);
		playerNotificationManager.SetSmallIcon(Resource.Drawable.media3_notification_small_icon);
		playerNotificationManager.SetPriority(NotificationCompat.PriorityDefault);
		playerNotificationManager.SetUseChronometer(true);
	}
	public override AndroidX.Media3.Session.MediaSession? OnGetSession(MediaSession.ControllerInfo? p0)
	{
		ArgumentNullException.ThrowIfNull(Session);
		return Session;
	}
}