﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Sample.Views.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace CommunityToolkit.Maui.Sample.ViewModels.Views;

public sealed partial class PopupAnchorViewModel : BaseViewModel
{
	static Page MainPage => Application.Current?.Windows[0].Page ?? throw new InvalidOperationException("MainPage cannot be null");

	[RelayCommand]
	static async Task ShowPopup(View anchor)
	{
		var popup = new TransparentPopup
		{
			
		};
		await MainPage.ShowPopup<TransparentPopup>(popup, new PopupOptions());
	}
}