﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CommunityToolkit.Maui.Sample.ViewModels.Views;

public sealed partial class CsharpBindingPopupViewModel : BaseViewModel
{
	[ObservableProperty]
	public partial string Title { get; set; } = "C# Binding Popup";

	[ObservableProperty]
	public partial string Message { get; set; } = "This message uses a ViewModel binding";

	public TaskCompletionSource<IPopupResult>? PopupResultManager { get; set; }
}