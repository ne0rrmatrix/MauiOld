﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Maui.UnitTests.Mocks;
using CommunityToolkit.Maui.UnitTests.Services;
using FluentAssertions;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests;

public abstract class BaseHandlerTest : BaseTest
{
	protected BaseHandlerTest()
	{
		InitializeServicesAndSetMockApplication(out var serviceProvider);
		ServiceProvider = serviceProvider;
	}

	protected IServiceProvider ServiceProvider { get; }

	protected override async ValueTask DisposeAsyncCore()
	{
		await base.DisposeAsyncCore();
		
		#region Cleanup Popup Tests

		Application.Current.Should().NotBeNull();
		var navigation = Application.Current.Windows[0].Page?.Navigation ?? throw new InvalidOperationException("Unable to locate Navigation Stack");

		while (navigation.ModalStack.Any())
		{
			await navigation.PopModalAsync();
		}
		#endregion
	}

	protected static TElementHandler CreateElementHandler<TElementHandler>(IElement view, bool doesRequireMauiContext = true)
		where TElementHandler : IElementHandler, new()
	{
		var mockElementHandler = new TElementHandler();
		mockElementHandler.SetVirtualView(view);

		if (doesRequireMauiContext)
		{
			mockElementHandler.SetMauiContext(Application.Current?.Handler?.MauiContext ?? throw new NullReferenceException());
		}

		return mockElementHandler;
	}

	protected static TViewHandler CreateViewHandler<TViewHandler>(IView view, bool doesRequireMauiContext = true)
		where TViewHandler : IViewHandler, new()
	{
		var mockViewHandler = new TViewHandler();
		mockViewHandler.SetVirtualView(view);

		if (doesRequireMauiContext)
		{
			mockViewHandler.SetMauiContext(Application.Current?.Handler?.MauiContext ?? throw new NullReferenceException());
		}

		return mockViewHandler;
	}

	static void InitializeServicesAndSetMockApplication(out IServiceProvider serviceProvider)
	{
#pragma warning disable CA1416 // Validate platform compatibility
		var appBuilder = MauiApp.CreateBuilder()
			.UseMauiCommunityToolkit()
			.UseMauiApp<MockApplication>();
#pragma warning restore CA1416 // Validate platform compatibility

		#region Register Services for CameraTests

		appBuilder.Services.AddSingleton<ICameraProvider, MockCameraProvider>();

		#endregion

		#region Register Services for PopupServiceTests

		var mockPageViewModel = new MockPageViewModel();
		var mockPopup = new MockSelfClosingPopup(mockPageViewModel, new());

		var page = new ContentPage();
		PopupService.AddPopup(mockPopup, mockPageViewModel, appBuilder.Services, ServiceLifetime.Transient);
		#endregion

		var mauiApp = appBuilder.Build();

		var application = (MockApplication)mauiApp.Services.GetRequiredService<IApplication>();
		application.AddWindow(new Window { Page = page });
		serviceProvider = mauiApp.Services;

		IPlatformApplication.Current = application;

		application.Handler = new ApplicationHandlerStub();
		application.Handler.SetMauiContext(new HandlersContextStub(serviceProvider));

		CreateViewHandler<MockPageHandler>(page);
	}
}