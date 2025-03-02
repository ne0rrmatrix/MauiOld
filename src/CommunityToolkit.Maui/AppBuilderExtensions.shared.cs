﻿using System.Runtime.Versioning;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Handlers;
using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui;

/// <summary>
/// Extensions for MauiAppBuilder
/// </summary>
[SupportedOSPlatform("iOS15.0")]
[SupportedOSPlatform("MacCatalyst15.0")]
[SupportedOSPlatform("Android21.0")]
[SupportedOSPlatform("Windows10.0.17763")]
[SupportedOSPlatform("Tizen6.5")]
public static class AppBuilderExtensions
{
	/// <summary>
	/// Initializes the .NET MAUI Community Toolkit Library
	/// </summary>
	/// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/> </param>
	/// <param name="options"><see cref="Options"/></param>
	/// <returns><see cref="MauiAppBuilder"/> initialized for <see cref="CommunityToolkit.Maui"/></returns>
	public static MauiAppBuilder UseMauiCommunityToolkit(this MauiAppBuilder builder, Action<Options>? options = null)
	{
		// Invokes options for both `CommunityToolkit.Maui` and `CommunityToolkit.Maui.Core`
		options?.Invoke(new Options(builder));

		// Pass `null` because `options?.Invoke()` will set options on both `CommunityToolkit.Maui` and `CommunityToolkit.Maui.Core`
		// Be sure to call `.UseMauiCommunityToolkitCore(null)` after `options.Invoke(new Options(builder))` to ensure `CommunityToolkit.Maui.Core.Options` have already been set
		builder.UseMauiCommunityToolkitCore(null);

		builder.Services.AddSingleton<IPopupService, PopupService>();

		builder.ConfigureMauiHandlers(static h =>
		{
			h.AddHandler<DrawingView, DrawingViewHandler>();
			h.AddHandler<SemanticOrderView, SemanticOrderViewHandler>();
		});

#if ANDROID
		NavigationBar.RemapForControls();
#endif
		return builder;
	}
}