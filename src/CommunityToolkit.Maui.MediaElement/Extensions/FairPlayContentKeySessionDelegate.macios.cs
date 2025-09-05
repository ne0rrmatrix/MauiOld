using AVFoundation;
using Foundation;

namespace CommunityToolkit.Maui.Extensions;
/// <summary>
/// FairPlayContentKeySessionDelegate is responsible for handling FairPlay DRM content key requests.
/// </summary>
public class FairPlayContentKeySessionDelegate : NSObject, IAVContentKeySessionDelegate
{
    readonly string certificateUrl;
    readonly string? licenseUrl;
    readonly string? licenseToken;
    readonly HttpClient httpClient;
    NSData? applicationCertificate;

/// <summary>
/// Initializes a new instance of the FairPlayContentKeySessionDelegate.
/// </summary>
/// <param name="certificateUrl"></param>
/// <param name="licenseUrl"></param>
/// <param name="licenseToken"></param>
    public FairPlayContentKeySessionDelegate(string certificateUrl, string licenseUrl, string? licenseToken = null)
    {
		this.certificateUrl = certificateUrl;
		this.licenseUrl = licenseUrl;
		this.licenseToken = licenseToken;
        httpClient = new HttpClient();
    }

    /// <summary>
    /// Handles the content key request by providing the content key data
    /// </summary>
    /// <param name="session"></param>
    /// <param name="keyRequest"></param>
    [Export("contentKeySession:didProvideContentKeyRequest:")]
    public async void DidProvideContentKeyRequest(AVContentKeySession session, AVContentKeyRequest keyRequest)
    {
        try
        {
            // Get the content identifier from the key request
            var contentId = keyRequest.Identifier as NSString;
            if (contentId == null)
            {
                System.Diagnostics.Debug.WriteLine("Invalid content identifier");
                return;
            }

            await ProcessContentKeyRequestAsync(contentId.ToString(), keyRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DidProvideContentKeyRequest: {ex.Message}");
        }
    }

    /// <summary>
    /// Processes the content key request asynchronously.
    /// </summary>
    /// <param name="contentId"></param>
    /// <param name="keyRequest"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ProcessContentKeyRequestAsync(string contentId, AVContentKeyRequest keyRequest)
    {
        try
        {
            // Step 1: Get the FairPlay Application Certificate
            if (applicationCertificate == null)
            {
                applicationCertificate = await GetApplicationCertificateAsync();
                if (applicationCertificate == null)
                {
                    throw new InvalidOperationException("Failed to retrieve application certificate");
                }
            }

            // Step 2: Generate SPC (Server Playback Context) data and process the key request
            var contentIdData = NSData.FromString(contentId, NSStringEncoding.UTF8);
            await ProcessWithContentKeyRequest(keyRequest, contentIdData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing content key request: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Error processing content key request: {ex.Message}");
        }
    }

    async Task ProcessWithContentKeyRequest(AVContentKeyRequest keyRequest, NSData contentIdData)
    {
        var tcs = new TaskCompletionSource<NSData>();
        if (applicationCertificate == null)
        {
            throw new InvalidOperationException("Application certificate is not set");
        }

        keyRequest.MakeStreamingContentKeyRequestData(applicationCertificate, contentIdData, null, (spcData, error) =>
        {
            if (error != null)
            {
                tcs.SetException(new Exception($"Failed to generate SPC: {error.LocalizedDescription}"));
            }
            else
            {
                tcs.SetResult(spcData);
            }
        });

        var spcData = await tcs.Task;

        // Step 3: Send SPC to KSM and get CKC
        var ckcData = await GetContentKeyContextAsync(spcData);

        // Step 4: Provide CKC to AVContentKeySession
        var ckcResponse = AVContentKeyResponse.Create(ckcData);
        keyRequest.Process(ckcResponse);
    }

    async Task<NSData> GetApplicationCertificateAsync()
    {
        try
        {
            Console.WriteLine($"Downloading FairPlay certificate from: {certificateUrl}");
            var certificateBytes = await httpClient.GetByteArrayAsync(certificateUrl);
            return NSData.FromArray(certificateBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download certificate: {ex.Message}");
            throw;
        }
    }

    async Task<NSData> GetContentKeyContextAsync(NSData spcData)
    {
        try
        {
            Console.WriteLine($"Requesting CKC from license server: {licenseUrl}");
            Console.WriteLine($"SPC data length: {spcData.Length} bytes");

            var request = new HttpRequestMessage(HttpMethod.Post, licenseUrl);

            // Add required headers for Axinom license server
            request.Headers.Add("Accept", "application/octet-stream");
            request.Headers.Add("User-Agent", "iOS FairPlay Player");
            
            // Add license token if provided
            if (!string.IsNullOrEmpty(licenseToken))
            {
                Console.WriteLine($"Adding license token: {licenseToken}");
                request.Headers.Add("X-AxDRM-Message", licenseToken);
                // Also try Authorization header as backup
                request.Headers.Add("Authorization", $"Bearer {licenseToken}");
            }
            else
            {
                Console.WriteLine("Warning: No license token provided - this may cause authentication failure");
            }

            // For Axinom, send the raw SPC data directly as binary content, not base64
            Console.WriteLine($"Sending raw SPC data as binary content");
            request.Content = new ByteArrayContent([.. spcData]);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            // Log request details for debugging
            Console.WriteLine($"Request headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");
            Console.WriteLine($"Content type: {request.Content.Headers.ContentType}");
            Console.WriteLine($"Content length: {request.Content.Headers.ContentLength}");

            var response = await httpClient.SendAsync(request);

            Console.WriteLine($"License server response status: {response.StatusCode}");
            Console.WriteLine($"Response headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"License server error response: {errorContent}");
                throw new HttpRequestException($"KSM request failed with status: {response.StatusCode}, Error: {errorContent}");
            }

            // Axinom should return raw binary CKC data
            var ckcBytes = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"Received CKC data length: {ckcBytes.Length} bytes");
            
            return NSData.FromArray(ckcBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get CKC from KSM: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Handles the failure of the content key session.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="error"></param>
    [Export("contentKeySession:didFailWithError:")]
    public void DidFailWithError(AVContentKeySession session, NSError error)
    {
        Console.WriteLine($"Content key session failed: {error.LocalizedDescription}");
    }

    /// <summary>
    /// Disposes of the FairPlayContentKeySessionDelegate instance.
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            httpClient?.Dispose();
        }
        base.Dispose(disposing);
    }
}