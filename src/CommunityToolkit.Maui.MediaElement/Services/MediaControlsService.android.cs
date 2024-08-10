using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Support.V4.Media.Session;
using AndroidX.Core.App;

namespace CommunityToolkit.Maui.Media.Services;

[Service(Exported = false, Enabled = true, Name = "communityToolkit.maui.media.services", ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
class MediaControlsService : Service
{
	PendingIntentFlags pendingIntentFlags;
	MediaSessionCompat? mediaSession;
	NotificationCompat.Builder? notification;
	MediaSessionCompat.Token? token;

	public override IBinder? OnBind(Intent? intent) => null;

	public override StartCommandResult OnStartCommand([NotNull] Intent? intent, StartCommandFlags flags, int startId)
	{
		ArgumentNullException.ThrowIfNull(intent);
		token ??= (MediaSessionCompat.Token)(intent.GetParcelableExtra("token") ?? throw new InvalidOperationException("Token cannot be null"));

		mediaSession ??= new MediaSessionCompat(Platform.AppContext, "notification")
		{
			Active = true,
		};
		pendingIntentFlags = Build.VERSION.SdkInt >= BuildVersionCodes.S
			? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
			: PendingIntentFlags.UpdateCurrent;

		InitializeNotification(mediaSession);
		return StartCommandResult.Sticky;
	}

	void InitializeNotification(MediaSessionCompat mediaSession)
	{
		var intent = new Intent(this, typeof(MediaControlsService));
		var pendingIntent = PendingIntent.GetActivity(this, 2, intent, pendingIntentFlags);
		var notificationManager = GetSystemService(NotificationService) as NotificationManager;
		
		var style = new AndroidX.Media.App.NotificationCompat.MediaStyle();
		style.SetMediaSession(token);
		style.SetShowActionsInCompactView(0, 1, 2, 3);

		notification ??= new NotificationCompat.Builder(Platform.AppContext, "1");
		notification.SetStyle(style);
		notification.SetSmallIcon(_Microsoft.Android.Resource.Designer.Resource.Drawable.exo_styled_controls_audiotrack);
		notification.SetAutoCancel(false);
		notification.SetVisibility(NotificationCompat.VisibilityPublic);

		mediaSession.SetExtras(intent.Extras);
		mediaSession.SetPlaybackToLocal(AudioManager.AudioSessionIdGenerate);
		mediaSession.SetSessionActivity(pendingIntent);

		
		var channel = new NotificationChannel("1", "notification", NotificationImportance.Low);
		notificationManager?.CreateNotificationChannel(channel);
		StartForeground(1, notification.Build(), ForegroundService.TypeMediaPlayback);
	}
}