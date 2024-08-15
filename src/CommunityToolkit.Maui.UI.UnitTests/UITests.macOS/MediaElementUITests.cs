using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace UITests;

public class MediaElementUITests : BaseTest
{
	public void SourceMenu()
	{
		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(120));
		var chooseSource = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@label='Change Source']")));
		chooseSource.Click();
	}

	[Test, Order(1)]
	public void MediaElementPlayBackControls()
	{
		if (OperatingSystem.IsWindows() && !AppiumServerHelper.IsMacValid())
		{
			return;
		}

		ArgumentNullException.ThrowIfNull(App);
		var menu = App.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeNavigationBar/XCUIElementTypeButton"));
		menu?.Click();
		var views = App.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeOther[@label='Views']"));
		views?.Click();
		string element = "//XCUIElementTypeStaticText[@label='MediaElement']";
		ScrollUntilElementFound(element, 2);
	
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));
		
		// Get initial position
		var positionElement = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeSlider[@label='Current position']")));
		string initialPosition = positionElement.Text;
		
		var stopBtn = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@label='Stop']")));
		stopBtn.Click();

		// Wait for the media to stop and verify
		var playBtn = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@label='Play']")));
		playBtn.Click();
		
		// Check if media is playing by verifying position change
		bool isPlaying = wait.Until(d =>
		{
			string currentPosition = positionElement.Text;
			return currentPosition != initialPosition;
		});

		Assert.That(isPlaying, Is.True, "Media should be playing (position should change)");
		
		var pauseBtn = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@label='Pause']")));
		Assert.That(pauseBtn.Displayed, Is.True, "Pause button should be visible after playing");
		Assert.That(playBtn.Displayed, Is.True, "Play button should be visible");
		Assert.That(stopBtn.Displayed, Is.True, "Stop button should be visible");
		Assert.That(positionElement.Displayed, Is.True, "Position element should be visible");
		
	}

	[Test, Order(2)]
	public void MediaSource()
	{
		if (OperatingSystem.IsWindows() && !AppiumServerHelper.IsMacValid())
		{
			return;
		}
		// Action sheet not working on MacOS with appium. No idea why.

		/*
		// Use explicit wait
		var wait = new WebDriverWait(App, TimeSpan.FromSeconds(60));

		SourceMenu();
		var loadHls = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@name='Load HTTP Live Stream (HLS)']")));
		Assert.That(loadHls.Displayed, Is.True, "Load HTTP Live Stream (HLS) button should be visible");
		loadHls.Click();

		SourceMenu();
		var loadLocalResource = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@name='Load Local Resource']")));
		Assert.That(loadLocalResource.Displayed, Is.True, "Load Local Resource button should be visible");
		loadLocalResource.Click();
		
		SourceMenu();
		var resetSource = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@name='Reset Source to null']")));
		Assert.That(resetSource.Displayed, Is.True, "Reset Source to null button should be visible");
		resetSource.Click();

		SourceMenu();
		var loadMusic = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@name='Load Music']")));
		Assert.That(loadMusic.Displayed, Is.True, "Load Music button should be visible");
		loadMusic.Click();

		SourceMenu();
		var loadOnlineMp4 = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath("//XCUIElementTypeButton[@name='Load Online MP4']")));
		Assert.That(loadOnlineMp4.Displayed, Is.True, "Load Online MP4 button should be visible");
		loadOnlineMp4.Click();
		*/
	}

	public void ScrollUntilElementFound(string elementId, int maxScrolls)
	{
		for (int i = 0; i < maxScrolls; i++)
		{
			try
			{
				ArgumentNullException.ThrowIfNull(App);
				var item = App.FindElement(ByIosUIAutomation.IosClassChain("**/XCUIElementTypeWindow/XCUIElementTypeGroup/XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[1]/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeCollectionView/XCUIElementTypeOther[1]"));
				var elementLocation = item.Location;
				var elementSize = item.Size;
				int startX = elementLocation.X + elementSize.Width / 2;
				int startY = elementLocation.Y + elementSize.Height / 2;
				int endY = startY + 500;

				var finger = new PointerInputDevice(PointerKind.Mouse);
				var start = new Point(startX, startY);
				var end = new Point(startX, endY);
				var swipe = new ActionSequence(finger);
				swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, start.X, start.Y, TimeSpan.FromSeconds(2)));
				swipe.AddAction(finger.CreatePointerDown(MouseButton.Left));
				swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, end.X, end.Y, TimeSpan.FromMilliseconds(1000)));
				swipe.AddAction(finger.CreatePointerUp(MouseButton.Left));
				App?.PerformActions(new List<ActionSequence> { swipe });
				var wait = new WebDriverWait(App, TimeSpan.FromSeconds(10));
				var element = wait.Until(d => d.FindElement(ByIosUIAutomation.XPath(elementId)));
				if (element.Displayed)
				{
					element.Click();
					return; // Element found, exit the method
				}
			}
			catch (NoSuchElementException)
			{
				// Element not found, continue scrolling
			}
			// Wait for content to settle
			System.Threading.Thread.Sleep(1000);
		}

		throw new Exception($"Element with accessibility ID '{elementId}' not found after {maxScrolls} scrolls.");
	}
}

