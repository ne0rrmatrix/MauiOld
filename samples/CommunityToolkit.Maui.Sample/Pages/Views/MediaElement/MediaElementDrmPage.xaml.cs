using System.ComponentModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Sample.Models;
using CommunityToolkit.Maui.Sample.ViewModels.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;

namespace CommunityToolkit.Maui.Sample.Pages.Views;

public partial class MediaElementDrmPage : BasePage<MediaElementDrmViewModel>
{
	// ──────────────────────────────────────────────
	//  Axinom public test vectors — v10
	//  https://github.com/Axinom/public-test-vectors
	//  License tokens from ContentKeys/ directory.
	// ──────────────────────────────────────────────

	// DASH / HLS H.264 Single Key
	const string tokenDashH264SingleKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICI0MDYwYTg2NS04ODc4LTQyNjctOWNiZi05MWFlNWJhZTFlNzIiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAid3QzRW51dVI1UkFybjZBRGYxNkNCQT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ.l8PnZznspJ6lnNmfAE9UQV532Ypzt1JXQkvrk8gFSRw";

	// ──────────────────────────────────────────────
	//  Device-specific DRM test vectors
	// ──────────────────────────────────────────────

	// Windows: PlayReady + DASH (PlayReady does not support CMAF cbcs)
	static readonly DrmTestVector windowsTestVector = new(
		"DASH H.264 Single Key",
		"DASH",
		"H.264 | cenc | 1 key | 1080p | PlayReady",
		"https://media.axprod.net/TestVectors/Dash/protected_dash_1080p_h264_singlekey/manifest.mpd",
		"",
		tokenDashH264SingleKey);

	// Android: Widevine + DASH
	static readonly DrmTestVector androidTestVector = new(
		"DASH H.264 Single Key",
		"DASH",
		"H.264 | cenc | 1 key | 1080p | Widevine",
		"https://media.axprod.net/TestVectors/Dash/protected_dash_1080p_h264_singlekey/manifest.mpd",
		"",
		tokenDashH264SingleKey);

	// iOS / macOS: FairPlay + HLS
	static readonly DrmTestVector appleTestVector = new(
		"HLS H.264 Single Key",
		"HLS",
		"H.264 | 1 key | 1080p | FairPlay",
		"",
		"https://media.axprod.net/TestVectors/Hls/protected_hls_1080p_h264_singlekey/manifest.m3u8",
		tokenDashH264SingleKey);

	// Fallback: clear content (no DRM) for unsupported platforms
	static readonly DrmTestVector fallbackTestVector = new(
		"Clear DASH (no DRM)",
		"Clear",
		"H.264 | AAC | No encryption | DASH-IF reference",
		"https://dash.akamaized.net/akamai/bbb_30fps/bbb_30fps.mpd",
		"",
		"");

	// ──────────────────────────────────────────────
	//  State
	// ──────────────────────────────────────────────

	readonly ILogger logger;

	bool hasLoadedSource;

	public MediaElementDrmPage(MediaElementDrmViewModel viewModel, ILogger<MediaElementDrmPage> logger) : base(viewModel)
	{
		InitializeComponent();

		this.logger = logger;
		MediaElement.PropertyChanged += MediaElement_PropertyChanged;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (!hasLoadedSource)
		{
			hasLoadedSource = true;
			LoadDeviceSpecificDrmContent();
		}
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
		MediaElement.Stop();
		MediaElement.Handler?.DisconnectHandler();
	}

	// ──────────────────────────────────────────────
	//  Device-specific DRM content loading
	// ──────────────────────────────────────────────

