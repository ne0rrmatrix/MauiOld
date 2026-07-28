using System.ComponentModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Sample.ViewModels.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;

namespace CommunityToolkit.Maui.Sample.Pages.Views;

public partial class MediaElementDrmPage : BasePage<MediaElementDrmViewModel>
{
	// Public license tokens from the Axinom test vectors
	// (https://github.com/Axinom/public-test-vectors). Each token is bound to
	// the vector's content key(s) and carries Policy A (min security level 150,
	// persistence allowed).
	const string axinomDashSingleKeyToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICI0MDYwYTg2NS04ODc4LTQyNjctOWNiZi05MWFlNWJhZTFlNzIiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAid3QzRW51dVI1UkFybjZBRGYxNkNCQT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ.l8PnZznspJ6lnNmfAE9UQV532Ypzt1JXQkvrk8gFSRw";
	const string axinomCmafSingleKeyToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICIzMDJmODBkZC00MTFlLTQ4ODYtYmNhNS1iYjFmODAxOGEwMjQiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAicm9LQWcwdDdKaTFpNDNmd3YremZ0UT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ._NfhLVY7S6k8TJDWPeMPhUawhympnrk6WAZHOVjER6M";

	readonly ILogger logger;
	readonly Dictionary<string, string> drmHeaders = new(StringComparer.OrdinalIgnoreCase);

	public MediaElementDrmPage(MediaElementDrmViewModel viewModel, ILogger<MediaElementDrmPage> logger) : base(viewModel)
	{
		InitializeComponent();

		this.logger = logger;
		MediaElement.PropertyChanged += MediaElement_PropertyChanged;

		InitializeDrmDefaults();
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
		MediaElement.Stop();
		MediaElement.Handler?.DisconnectHandler();
	}

