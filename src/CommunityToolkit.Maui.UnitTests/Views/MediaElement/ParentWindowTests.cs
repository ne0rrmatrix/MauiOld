using Xunit;
using CommunityToolkit.Maui.Extensions;
using ParentWindow = CommunityToolkit.Maui.Extensions.PageExtensions.ParentWindow;

namespace CommunityToolkit.Maui.UnitTests.Views;


public class ParentWindowTests
    {
        [Fact]
        public void Exists_WhenParentWindowIsNull_ReturnsFalse()
        {
            Application.Current = new Application();
            Application.Current.MainPage = new ContentPage();

            Assert.False(ParentWindow.Exists);
        }

        [Fact]
        public void Exists_WhenParentWindowHandlerIsNull_ReturnsFalse()
        {
            var mockWindow = new Window();
            var mockPage = new ContentPage();
            mockWindow.Page = mockPage;
            Application.Current = new Application();
            Application.Current.MainPage = mockPage;

            Assert.False(ParentWindow.Exists);
        }

        [Fact]
        public void Exists_WhenParentWindowHandlerPlatformViewIsNull_ReturnsFalse()
        {
            var mockWindow = new Window();
            var mockPage = new ContentPage();
            mockWindow.Page = mockPage;
            Application.Current = new Application();
            Application.Current.MainPage = mockPage;

            // Simulate a scenario where the handler is set but the platform view is null
            mockWindow.Handler = new MockWindowHandler();

            Assert.False(ParentWindow.Exists);
        }

        [Fact]
        public void Exists_WhenAllConditionsAreMet_ReturnsTrue()
        {
            var mockWindow = new Window();
            var mockPage = new ContentPage();
            mockWindow.Page = mockPage;
            Application.Current = new Application();
            Application.Current.MainPage = mockPage;

            // Simulate a scenario where all conditions are met
            mockWindow.Handler = new MockWindowHandler { PlatformView = new object() };

            Assert.True(ParentWindow.Exists);
        }

        class MockWindowHandler : IElementHandler
        {
            public object? PlatformView { get; set; }
            public IElement? VirtualView { get; set; }

		public IMauiContext? MauiContext => new object() as IMauiContext;

		public void DisconnectHandler() { }

		public void Invoke(string command, object? args = null)
		{
			// No-op
		}

		public void SetMauiContext(IMauiContext mauiContext)
		{
			// No-op
		}

		public void SetVirtualView(IElement view) { }
            public void UpdateValue(string property) { }
        }
    }
