using Microsoft.UI.Windowing;

namespace CommunityToolkit.Maui.Extensions;

class FullScreenExtension
{
	readonly AppWindow appWindow;
	public FullScreenExtension(AppWindow appWindow)
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
