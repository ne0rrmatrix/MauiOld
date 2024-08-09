using OpenQA.Selenium.Appium.Service;

namespace UITests;
public static class AppiumServerHelper
{
	static AppiumLocalService? appiumLocalService;

	public const string DefaultHostAddress = "127.0.0.1";
	public const int DefaultHostPort = 4723;

	public static void StartAppiumLocalServer(string host = DefaultHostAddress,
		int port = DefaultHostPort)
	{
		if (appiumLocalService is not null)
		{
			return;
		}

		var builder = new AppiumServiceBuilder()
			.WithIPAddress(host)
			.UsingPort(port);

		// Start the server with the builder
		appiumLocalService = builder.Build();
		appiumLocalService.Start();
	}

	public static void DisposeAppiumLocalServer()
	{
		appiumLocalService?.Dispose();
	}
}