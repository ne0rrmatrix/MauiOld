using System.Runtime.Versioning;
using Android.App;
using Android.Content;
using Android.Content.PM;
using AndroidX.Media3.Common;
using AndroidX.Media3.DataSource;
using AndroidX.Media3.ExoPlayer;
using AndroidX.Media3.ExoPlayer.TrackSelection;
using AndroidX.Media3.Session;
using Java.Util;

namespace CommunityToolkit.Maui.Media.Services;

[SupportedOSPlatform("Android26.0")]
[IntentFilter(["androidx.media3.session.MediaSessionService"])]
[Service(Exported = false, Enabled = true, Name = "communityToolkit.maui.media.services", ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
sealed partial class MediaControlsService : MediaSessionService
{
	MediaSession? mediaSession;
	IExoPlayer? exoPlayer;
	DefaultTrackSelector? trackSelector;

	public override void OnTaskRemoved(Intent? rootIntent)
	{
		base.OnTaskRemoved(rootIntent);
		PauseAllPlayersAndStopSelf();
	}

	public override void OnCreate()
	{
		base.OnCreate();

		var audioAttribute = new AndroidX.Media3.Common.AudioAttributes.Builder()?
			.SetContentType(C.AudioContentTypeMusic)? // When phonecalls come in, music is paused
			.SetUsage(C.UsageMedia)?
			.Build();

		trackSelector = new DefaultTrackSelector(this);
		var trackSelectionParameters = trackSelector.BuildUponParameters()?
			.SetPreferredAudioLanguage(C.LanguageUndetermined)? // Fallback to system language if no preferred language found
			.SetPreferredTextLanguage(C.LanguageUndetermined)? // Fallback to system language if no preferred language found
			.SetIgnoredTextSelectionFlags(C.SelectionFlagAutoselect); // Ignore text tracks that are not explicitly selected by the user
		trackSelector.SetParameters((DefaultTrackSelector.Parameters.Builder?)trackSelectionParameters); // Allows us to select tracks based on user preferences

		var loadControlBuilder = new DefaultLoadControl.Builder();
		loadControlBuilder.SetBufferDurationsMs(
			minBufferMs: 15000,
			maxBufferMs: 50000,
			bufferForPlaybackMs: 2500,
			bufferForPlaybackAfterRebufferMs: 5000); // Custom buffering strategy

		var builder = new ExoPlayerBuilder(this) ?? throw new InvalidOperationException("ExoPlayerBuilder returned null");
		builder.SetTrackSelector(trackSelector);
		builder.SetAudioAttributes(audioAttribute, true);
		builder.SetHandleAudioBecomingNoisy(true); // Unplugging headphones will pause playback
		builder.SetLoadControl(loadControlBuilder.Build());
		exoPlayer = builder.Build() ?? throw new InvalidOperationException("ExoPlayerBuilder.Build() returned null");
	
		var mediaSessionBuilder = new MediaSession.Builder(this, exoPlayer);
		UUID sessionId = UUID.RandomUUID() ?? throw new InvalidOperationException("UUID.RandomUUID() returned null");
		mediaSessionBuilder.SetId(sessionId.ToString());

		var dataSourceBitmapFactory = new DataSourceBitmapLoader(this);
		mediaSessionBuilder.SetBitmapLoader(dataSourceBitmapFactory);
		mediaSession = mediaSessionBuilder.Build() ?? throw new InvalidOperationException("MediaSession.Builder.Build() returned null");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			PauseAllPlayersAndStopSelf();
			mediaSession?.Release();
			mediaSession?.Dispose();
			mediaSession = null;
			exoPlayer?.Release();
			exoPlayer = null;
			trackSelector?.Dispose();
			trackSelector = null;
		}
		base.Dispose(disposing);
	}
	
	public override void OnDestroy()
	{
		base.OnDestroy();
		PauseAllPlayersAndStopSelf();
	}

	public override MediaSession? OnGetSession(MediaSession.ControllerInfo? p0)
	{
		return mediaSession;
	[MemberNotNull(nameof(notificationBuilder), nameof(NotificationManager))]
	void StartForegroundServices()
	{
		NotificationManager ??= GetSystemService(NotificationService) as NotificationManager ?? throw new InvalidOperationException($"{nameof(NotificationManager)} cannot be null");
		notificationBuilder ??= new NotificationCompat.Builder(Platform.AppContext, "1");
		var pendingIntent = CreateActivityPendingIntent();
		notificationBuilder.SetSmallIcon(Resource.Drawable.media3_notification_small_icon);
		notificationBuilder.SetAutoCancel(false);
		notificationBuilder.SetForegroundServiceBehavior(NotificationCompat.ForegroundServiceImmediate);
		notificationBuilder.SetVisibility(NotificationCompat.VisibilityPublic);
		notificationBuilder.SetContentIntent(pendingIntent);

		CreateNotificationChannel(NotificationManager);

		if (OperatingSystem.IsAndroidVersionAtLeast(29))
		{
			if (notificationBuilder.Build() is Notification notification)
			{
				StartForeground(1, notification, ForegroundService.TypeMediaPlayback);
			}
		}
		else
		{
			StartForeground(1, notificationBuilder.Build());
		}
	}

	static PendingIntent CreateActivityPendingIntent()
	{
		var packageName = Platform.AppContext.PackageName ?? throw new InvalidOperationException("PackageName cannot be null");
		var packageManager = Platform.AppContext.PackageManager ?? throw new InvalidOperationException("PackageManager cannot be null");
		var launchIntent = packageManager.GetLaunchIntentForPackage(packageName) ?? throw new InvalidOperationException("Launch intent cannot be null");

		launchIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

		var flags = PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable;
		return PendingIntent.GetActivity(Platform.AppContext, 0, launchIntent, flags)
			   ?? throw new InvalidOperationException("PendingIntent cannot be null");
	}
}