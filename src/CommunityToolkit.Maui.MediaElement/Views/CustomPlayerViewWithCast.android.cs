using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Media3.UI;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Views;

[Android.Runtime.Preserve(AllMembers = true)]
public class CustomPlayerViewWithCast : PlayerView
{
	bool isDisposed = false;
	bool menuHooked = false;

	public event Action<MediaTrackType>? TrackSelectionRequested;

	public CustomPlayerViewWithCast(Context context) : base(context)
	{
		Initialize();
	}

	public CustomPlayerViewWithCast(Context context, IAttributeSet attrs) : base(context, attrs)
	{
		Initialize();
	}

	void Initialize()
	{
		if (ViewTreeObserver is null)
		{
			throw new InvalidOperationException("ViewTreeObserver is not available at this point.");
		}

		ViewTreeObserver.GlobalLayout += OnGlobalLayout;
	}

	void OnGlobalLayout(object? sender, EventArgs e)
	{
		if (isDisposed)
		{
			return;
		}

		if (menuHooked && ViewTreeObserver is not null)
		{
			ViewTreeObserver.GlobalLayout -= OnGlobalLayout;
			return;
		}

		TryHookSettingsMenu();
	}

	protected override void Dispose(bool disposing)
	{
		if (!isDisposed)
		{
			isDisposed = true;
			if (ViewTreeObserver is not null)
			{
				ViewTreeObserver.GlobalLayout -= OnGlobalLayout;
			}
		}
		base.Dispose(disposing);
	}

	void TryHookSettingsMenu()
	{
		if (Context is null || menuHooked)
		{
			return;
		}

		var resources = Context.Resources;
		var packageName = Context.PackageName ??
			throw new InvalidOperationException("Context cannot be null.");

		var settingsId = resources?.GetIdentifier("exo_settings", "id", packageName) ??
			throw new InvalidOperationException("Settings button not found in PlayerView.");
		var settingsButton = FindViewById<Android.Widget.ImageButton>(settingsId) ?? 
			throw new InvalidOperationException("Settings button not found in PlayerView.");
		settingsButton.Click -= SettingsButton_Click;
		settingsButton.Click += SettingsButton_Click;

		menuHooked = true;
	}

	void SettingsButton_Click(object? sender, EventArgs e)
	{
		if (Context is null)
		{
			return;
		}

		var resources = Context.Resources;
		var packageName = Context.PackageName
			?? throw new InvalidOperationException("Context cannot be null.");

		int styleId = resources?.GetIdentifier(
			"CustomPopupMenuStyle", // Theme overlay name
			"style",
			packageName) ?? 0;
		
		var themedContext = new ContextThemeWrapper(Context, styleId);
		
		if(sender is not Android.Views.View view)
		{
			return;
		}
		var popup = new PopupMenu(themedContext, view);
		
		popup.Menu?.Add("Audio Tracks")?.SetOnMenuItemClickListener(new MenuClickListener(() =>
		{
			TrackSelectionRequested?.Invoke(MediaTrackType.Audio);
		}));
		popup.Menu?.Add("Video Quality")?.SetOnMenuItemClickListener(new MenuClickListener(() =>
		{
			TrackSelectionRequested?.Invoke(MediaTrackType.Video);
		}));
		popup.Menu?.Add("Subtitles")?.SetOnMenuItemClickListener(new MenuClickListener(() =>
		{
			TrackSelectionRequested?.Invoke(MediaTrackType.Text);
		}));
		popup.Show();
	}

	class MenuClickListener(Action action) : Java.Lang.Object, IMenuItemOnMenuItemClickListener
	{
		readonly Action action = action;

		public bool OnMenuItemClick(IMenuItem item)
		{
			action();
			return true;
		}
	}
}