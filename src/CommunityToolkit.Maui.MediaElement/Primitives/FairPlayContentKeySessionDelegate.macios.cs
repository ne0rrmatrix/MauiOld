using AVFoundation;
using Foundation;

namespace CommunityToolkit.Maui.Core;

/// <summary>
/// Delegate for <see cref="AVContentKeySession"/> that handles FairPlay Streaming
/// SPC/CKC key exchange using the modern <c>AVContentKeySession</c> API (iOS 11.2+).
/// </summary>
sealed class FairPlayContentKeySessionDelegate(
	Uri licenseServerUri,
	IDictionary<string, string> licenseRequestHeaders) : AVContentKeySessionDelegate
{
	readonly Uri licenseServerUri = licenseServerUri;
	readonly IDictionary<string, string> licenseRequestHeaders = licenseRequestHeaders;
	readonly HttpClient httpClient = new();

	/// <summary>
	/// Called when the content key session needs a new content key.
	/// Performs the SPC/CKC handshake.
	/// </summary>
	public override async void DidProvideContentKeyRequest(
		AVContentKeySession session,
		AVContentKeyRequest keyRequest)
	{
		await HandleKeyRequestAsync(session, keyRequest);
	}

	/// <summary>
	/// Called when a content key needs to be renewed.
	/// </summary>
	public override async void DidProvideRenewingContentKeyRequest(
		AVContentKeySession session,
		AVContentKeyRequest keyRequest)
	{
		await HandleKeyRequestAsync(session, keyRequest);
	}

	async Task HandleKeyRequestAsync(
		AVContentKeySession session,
		AVContentKeyRequest keyRequest)
	{
		try
		{
			// Step 1: Fetch Application Certificate
			var certData = await FetchApplicationCertificateAsync().ConfigureAwait(false);
			if (certData is null)
			{
				System.Diagnostics.Trace.WriteLine(
					"MediaElement [Apple]: FairPlay - failed to fetch application certificate");
				return;
			}

			// Step 2: Generate SPC via MakeStreamingContentKeyRequestData.
			// The content identifier can be null for FairPlay — AVFoundation derives
			// it from the key request's own context.
			var certNsData = NSData.FromArray(certData);

			// Use a TaskCompletionSource to bridge the callback-based API
			var spcTcs = new TaskCompletionSource<(NSData? spcData, NSError? error)>();

			keyRequest.MakeStreamingContentKeyRequestData(
				certNsData,
				null,
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

			// Step 3: Exchange SPC for CKC via license server
			var spcBytes = spcData.ToArray();
			var ckcBytes = await ExchangeSpcForCkcAsync(spcBytes).ConfigureAwait(false);
			if (ckcBytes is null)
			{
				System.Diagnostics.Trace.WriteLine(
					$"MediaElement [Apple]: FairPlay CKC exchange failed for {licenseServerUri}");
				return;
			}

			// Step 4: Provide CKC to the key request via Process()
			var ckcNsData = NSData.FromArray(ckcBytes);
			var keyResponse = AVContentKeyResponse.Create(ckcNsData);
			keyRequest.Process(keyResponse);

			System.Diagnostics.Trace.WriteLine(
				"MediaElement [Apple]: FairPlay license acquired successfully");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.WriteLine(
				$"MediaElement [Apple]: FairPlay handshake failed: {ex.Message}");
		}
	}

	async Task<byte[]?> FetchApplicationCertificateAsync()
	{
		try
		{
			var certUri = new Uri(licenseServerUri, "/fairplay/certificate");

			using var request = new HttpRequestMessage(HttpMethod.Get, certUri);
			ApplyCustomHeaders(request);

			using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
		}
		catch
		{
			try
			{
				using var request = new HttpRequestMessage(HttpMethod.Get, licenseServerUri);
				ApplyCustomHeaders(request);

				using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			}
			catch
			{
				return null;
			}
		}
	}

	async Task<byte[]?> ExchangeSpcForCkcAsync(byte[] spcBytes)
	{
		try
		{
			using var content = new ByteArrayContent(spcBytes);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

			using var request = new HttpRequestMessage(HttpMethod.Post, licenseServerUri)
			{
				Content = content
			};
			ApplyCustomHeaders(request);

			using var response = await httpClient.SendAsync(request).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Trace.WriteLine($"MediaElement [Apple]: CKC exchange failed: {ex.Message}");
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