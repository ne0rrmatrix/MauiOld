﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Sample.Views.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace CommunityToolkit.Maui.Sample.ViewModels.Views;

public partial class StylePopupViewModel : BaseViewModel
{
	static Page MainPage => Shell.Current;

	[RelayCommand]
	static async Task DisplayImplicitStylePopup()
	{
		var popup = new ImplicitStylePopup();
		await MainPage.ShowPopup<ImplicitStylePopup>(popup, new PopupOptions());
	}

	[RelayCommand]
	static async Task DisplayExplicitStylePopup()
	{
		var popup = new ExplicitStylePopup();
		await MainPage.ShowPopup<ExplicitStylePopup>(popup, new PopupOptions());
	}

	[RelayCommand]
	static async Task DisplayDynamicStylePopup()
	{
		var popup = new DynamicStylePopup();
		await MainPage.ShowPopup<DynamicStylePopup>(popup, new PopupOptions());
	}

	[RelayCommand]
	static async Task DisplayApplyToDerivedTypesPopup()
	{
		var popup = new ApplyToDerivedTypesPopup();
		await MainPage.Navigation.ShowPopup<ApplyToDerivedTypesPopup>(popup, new PopupOptions());
	}

	[RelayCommand]
	static async Task DisplayStyleInheritancePopup()
	{
		var popup = new StyleInheritancePopup();
		await MainPage.ShowPopup<StyleInheritancePopup>(popup, new PopupOptions());
	}

	[RelayCommand]
	static async Task DisplayDynamicStyleInheritancePopup()
	{
		var popup = new DynamicStyleInheritancePopup();
		await MainPage.ShowPopup<DynamicStyleInheritancePopup>(popup, new PopupOptions());
	}

	[RelayCommand]
	static async Task DisplayStyleClassPopup()
	{
		var popup = new StyleClassPopup();
		await MainPage.ShowPopup<StyleClassPopup>(popup, new PopupOptions());
	}
}