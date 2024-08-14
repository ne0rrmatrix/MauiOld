using System.Diagnostics;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace UITests;
[SetUpFixture]
public class AppiumSetup
{
	static AppiumDriver? driver;

	public static AppiumDriver? App => driver;

	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		if(!AppiumServerHelper.IsWindowsValid() && !OperatingSystem.IsMacOS())
		{
			return;
		}

		var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
		AppiumServerHelper.StartAppiumLocalServer();
		var androidOptions = new AppiumOptions
		{
			AutomationName = "UiAutomator2",
			PlatformName = "Android",
		};
		string appPath = string.Empty;
		if(OperatingSystem.IsWindows())
		{
			appPath = "./../../../../..//..//samples/CommunityToolkit.Maui.Sample/bin/debug/net8.0-android/com.microsoft.CommunityToolkit.Maui.Sample-Signed.apk";
		}
		else
		{
			appPath = System.IO.Path.GetFullPath("./../../../../..//..//samples/CommunityToolkit.Maui.Sample/bin/debug/net8.0-android/com.microsoft.CommunityToolkit.Maui.Sample-Signed.apk");
		}

		// DEBUG BUILD SETUP
		// If you're running your tests against debug builds you'll need to set NoReset to true
		// otherwise appium will delete all the libraries used for Fast Deployment on Android
		// Release builds have Fast Deployment disabled
		// https://learn.microsoft.com/xamarin/android/deploy-test/building-apps/build-process#fast-deployment
		androidOptions.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, "true");
		androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "com.microsoft.CommunityToolkit.Maui.Sample");
		
		androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, $"com.microsoft.CommunityToolkit.Maui.Sample.MainActivity");
		// END DEBUG BUILD SETUP

		// Specifying the avd option will boot the emulator for you
		// make sure there is an emulator with the name below
		// If not specified, make sure you have an emulator booted
		androidOptions.AddAdditionalAppiumOption("avd", "pixel_5_-_api_33");

		// Note there are many more options that you can use to influence the app under test according to your needs
		androidOptions.App = appPath;

		driver = new AndroidDriver(serverUri, androidOptions, TimeSpan.FromSeconds(180));
		
		driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(180);

		if (driver.IsAppInstalled("com.microsoft.CommunityToolkit.Maui.Sample"))
		{
			driver.RemoveApp("com.microsoft.CommunityToolkit.Maui.Sample");
			driver.InstallApp(appPath);
			driver.ActivateApp("com.microsoft.CommunityToolkit.Maui.Sample");
		}
		else
		{
			driver.InstallApp(appPath);
			driver.ActivateApp("com.microsoft.CommunityToolkit.Maui.Sample");
		}
	}

	[OneTimeTearDown]
	public async Task RunAfterAnyTests()
	{
		driver?.Quit();
		AppiumServerHelper.DisposeAppiumLocalServer();
		if (OperatingSystem.IsWindows())
		{
			string cmdSTR = "adb emu kill";
			ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + cmdSTR)
			{
				RedirectStandardOutput = false,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			var process = Process.Start(processStartInfo);
			if (process is null)
			{
				return;
			}
			await process.WaitForExitAsync();
		}
		else
		{
			string cmdSTR = " emu kill";
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = "/bin/bash";
			processStartInfo.Arguments = $"-c \"adb {cmdSTR}\"";
			processStartInfo.RedirectStandardOutput = false;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;

			var process = Process.Start(processStartInfo);
			if (process is null)
			{
				return;
			}
			await process.WaitForExitAsync();
		}
		// If an Appium server was started locally above, make sure we clean it up here
		AppiumServerHelper.DisposeAppiumLocalServer();
	}
}