	void InitializeDrmDefaults()
	{
		// Auto-select the appropriate DRM scheme for the current platform
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			DrmSchemePicker.SelectedIndex = 1; // PlayReady
			PlatformHintLabel.Text = "(recommended for Windows)";
		}
		else if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			DrmSchemePicker.SelectedIndex = 0; // Widevine
			PlatformHintLabel.Text = "(recommended for Android)";
		}
		else if (DeviceInfo.Platform == DevicePlatform.iOS
				 || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
		{
			DrmSchemePicker.SelectedIndex = 2; // FairPlay
			PlatformHintLabel.Text = "(required for iOS/macOS)";
		}

		UpdateForScheme(GetSelectedScheme());
	}

	void UpdateForScheme(DrmScheme scheme)
	{
		// Set license URL, source URL, and matching license token based on DRM scheme.
		// Using Axinom public test vectors — see constants above for the tokens.
		switch (scheme)
		{
			case DrmScheme.Widevine:
				LicenseServerEntry.Text = "https://drm-widevine-licensing.axprod.net/AcquireLicense";
				SourceUrlEntry.Text = "https://media.axprod.net/TestVectors/Cmaf/protected_1080p_h264_cbcs/manifest.mpd";
				drmHeaders["X-AxDRM-Message"] = axinomCmafSingleKeyToken;
				break;
			case DrmScheme.PlayReady:
				LicenseServerEntry.Text = "https://drm-playready-licensing.axprod.net/AcquireLicense";
				SourceUrlEntry.Text = "https://media.axprod.net/TestVectors/Dash/protected_dash_1080p_h264_singlekey/manifest.mpd";
				drmHeaders["X-AxDRM-Message"] = axinomDashSingleKeyToken;
				break;
			case DrmScheme.FairPlay:
				LicenseServerEntry.Text = "https://drm-fairplay-licensing.axprod.net/AcquireLicense";
				SourceUrlEntry.Text = "https://media.axprod.net/TestVectors/Cmaf/protected_1080p_h264_cbcs/manifest.m3u8";
				drmHeaders["X-AxDRM-Message"] = axinomCmafSingleKeyToken;
				break;
		}

		UpdateDrmHeadersSummary();
	}

	DrmScheme GetSelectedScheme() => DrmSchemePicker.SelectedIndex switch
	{
		0 => DrmScheme.Widevine,
		1 => DrmScheme.PlayReady,
		2 => DrmScheme.FairPlay,
		_ => DrmScheme.Unknown,
	};

	void OnDrmSchemeChanged(object? sender, EventArgs e)
	{
		var scheme = GetSelectedScheme();

		PlatformHintLabel.Text = scheme switch
		{
			DrmScheme.Widevine => "(Google Widevine — Android, Chrome)",
			DrmScheme.PlayReady => "(Microsoft PlayReady — Windows, Xbox)",
			DrmScheme.FairPlay => "(Apple FairPlay — iOS, macOS, Safari)",
			_ => "",
		};

		UpdateForScheme(scheme);
	}

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
		//StateText.Text = e.NewState.ToString();
	}

	void OnMediaFailed(object? sender, MediaFailedEventArgs e)
	{
		logger.LogError("DRM Media failed: {ErrorMessage}", e.ErrorMessage);
		//StateText.Text = "Failed";
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

	/// <summary>
	/// Builds and loads the DRM-protected source from the current UI fields.
	/// </summary>
	void LoadDrmSource()
	{
		var sourceUrl = SourceUrlEntry.Text?.Trim();
		if (string.IsNullOrWhiteSpace(sourceUrl))
		{
			logger.LogWarning("No source URL provided for DRM load.");
			return;
		}

		if (!Uri.TryCreate(sourceUrl, UriKind.Absolute, out var sourceUri))
		{
			logger.LogWarning("Invalid source URL: {Url}", sourceUrl);
			return;
		}

		var scheme = GetSelectedScheme();
		if (scheme is DrmScheme.Unknown)
		{
			logger.LogWarning("No DRM scheme selected.");
			return;
		}

		// Build DRM configuration
		var drmConfig = new DrmConfiguration
		{
			Scheme = scheme,
			RequiresHardwareSecurity = HardwareSecuritySwitch.IsToggled,
		};

		var licenseUrl = LicenseServerEntry.Text?.Trim();
		if (!string.IsNullOrWhiteSpace(licenseUrl) && Uri.TryCreate(licenseUrl, UriKind.Absolute, out var licenseUri))
		{
			drmConfig.LicenseServerUrl = licenseUri;
		}

		foreach (var header in drmHeaders)
		{
			drmConfig.LicenseRequestHeaders[header.Key] = header.Value;
		}

		// Build UriMediaSource with DRM
		var uriMediaSource = new UriMediaSource
		{
			Uri = sourceUri,
			DrmConfiguration = drmConfig,
		};

		logger.LogInformation(
			"Loading DRM source — Scheme: {Scheme}, URL: {Url}, License: {LicenseUrl}, HW: {Hw}",
			scheme, sourceUri, drmConfig.LicenseServerUrl, drmConfig.RequiresHardwareSecurity);

		MediaElement.Source = uriMediaSource;
	}

	void OnLoadDrmSourceClicked(object? sender, EventArgs? e) => LoadDrmSource();

	void AddDrmHeaderClicked(object? sender, EventArgs? e)
	{
		var name = HeaderNameEntry.Text?.Trim();
		var value = HeaderValueEntry.Text?.Trim();

		if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
		{
			return;
		}

		drmHeaders[name] = value;
		UpdateDrmHeadersSummary();

		HeaderNameEntry.Text = string.Empty;
		HeaderValueEntry.Text = string.Empty;
	}

	void ClearDrmHeadersClicked(object? sender, EventArgs? e)
	{
		drmHeaders.Clear();
		UpdateDrmHeadersSummary();
	}

	void UpdateDrmHeadersSummary()
	{
		DrmHeadersSummaryLabel.Text = drmHeaders.Count <= 0
			? "No headers defined"
			: string.Join(", ", drmHeaders.Keys);
	}
}
