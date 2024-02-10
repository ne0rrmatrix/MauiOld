﻿using Microsoft.UI.Windowing;

namespace CommunityToolkit.Maui.Views;
class CustomBindings
{
	readonly AppWindow appWindow;
	public CustomBindings(AppWindow appWindow)
	{
		this.appWindow = appWindow;
	}

	public void SetFullScreen()
	{
		if (appWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
		{
			appWindow.SetPresenter(AppWindowPresenterKind.Default);
		}
		else
		{
			appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
		}
	}
}
