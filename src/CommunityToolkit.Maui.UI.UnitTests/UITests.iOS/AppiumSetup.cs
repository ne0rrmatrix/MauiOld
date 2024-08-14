using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace UITests;
[SetUpFixture]
public class AppiumSetup
{
	static AppiumDriver? driver;

	public static AppiumDriver? App => driver;

	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		if (OperatingSystem.IsWindows() || App is null)
		{
			return;
		}
		// If you started an Appium server manually, make sure to comment out the next line
		// This line starts a local Appium server for you as part of the test run
		AppiumServerHelper.StartAppiumLocalServer();

		var iOSOptions = new AppiumOptions
		{
			// Specify XCUITest as the driver, typically don't need to change this
			AutomationName = "XCUITest",
			// Always iOS for iOS
			PlatformName = "iOS",
			// iOS Version
			PlatformVersion = "17.5",
			// Don't specify if you don't want a specific device
			DeviceName = "iPhone 15 Pro",
			// The full path to the .app file to test or the bundle id if the app is already installed on the device
			App = "..//..//..//..//..//..//samples/CommunityToolkit.Maui.Sample/bin/Debug/net8.0-ios/iossimulator-arm64/CommunityToolkit.Maui.Sample.app",
		};
		iOSOptions.AddAdditionalAppiumOption("appium:includeSafariInWebviews", true);
        iOSOptions.AddAdditionalAppiumOption("appium:newCommandTimeout", 3600);
        iOSOptions.AddAdditionalAppiumOption("appium:connectHardwareKeyboard", true);
		// Note there are many more options that you can use to influence the app under test according to your needs

		driver = new IOSDriver(iOSOptions);
	}

	[OneTimeTearDown]
	public void RunAfterAnyTests()
	{
		if (OperatingSystem.IsWindows()) 
		{
			return;
		}
		driver?.Quit();

		// If an Appium server was started locally above, make sure we clean it up here
		AppiumServerHelper.DisposeAppiumLocalServer();
	}
}