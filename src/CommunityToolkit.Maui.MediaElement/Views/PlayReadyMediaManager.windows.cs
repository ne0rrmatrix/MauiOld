using CommunityToolkit.Maui.Views;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml.Linq;
using Windows.Media.Protection.PlayReady;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpMethod = Windows.Web.Http.HttpMethod;
using HttpRequestMessage = Windows.Web.Http.HttpRequestMessage;

namespace CommunityToolkit.Maui.Core.Views;

partial class MediaManager
{
	CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine? playReadyEngine;
	SwapChainPanel? drmSwapChainPanel;

	// Playback host selection. The MF media engine's ENABLE_PROTECTED_CONTENT
	// fails unconditionally on this machine (E_UNEXPECTED at open — verified
	// against clear content too), so the WinRT MediaPlayer + MediaProtectionManager
	// path is used. The native engine path is kept as a compiled-in fallback.
	static readonly bool useNativeEngineForDrm = false;

	// Dedicated client for license acquisition POSTs. Kept separate from
	// headerHttpClient so license-specific headers never leak into
	// manifest/segment requests and vice versa.
	HttpClient? licenseHttpClient;

	bool IsPlayReadyContent =>
		MediaElement.Source is UriMediaSource { DrmConfiguration: { Scheme: DrmScheme.PlayReady } };

	/// <summary>
	/// Loads PlayReady-protected content. WinUI 3's MediaPlayer has no protected
	/// media path (microsoft-ui-xaml#10918), so rendering runs through the native
	/// PlayReadyMediaEngine (IMFMediaEngine + SwapChainPanel). License acquisition
	/// happens out-of-band via the WinRT PlayReady API before the engine opens the
	/// content — the MF content enabler has no way to attach custom headers (e.g.
	/// X-AxDRM-Message), and this content carries no license URL of its own.
	/// </summary>
	async Task SetUriSourceWithPlayReadyAsync(Uri manifestUri, IDictionary<string, string> headers, DrmConfiguration drmConfig)
	{
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] SetUriSourceWithPlayReadyAsync — URI={manifestUri}, AutoPlay={MediaElement.ShouldAutoPlay}, SourceType={MediaElement.Source?.GetType().Name}");
		if (Player is null)
		{
			Trace.WriteLine("[MediaElement.Windows.PlayReady] Source setup skipped: Player is null");
			return;
		}

		// Step 1: Download the DASH manifest via managed HttpClient (works in the
		// AppX container; MF's WinHTTP stack is unreliable there and its default
		// HTTP scheme handler cannot classify a raw .mpd URL).
		byte[] manifestBytes;
		try
		{
			headerHttpClient ??= new HttpClient();
			headerHttpClient.DefaultRequestHeaders.Clear();
			foreach (var header in headers)
			{
				headerHttpClient.DefaultRequestHeaders.TryAppendWithoutValidation(header.Key, header.Value);
			}

			using var request = new HttpRequestMessage(HttpMethod.Get, manifestUri);
			using var response = await headerHttpClient.SendRequestAsync(request).AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
			response.EnsureSuccessStatusCode();
			var responseBuffer = await response.Content.ReadAsBufferAsync().AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
			manifestBytes = responseBuffer.ToArray();
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Downloaded DASH manifest — {manifestBytes.Length} bytes");
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Failed to download DASH manifest: {ex.Message}");
			ReportMediaFailed($"Failed to download DRM manifest: {ex.Message}");
			return;
		}

		// Step 2: Extract the content key ID from the manifest.
		var keyId = TryExtractKeyId(manifestBytes);
		if (keyId is null)
		{
			ReportMediaFailed("No cenc:default_KID found in the DASH manifest — cannot determine which license to acquire");
			return;
		}

		// Step 3: Proactively acquire the license (with custom headers) BEFORE the
		// engine opens the content. The license lands in this app's PlayReady
		// license store, keyed by KID; the protected media path then finds it
		// without invoking the (header-less) MF content enabler.
		if (!await TryAcquireLicenseProactivelyAsync(keyId.Value, drmConfig).ConfigureAwait(ConfigureAwaitOptions.ForceYielding))
		{
			return; // Failure already reported
		}

		// Windows has no DASH byte-stream handler (HKLM\SOFTWARE\Microsoft\Windows
		// Media Foundation\ByteStreamHandlers), so the media engine cannot consume
		// the .mpd directly — feeding it crashed source resolution when the MKV
		// handler probed the XML (mfmkvsrcsnk.dll, 0xc0000005). Present the content
		// as a fragmented MP4 instead: init segment + media segments concatenated.
		// CENC encryption and the PlayReady PSSH survive untouched, so the engine's
		// protected pipeline still decrypts with the license acquired above.
		var fmp4 = await BuildFmp4FromDashManifestAsync(manifestBytes, manifestUri, headers);
		if (fmp4 is null)
		{
			ReportMediaFailed("Failed to build a playable stream from the DASH manifest");
			return;
		}

