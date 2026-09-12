using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>A single AdaptationSet+Representation resolved into the SegmentTemplate parts the native PlayReady
/// player wants directly (BaseUrl+Prefix+Number+Suffix, no closure) — DASH $Number$-templated SegmentTemplate
/// only, the style Axinom's v10 DASH vectors use.</summary>
sealed class DashTrack
{
	public required Uri InitUrl { get; init; }
	public required string SegmentBaseUrl { get; init; }
	public required string SegmentPrefix { get; init; }
	public required string SegmentSuffix { get; init; }
	public required int StartNumber { get; init; }
	public required int SegmentCount { get; init; }
}

/// <summary>Parses a DASH MPD's video and audio <c>SegmentTemplate</c> into the fields the native PlayReady
/// player's open request needs. Unlike the previous WebView2/dash.js path — where dash.js parsed the manifest
/// itself in JS — the native player has no manifest parser of its own, so this is done here in managed code.</summary>
sealed partial class DashManifest
{
	public required DashTrack Video { get; init; }
	public required DashTrack Audio { get; init; }

	public static async Task<DashManifest> LoadAsync(Uri mpdUri, HttpClient http)
	{
		var xml = await http.GetStringAsync(mpdUri).ConfigureAwait(false);
		var doc = XDocument.Parse(xml);
		var ns = doc.Root?.Name.Namespace ?? throw new InvalidOperationException("MPD has no root element");

		var period = doc.Root.Element(ns + "Period")
			?? throw new InvalidOperationException("MPD has no Period element");

		var periodDuration = ParseIsoDuration(
			doc.Root.Attribute("mediaPresentationDuration")?.Value
			?? period.Attribute("duration")?.Value
			?? "PT0S");

		var adaptationSets = period.Elements(ns + "AdaptationSet").ToList();

		var videoSet = adaptationSets.FirstOrDefault(a => (string?)a.Attribute("contentType") == "video")
			?? throw new InvalidOperationException("MPD has no video AdaptationSet");
		var audioSet = adaptationSets.FirstOrDefault(a => (string?)a.Attribute("contentType") == "audio")
			?? throw new InvalidOperationException("MPD has no audio AdaptationSet");

		var videoRep = videoSet.Elements(ns + "Representation")
			.OrderBy(r => (int?)r.Attribute("bandwidth") ?? int.MaxValue)
			.FirstOrDefault()
			?? throw new InvalidOperationException("video AdaptationSet has no Representation");
		var audioRep = audioSet.Elements(ns + "Representation").FirstOrDefault()
			?? throw new InvalidOperationException("audio AdaptationSet has no Representation");

		return new DashManifest
		{
			Video = BuildTrack("video", videoSet, videoRep, ns, mpdUri, periodDuration),
			Audio = BuildTrack("audio", audioSet, audioRep, ns, mpdUri, periodDuration),
		};
	}

	static DashTrack BuildTrack(string kind, XElement adaptationSet, XElement representation, XNamespace ns, Uri mpdUri, TimeSpan periodDuration)
	{
		var template = representation.Element(ns + "SegmentTemplate")
			?? adaptationSet.Element(ns + "SegmentTemplate")
			?? throw new InvalidOperationException($"{kind} Representation has no SegmentTemplate");

		var initialization = (string?)template.Attribute("initialization")
			?? throw new InvalidOperationException($"{kind} SegmentTemplate has no initialization attribute");
		var media = (string?)template.Attribute("media")
			?? throw new InvalidOperationException($"{kind} SegmentTemplate has no media attribute");
		var startNumber = (int?)template.Attribute("startNumber") ?? 1;
		var timescale = (uint?)template.Attribute("timescale") ?? 1;

		int segmentCount;
		var timeline = template.Element(ns + "SegmentTimeline");
		if (timeline is not null)
		{
			segmentCount = 0;
			foreach (var s in timeline.Elements(ns + "S"))
			{
				var repeat = (int?)s.Attribute("r") ?? 0;
				segmentCount += repeat + 1;
			}
		}
		else
		{
			var segDuration = (uint?)template.Attribute("duration")
				?? throw new InvalidOperationException($"{kind} SegmentTemplate has neither SegmentTimeline nor duration");
			var totalTicks = periodDuration.TotalSeconds * timescale;
			segmentCount = (int)Math.Ceiling(totalTicks / segDuration);
		}

		const string numberToken = "$Number$";
		var tokenIndex = media.IndexOf(numberToken, StringComparison.Ordinal);
		if (tokenIndex < 0)
		{
			throw new InvalidOperationException($"{kind} SegmentTemplate media pattern has no $Number$ token: {media}");
		}
		var mediaPrefix = media[..tokenIndex];
		var mediaSuffix = media[(tokenIndex + numberToken.Length)..];

		Uri baseUri = new(mpdUri, ".");
		return new DashTrack
		{
			InitUrl = new Uri(mpdUri, initialization),
			SegmentBaseUrl = baseUri.ToString(),
			SegmentPrefix = mediaPrefix,
			SegmentSuffix = mediaSuffix,
			StartNumber = startNumber,
			SegmentCount = segmentCount,
		};
	}

	[GeneratedRegex(@"^P(?:(?<days>\d+)D)?(?:T(?:(?<hours>\d+)H)?(?:(?<minutes>\d+)M)?(?:(?<seconds>[\d.]+)S)?)?$")]
	private static partial Regex IsoDurationRegex();

	static TimeSpan ParseIsoDuration(string value)
	{
		var m = IsoDurationRegex().Match(value);
		if (!m.Success)
		{
			return TimeSpan.Zero;
		}

		var days = m.Groups["days"].Success ? double.Parse(m.Groups["days"].Value, CultureInfo.InvariantCulture) : 0;
		var hours = m.Groups["hours"].Success ? double.Parse(m.Groups["hours"].Value, CultureInfo.InvariantCulture) : 0;
		var minutes = m.Groups["minutes"].Success ? double.Parse(m.Groups["minutes"].Value, CultureInfo.InvariantCulture) : 0;
		var seconds = m.Groups["seconds"].Success ? double.Parse(m.Groups["seconds"].Value, CultureInfo.InvariantCulture) : 0;
		return TimeSpan.FromSeconds(days * 86400 + hours * 3600 + minutes * 60 + seconds);
	}
}
