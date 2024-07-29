﻿using Android.Graphics;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer2.UI;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Primitives;
using static Android.Views.ViewGroup;
using static CommunityToolkit.Maui.Core.Views.MauiMediaElement;
using CurrentPlatformActivity = CommunityToolkit.Maui.Core.Views.MauiMediaElement.CurrentPlatformContext;

namespace CommunityToolkit.Maui.Extensions;

partial class SubtitleExtensions : SubtitleTimer<TextView>, IDisposable
{
	FrameLayout.LayoutParams? subtitleLayout;
	readonly StyledPlayerView styledPlayerView;
	MediaElementScreenState screenState;
	bool disposedValue;

	public SubtitleExtensions(StyledPlayerView styledPlayerView, IDispatcher dispatcher)
	{
		screenState = MediaElementScreenState.Default;
		this.dispatcher = dispatcher;
		this.styledPlayerView = styledPlayerView;
		Cues = [];
		InitializeLayout();
		InitializeTextBlock();
	}
	
	public void StartSubtitleDisplay()
	{
		ArgumentNullException.ThrowIfNull(subtitleTextBlock);
		ArgumentNullException.ThrowIfNull(Cues);

		if (Cues.Count == 0 || string.IsNullOrEmpty(MediaElement?.SubtitleUrl))
		{
			return;
		}

		MediaManager.FullScreenEvents.WindowsChanged += OnFullScreenChanged;
		InitializeText();
		dispatcher.Dispatch(() => styledPlayerView.AddView(subtitleTextBlock));
		StartTimer();
	}

	public void StopSubtitleDisplay()
	{
		StopTimer();
		MediaManager.FullScreenEvents.WindowsChanged -= OnFullScreenChanged;
		ArgumentNullException.ThrowIfNull(subtitleTextBlock);
		subtitleTextBlock.Text = string.Empty;
		Cues?.Clear();
		
		dispatcher.Dispatch(() => styledPlayerView?.RemoveView(subtitleTextBlock));
	}

	public override void UpdateSubtitle(object? sender, System.Timers.ElapsedEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(subtitleTextBlock);
		ArgumentNullException.ThrowIfNull(MediaElement);
		ArgumentNullException.ThrowIfNull(Cues);

		if (Cues.Count == 0 || styledPlayerView is null)
		{
			return;
		}

		var cue = Cues.Find(c => c.StartTime <= MediaElement.Position && c.EndTime >= MediaElement.Position);
		dispatcher.Dispatch(() =>
		{
			SetHeight();
			if (cue is not null)
			{
				subtitleTextBlock.Text = cue.Text;
				subtitleTextBlock.Visibility = ViewStates.Visible;
			}
			else
			{
				subtitleTextBlock.Text = string.Empty;
				subtitleTextBlock.Visibility = ViewStates.Gone;
			}
		});
	}

	void OnFullScreenChanged(object? sender, FullScreenStateChangedEventArgs e)
	{
		var layout = CurrentPlatformContext.CurrentWindow.DecorView as ViewGroup;
		ArgumentNullException.ThrowIfNull(layout);
		dispatcher.Dispatch(() =>
		{ 
			switch(e.NewState)
			{
				case MediaElementScreenState.FullScreen:
					screenState = MediaElementScreenState.FullScreen;
					styledPlayerView.RemoveView(subtitleTextBlock);
					InitializeLayout();
					InitializeTextBlock();
					InitializeText();
					layout.AddView(subtitleTextBlock);
					break;
				default:
					screenState = MediaElementScreenState.Default;
					layout.RemoveView(subtitleTextBlock);
					InitializeLayout();
					InitializeTextBlock();
					InitializeText();
					styledPlayerView.AddView(subtitleTextBlock);
					break;
			}
		});
	}

	void SetHeight()
	{
		if (styledPlayerView is null || subtitleLayout is null || subtitleTextBlock is null)
		{
			return;
		}
		int height = styledPlayerView.Height;
		switch (screenState)
		{
			case MediaElementScreenState.Default:
				height = (int)(height * 0.1);
				break;
			case MediaElementScreenState.FullScreen:
				height = (int)(height * 0.2);
				break;
		}
		dispatcher.Dispatch(() => subtitleLayout?.SetMargins(20, 0, 20, height));
	}

	void InitializeText()
	{
		ArgumentNullException.ThrowIfNull(subtitleTextBlock);
		ArgumentNullException.ThrowIfNull(MediaElement);
		Typeface? typeface = Typeface.CreateFromAsset(Platform.AppContext.ApplicationContext?.Assets, new Core.FontExtensions.FontFamily(MediaElement.SubtitleFont).Android) ?? Typeface.Default;
		subtitleTextBlock.TextSize = (float)MediaElement.SubtitleFontSize;
		subtitleTextBlock.SetTypeface(typeface, TypefaceStyle.Normal);
	}

	void InitializeTextBlock()
	{
		subtitleTextBlock = new(CurrentPlatformActivity.CurrentActivity.ApplicationContext)
		{
			Text = string.Empty,
			HorizontalScrollBarEnabled = false,
			VerticalScrollBarEnabled = false,
			TextAlignment = Android.Views.TextAlignment.Center,
			Visibility = Android.Views.ViewStates.Gone,
			LayoutParameters = subtitleLayout
		};
		subtitleTextBlock.SetBackgroundColor(Android.Graphics.Color.Argb(150, 0, 0, 0));
		subtitleTextBlock.SetTextColor(Android.Graphics.Color.White);
		subtitleTextBlock.SetPaddingRelative(10, 10, 10, 20);
	}

	void InitializeLayout()
	{
		subtitleLayout = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
		{
			Gravity = GravityFlags.Center | GravityFlags.Bottom,
		};
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{

			if (disposing)
			{
				MediaManager.FullScreenEvents.WindowsChanged -= OnFullScreenChanged;
				StopTimer();
				subtitleLayout?.Dispose();
				subtitleTextBlock?.Dispose();
			}

			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
