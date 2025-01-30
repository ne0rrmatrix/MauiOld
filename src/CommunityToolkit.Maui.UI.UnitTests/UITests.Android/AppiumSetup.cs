using System.Diagnostics;
using System.Text;
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
	static string appPath = string.Empty;

	[OneTimeSetUp]
	public async Task RunBeforeAnyTestsAsync()
	{
		if (OperatingSystem.IsWindows())
		{
			appPath = "./../../../../..//..//samples/CommunityToolkit.Maui.Sample/bin/Release/net9.0-android/com.microsoft.CommunityToolkit.Maui.Sample-Signed.apk";
		}
		else
		{
			appPath = System.IO.Path.GetFullPath("./../../../../..//..//samples/CommunityToolkit.Maui.Sample/bin/Release/net.0-android/com.microsoft.CommunityToolkit.Maui.Sample-Signed.apk");
		}


		if (File.Exists(appPath))
		{
			Trace.WriteLine("APK file exists");
		}
		else
		{

			// Redirect console output to NUnit's test context
			TestContext.Progress.WriteLine("Starting build process...");

			string buildCmd = "dotnet build ./../../../../../../samples/CommunityToolkit.Maui.Sample/CommunityToolkit.Maui.Sample.csproj -c:Release -f net9.0-android";

			ProcessStartInfo buildStartInfo = new("cmd.exe", "/c " + buildCmd)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			var buildProcess = Process.Start(buildStartInfo);

			if (buildProcess is not null)
			{
				buildProcess.OutputDataReceived += (sender, args) =>
				{
					if (args.Data != null)
					{
						TestContext.Progress.WriteLine(args.Data);
					}
				};

				buildProcess.ErrorDataReceived += (sender, args) =>
				{
					if (args.Data != null)
					{
						TestContext.Progress.WriteLine(args.Data);
					}
				};

				buildProcess.BeginOutputReadLine();
				buildProcess.BeginErrorReadLine();

				await buildProcess.WaitForExitAsync();
			}
		}
		var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
		AppiumServerHelper.StartAppiumLocalServer();
		var androidOptions = new AppiumOptions
		{
			AutomationName = "UiAutomator2",
			PlatformName = "Android",
			AcceptInsecureCertificates = true,
		};
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

		driver = new AndroidDriver(serverUri, androidOptions, TimeSpan.FromSeconds(30));

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
		driver?.TerminateApp("com.microsoft.CommunityToolkit.Maui.Sample");
		driver?.RemoveApp("com.microsoft.CommunityToolkit.Maui.Sample");
		driver?.Quit();
		AppiumServerHelper.DisposeAppiumLocalServer();
		if (OperatingSystem.IsWindows())
		{
			string adbPath = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
		"Android", "android-sdk", "platform-tools", "adb.exe");

			ProcessStartInfo processInfo = new()
			{
				FileName = adbPath,
				Arguments = "emu kill",
				RedirectStandardOutput = false,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(processInfo);
			if (process is not null)
			{
				await process.WaitForExitAsync();
			}
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