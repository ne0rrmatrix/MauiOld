using System.Diagnostics;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace UITests;
public class MediaElementTests : BaseTest
{
	public void SourceMenu()
	{
		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));
		var chooseSource = wait.Until(d => d.FindElement(ByWindowsAutomation.AccessibilityId("ChooseSource")));
		chooseSource.Click();
	}

	[Test, Order(1)]
	public void MediaElementPlayBackControls()
	{
		if (OperatingSystem.IsMacOS() || !AppiumServerHelper.IsWindowsValid())
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

	[Test, Order(2)]
	public void FullScreen()
	{
		if (OperatingSystem.IsMacOS() || !AppiumServerHelper.IsWindowsValid())
		{
			return;
		}

		ArgumentNullException.ThrowIfNull(App);
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));
		var windowSize = App.Manage().Window.Size;
		App.ExecuteScript("windows: hover", new Dictionary<string, object>() 
		{
			{"startX", 0},
			{"startY", 0},
			{"endX", windowSize.Width/2 },
			{"endY", windowSize.Height/2 },
			{"duration", 700 }
		});
		
		var fullScreen = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Full Screen Button")));
		Assert.That(fullScreen.Displayed, Is.True, "Full Screen button should be visible");
		fullScreen.Click();
		var currentWindowSize = App.Manage().Window.Size;
		Assert.That(windowSize, Is.Not.EqualTo(currentWindowSize), "Window size should change when full screen is clicked");
		fullScreen.Click();
		currentWindowSize = App.Manage().Window.Size;
		Assert.That(windowSize, Is.EqualTo(currentWindowSize), "Window size should change back when full screen is clicked again");
	}

	[Test, Order(3)]
	public void MediaSource()
	{
		if (OperatingSystem.IsMacOS() || !AppiumServerHelper.IsWindowsValid())
		{
			return;
		}

		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(60));

		SourceMenu();
		var loadHls = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Load HTTP Live Stream (HLS)")));
		Assert.That(loadHls.Displayed, Is.True, "Load HTTP Live Stream (HLS) button should be visible");
		loadHls.Click();

		SourceMenu();
		var loadLocalResource = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Load Local Resource")));
		Assert.That(loadLocalResource.Displayed, Is.True, "Load Local Resource button should be visible");
		loadLocalResource.Click();
		
		SourceMenu();
		var resetSource = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Reset Source to null")));
		Assert.That(resetSource.Displayed, Is.True, "Reset Source to null button should be visible");
		resetSource.Click();

		SourceMenu();
		var loadMusic = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Load Music")));
		Assert.That(loadMusic.Displayed, Is.True, "Load Music button should be visible");
		loadMusic.Click();

		SourceMenu();
		var loadOnlineMp4 = wait.Until(d => d.FindElement(ByWindowsAutomation.Name("Load Online MP4")));
		Assert.That(loadOnlineMp4.Displayed, Is.True, "Load Online MP4 button should be visible");
		loadOnlineMp4.Click();
	}
}
