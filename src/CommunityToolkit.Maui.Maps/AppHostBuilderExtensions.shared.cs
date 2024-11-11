﻿namespace CommunityToolkit.Maui.Maps;

/// <summary>
/// Maps Extensions for <see cref="MauiAppBuilder"/>
/// </summary>
public static class AppHostBuilderExtensions
{
	/// <summary>
	/// Initializes the .NET MAUI Community Toolkit Maps Library
	/// </summary>
	/// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/> </param>
	/// <param name="key"></param>
	/// <returns><see cref="MauiAppBuilder"/></returns>
	/// <remarks>
	/// In order to use the .NET MAUI Community Toolkit Maps you need to call the extension method in your `MauiProgram.cs` file as follows:
	/// </remarks>
	/// <example>
	/// <code>
	/// public static MauiApp CreateMauiApp()
	///	{
	///		var builder = MauiApp.CreateBuilder();
	///		builder
	///			.UseMauiApp&amp;lt;App&amp;gt;()
	///	
	/// #if WINDOWS
	///			// Initialize the .NET MAUI Community Toolkit Maps by adding the below line of code
	///			.UseMauiCommunityToolkitMaps("key")
	///	#else
	///			.UseMauiMaps()
	///	#endif	
	///
	///		// Continue initializing your .NET MAUI App here
	///
	///		return builder.Build();
	/// }	
	/// </code>
	/// </example>
	/// 
	public static MauiAppBuilder UseMauiCommunityToolkitMaps(this MauiAppBuilder builder, string key)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
#if WINDOWS
			CommunityToolkit.Maui.Maps.Handlers.MapHandlerWindows.MapsKey = key;
			handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CommunityToolkit.Maui.Maps.Handlers.MapHandlerWindows>();
#endif
		});

		return builder;
	}
}