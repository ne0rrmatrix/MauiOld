using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace UITests;
public class MediaElementTests : BaseTest
{
	public void SourceMenu()
	{
		if (!AppiumServerHelper.IsWindowsValid() && !OperatingSystem.IsMacOS())
		{
			return;
		}

		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(60));
		var chooseSource = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Change Source\")")));
		chooseSource.Click();
	}

	[Test, Order(1)]
    public void MediaElementPlayBackControls()
    {
		if(!AppiumServerHelper.IsWindowsValid() && !OperatingSystem.IsMacOS())
		{
			return;
		}

		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));

		// Open the menu
		var menu = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().description(\"Open navigation drawer\")")));
		menu.Click();

		// Click on Views
		var views = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Views\")")));
		views.Click();

		// Scroll to MediaElement and click on it
		var mediaElement = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiScrollable(new UiSelector().scrollable(true).instance(0)).scrollIntoView(new UiSelector().text(\"MediaElement\"))")));
		mediaElement.Click();

		// click on Allow button for permissions
		var permissions = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.android.permissioncontroller:id/permission_allow_button\")")));
		permissions.Click();

		// Get initial position
		var positionElement = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.microsoft.CommunityToolkit.Maui.Sample:id/Position\")")));
		Assert.That(positionElement.Displayed, Is.True, "Position element should be visible");
		string initialPosition = positionElement.Text;
		
		// Check if media is playing by verifying position change
		bool isPlaying = wait.Until(d =>
		{
			string currentPosition = positionElement.Text;
			return currentPosition != initialPosition;
		});
		Assert.That(isPlaying, Is.True, "Media should be playing (position should change)");
		
		// click on Pause button
		var pauseBtn = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.microsoft.CommunityToolkit.Maui.Sample:id/PauseBtn\")")));
		Assert.That(pauseBtn.Displayed, Is.True, "Pause button should be visible after playing");
		pauseBtn.Click();

		// Click on Play button
		var playBtn = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.microsoft.CommunityToolkit.Maui.Sample:id/PlayBtn\")")));
		Assert.That(playBtn.Displayed, Is.True, "Play button should be visible");
		playBtn.Click();

		// Click on Stop button
		var stopBtn = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.microsoft.CommunityToolkit.Maui.Sample:id/StopBtn\")")));
		Assert.That(stopBtn.Displayed, Is.True, "Stop button should be visible");
		stopBtn.Click();
	}

	[Test, Order(2)]
	public void MediaSource()
	{
		if (OperatingSystem.IsMacOS() || !AppiumServerHelper.IsWindowsValid())
		{
			return;
		}

		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(120));

		SourceMenu();
		var loadHls = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Load HTTP Live Stream (HLS)\")")));
		Assert.That(loadHls.Displayed, Is.True, "Load HTTP Live Stream (HLS) button should be visible");
		loadHls.Click();

		SourceMenu();
		var loadLocalResource = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Load Local Resource\")")));
		Assert.That(loadLocalResource.Displayed, Is.True, "Load Local Resource button should be visible");
		loadLocalResource.Click();

		SourceMenu();
		var resetSource = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Reset Source to null\")")));
		Assert.That(resetSource.Displayed, Is.True, "Reset Source to null button should be visible");
		resetSource.Click();

		SourceMenu();
		var loadMusic = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Load Music\")")));
		Assert.That(loadMusic.Displayed, Is.True, "Load Music button should be visible");
		loadMusic.Click();

		SourceMenu();
		var loadOnlineMp4 = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().text(\"Load Online MP4\")")));
		Assert.That(loadOnlineMp4.Displayed, Is.True, "Load Online MP4 button should be visible");
		loadOnlineMp4.Click();
	}
}