	void LoadDeviceSpecificDrmContent()
	{
		var (testVector, scheme) = GetDeviceSpecificContent();

		// Determine the manifest URL based on the DRM scheme
		var sourceUrl = scheme switch
		{
			DrmScheme.FairPlay => string.IsNullOrWhiteSpace(testVector.HlsManifestUrl)
				? testVector.DashManifestUrl
				: testVector.HlsManifestUrl,
			_ => string.IsNullOrWhiteSpace(testVector.DashManifestUrl)
				? testVector.HlsManifestUrl
				: testVector.DashManifestUrl,
		};

		if (string.IsNullOrWhiteSpace(sourceUrl) || !Uri.TryCreate(sourceUrl, UriKind.Absolute, out var sourceUri))
		{
			logger.LogWarning("Invalid source URL for DRM content: {Url}", sourceUrl);
			DrmInfoLabel.Text = "Error: Unable to resolve DRM content URL.";
			return;
		}

		// Build DRM configuration
		var drmConfig = new DrmConfiguration
		{
			Scheme = scheme,
			RequiresHardwareSecurity = false, // Software DRM — works on dev machines without HDCP/TEE
		};

		// Set the license server URL based on the DRM scheme
		if (scheme is not DrmScheme.Unknown)
		{
			var licenseUrl = scheme switch
			{
				DrmScheme.Widevine => "https://drm-widevine-licensing.axprod.net/AcquireLicense",
				DrmScheme.PlayReady => "https://drm-playready-licensing.axprod.net/AcquireLicense",
				DrmScheme.FairPlay => "https://drm-fairplay-licensing.axprod.net/AcquireLicense",
				_ => "",
			};

			if (!string.IsNullOrWhiteSpace(licenseUrl) && Uri.TryCreate(licenseUrl, UriKind.Absolute, out var licenseUri))
			{
				drmConfig.LicenseServerUrl = licenseUri;
			}
		}

		// Add the Axinom license token header if present
		if (!string.IsNullOrWhiteSpace(testVector.LicenseToken))
		{
			drmConfig.LicenseRequestHeaders["X-AxDRM-Message"] = testVector.LicenseToken;
		}

		var uriMediaSource = new UriMediaSource
		{
			Uri = sourceUri,
			DrmConfiguration = drmConfig,
		};

		logger.LogInformation(
			"Auto-loading device-specific DRM content — Vector: {Vector}, Scheme: {Scheme}, URL: {Url}, License: {LicenseUrl}",
			testVector.Name, scheme, sourceUri, drmConfig.LicenseServerUrl);

		DrmInfoLabel.Text = $"Platform: {DeviceInfo.Platform} | Scheme: {scheme} | Vector: {testVector.Name} | {testVector.Description}";

		MediaElement.Source = uriMediaSource;
	}

	static (DrmTestVector TestVector, DrmScheme Scheme) GetDeviceSpecificContent()
	{
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			return (windowsTestVector, DrmScheme.PlayReady);
		}

		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			return (androidTestVector, DrmScheme.Widevine);
		}

		if (DeviceInfo.Platform == DevicePlatform.iOS
			|| DeviceInfo.Platform == DevicePlatform.MacCatalyst)
		{
			return (appleTestVector, DrmScheme.FairPlay);
		}

		// Unsupported platform — fall back to clear content
		return (fallbackTestVector, DrmScheme.Unknown);
	}

	// ──────────────────────────────────────────────
	//  Media event handlers
	// ──────────────────────────────────────────────

	void MediaElement_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
		{
			logger.LogInformation("DRM Media — Duration: {NewDuration}", MediaElement.Duration);
			PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
		}
	}

	void OnMediaOpened(object? sender, EventArgs? e)
	{
		logger.LogInformation("DRM Media opened.");
		DimensionsText.Text = $"{MediaElement.MediaWidth}x{MediaElement.MediaHeight}";
	}

	void OnStateChanged(object? sender, MediaStateChangedEventArgs e)
	{
		logger.LogInformation("DRM Media State Changed: {Previous} → {New}", e.PreviousState, e.NewState);
		StateText.Text = e.NewState.ToString();
	}

	void OnMediaFailed(object? sender, MediaFailedEventArgs e)
	{
		logger.LogError("DRM Media failed: {ErrorMessage}", e.ErrorMessage);
		DrmInfoLabel.Text = $"Playback failed: {e.ErrorMessage}";
	}

	void OnMediaEnded(object? sender, EventArgs? e) => logger.LogInformation("DRM Media ended.");

	void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
	{
		PositionSlider.Value = e.Position.TotalSeconds;
	}

	void OnSeekCompleted(object? sender, EventArgs? e) => logger.LogInformation("Seek completed.");

	void OnPlayClicked(object? sender, EventArgs? e) => MediaElement.Play();

	void OnPauseClicked(object? sender, EventArgs? e) => MediaElement.Pause();

	void OnStopClicked(object? sender, EventArgs? e) => MediaElement.Stop();

	void OnMuteClicked(object? sender, EventArgs? e) => MediaElement.ShouldMute = !MediaElement.ShouldMute;

	async void Slider_DragCompleted(object? sender, EventArgs? e)
	{
		ArgumentNullException.ThrowIfNull(sender);

		var newValue = ((Slider)sender).Value;
		await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

		MediaElement.Play();
	}

	void Slider_DragStarted(object? sender, EventArgs? e)
	{
		MediaElement.Pause();
	}
}
