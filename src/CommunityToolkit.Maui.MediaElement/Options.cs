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

	internal static bool ShouldEnableServiceOnAndroid { get; private set; }

	/// <summary>
	/// Sets the value to enable the MediaElement service on Android.
	/// </summary>
	/// <remarks>
	/// Default value is false.
	/// </remarks>
	public void SetShouldEnableServiceOnAndroid(bool value)
	{
		if (value is true && builder is null)
		{
			throw new InvalidOperationException("The MauiAppBuilder must be provided to enable Service on Android.");
		}
		ShouldEnableServiceOnAndroid = value;
	}
}
