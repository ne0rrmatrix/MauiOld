﻿#if ANDROID
using CommunityToolkit.Maui.Media.Services;
#endif

using CommunityToolkit.Maui.Core.Handlers;
using CommunityToolkit.Maui.Views;

namespace CommunityToolkit.Maui;

/// <summary>
/// This class contains MediaElement's <see cref="MauiAppBuilder"/> extensions.
/// </summary>
public static class AppBuilderExtensions
{
	/// <summary>
	/// Initializes the .NET MAUI Community Toolkit MediaElement Library
	/// </summary>
	/// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/>.</param>
	/// <returns><see cref="MauiAppBuilder"/> initialized for <see cref="MediaElement"/>.</returns>
	public static MauiAppBuilder UseMauiCommunityToolkitMediaElement(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(h =>
		{
			h.AddHandler<MediaElement, MediaElementHandler>();
		});
#if ANDROID
		builder.Services.AddSingleton<MediaControlsService>();
#endif
		return builder;
	}
}