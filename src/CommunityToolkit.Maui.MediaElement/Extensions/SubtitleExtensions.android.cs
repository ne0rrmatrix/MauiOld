using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Util;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui.Extensions;
/// <summary>
/// 
/// </summary>
public  static class SubtitleExtensions
{
	public static ConcatenatingMediaSource SetSubtitles(IMediaElement mediaElement, string media, string package)
	{
		if (mediaElement.SrtFile is UriMediaSource uriMediaSource)
		{
			var uri = uriMediaSource.Uri;
			if (!string.IsNullOrWhiteSpace(uri?.AbsoluteUri))
			{
				return CreateSRTTextSource(uri.ToString(), media);
			}
		}
		if (mediaElement.SrtFile is FileMediaSource fileMediaSource)
		{
			var filename = fileMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				return CreateSRTTextSource(filename, media);
			}
		}
		if (mediaElement.SrtFile is ResourceMediaSource resourceMediaSource)
		{
			var path = resourceMediaSource.Path;
			if (!string.IsNullOrWhiteSpace(path))
			{
				var assetFilePath = $"asset://{package}{Path.PathSeparator}{path}";
				return CreateSRTTextSource(assetFilePath, media);
			}
		}
		var temp = CreateSRTTextSource(string.Empty, media);
		return temp;

	}

	static ConcatenatingMediaSource CreateSRTTextSource(string srtUrl, string videoUrl)
	{
		var mediaUrl = Android.Net.Uri.Parse(videoUrl);
		var subtitleUri = Android.Net.Uri.Parse(srtUrl);
		var httpDataSourceFactory = new DefaultHttpDataSource.Factory();

		var dataSourceFactory = new DefaultDataSourceFactory(Platform.CurrentActivity, Util.GetUserAgent(Platform.CurrentActivity, "CommunityToolkit.Maui.Sample"));
		MediaItem.Subtitle subtitle = new(subtitleUri, MimeTypes.ApplicationSubrip, "en", C.SelectionFlagDefault);
		
		var subtitleSource = new SingleSampleMediaSource.Factory(httpDataSourceFactory).CreateMediaSource(subtitle, Int32.MaxValue);
		var mediaSource = new DefaultMediaSourceFactory(httpDataSourceFactory).CreateMediaSource(MediaItem.FromUri(mediaUrl));
		
		if (videoUrl.Contains("asset://"))
		{
			mediaSource = new ProgressiveMediaSource.Factory(dataSourceFactory).CreateMediaSource(MediaItem.FromUri(mediaUrl));
		}	
		
		if (srtUrl.Contains("asset://"))
		{
			subtitleSource = new SingleSampleMediaSource.Factory(dataSourceFactory).CreateMediaSource(subtitle, Int32.MaxValue);
		}

		if (mediaSource is not null && string.IsNullOrEmpty(srtUrl) && subtitleSource is null)
		{
			return new ConcatenatingMediaSource(mediaSource);
		}
		if (mediaSource is null || subtitleSource is null)
		{
			return new ConcatenatingMediaSource();
		}

		var mergedSource = new MergingMediaSource(mediaSource, subtitleSource);
		return new ConcatenatingMediaSource(mergedSource);
	}
}
