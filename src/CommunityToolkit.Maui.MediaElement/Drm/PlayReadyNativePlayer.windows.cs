using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>Native backend playback/error state, mirroring the native <c>FgPlayReadySnapshot.state</c> values.</summary>
enum PlayReadyNativeState
{
	Idle = 0,
	Loading = 1,
	Playing = 2,
	Paused = 3,
	Stopped = 4,
	Error = 5,
	Ended = 6,
}

/// <summary>Everything <see cref="PlayReadyNativePlayer"/> needs to open a DASH/CENC source: the manifest's resolved
/// segment addressing (see <see cref="DashManifest"/>) plus the license server the license challenge is POSTed to.</summary>
sealed class PlayReadyOpenRequest
{
	public required string InitUrl { get; init; }
	public required string SegmentBaseUrl { get; init; }
	public required string SegmentPrefix { get; init; }
	public required string SegmentSuffix { get; init; }
	public required int StartNumber { get; init; }
	public required int SegmentCount { get; init; }

	public string? AudioInitUrl { get; init; }
	public string? AudioSegmentBaseUrl { get; init; }
	public string? AudioSegmentPrefix { get; init; }
	public string? AudioSegmentSuffix { get; init; }

	public required Uri LicenseServerUrl { get; init; }
	public IDictionary<string, string> LicenseRequestHeaders { get; init; } = new Dictionary<string, string>();
	public IDictionary<string, string> SegmentRequestHeaders { get; init; } = new Dictionary<string, string>();

	/// <summary>Axinom-issued test licenses (the only license server style this backend has been validated
	/// against) carry <c>allow_persistence: true</c> and are rejected by a TEMPORARY CDM session with a TypeError
	/// (0x80704005) — they need <c>MF_MEDIAKEYSESSION_TYPE_PERSISTENT_LICENSE</c>. Defaulted on rather than exposed
	/// as a <see cref="DrmConfiguration"/> option because there is only one tested license issuer shape today;
	/// widen this if a temporary-license server needs supporting.</summary>
	public bool RequiresPersistentLicense { get; init; } = true;
}

/// <summary>Blittable mirror of the native <c>FgPlayReadySnapshot</c>. Field order and types must match exactly —
/// this is read straight out of native process-global atomics on every poll.</summary>
[StructLayout(LayoutKind.Sequential)]
struct PlayReadyNativeSnapshot
{
	public int State;
	public int ErrorHr;
	public ulong Handle;
	public int Width;
	public int Height;
	public long PositionMs;
	public long DurationMs;
	public ulong PlayAppliedSeq;
	public ulong SeekAppliedSeq;
	public ulong VolumeAppliedSeq;
	public ulong RateAppliedSeq;
}

/// <summary>
/// Drives the vendored native PlayReady backend (<c>Native/PlayReadyNative</c>, built as
/// <c>FluentGpu.PlayReady.Native.dll</c>): an in-process Media Foundation CDM + custom CENC <c>IMFMediaSource</c>
/// that decodes protected DASH/CENC content and publishes a same-process, shareable DirectComposition surface
/// handle (see <see cref="DirectComposition"/> for how that handle gets hosted).
/// <para>License acquisition is bridged back to managed code: the native CDM raises a challenge on an MF thread via
/// <see cref="LicenseThunk"/>, which POSTs it to <see cref="PlayReadyOpenRequest.LicenseServerUrl"/> with
/// <see cref="PlayReadyOpenRequest.LicenseRequestHeaders"/> and hands the response back to the CDM. The engine never
/// sees a key or a decrypted pixel outside the native module.</para>
/// </summary>
sealed unsafe partial class PlayReadyNativePlayer : IDisposable
{
	const string nativeLibraryName = "FluentGpu.PlayReady.Native.dll";
	const int errorBusy = unchecked((int)0x800700AA); // HRESULT_FROM_WIN32(ERROR_BUSY)

	static readonly HttpClient licenseHttpClient = new();

	readonly string dataRoot;

	Thread? thread;
	PlayReadyOpenRequest? request;
	volatile bool disposed;
	volatile string? startupError;
	volatile string? licenseError;

