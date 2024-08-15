using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Mac;

namespace UITests;
[SetUpFixture]
public class AppiumSetup : IDisposable
{
	bool disposedValue;
	static AppiumDriver? driver;

	public static AppiumDriver? App => driver;

	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		if (OperatingSystem.IsWindows() && !AppiumServerHelper.IsMacValid())
		{
			return;
		}
		// If you started an Appium server manually, make sure to comment out the next line
		// This line starts a local Appium server for you as part of the test run
		AppiumServerHelper.StartAppiumLocalServer();
		var macOptions = new AppiumOptions
		{
			// Specify mac2 as the driver, typically don't need to change this
			AutomationName = "mac2",
			// Always Mac for Mac
			PlatformName = "Mac",
			// The full path to the .app file to test
			App = "..//..//..//..//..//..//samples/CommunityToolkit.Maui.Sample/bin/Debug/net8.0-maccatalyst/maccatalyst-x64/CommunityToolkit.Maui.Sample.app",
		};

		// Setting the Bundle ID is required, else the automation will run on Finder
		macOptions.AddAdditionalAppiumOption(IOSMobileCapabilityType.BundleId, "com.microsoft.CommunityToolkit.Maui.Sample");
		macOptions.AddAdditionalAppiumOption("appium:includeSafariInWebviews", true);
        macOptions.AddAdditionalAppiumOption("appium:connectHardwareKeyboard", true);
		// Note there are many more options that you can use to influence the app under test according to your needs

		driver = new MacDriver(macOptions);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				driver?.Quit();
				// If an Appium server was started locally above, make sure we clean it up here
				AppiumServerHelper.DisposeAppiumLocalServer();
			}

			disposedValue = true;
		}
	}

	[OneTimeTearDown]
	public void RunAfterAnyTests()
	{
		if (OperatingSystem.IsWindows() && !AppiumServerHelper.IsMacValid())
		{
			return;
		}
		driver?.Quit();

		// If an Appium server was started locally above, make sure we clean it up here
		AppiumServerHelper.DisposeAppiumLocalServer();
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}