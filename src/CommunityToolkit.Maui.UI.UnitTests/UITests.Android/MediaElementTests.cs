using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace UITests;
public class MediaElementTests : BaseTest
{
    [Test]
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
		pauseBtn.Click();

		// Click on Play button
		var playBtn = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.microsoft.CommunityToolkit.Maui.Sample:id/PlayBtn\")")));
		playBtn.Click();

		// Click on Stop button
		var stopBtn = wait.Until(d => d.FindElement(ByAndroidUIAutomator.AndroidUIAutomator("new UiSelector().resourceId(\"com.microsoft.CommunityToolkit.Maui.Sample:id/StopBtn\")")));
		stopBtn.Click();

		Assert.That(pauseBtn.Displayed, Is.True, "Pause button should be visible after playing");
		Assert.That(playBtn.Displayed, Is.True, "Play button should be visible");
		Assert.That(stopBtn.Displayed, Is.True, "Stop button should be visible");
		Assert.That(positionElement.Displayed, Is.True, "Position element should be visible");
	}
}
