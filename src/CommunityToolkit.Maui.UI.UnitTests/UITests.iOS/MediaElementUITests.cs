using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace UITests;

public class MediaElementUITests : BaseTest
{
    [Test]
	public void MediaElementPlayBackControls()
	{
		if(OperatingSystem.IsWindows() || App is null)
		{
			return;
		}
		var menu = App.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@name='OK']"));
		menu?.Click();
		var views = App.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeOther[@name='Views']"));
		views?.Click();
		string element = "//XCUIElementTypeStaticText[@name='MediaElement']";
		ScrollUntilElementFound(element, 10);
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));
		var mediaElement = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeStaticText[@name='MediaElement']")));
		mediaElement?.Click();
		
		// Get initial position
		var positionElement = wait.Until(d => d.FindElement(ByIosUIAutomation.AccessibilityId("Position")));
		string initialPosition = positionElement.Text;

		var stopBtn = wait.Until(d => d.FindElement(ByIosUIAutomation.AccessibilityId("StopBtn")));
		stopBtn.Click();

		// Wait for the media to stop and verify
		var playBtn = wait.Until(d => d.FindElement(ByIosUIAutomation.AccessibilityId("PlayBtn")));
		playBtn.Click();

		// Check if media is playing by verifying position change
		bool isPlaying = wait.Until(d =>
		{
			string currentPosition = positionElement.Text;
			return currentPosition != initialPosition;
		});

		Assert.That(isPlaying, Is.True, "Media should be playing (position should change)");

		var pauseBtn = wait.Until(d => d.FindElement(ByIosUIAutomation.AccessibilityId("PauseBtn")));
		Assert.That(pauseBtn.Displayed, Is.True, "Pause button should be visible after playing");
		Assert.That(playBtn.Displayed, Is.True, "Play button should be visible");
		Assert.That(stopBtn.Displayed, Is.True, "Stop button should be visible");
		Assert.That(positionElement.Displayed, Is.True, "Position element should be visible");
	}
	
	public void ScrollUntilElementFound(string elementId, int maxScrolls = 10)
	{
		for (int i = 0; i < maxScrolls; i++)
		{
			try
			{
				ArgumentNullException.ThrowIfNull(App);
				var element = App.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeStaticText[@name='MediaElement']"));
				if (element.Displayed)
				{
					return; // Element found, exit the method
				}
			}
			catch (NoSuchElementException)
			{
				// Element not found, continue scrolling
			}
			ArgumentNullException.ThrowIfNull(App);
			// Perform scroll
			var screenSize = App.Manage().Window.Size;
			int startX = screenSize.Width / 2;
			int startY = (int)(screenSize.Height * 0.8);
			int endY = (int)(screenSize.Height * 0.2);

			var finger = new PointerInputDevice(PointerKind.Touch);
			var start = new Point(startX, startY);
			var end = new Point(startX, endY);
			var swipe = new ActionSequence(finger);
			swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, start.X, start.Y, TimeSpan.Zero));
			swipe.AddAction(finger.CreatePointerDown(MouseButton.Left));
			swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, end.X, end.Y, TimeSpan.FromMilliseconds(1000)));
			swipe.AddAction(finger.CreatePointerUp(MouseButton.Left));
			App?.PerformActions(new List<ActionSequence> { swipe });

			// Wait for content to settle
			System.Threading.Thread.Sleep(1000);
		}

		throw new Exception($"Element with accessibility ID '{elementId}' not found after {maxScrolls} scrolls.");
	}
}

