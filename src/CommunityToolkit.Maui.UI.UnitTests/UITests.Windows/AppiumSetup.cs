using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;
[SetUpFixture]
public class AppiumSetup
{
	static AppiumDriver? driver;

	public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		// If you started an Appium server manually, make sure to comment out the next line
		// This line starts a local Appium server for you as part of the test run
		AppiumServerHelper.StartAppiumLocalServer();
		var windowsOptions = new AppiumOptions
		{
			// Specify windows as the driver, typically don't need to change this
			AutomationName = "windows",
			// Always Windows for Windows
			PlatformName = "Windows",
			// The identifier of the deployed application to test
			App = "com.microsoft.CommunityToolkit.Maui.Sample_ph1m9x8skttmg!App",
		};

		// Note there are many more options that you can use to influence the app under test according to your needs

		driver = new WindowsDriver(windowsOptions);
	}

	[Test]
	public void MediaElementTest()
	{
		// Arrange
		var currentPage = App.FindElement()
		var mediaElement = App.FindElementByAccessibilityId("MediaElement");

		// Act
		mediaElement.Click();

		// Assert
		Assert.Pass();
	}

	[OneTimeTearDown]
	public void RunAfterAnyTests()
	{
		driver?.Quit();

		// If an Appium server was started locally above, make sure we clean it up here
		AppiumServerHelper.DisposeAppiumLocalServer();
	}
}