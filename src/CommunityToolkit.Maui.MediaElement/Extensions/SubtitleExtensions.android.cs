﻿using Android.Graphics;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer2.UI;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Primitives;
using static Android.Views.ViewGroup;
using CurrentPlatformActivity = CommunityToolkit.Maui.Extensions.PageExtensions.CurrentPlatformActivity;

namespace CommunityToolkit.Maui.Extensions;

class SubtitleExtensions : Java.Lang.Object
{
	readonly IDispatcher dispatcher;
	readonly RelativeLayout.LayoutParams? subtitleLayout;
	readonly StyledPlayerView styledPlayerView;
	List<SubtitleCue> cues;
	IMediaElement? mediaElement;
	TextView? subtitleView;
	System.Timers.Timer? timer;

	public SubtitleExtensions(StyledPlayerView styledPlayerView, IDispatcher dispatcher)
	{
		this.dispatcher = dispatcher;
		this.styledPlayerView = styledPlayerView;
		cues = [];

		subtitleLayout = new RelativeLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
		subtitleLayout.AddRule(LayoutRules.AlignParentBottom);
		subtitleLayout.AddRule(LayoutRules.CenterHorizontal);

		InitializeTextBlock();
		MauiMediaElement.FullScreenChanged += OnFullScreenChanged;
	}

	public async Task LoadSubtitles(IMediaElement mediaElement)
	{
		ArgumentNullException.ThrowIfNull(subtitleView);
		this.mediaElement = mediaElement;
		
		cues.Clear();
		subtitleView.Text = string.Empty;
		if (string.IsNullOrEmpty(mediaElement.SubtitleUrl))
		{
			return;
		}

		SubtitleParser parser;
		var content = await SubtitleParser.Content(mediaElement.SubtitleUrl);
		if (mediaElement.CustomSubtitleParser is not null)
		{
			parser = new(mediaElement.CustomSubtitleParser);
			cues = parser.ParseContent(content);
			return;
		}

		switch (mediaElement.SubtitleUrl)
		{
			case var url when url.EndsWith("srt"):
				parser = new(new SrtParser());
				cues = parser.ParseContent(content);
				break;
			case var url when url.EndsWith("vtt"):
				parser = new(new VttParser());
				cues = parser.ParseContent(content);
				break;
			default:
				System.Diagnostics.Trace.TraceError("Unsupported Subtitle file.");
				return;
		}
	}

	public void StartSubtitleDisplay()
	{
		if(cues.Count == 0 || string.IsNullOrEmpty(mediaElement?.SubtitleUrl))
		{
			return;
		}

		ArgumentNullException.ThrowIfNull(subtitleView);
		if(styledPlayerView.Parent is not ViewGroup parent)
		{
			System.Diagnostics.Trace.TraceError("StyledPlayerView parent is not a ViewGroup");
			return;
		}
		dispatcher.Dispatch(() => parent.AddView(subtitleView));
		timer = new System.Timers.Timer(1000);
		timer.Elapsed += UpdateSubtitle;
		timer.Start();
	}

	public void StopSubtitleDisplay()
	{
		if (timer is null || subtitleView is null)
		{
			cues.Clear();
			return;
		}
		if (styledPlayerView.Parent is ViewGroup parent)
		{
			dispatcher.Dispatch(() => parent.RemoveView(subtitleView));
		}
		subtitleView.Text = string.Empty;
		timer.Stop();
		timer.Elapsed -= UpdateSubtitle;
	}

	void UpdateSubtitle(object? sender, System.Timers.ElapsedEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(subtitleView);
		ArgumentNullException.ThrowIfNull(mediaElement);
		if (cues.Count == 0)
		{
			return;
		}
		
		var cue = cues.Find(c => c.StartTime <= mediaElement.Position && c.EndTime >= mediaElement.Position);
		dispatcher.Dispatch(() =>
		{
			if (cue is not null)
			{
				Typeface? typeface = Typeface.CreateFromAsset(Platform.AppContext.ApplicationContext?.Assets, new Core.FontExtensions.FontFamily(mediaElement.SubtitleFont).Android) ?? Typeface.Default;
				subtitleView.SetTypeface(typeface, TypefaceStyle.Normal);
				subtitleView.Text = cue.Text;
				subtitleView.TextSize = (float)mediaElement.SubtitleFontSize;
				subtitleView.Visibility = ViewStates.Visible;
			}
			else
			{
				subtitleView.Text = string.Empty;
				subtitleView.Visibility = ViewStates.Gone;
			}
		});
	}

	void InitializeTextBlock()
	{
		subtitleView = new(CurrentPlatformActivity.CurrentActivity.ApplicationContext)
		{
			Text = string.Empty,
			HorizontalScrollBarEnabled = false,
			VerticalScrollBarEnabled = false,
			TextAlignment = Android.Views.TextAlignment.Center,
			Visibility = Android.Views.ViewStates.Gone,
			LayoutParameters = subtitleLayout
		};
		subtitleView.SetBackgroundColor(Android.Graphics.Color.Argb(150, 0, 0, 0));
		subtitleView.SetTextColor(Android.Graphics.Color.White);
		subtitleView.SetPaddingRelative(10, 10, 10, 20);
	}

	void OnFullScreenChanged(object? sender, FullScreenEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(subtitleView);

		// If the subtitle URL is empty do nothing
		if (string.IsNullOrEmpty(mediaElement?.SubtitleUrl))
		{
			return;
		}
		
		if (CurrentPlatformActivity.CurrentViewGroup.Parent is not ViewGroup parent)
		{
			return;
		}

		switch (e.isFullScreen)
		{
			case true:
				CurrentPlatformActivity.CurrentViewGroup.RemoveView(subtitleView);
				InitializeTextBlock();
				parent.AddView(subtitleView);
				break;
			case false:
				parent.RemoveView(subtitleView);
				InitializeTextBlock();
				CurrentPlatformActivity.CurrentViewGroup.AddView(subtitleView);
				break;
		}
	}
}
