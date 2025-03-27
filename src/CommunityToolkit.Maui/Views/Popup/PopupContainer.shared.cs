using System.ComponentModel;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace CommunityToolkit.Maui.Views;

sealed partial class PopupContainer<T> : PopupContainer
{
	public PopupContainer(View view, IPopupOptions popupOptions)
		: this(view as Popup<T> ?? CreatePopupFromView<Popup<T>>(view), popupOptions)
	{
	}

	public PopupContainer(Popup<T> popup, IPopupOptions popupOptions)
		: base(popup, popupOptions)
	{
		
	}

	public Task Close(PopupResult<T> result, CancellationToken token = default) => base.Close(result, token);
}

partial class PopupContainer : ContentPage
{
	readonly Popup popup;
	readonly IPopupOptions popupOptions;
	readonly Command tapOutsideOfPopupCommand;
	readonly WeakEventManager popupClosedEventManager = new();

	public PopupContainer(View view, IPopupOptions popupOptions)
		: this(view as Popup ?? CreatePopupFromView<Popup>(view), popupOptions)
	{
		ArgumentNullException.ThrowIfNull(view);

		// Only set the content if overloaded constructor hasn't set the content already; don't override content if it already exists
		base.Content ??= new PopupContainerContent(view, popupOptions);
	}

	public PopupContainer(Popup popup, IPopupOptions popupOptions)
	{
		ArgumentNullException.ThrowIfNull(popup);
		ArgumentNullException.ThrowIfNull(popupOptions);

		this.popup = popup;
		this.popupOptions = popupOptions;

		// Only set the content if overloaded constructor hasn't set the content already; don't override content if it already exists
		base.Content ??= new PopupContainerContent(popup, popupOptions);

		tapOutsideOfPopupCommand = new Command(async () =>
		{
			popupOptions.OnTappingOutsideOfPopup?.Invoke();
			await Close(new PopupResult(true));
		}, () => popupOptions.CanBeDismissedByTappingOutsideOfPopup);

		Content.GestureRecognizers.Add(new TapGestureRecognizer { Command = tapOutsideOfPopupCommand });

		if (popupOptions is BindableObject bindablePopupOptions)
		{
			bindablePopupOptions.PropertyChanged += HandlePopupPropertyChanged;
		}
		
		this.SetBinding(BindingContextProperty, static (Popup x) => x.BindingContext, source: popup, mode: BindingMode.OneWay);
		this.SetBinding(BackgroundColorProperty, static (IPopupOptions options) => options.PageOverlayColor, source: popupOptions, mode: BindingMode.OneWay);

		Shell.SetPresentationMode(this, PresentationMode.ModalNotAnimated);
		On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.OverFullScreen);
	}

	public event EventHandler<IPopupResult> PopupClosed
	{
		add => popupClosedEventManager.AddEventHandler(value);
		remove => popupClosedEventManager.RemoveEventHandler(value);
	}

	// Prevent Content from being set by external class
	// Casts `PopupContainer.Content` to return typeof(PopupContainerContent)
	internal new PopupContainerContent Content => (PopupContainerContent)base.Content;

	public async Task Close(PopupResult result, CancellationToken token = default)
	{
		token.ThrowIfCancellationRequested();
		
		await Navigation.PopModalAsync(false).WaitAsync(token);
		
		popupClosedEventManager.HandleEvent(this, result, nameof(PopupClosed));
	}

	// Prevent the Android Back Button from dismissing the Popup if CanBeDismissedByTappingOutsideOfPopup is true
	protected override bool OnBackButtonPressed()
	{
		if (popupOptions.CanBeDismissedByTappingOutsideOfPopup)
		{
			return base.OnBackButtonPressed();
		}

		return true;
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		popup.NotifyPopupIsClosed();
		base.OnNavigatedFrom(args);
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		popup.NotifyPopupIsOpened();
	}

	protected static T CreatePopupFromView<T>(in View view) where T : Popup, new()
	{
		return new T
		{
			Content = view
		};
	}

	void HandlePopupPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(IPopupOptions.CanBeDismissedByTappingOutsideOfPopup))
		{
			tapOutsideOfPopupCommand.ChangeCanExecute();
		}
	}

	internal sealed partial class PopupContainerContent : Grid
	{
		public PopupContainerContent(in View popupContent, in IPopupOptions options)
		{
			Background = BackgroundColor = null;

			popupContent.VerticalOptions = popupContent.HorizontalOptions = LayoutOptions.Center;

			var border = new Border
			{
				Content = popupContent
			};
			
			border.SetBinding(Border.BackgroundProperty, static (View content) => content.Background, source: popupContent, mode: BindingMode.OneWay);
			border.SetBinding(Border.BackgroundColorProperty, static (View content) => content.BackgroundColor, source: popupContent, mode: BindingMode.OneWay);
			border.SetBinding(Border.VerticalOptionsProperty, static (IPopupOptions options) => options.VerticalOptions, source: options, mode: BindingMode.OneWay);
			border.SetBinding(Border.HorizontalOptionsProperty, static (IPopupOptions options) => options.HorizontalOptions, source: options, mode: BindingMode.OneWay);
			border.SetBinding(Border.StrokeShapeProperty, static (IPopupOptions options) => options.Shape, source: options, mode: BindingMode.OneWay);
			border.SetBinding(Border.StrokeProperty, static (IPopupOptions options) => options.BorderStroke, source: options, mode: BindingMode.OneWay);
			border.SetBinding(Border.MarginProperty, static (IPopupOptions options) => options.Margin, source: options, mode: BindingMode.OneWay);
			border.SetBinding(Border.PaddingProperty, static (IPopupOptions options) => options.Padding, source: options, mode: BindingMode.OneWay);

			Children.Add(border);
			
		}
	}
}