	public PlayReadyNativePlayer()
	{
		dataRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"CommunityToolkit.Maui", "PlayReady");
	}

	/// <summary>Whether the native DLL can be loaded on this machine/architecture.</summary>
	public static bool IsAvailable
	{
		get
		{
			try
			{
				if (!NativeLibrary.TryLoad(nativeLibraryName, typeof(PlayReadyNativePlayer).Assembly,
						DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.AssemblyDirectory, out var handle))
				{
					return false;
				}

				NativeLibrary.Free(handle);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	/// <summary>The most recent license-acquisition failure, if any (surfaced separately from the native snapshot's
	/// <see cref="PlayReadyNativeSnapshot.ErrorHr"/> because a relay failure never reaches the native error state on
	/// an old/permissive native build — it just leaves the key unusable).</summary>
	public string? LastLicenseError => licenseError;

	/// <summary>Starts the native backend on a dedicated MTA background thread (the native session is MTA-affine and
	/// blocks for the lifetime of playback). No-op if a session is already running.</summary>
	public void Start(PlayReadyOpenRequest openRequest)
	{
		ObjectDisposedException.ThrowIf(disposed, this);
		if (thread is { IsAlive: true })
		{
			return;
		}

		Directory.CreateDirectory(dataRoot);
		request = openRequest;
		startupError = null;
		licenseError = null;

		thread = new Thread(RunNative) { IsBackground = true, Name = "maui-playready-native" };
		thread.SetApartmentState(ApartmentState.MTA);
		thread.Start();
	}

	void RunNative()
	{
		var openRequest = request ?? throw new InvalidOperationException($"{nameof(RunNative)} started without a request.");

		var initUrl = Marshal.StringToHGlobalUni(openRequest.InitUrl);
		var segBase = Marshal.StringToHGlobalUni(openRequest.SegmentBaseUrl);
		var segPrefix = Marshal.StringToHGlobalUni(openRequest.SegmentPrefix);
		var segSuffix = Marshal.StringToHGlobalUni(openRequest.SegmentSuffix);
		var licUrl = Marshal.StringToHGlobalUni(openRequest.LicenseServerUrl.AbsoluteUri);
		var headers = Marshal.StringToHGlobalUni(FormatHeaderLines(openRequest.SegmentRequestHeaders));
		var audioInit = Marshal.StringToHGlobalUni(openRequest.AudioInitUrl);
		var audioBase = Marshal.StringToHGlobalUni(openRequest.AudioSegmentBaseUrl);
		var audioPrefix = Marshal.StringToHGlobalUni(openRequest.AudioSegmentPrefix);
		var audioSuffix = Marshal.StringToHGlobalUni(openRequest.AudioSegmentSuffix);

		var self = GCHandle.Alloc(this);
		try
		{
			var desc = new NativeOpenDesc
			{
				StructSize = (uint)sizeof(NativeOpenDesc),
				Mode = 0,
				InitUrl = initUrl,
				SegmentBaseUrl = segBase,
				SegmentPrefix = segPrefix,
				SegmentSuffix = segSuffix,
				StartNumber = openRequest.StartNumber,
				SegmentCount = openRequest.SegmentCount,
				Pssh = 0,
				PsshLen = 0,
				HttpHeaders = headers,
				LicenseServerUrl = licUrl,
				SegmentStride = 1,
				AudioInitUrl = audioInit,
				AudioSegmentBaseUrl = audioBase,
				AudioSegmentPrefix = audioPrefix,
				AudioSegmentSuffix = audioSuffix,
				PersistentLicense = openRequest.RequiresPersistentLicense ? 1 : 0,
			};

			var callback = (nint)(delegate* unmanaged[Stdcall]<nint, byte*, int, char*, delegate* unmanaged[Stdcall]<nint, byte*, int, void>, nint, int>)&LicenseThunk;
			var ctx = GCHandle.ToIntPtr(self);

			var hr = Native.FgPlayReadyRunEx(dataRoot, ref desc, callback, ctx);

			// The native session is a process-global singleton (a CAS latch inside FgPlayReadyRunEx). A previous
			// session that wasn't fully torn down (a missed Stop, or teardown still mid-flight) wedges every later
			// open with ERROR_BUSY — signal it to stop and retry on a bounded backoff instead of failing outright.
			if (hr == errorBusy && !disposed)
			{
				var deadline = Environment.TickCount64 + 5_000;
				while (hr == errorBusy && !disposed && Environment.TickCount64 < deadline)
				{
					try
					{
						Native.FgPlayReadyStop();
					}
					catch (DllNotFoundException)
					{
						break;
					}

					Thread.Sleep(200);
					hr = Native.FgPlayReadyRunEx(dataRoot, ref desc, callback, ctx);
				}
			}
		}
		catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or MarshalDirectiveException)
		{
			startupError = ex.Message;
		}
		finally
		{
			if (self.IsAllocated)
			{
				self.Free();
			}

			Marshal.FreeHGlobal(initUrl);
			Marshal.FreeHGlobal(segBase);
			Marshal.FreeHGlobal(segPrefix);
			Marshal.FreeHGlobal(segSuffix);
			Marshal.FreeHGlobal(licUrl);
			Marshal.FreeHGlobal(headers);
			Marshal.FreeHGlobal(audioInit);
			Marshal.FreeHGlobal(audioBase);
			Marshal.FreeHGlobal(audioPrefix);
			Marshal.FreeHGlobal(audioSuffix);
		}
	}

	/// <summary>Native→managed license bridge. Invoked by the CDM on an MF worker thread with the challenge; POSTs it
	/// to the license server and hands the response back through <paramref name="deliver"/>. Returns 0 on success, a
	/// negative HRESULT-shaped code on failure (the native side then leaves the key unusable, surfaced as a DRM
	/// error by the caller's snapshot poll).</summary>
	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
	static int LicenseThunk(nint ctx, byte* challenge, int challengeLen, char* keyIdHex,
		delegate* unmanaged[Stdcall]<nint, byte*, int, void> deliver, nint deliverCtx)
	{
		try
		{
			if (GCHandle.FromIntPtr(ctx).Target is not PlayReadyNativePlayer self || self.request is not { } openRequest)
			{
				return unchecked((int)0x80004005);
			}

			var challengeBytes = new byte[challengeLen < 0 ? 0 : challengeLen];
			if (challengeBytes.Length > 0)
			{
				new ReadOnlySpan<byte>(challenge, challengeBytes.Length).CopyTo(challengeBytes);
			}

			var license = self.AcquireLicense(openRequest, challengeBytes);
			if (license is not { Length: > 0 })
			{
				return unchecked((int)0x8004110E); // DRM_E_CH_BAD_KEY-shaped: "no usable license"
			}

			fixed (byte* licensePtr = license)
			{
				deliver(deliverCtx, licensePtr, license.Length);
			}

			return 0;
		}
		catch
		{
			return unchecked((int)0x80004005);
		}
	}

	byte[]? AcquireLicense(PlayReadyOpenRequest openRequest, byte[] challenge)
	{
		try
		{
			using var httpRequest = new HttpRequestMessage(HttpMethod.Post, openRequest.LicenseServerUrl)
			{
				Content = new ByteArrayContent(challenge),
			};

			foreach (var header in openRequest.LicenseRequestHeaders)
			{
				if (!httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value))
				{
					httpRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
				}
			}

			using var response = licenseHttpClient.Send(httpRequest);
			response.EnsureSuccessStatusCode();

			using var responseStream = response.Content.ReadAsStream();
			using var buffer = new MemoryStream();
			responseStream.CopyTo(buffer);
			return buffer.ToArray();
		}
		catch (Exception ex)
		{
			licenseError = $"License request to {openRequest.LicenseServerUrl} failed: {ex.Message}";
			return null;
		}
	}

	static string? FormatHeaderLines(IDictionary<string, string> headers) =>
		headers.Count is 0 ? null : string.Join('\n', headers.Select(h => $"{h.Key}: {h.Value}"));

	public void Play() => TryInvoke(static () => Native.FgPlayReadyPlay());

	public void Pause() => TryInvoke(static () => Native.FgPlayReadyPause());

	public void Stop()
	{
		TryInvoke(static () => Native.FgPlayReadyStop());
	}

	public void Seek(TimeSpan position, bool exact = true) =>
		TryInvoke(() => Native.FgPlayReadySeekEx((long)position.TotalMilliseconds, exact ? 0 : 1));

	public void SetVolume(double volume) => TryInvoke(() => Native.FgPlayReadySetVolume(Math.Clamp(volume, 0, 1)));

	public void SetRate(double rate) => TryInvoke(() => Native.FgPlayReadySetRate(rate <= 0 ? 1 : rate));

	/// <summary>Reads the current native state/position/duration/surface-handle snapshot. Returns <see langword="false"/>
	/// only when the native module is entirely unavailable (never thrown as <see cref="DllNotFoundException"/> —
	/// callers poll this every tick and must not crash a missing-DLL install).</summary>
	public bool TryGetSnapshot(out PlayReadyNativeSnapshot snapshot)
	{
		if (startupError is { Length: > 0 } error)
		{
			snapshot = new PlayReadyNativeSnapshot { State = (int)PlayReadyNativeState.Error, ErrorHr = unchecked((int)0x80004005) };
			licenseError ??= error;
			return true;
		}

		try
		{
			return Native.FgPlayReadyGetSnapshot(out snapshot) >= 0;
		}
		catch (DllNotFoundException)
		{
			snapshot = default;
			return false;
		}
	}

	static void TryInvoke(Action nativeCall)
	{
		try
		{
			nativeCall();
		}
		catch (DllNotFoundException)
		{
			// Native module unavailable — transport calls become no-ops rather than crashing the caller.
		}
	}

	/// <summary>Signals the native session to stop and returns immediately — it does not block waiting for the
	/// background thread to exit. Callers (MediaManager's cleanup, invoked from the UI thread on every source
	/// change and on element teardown) must never be blocked for the native session's bounded-but-real teardown
	/// time; the background thread is joined off-thread instead, purely so its resources are eventually reclaimed
	/// deterministically rather than left to finalization.</summary>
	public void Dispose()
	{
		if (disposed)
		{
			return;
		}

		disposed = true;
		try
		{
			Native.FgPlayReadyStop();
		}
		catch (DllNotFoundException)
		{
			// Native module was never available — nothing to stop.
		}

		var threadToJoin = thread;
		thread = null;
		if (threadToJoin is { IsAlive: true })
		{
			ThreadPool.UnsafeQueueUserWorkItem(static state => state.Join(3_000), threadToJoin, preferLocal: false);
		}
	}

	/// <summary>Blittable mirror of the native <c>FgPlayReadyOpenDesc</c> (pointer fields are hand-marshaled to
	/// HGlobal so the whole struct stays blittable for <c>LibraryImport</c>). Layout must match the native struct in
	/// <c>Native/PlayReadyNative/PlayReadyNative.cpp</c> field-for-field; <see cref="PersistentLicense"/> is an
	/// ABI-appended field the native side only reads when <c>structSize</c> says it is present.</summary>
	[StructLayout(LayoutKind.Sequential)]
	struct NativeOpenDesc
	{
		public uint StructSize;
		public int Mode;
		public nint InitUrl;
		public nint SegmentBaseUrl;
		public nint SegmentPrefix;
		public nint SegmentSuffix;
		public int StartNumber;
		public int SegmentCount;
		public nint Pssh;
		public int PsshLen;
		public nint HttpHeaders;
		public nint LicenseServerUrl;
		public int SegmentStride;
		public nint AudioInitUrl;
		public nint AudioSegmentBaseUrl;
		public nint AudioSegmentPrefix;
		public nint AudioSegmentSuffix;
		public int PersistentLicense;
	}

	static partial class Native
	{
		[LibraryImport(nativeLibraryName, StringMarshalling = StringMarshalling.Utf16)]
		internal static partial int FgPlayReadyRunEx(string baseDir, ref NativeOpenDesc desc, nint licenseCallback, nint licenseCtx);

		[LibraryImport(nativeLibraryName)]
		internal static partial int FgPlayReadyGetSnapshot(out PlayReadyNativeSnapshot value);

		[LibraryImport(nativeLibraryName)]
		internal static partial ulong FgPlayReadyPlay();

		[LibraryImport(nativeLibraryName)]
		internal static partial ulong FgPlayReadyPause();

		[LibraryImport(nativeLibraryName)]
		internal static partial void FgPlayReadyStop();

		[LibraryImport(nativeLibraryName)]
		internal static partial ulong FgPlayReadySeekEx(long positionMs, int mode);

		[LibraryImport(nativeLibraryName)]
		internal static partial ulong FgPlayReadySetVolume(double volume);

		[LibraryImport(nativeLibraryName)]
		internal static partial ulong FgPlayReadySetRate(double rate);
	}
}
