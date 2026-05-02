using Android.OS;
using AndroidX.Media3.Session;
using CommunityToolkit.Maui.Media.Services;

namespace CommunityToolkit.Maui.Services;

sealed partial class MediaSessionCallback(MediaControlsService mediaControlsService) : Java.Lang.Object, MediaSession.ICallback
{
	public const string PlayerIdKey = "CommunityToolkit.Maui.MediaElement.PlayerId";
	public const string ReleasePlayer = "ReleasePlayer";
	readonly MediaControlsService mediaControlsService = mediaControlsService;

	public MediaSession.ConnectionResult OnConnect(MediaSession? session, MediaSession.ControllerInfo? controller)
	{
		ArgumentNullException.ThrowIfNull(session);
		ArgumentNullException.ThrowIfNull(controller);

		var sessionCommands = MediaSession.ConnectionResult.DefaultSessionCommands?.BuildUpon()?
			.Add(new SessionCommand(ReleasePlayer, new Bundle()))?
			.Build() ?? throw new InvalidOperationException("Failed to build session commands.");

		return new MediaSession.ConnectionResult.AcceptedResultBuilder(session)
          .SetAvailableSessionCommands(sessionCommands)?
			.Build() ?? throw new InvalidOperationException("Failed to build connection result.");
	}

 public global::Google.Common.Util.Concurrent.IListenableFuture? OnCustomCommand(MediaSession? session, MediaSession.ControllerInfo? controller, SessionCommand? customCommand, Bundle? args)
	{
        TaskCompletionSource<SessionResult> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

		try
		{
			if (customCommand?.CustomAction?.Equals(ReleasePlayer, StringComparison.Ordinal) == true)
			{
				var playerId = args?.GetString(PlayerIdKey) ?? controller?.ConnectionHints?.GetString(PlayerIdKey);
				mediaControlsService.ReleasePlayer(playerId);
			}

			taskCompletionSource.SetResult(new SessionResult(SessionResult.ResultSuccess));
		}
        catch (InvalidOperationException exception)
		{
         taskCompletionSource.SetException(exception);
		}

		return new TaskListenableFuture(taskCompletionSource.Task);
	}
}
