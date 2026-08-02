using AVFoundation;
using Foundation;

namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Delegate for <see cref="AVContentKeySession"/> that handles FairPlay Streaming
/// SPC/CKC key exchange using the modern <c>AVContentKeySession</c> API (iOS 11.2+).
/// </summary>
sealed class FairPlayContentKeySessionDelegate(
	Uri licenseServerUri,
	IDictionary<string, string> licenseRequestHeaders,
	Uri? certificateUri = null) : AVContentKeySessionDelegate
{
	readonly Uri licenseServerUri = licenseServerUri;
	readonly Uri? certificateUri = certificateUri;
	readonly IDictionary<string, string> licenseRequestHeaders = licenseRequestHeaders;
	readonly HttpClient httpClient = new();
	static int requestCounter;

	/// <summary>
	/// Called when the content key session needs a new content key.
	/// Performs the SPC/CKC handshake.
	/// </summary>
	public override async void DidProvideContentKeyRequest(
		AVContentKeySession session,
		AVContentKeyRequest keyRequest)
	{
		var requestId = Interlocked.Increment(ref requestCounter);
		await HandleKeyRequestAsync(session, keyRequest, requestId);
	}

	/// <summary>
	/// Called when a content key needs to be renewed.
	/// </summary>
	public override async void DidProvideRenewingContentKeyRequest(
		AVContentKeySession session,
		AVContentKeyRequest keyRequest)
	{
		var requestId = Interlocked.Increment(ref requestCounter);
		await HandleKeyRequestAsync(session, keyRequest, requestId);
	}

	async Task HandleKeyRequestAsync(
		AVContentKeySession session,
		AVContentKeyRequest keyRequest,
		int requestId)
	{
		try
		{
			// Step 1: Fetch Application Certificate
			var certData = await FetchApplicationCertificateAsync(requestId).ConfigureAwait(false);
			if (certData is null)
			{
				return;
			}
			var certNsData = NSData.FromArray(certData);

			// Parse the content identifier from the key request's identifier
			var identifierStr = keyRequest.Identifier?.ToString() ?? string.Empty;
			NSData? contentIdData = null;
			if (!string.IsNullOrEmpty(identifierStr))
			{
				// Strip skd:// prefix for the content ID
				var contentId = identifierStr.StartsWith("skd://", StringComparison.OrdinalIgnoreCase)
					? identifierStr["skd://".Length..]
					: identifierStr;
				contentIdData = NSData.FromString(contentId, NSStringEncoding.UTF8);
			}

			// Use a TaskCompletionSource to bridge the callback-based API
			var spcTcs = new TaskCompletionSource<(NSData? spcData, NSError? error)>();

			keyRequest.MakeStreamingContentKeyRequestData(
				certNsData,
				contentIdData,
				null,
				(spcData, error) =>
				{
					spcTcs.TrySetResult((spcData, error));
				});

			var (spcData, spcError) = await spcTcs.Task.ConfigureAwait(false);

			if (spcError is not null || spcData is null)
			{
				System.Diagnostics.Trace.WriteLine(
					$"MediaElement [Apple]: FairPlay SPC generation failed: {spcError?.LocalizedDescription}");
				return;
			}

			var spcBytes = spcData.ToArray();
			
			// Step 3: Exchange SPC for CKC via license server
			var ckcBytes = await ExchangeSpcForCkcAsync(spcBytes, requestId).ConfigureAwait(false);
			if (ckcBytes is null)
			{
				System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] FAILED: CKC exchange returned null");
				return;
			}

			var ckcNsData = NSData.FromArray(ckcBytes);
			var keyResponse = AVContentKeyResponse.Create(ckcNsData);
			keyRequest.Process(keyResponse);

		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.WriteLine(
				$"MediaElement [Apple]: FairPlay handshake failed: {ex.Message}");
		}
	}

	async Task<byte[]?> FetchApplicationCertificateAsync(int requestId)
	{
		// Try the dedicated certificate URL first if provided
		if (certificateUri is not null)
		{
			try
			{
				using var request = new HttpRequestMessage(HttpMethod.Get, certificateUri);
				using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
				response.EnsureSuccessStatusCode();
				var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
				return data;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Dedicated cert URL failed: {ex.Message}");
			}
		}

		// Fallback 1: /fairplay/certificate relative to license server
		try
		{
			var certUri = new Uri(licenseServerUri, "/fairplay/certificate");
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Fetching certificate from: {certUri}");

			using var request = new HttpRequestMessage(HttpMethod.Get, certUri);
			ApplyCustomHeaders(request);

			using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Cert response — Status: {(int)response.StatusCode}, Length: {response.Content.Headers.ContentLength}");
			response.EnsureSuccessStatusCode();

			var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Certificate downloaded: {data.Length} bytes from /fairplay/certificate");
			return data;
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] /fairplay/certificate failed: {ex.Message}");
		}

		// Fallback 2: license server root
		try
		{
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Falling back: license server root: {licenseServerUri}");

			using var request = new HttpRequestMessage(HttpMethod.Get, licenseServerUri);
			ApplyCustomHeaders(request);

			using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Fallback response — Status: {(int)response.StatusCode}, Length: {response.Content.Headers.ContentLength}");
			response.EnsureSuccessStatusCode();

			var data = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Certificate downloaded: {data.Length} bytes from license server root");
			return data;
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] All certificate attempts failed: {ex.Message}");
			return null;
		}
	}

	async Task<byte[]?> ExchangeSpcForCkcAsync(byte[] spcBytes, int requestId)
	{
		try
		{
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Posting SPC to license server: {licenseServerUri}");
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] SPC size: {spcBytes.Length} bytes, Content-Type: application/octet-stream");

			using var content = new ByteArrayContent(spcBytes);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

			using var request = new HttpRequestMessage(HttpMethod.Post, licenseServerUri)
			{
				Content = content
			};
			ApplyCustomHeaders(request);

			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Request headers: {string.Join("; ", request.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");

			using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] License server response — Status: {(int)response.StatusCode} {response.ReasonPhrase}");
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Response headers: {string.Join("; ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] Response Content-Type: {response.Content.Headers.ContentType}, Content-Length: {response.Content.Headers.ContentLength}");

			if (!response.IsSuccessStatusCode)
			{
				var errorBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] License server error body: {errorBody}");
			}

			response.EnsureSuccessStatusCode();

			var ckcData = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] CKC received: {ckcData.Length} bytes");
			return ckcData;
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] CKC exchange EXCEPTION — Type: {ex.GetType().FullName}, Message: {ex.Message}");
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple DRM] [{requestId}] CKC exchange stack trace: {ex.StackTrace}");
			return null;
		}
	}

	void ApplyCustomHeaders(HttpRequestMessage request)
	{
		foreach (var header in licenseRequestHeaders)
		{
			request.Headers.TryAddWithoutValidation(header.Key, header.Value);
		}
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			httpClient.Dispose();
		}

		base.Dispose(disposing);
	}
}