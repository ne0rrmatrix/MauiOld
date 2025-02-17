namespace CommunityToolkit.Maui;

/// <summary>
/// .NET MAUI Community Toolkit Options for MediaElement.
/// </summary>
public class MediaElementOptions()
{
	readonly MauiAppBuilder? builder;
	internal MediaElementOptions(in MauiAppBuilder builder) : this()
	{
		this.builder = builder;
	}

	internal static bool ShouldEnableTextureViewOnAndroid { get; private set; }
	/// <summary>
	/// Will return the default value instead of throwing an exception.
	/// </summary>
	/// <remarks>
	/// Default value is false.
	/// </remarks>
	
	public void SetShouldEnableTextureViewOnAndroid(bool value)
	{
		if (value is true && builder is null)
		{
			throw new InvalidOperationException("The MauiAppBuilder must be provided to enable TextureView on Android.");
		}
		ShouldEnableTextureViewOnAndroid = value;
	}
}