		var (fmp4Bytes, fmp4CachePath) = fmp4.Value;

		// Diagnostic probe: does the CDM license-request flow work inside the
		// packaged app? (It fails in unpackaged processes with 0x80041301/0x8004C3E8 —
		// matching microsoft/media-foundation issue #37.)
		try
		{
			var cdmProbe = new CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine();
			var firstHeader = drmConfig.LicenseRequestHeaders.FirstOrDefault();
			var probeSummary = cdmProbe.TestCdmGenerateRequest(
				fmp4CachePath,
				drmConfig.LicenseServerUrl?.AbsoluteUri ?? string.Empty,
				firstHeader.Key ?? string.Empty,
				firstHeader.Value ?? string.Empty);
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] CDM probe — {probeSummary}");
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] CDM probe threw: {ex.Message}");
		}

		// Step 4: Play on the UI thread. The MF media engine's ENABLE_PROTECTED_CONTENT
		// fails unconditionally on this machine (E_UNEXPECTED at open — verified
		// against clear content too), so playback goes through the WinRT MediaPlayer
		// + MediaProtectionManager pipeline: the same Windows.Media.Protection
		// runtime that acquired the license above. If the pipeline reaches Playing,
		// frame-server rendering comes next.
		await Dispatcher.DispatchAsync(async () =>
		{
			if (useNativeEngineForDrm)
			{
				var drmPanel = CreateDrmPlatformView();
				if (mauiMediaElement is not null)
				{
					Trace.WriteLine("[MediaElement.Windows.PlayReady] Attaching DRM SwapChainPanel to MauiMediaElement");
					mauiMediaElement.SwapToDrmView(drmPanel);
				}
				else
				{
					Trace.WriteLine("[MediaElement.Windows.PlayReady] WARNING: MauiMediaElement is null; DRM panel cannot be attached");
				}

				if (drmConfig.LicenseServerUrl is null || playReadyEngine is null)
				{
					Trace.WriteLine("[MediaElement.Windows.PlayReady] WARNING: missing license URL or engine; native source not set");
					return;
				}

				var winrtDrmConfig = new CommunityToolkit.Maui.Media.WinRT.PlayReadyDrmConfiguration
				{
					LicenseServerUrl = drmConfig.LicenseServerUrl.AbsoluteUri
				};

				foreach (var header in drmConfig.LicenseRequestHeaders)
				{
					winrtDrmConfig.CustomHeaders.Add(header.Key, header.Value);
				}

				Trace.WriteLine($"[MediaElement.Windows.PlayReady] DRM config — LicenseServer={drmConfig.LicenseServerUrl}, HeaderNames=[{string.Join(", ", drmConfig.LicenseRequestHeaders.Keys)}], HardwareSecurity={drmConfig.RequiresHardwareSecurity}");

				try
				{
					playReadyEngine.SetSourceFromManifestBytes(fmp4Bytes, manifestUri.AbsoluteUri, winrtDrmConfig);
					Trace.WriteLine("[MediaElement.Windows.PlayReady] Native SetSourceFromManifestBytes returned");

					if (MediaElement.ShouldAutoPlay)
					{
						Trace.WriteLine("[MediaElement.Windows.PlayReady] AutoPlay requested; calling native Play");
						playReadyEngine.Play();
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine($"[MediaElement.Windows.PlayReady] Failed to set source: {ex.Message}");
					ReportMediaFailed($"Source setup failed: {ex.Message}");
				}

				return;
			}

			// CDM probe results determine the next playback implementation.
			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Extracts the content key ID from a DASH manifest's
	/// <c>cenc:default_KID</c> attribute on a ContentProtection element.
	/// </summary>
	static Guid? TryExtractKeyId(byte[] manifestBytes)
	{
		try
		{
			// Load from a stream, not a decoded string: Encoding.UTF8.GetString turns
			// a UTF-8 BOM into a U+FEFF at position 0, which XDocument.Parse(string)
			// rejects ("Data at the root level is invalid. Line 1, position 1.").
			using var stream = new MemoryStream(manifestBytes, writable: false);
			var doc = XDocument.Load(stream);
			foreach (var contentProtection in doc.Descendants().Where(e => e.Name.LocalName == "ContentProtection"))
			{
				var kidValue = contentProtection.Attributes()
					.FirstOrDefault(a => a.Name.LocalName == "default_KID")?.Value;
				if (Guid.TryParse(kidValue, out var keyId))
				{
					Trace.WriteLine($"[MediaElement.Windows.PlayReady] Extracted content KID {keyId} from manifest");
					return keyId;
				}
			}

			Trace.WriteLine("[MediaElement.Windows.PlayReady] No default_KID attribute found in manifest");
			return null;
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Failed to parse manifest for KID: {ex.Message}");
			return null;
		}
	}

	/// <summary>
	/// Builds a playable fragmented-MP4 stream from a DASH manifest by
	/// concatenating the init segment and all media segments of the
	/// lowest-bandwidth video representation (smallest download).
	/// Audio is skipped for now — muxing a second track requires merging
	/// moov boxes, which comes later.
	/// </summary>
	async Task<(byte[] Bytes, string CachePath)?> BuildFmp4FromDashManifestAsync(byte[] manifestBytes, Uri manifestUri, IDictionary<string, string> headers)
	{
		try
		{
			using var stream = new MemoryStream(manifestBytes, writable: false);
			var doc = XDocument.Load(stream);

			var videoRepresentation = doc.Descendants()
				.Where(e => e.Name.LocalName == "AdaptationSet" && (string?)e.Attribute("contentType") == "video")
				.SelectMany(a => a.Elements().Where(e => e.Name.LocalName == "Representation"))
				.OrderBy(r => (long?)r.Attribute("bandwidth") ?? long.MaxValue)
				.FirstOrDefault();
			if (videoRepresentation is null)
			{
				Trace.WriteLine("[MediaElement.Windows.PlayReady] No video Representation found in manifest");
				return null;
			}

			var segmentTemplate = videoRepresentation.Elements().FirstOrDefault(e => e.Name.LocalName == "SegmentTemplate");
			var initialization = (string?)segmentTemplate?.Attribute("initialization");
			var media = (string?)segmentTemplate?.Attribute("media");
			var startNumber = (int?)segmentTemplate?.Attribute("startNumber") ?? 1;
			if (segmentTemplate is null || initialization is null || media is null)
			{
				Trace.WriteLine("[MediaElement.Windows.PlayReady] Unsupported segment layout — expected SegmentTemplate with initialization and media");
				return null;
			}

			// Expand the SegmentTimeline into per-segment durations: each S is one
			// segment plus r repeats. (r = -1 for open-ended live is not handled —
			// this flow targets static presentations.)
			var segmentDurations = new List<long>();
			foreach (var s in segmentTemplate.Descendants().Where(e => e.Name.LocalName == "S"))
			{
				var duration = (long?)s.Attribute("d") ?? 0;
				var repeat = Math.Max((int?)s.Attribute("r") ?? 0, 0);
				for (var i = 0; i <= repeat; i++)
				{
					segmentDurations.Add(duration);
				}
			}

			var segmentCount = segmentDurations.Count;
			if (segmentCount == 0)
			{
				Trace.WriteLine("[MediaElement.Windows.PlayReady] Unsupported segment layout — SegmentTemplate has no SegmentTimeline");
				return null;
			}

			// ALL segments are stitched: the init segment's mehd declares the full
			// presentation duration, and the MP4 source reconciles that against the
			// media actually present — a truncated stream fails open with
			// E_UNEXPECTED (verified: 6/184 segments → SRC_NOT_SUPPORTED).
			var segmentsToFetch = segmentCount;
			var resolution = $"{videoRepresentation.Attribute("width")?.Value}x{videoRepresentation.Attribute("height")?.Value}";
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Stitching fMP4 — init + {segmentsToFetch}/{segmentCount} video segments ({resolution})");

			// The stitched stream is cached in LocalState — re-downloading tens of
			// MB on every launch makes the dev loop painful. Keyed by content and
			// layout; delete the file to force a refresh.
			var cacheKey = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes($"{manifestUri.AbsoluteUri}|{resolution}|{segmentCount}|mfra1")))[..16];
			var cacheFile = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, $"fmp4-{cacheKey}.mp4");
			if (File.Exists(cacheFile))
			{
				var cached = await File.ReadAllBytesAsync(cacheFile);
				Trace.WriteLine($"[MediaElement.Windows.PlayReady] fMP4 loaded from cache — {cached.Length} bytes");
				return (cached, cacheFile);
			}

			headerHttpClient ??= new HttpClient();
			headerHttpClient.DefaultRequestHeaders.Clear();
			foreach (var header in headers)
			{
				headerHttpClient.DefaultRequestHeaders.TryAppendWithoutValidation(header.Key, header.Value);
			}

			// Download init + media segments in parallel (throttled), order preserved.
			var parts = new byte[]?[segmentsToFetch + 1];
			var completedCount = 0;
			using var throttle = new SemaphoreSlim(8);

			async Task DownloadPartAsync(int index)
			{
				// Index 0 is the init segment; 1..N the media segments in order.
				var relativePath = index == 0
					? initialization
					: media.Replace("$Number$", (startNumber + index - 1).ToString(CultureInfo.InvariantCulture));

				await throttle.WaitAsync();
				try
				{
					using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(manifestUri, relativePath));
					using var response = await headerHttpClient.SendRequestAsync(request);
					response.EnsureSuccessStatusCode();
					var buffer = await response.Content.ReadAsBufferAsync();
					parts[index] = buffer.ToArray();

					var done = Interlocked.Increment(ref completedCount);
					if (done % 25 == 0 || done == parts.Length)
					{
						Trace.WriteLine($"[MediaElement.Windows.PlayReady] Segment downloads — {done}/{parts.Length}");
					}
				}
				finally
				{
					throttle.Release();
				}
			}

			var downloads = new Task[parts.Length];
			for (var i = 0; i < downloads.Length; i++)
			{
				downloads[i] = DownloadPartAsync(i);
			}

			await Task.WhenAll(downloads);

			// Validate the downloads and build the mfra random-access index the MP4
			// source needs: with empty stbl sample tables (fMP4), the source locates
			// samples via mfra at the tail — DASH segments don't carry one, and its
			// absence fails the open with E_UNEXPECTED (observed: tail scan to EOF,
			// then SRC_NOT_SUPPORTED). Each segment starts with an IDR
			// (segmentAlignment=true), so segment starts are valid tfra entries.
			uint trackId = 0;
			long mediaTime = 0;
			long byteOffset = 0;
			var totalLength = 0;
			var tfraEntries = new List<(long Time, long MoofOffset)>(segmentCount);
			for (var index = 0; index < parts.Length; index++)
			{
				var part = parts[index];
				if (part is null)
				{
					Trace.WriteLine($"[MediaElement.Windows.PlayReady] Segment {index} download produced no data");
					return null;
				}

				if (index == 0)
				{
					trackId = GetTrackId(part);
					if (trackId == 0)
					{
						Trace.WriteLine("[MediaElement.Windows.PlayReady] Could not read the track ID from the init segment");
						return null;
					}
				}
				else
				{
					var moofOffset = FindChildBox(part, 0, part.Length, "moof");
					if (moofOffset < 0)
					{
						Trace.WriteLine($"[MediaElement.Windows.PlayReady] Segment {index} has no moof box");
						return null;
					}

					tfraEntries.Add((mediaTime, byteOffset + moofOffset));
					mediaTime += segmentDurations[index - 1];
				}

				byteOffset += part.Length;
				totalLength += part.Length;
			}

			var mfra = BuildMfra(trackId, tfraEntries);
			var fmp4 = new byte[totalLength + mfra.Length];
			var offset = 0;
			foreach (var part in parts)
			{
				if (part is null)
				{
					continue; // Unreachable — validated above; keeps nullable flow happy.
				}

				Buffer.BlockCopy(part, 0, fmp4, offset, part.Length);
				offset += part.Length;
			}

			Buffer.BlockCopy(mfra, 0, fmp4, offset, mfra.Length);
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] fMP4 stitched — {fmp4.Length} bytes from {parts.Length} parts + mfra index ({tfraEntries.Count} entries)");
			await File.WriteAllBytesAsync(cacheFile, fmp4);
			return (fmp4, cacheFile);
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Failed to stitch fMP4: {ex.Message}");
			return null;
		}
	}

	/// <summary>Reads a big-endian UInt32 from an MP4 box field.</summary>
	static uint ReadBe32(byte[] data, int offset) =>
		((uint)data[offset] << 24) | ((uint)data[offset + 1] << 16) | ((uint)data[offset + 2] << 8) | data[offset + 3];

	static void WriteBe32(byte[] data, int offset, uint value)
	{
		data[offset] = (byte)(value >> 24);
		data[offset + 1] = (byte)(value >> 16);
		data[offset + 2] = (byte)(value >> 8);
		data[offset + 3] = (byte)value;
	}

	static void WriteBe64(byte[] data, int offset, ulong value)
	{
		for (var i = 0; i < 8; i++)
		{
			data[offset + i] = (byte)(value >> (56 - (8 * i)));
		}
	}

	/// <summary>Returns the offset of the first box with the given fourcc, or -1.</summary>
	static int FindChildBox(byte[] data, int start, int end, string fourcc)
	{
		var position = start;
		while (position + 8 <= end)
		{
			var size = (int)ReadBe32(data, position);
			if (size < 8)
			{
				return -1;
			}

			if (Encoding.ASCII.GetString(data, position + 4, 4) == fourcc)
			{
				return position;
			}

			position += size;
		}

		return -1;
	}

	/// <summary>Reads the track ID from the init segment's moov/trak/tkhd chain.</summary>
	static uint GetTrackId(byte[] initSegment)
	{
		var moov = FindChildBox(initSegment, 0, initSegment.Length, "moov");
		var trak = moov >= 0 ? FindChildBox(initSegment, moov + 8, moov + (int)ReadBe32(initSegment, moov), "trak") : -1;
		var tkhd = trak >= 0 ? FindChildBox(initSegment, trak + 8, trak + (int)ReadBe32(initSegment, trak), "tkhd") : -1;
		// tkhd v0 content: version+flags(4), creation(4), modification(4), track_ID(4)
		return tkhd >= 0 ? ReadBe32(initSegment, tkhd + 20) : 0;
	}

	/// <summary>
	/// Builds an mfra box (tfra v1 + mfro) from per-segment (time, moof offset)
	/// pairs, giving the MP4 source the random-access index it requires for fMP4
	/// byte streams. Appended at the tail, where mfro lets the parser find it.
	/// </summary>
	static byte[] BuildMfra(uint trackId, IReadOnlyList<(long Time, long MoofOffset)> entries)
	{
		const int tfraEntrySize = 8 + 8 + 3; // v1: 8-byte time, 8-byte offset, 1-byte traf/trun/sample
		var tfraSize = 8 + 4 + 4 + 4 + 4 + (entries.Count * tfraEntrySize);
		var mfraSize = 8 + tfraSize + 16;
		var mfra = new byte[mfraSize];
		var position = 0;

		WriteBe32(mfra, position, (uint)mfraSize); position += 4;
		Encoding.ASCII.GetBytes("mfra").CopyTo(mfra, position); position += 4;

		WriteBe32(mfra, position, (uint)tfraSize); position += 4;
		Encoding.ASCII.GetBytes("tfra").CopyTo(mfra, position); position += 4;
		mfra[position] = 1; position += 4; // version = 1, flags = 0
		WriteBe32(mfra, position, trackId); position += 4;
		WriteBe32(mfra, position, 0); position += 4; // reserved + length sizes (all 1-byte)
		WriteBe32(mfra, position, (uint)entries.Count); position += 4;

		foreach (var (time, moofOffset) in entries)
		{
			WriteBe64(mfra, position, (ulong)time); position += 8;
			WriteBe64(mfra, position, (ulong)moofOffset); position += 8;
			mfra[position] = 1; mfra[position + 1] = 1; mfra[position + 2] = 1; position += 3; // traf, trun, sample
		}

		WriteBe32(mfra, position, 16); position += 4;
		Encoding.ASCII.GetBytes("mfro").CopyTo(mfra, position); position += 4;
		position += 4; // version + flags = 0
		WriteBe32(mfra, position, (uint)mfraSize); position += 4;

		return mfra;
	}

	/// <summary>
	/// Acquires a PlayReady license out-of-band using the WinRT PlayReady API
	/// (<see cref="PlayReadyLicenseAcquisitionServiceRequest"/> manual enabling),
	/// applying <see cref="DrmConfiguration.LicenseRequestHeaders"/> to the POST.
	/// Returns false (after reporting) when the license could not be acquired.
	/// </summary>
	async Task<bool> TryAcquireLicenseProactivelyAsync(Guid keyId, DrmConfiguration drmConfig)
	{
		if (drmConfig.LicenseServerUrl is null)
		{
			ReportMediaFailed("DrmConfiguration.LicenseServerUrl is required for PlayReady content");
			return false;
		}

		var licenseUri = drmConfig.LicenseServerUrl;

		// Two attempts: the first may fail with MSPR_E_NEEDS_INDIVIDUALIZATION
		// (0x8004B822) when the machine's PlayReady runtime has never been
		// individualized. Individualize once, then retry.
		for (var attempt = 1; ; attempt++)
		{
			try
			{
				await AcquireLicenseOnceAsync(keyId, licenseUri, drmConfig).ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
				return true;
			}
			catch (Exception ex) when (IsNeedsIndividualization(ex) && attempt == 1)
			{
				Trace.WriteLine("[MediaElement.Windows.PlayReady] PlayReady runtime requires individualization (0x8004B822); individualizing and retrying");
				if (!await TryIndividualizeAsync().ConfigureAwait(ConfigureAwaitOptions.ForceYielding))
				{
					ReportMediaFailed("PlayReady individualization failed — cannot acquire licenses on this machine");
					return false;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"[MediaElement.Windows.PlayReady] License acquisition failed: {ex.Message}");
				ReportMediaFailed($"DRM license acquisition failed: {ex.Message}");
				return false;
			}
		}
	}

	static bool IsNeedsIndividualization(Exception ex) =>
		ex is COMException comException && unchecked((uint)comException.HResult) == 0x8004B822u;

	/// <summary>
	/// Performs PlayReady individualization (the one-time client provisioning step
	/// against Microsoft's individualization service). Required before the runtime
	/// will generate license challenges on a machine that has never individualized.
	/// </summary>
	async Task<bool> TryIndividualizeAsync()
	{
		try
		{
			Trace.WriteLine("[MediaElement.Windows.PlayReady] Starting PlayReady individualization");
			var individualizationRequest = new PlayReadyIndividualizationServiceRequest();
			await individualizationRequest.BeginServiceRequest().AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
			Trace.WriteLine("[MediaElement.Windows.PlayReady] Individualization completed successfully");
			return true;
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Individualization failed: {ex.Message}");
			return false;
		}
	}

	async Task AcquireLicenseOnceAsync(Guid keyId, Uri licenseUri, DrmConfiguration drmConfig)
	{
		// PlayReady 4.0 content headers use AES-CTR (WRMHEADER ALGID=AESCTR).
		// The second parameter is contentKeyIdString (base64 KID) — optional
		// when the Guid form of the KID is supplied.
		var contentHeader = new PlayReadyContentHeader(
			keyId,
			null,
			PlayReadyEncryptionAlgorithm.Aes128Ctr,
			licenseUri,
			licenseUri,
			string.Empty,
			Guid.Empty);

		var licenseRequest = new PlayReadyLicenseAcquisitionServiceRequest
		{
			ContentHeader = contentHeader,
			Uri = licenseUri,
		};

		var challenge = licenseRequest.GenerateManualEnablingChallenge();
		var challengeBody = challenge.GetMessageBody();
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] License challenge generated — {challengeBody.Length} bytes, POST to {licenseUri}");

		licenseHttpClient ??= new HttpClient();
		using var request = new HttpRequestMessage(HttpMethod.Post, licenseUri)
		{
			Content = new Windows.Web.Http.HttpBufferContent(challengeBody.AsBuffer())
		};

		// Headers mandated by the challenge itself (e.g. Content-Type) first;
		// user-configured headers (e.g. X-AxDRM-Message) second so they win conflicts.
		AppendHeaders(request, challenge.MessageHeaders);
		AppendHeaders(request, drmConfig.LicenseRequestHeaders);
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] License request headers — [{string.Join(", ", request.Headers.Select(h => h.Key))}]; content headers — [{string.Join(", ", request.Content.Headers.Select(h => h.Key))}]");

		using var response = await licenseHttpClient.SendRequestAsync(request)
			.AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding);

		var responseBytes = (await response.Content.ReadAsBufferAsync()
			.AsTask().ConfigureAwait(ConfigureAwaitOptions.ForceYielding)).ToArray();

		if (!response.IsSuccessStatusCode)
		{
			// Log the server's error body — Axinom returns a diagnostic message
			// (e.g. missing/invalid X-AxDRM-Message) that explains the rejection.
			var errorBody = Encoding.UTF8.GetString(responseBytes);
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] License server error {(int)response.StatusCode} ({response.StatusCode}) — body: {(errorBody.Length > 1024 ? errorBody[..1024] : errorBody)}");
			throw new InvalidOperationException($"License server returned {(int)response.StatusCode}: {(errorBody.Length > 256 ? errorBody[..256] : errorBody)}");
		}

		Trace.WriteLine($"[MediaElement.Windows.PlayReady] License response received — {responseBytes.Length} bytes");

		licenseRequest.ProcessManualEnablingResponse(responseBytes);
		Trace.WriteLine("[MediaElement.Windows.PlayReady] License acquired and installed successfully");
	}

	static void AppendHeaders(HttpRequestMessage request, Windows.Foundation.Collections.IPropertySet headers)
	{
		foreach (var header in headers)
		{
			AppendHeader(request, header.Key, header.Value?.ToString());
		}
	}

	static void AppendHeaders(HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> headers)
	{
		foreach (var header in headers)
		{
			AppendHeader(request, header.Key, header.Value);
		}
	}

	static void AppendHeader(HttpRequestMessage request, string key, string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}

		// Content headers (Content-Type, Content-Length, ...) are rejected by the
		// request header collection — route them to the content header collection.
		if (!request.Headers.TryAppendWithoutValidation(key, value))
		{
			request.Content?.Headers.TryAppendWithoutValidation(key, value);
		}
	}

	void ReportMediaFailed(string message)
	{
		if (isDisposed)
		{
			return;
		}

		if (Dispatcher.IsDispatchRequired)
		{
			Dispatcher.Dispatch(() =>
			{
				if (!isDisposed)
				{
					MediaElement.MediaFailed(new MediaFailedEventArgs(message));
				}
			});
		}
		else
		{
			MediaElement.MediaFailed(new MediaFailedEventArgs(message));
		}
	}

	// ---------------------------------------------------------------------
	// Native engine plumbing
	// ---------------------------------------------------------------------

	SwapChainPanel CreateDrmPlatformView()
	{
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] CreateDrmPlatformView — existingEngine={playReadyEngine is not null}, existingPanel={drmSwapChainPanel is not null}");
		CleanupPlayReadyEngine();
		Trace.WriteLine("[MediaElement.Windows.PlayReady] Creating new SwapChainPanel for DRM playback");
		drmSwapChainPanel = new SwapChainPanel();
		drmSwapChainPanel.SizeChanged += OnDrmSwapChainPanelSizeChanged;
		playReadyEngine = new CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine();
		Trace.WriteLine("[MediaElement.Windows.PlayReady] Calling native Initialize");

		// Get the ISwapChainPanelNative COM interface pointer directly.
		// This is a classic COM interface (not WinRT), so it bypasses the
		// C++/WinRT QI issues that occur when crossing the projection boundary.
		// ISwapChainPanelNative is the interface that provides SetSwapChain().
		var panelNativeComPtr = Marshal.GetComInterfaceForObject(
			drmSwapChainPanel, typeof(ISwapChainPanelNativeInterop));
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] SwapChainPanel COM pointer: {panelNativeComPtr.ToInt64()}");
		playReadyEngine.Initialize(panelNativeComPtr.ToInt64());
		Trace.WriteLine("[MediaElement.Windows.PlayReady] Native engine initialized");
		playReadyEngine.MediaOpened += OnPlayReadyMediaOpened;
		playReadyEngine.MediaFailed += OnPlayReadyMediaFailed;
		playReadyEngine.MediaEnded += OnPlayReadyMediaEnded;
		playReadyEngine.TimeChanged += OnPlayReadyTimeChanged;
		playReadyEngine.SizeChanged += OnPlayReadySizeChanged;
		playReadyEngine.StateChanged += OnPlayReadyStateChanged;
		playReadyEngine.LicenseAcquired += OnPlayReadyLicenseAcquired;

		Trace.WriteLine("[MediaElement.Windows.PlayReady] Native engine initialized and all event handlers wired");
		return drmSwapChainPanel;
	}

	void CleanupPlayReadyEngine()
	{
		Trace.WriteLine($"[MediaElement.Windows.PlayReady] CleanupPlayReadyEngine — engine={playReadyEngine is not null}, panel={drmSwapChainPanel is not null}");
		if (playReadyEngine is not null)
		{
			playReadyEngine.MediaOpened -= OnPlayReadyMediaOpened;
			playReadyEngine.MediaFailed -= OnPlayReadyMediaFailed;
			playReadyEngine.MediaEnded -= OnPlayReadyMediaEnded;
			playReadyEngine.TimeChanged -= OnPlayReadyTimeChanged;
			playReadyEngine.SizeChanged -= OnPlayReadySizeChanged;
			playReadyEngine.StateChanged -= OnPlayReadyStateChanged;
			playReadyEngine.LicenseAcquired -= OnPlayReadyLicenseAcquired;

			Trace.WriteLine("[MediaElement.Windows.PlayReady] Closing native engine");
			playReadyEngine.Close();
			playReadyEngine = null;
		}

		if (drmSwapChainPanel is not null)
		{
			drmSwapChainPanel.SizeChanged -= OnDrmSwapChainPanelSizeChanged;
		}

		drmSwapChainPanel = null;
	}

	void CleanupPlayReady()
	{
		CleanupPlayReadyEngine();
		licenseHttpClient?.Dispose();
		licenseHttpClient = null;
	}

	void OnDrmSwapChainPanelSizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
	{
		if (playReadyEngine is not null && e.NewSize.Width > 0 && e.NewSize.Height > 0)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Panel size changed: {e.NewSize.Width}x{e.NewSize.Height}; resizing swap chain");
			playReadyEngine.ResizeSwapChain((uint)e.NewSize.Width, (uint)e.NewSize.Height);
		}
	}

	protected virtual async partial void PlatformUpdateDrmConfiguration()
	{
		Trace.WriteLine("[MediaElement.Windows.PlayReady] PlatformUpdateDrmConfiguration");
		if (IsPlayReadyContent && MediaElement.Source is UriMediaSource { DrmConfiguration: not null } uriMediaSource)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] Rebuilding DRM pipeline for URI={uriMediaSource.Uri}");
			await SetUriSourceWithPlayReadyAsync(uriMediaSource.Uri!, uriMediaSource.HttpHeaders, uriMediaSource.DrmConfiguration);
		}
	}

	// ---------------------------------------------------------------------
	// C++/WinRT engine event handlers. The native engine marshals these to
	// the UI thread via its DispatcherQueue; guard anyway in case that ever
	// regresses — MediaElement members are not safe on background threads.
	// ---------------------------------------------------------------------

	void DispatchToUi(Action action)
	{
		if (Dispatcher.IsDispatchRequired)
		{
			Dispatcher.Dispatch(() =>
			{
				if (!isDisposed)
				{
					action();
				}
			});
		}
		else
		{
			action();
		}
	}

	void OnPlayReadyMediaOpened(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, object args)
	{
		if (isDisposed)
		{
			return;
		}

		Trace.WriteLine("[MediaElement.Windows.PlayReady] OnMediaOpened");
		DispatchToUi(() =>
		{
			MediaElement.MediaOpened();
			MediaElement.CurrentStateChanged(MediaElementState.Playing);
		});
	}

	void OnPlayReadyMediaFailed(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, CommunityToolkit.Maui.Media.WinRT.MediaEngineErrorEventArgs args)
	{
		if (isDisposed)
		{
			return;
		}

		Trace.WriteLine($"[MediaElement.Windows.PlayReady] OnMediaFailed — {args.ErrorMessage}");
		DispatchToUi(() => MediaElement.MediaFailed(new MediaFailedEventArgs(args.ErrorMessage)));
	}

	void OnPlayReadyMediaEnded(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, object args)
	{
		if (isDisposed)
		{
			return;
		}

		Trace.WriteLine("[MediaElement.Windows.PlayReady] OnMediaEnded");
		DispatchToUi(() => MediaElement.MediaEnded());
	}

	void OnPlayReadyTimeChanged(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, CommunityToolkit.Maui.Media.WinRT.MediaEngineTimeChangedEventArgs args)
	{
		if (isDisposed)
		{
			return;
		}

		DispatchToUi(() =>
		{
			MediaElement.Position = TimeSpan.FromSeconds(args.PositionSeconds);
			MediaElement.Duration = TimeSpan.FromSeconds(args.DurationSeconds);
		});
	}

	void OnPlayReadySizeChanged(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, CommunityToolkit.Maui.Media.WinRT.MediaEngineSizeChangedEventArgs args)
	{
		if (isDisposed)
		{
			return;
		}

		Trace.WriteLine($"[MediaElement.Windows.PlayReady] OnSizeChanged — {args.Width}x{args.Height}");
		DispatchToUi(() =>
		{
			MediaElement.MediaWidth = (int)args.Width;
			MediaElement.MediaHeight = (int)args.Height;
		});
	}

	void OnPlayReadyStateChanged(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, CommunityToolkit.Maui.Media.WinRT.MediaEngineStateChangedEventArgs args)
	{
		if (isDisposed)
		{
			return;
		}

		var newState = args.NewState switch
		{
			0 => MediaElementState.None,
			1 => MediaElementState.Opening,
			>= 2 and <= 3 => MediaElementState.Buffering,
			4 => MediaElementState.Playing,
			_ => MediaElementState.None,
		};

		DispatchToUi(() => MediaElement.CurrentStateChanged(newState));
	}

	void OnPlayReadyLicenseAcquired(CommunityToolkit.Maui.Media.WinRT.PlayReadyMediaEngine sender, CommunityToolkit.Maui.Media.WinRT.DrmLicenseAcquiredEventArgs args)
	{
		if (isDisposed)
		{
			return;
		}

		if (!args.Success)
		{
			Trace.WriteLine($"[MediaElement.Windows.PlayReady] License FAILED — Error={args.ErrorMessage}");
			DispatchToUi(() => MediaElement.MediaFailed(new MediaFailedEventArgs($"DRM license: {args.ErrorMessage}")));
		}
		else
		{
			Trace.WriteLine("[MediaElement.Windows.PlayReady] License acquired successfully");
		}
	}
}

/// <summary>
/// COM interop definition for ISwapChainPanelNative.
/// IID: {63AAD0B8-7C24-40FF-85A8-640D944CC325}
/// This is a classic COM interface (not WinRT) that provides SetSwapChain.
/// Verified against microsoft.ui.xaml.media.dxinterop.h (WindowsAppSDK 1.7):
/// WinUI 3's SwapChainPanel uses the SAME IID as UWP — the F92F19D2-… IID
/// found in some documentation is NOT implemented (QI → E_NOINTERFACE).
/// </summary>
[ComImport]
[Guid("63AAD0B8-7C24-40FF-85A8-640D944CC325")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface ISwapChainPanelNativeInterop
{
	[PreserveSig]
	int SetSwapChain([In] IntPtr swapChain);
}
