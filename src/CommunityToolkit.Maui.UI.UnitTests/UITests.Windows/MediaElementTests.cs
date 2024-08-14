using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace UITests;
public class MediaElementTests : BaseTest
{
	[Test]
	public void MediaElementPlayBackControls()
	{
		if(OperatingSystem.IsMacOS() || !AppiumServerHelper.IsWindowsValid())
		{
			return;
		}
		
		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));

		var views = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Views")));
		views.Click();

		var mediaElement = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("MediaElement")));
		mediaElement.Click();

		// Get initial position
		var positionElement = wait.Until(d => d.FindElement(ByWindowsAutomation.AccessibilityId("Position")));
		string initialPosition = positionElement.Text;

		var stopBtn = wait.Until(d => d.FindElement(ByWindowsAutomation.AccessibilityId("StopBtn")));
		stopBtn.Click();

		// Wait for the media to stop and verify
		var playBtn = wait.Until(d => d.FindElement(ByWindowsAutomation.AccessibilityId("PlayBtn")));
		playBtn.Click();

		// Check if media is playing by verifying position change
		bool isPlaying = wait.Until(d =>
		{
			string currentPosition = positionElement.Text;
			return currentPosition != initialPosition;
		});

		Assert.That(isPlaying, Is.True, "Media should be playing (position should change)");

		var pauseBtn = wait.Until(d => d.FindElement(ByWindowsAutomation.AccessibilityId("PauseBtn")));
		Assert.That(pauseBtn.Displayed, Is.True, "Pause button should be visible after playing");
		Assert.That(playBtn.Displayed, Is.True, "Play button should be visible");
		Assert.That(stopBtn.Displayed, Is.True, "Stop button should be visible");
		Assert.That(positionElement.Displayed, Is.True, "Position element should be visible");
	}
}
