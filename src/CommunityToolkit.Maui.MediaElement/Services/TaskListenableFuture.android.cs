using AndroidX.Media3.Session;
using Google.Common.Util.Concurrent;
using Java.Lang;
using Java.Util.Concurrent;

namespace CommunityToolkit.Maui.Services;

sealed partial class MediaSessionCallback
{
	sealed class TaskListenableFuture(Task<SessionResult> task) : Java.Lang.Object, IListenableFuture
	{
		public void AddListener(IRunnable? listener, IExecutor? executor)
		{
          task.ContinueWith(static (_, state) =>
			{
				if (state is not ListenerState listenerState || listenerState.Listener is null)
				{
					return;
				}

				if (listenerState.Executor is null)
				{
					listenerState.Listener.Run();
					return;
				}

				listenerState.Executor.Execute(listenerState.Listener);
			}, new ListenerState(listener, executor), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}

		public bool Cancel(bool mayInterruptIfRunning)
		{
			return false;
		}

		public Java.Lang.Object? Get()
		{
			return task.GetAwaiter().GetResult();
		}

		public Java.Lang.Object? Get(long timeout, TimeUnit? unit)
		{
			ArgumentNullException.ThrowIfNull(unit);

			if (task.Wait(TimeSpan.FromMilliseconds(unit.ToMillis(timeout))))
			{
				return task.GetAwaiter().GetResult();
			}

         throw new System.TimeoutException("The operation timed out.");
		}

       public bool IsCancelled => task.IsCanceled;

		public bool IsDone => task.IsCompleted;

		sealed class ListenerState(IRunnable? listener, IExecutor? executor)
		{
			public IExecutor? Executor { get; } = executor;
			public IRunnable? Listener { get; } = listener;
		}
	}
}
