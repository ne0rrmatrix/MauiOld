﻿using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui;

/// <summary>
/// Popup options.
/// </summary>
public partial class PopupOptions : BindableObject, IPopupOptions
{
	/// <summary>
	///  Backing BindableProperty for the <see cref="CanBeDismissedByTappingOutsideOfPopup"/> property.
	/// </summary>
	public static readonly BindableProperty CanBeDismissedByTappingOutsideOfPopupProperty = BindableProperty.Create(nameof(CanBeDismissedByTappingOutsideOfPopup), typeof(bool), typeof(PopupOptions), PopupOptionsDefaults.CanBeDismissedByTappingOutsideOfPopup);

	/// <summary>
	///  Backing BindableProperty for the <see cref="OnTappingOutsideOfPopup"/> property.
	/// </summary>
	public static readonly BindableProperty OnTappingOutsideOfPopupProperty = BindableProperty.Create(nameof(OnTappingOutsideOfPopup), typeof(Action), typeof(PopupOptions), PopupOptionsDefaults.OnTappingOutsideOfPopup);

	/// <summary>
	///  Backing BindableProperty for the <see cref="BackgroundColor"/> property.
	/// </summary>
	public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(PopupOptions), PopupOptionsDefaults.BackgroundColor);

	/// <summary>
	///  Backing BindableProperty for the <see cref="Shape"/> property.
	/// </summary>
	public static readonly BindableProperty ShapeProperty = BindableProperty.Create(nameof(Shape), typeof(IShape), typeof(PopupOptions), PopupOptionsDefaults.Shape);

	/// <summary>
	///  Backing BindableProperty for the <see cref="Margin"/> property.
	/// </summary>
	public static readonly BindableProperty MarginProperty = BindableProperty.Create(nameof(Margin), typeof(Thickness), typeof(PopupOptions), PopupOptionsDefaults.Margin);

	/// <summary>
	///  Backing BindableProperty for the <see cref="Padding"/> property.
	/// </summary>
	public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(PopupOptions), PopupOptionsDefaults.Padding);

	/// <summary>
	///  Backing BindableProperty for the <see cref="VerticalOptions"/> property.
	/// </summary>
	public static readonly BindableProperty VerticalOptionsProperty = BindableProperty.Create(nameof(VerticalOptions), typeof(LayoutOptions), typeof(PopupOptions), ConvertToLayoutOptions(PopupOptionsDefaults.VerticalOptions));

	/// <summary>
	///  Backing BindableProperty for the <see cref="HorizontalOptions"/> property.
	/// </summary>
	public static readonly BindableProperty HorizontalOptionsProperty = BindableProperty.Create(nameof(HorizontalOptions), typeof(LayoutOptions), typeof(PopupOptions), ConvertToLayoutOptions(PopupOptionsDefaults.HorizontalOptions));

	/// <summary>
	/// An empty instance of <see cref="IPopupOptions"/> containing default values.
	/// </summary>
	public static IPopupOptions Empty { get; } = new PopupOptions();

	/// <inheritdoc/>
	/// <remarks>
	/// When true and the user taps outside the popup, it will dismiss.
	/// On Android - when false the hardware back button is disabled.
	/// </remarks>
	public bool CanBeDismissedByTappingOutsideOfPopup
	{
		get => (bool)GetValue(CanBeDismissedByTappingOutsideOfPopupProperty);
		set => SetValue(CanBeDismissedByTappingOutsideOfPopupProperty, value);
	}

	/// <inheritdoc/>
	public Color BackgroundColor
	{
		get => (Color)GetValue(BackgroundColorProperty);
		set => SetValue(BackgroundColorProperty, value);
	}

	/// <inheritdoc/>
	public Action? OnTappingOutsideOfPopup
	{
		get => (Action?)GetValue(OnTappingOutsideOfPopupProperty);
		set => SetValue(OnTappingOutsideOfPopupProperty, value);
	}

	/// <inheritdoc/>
	public IShape? Shape
	{
		get => (IShape?)GetValue(ShapeProperty);
		set => SetValue(ShapeProperty, value);
	}

	/// <summary>
	/// Popup margin.
	/// </summary>
	public Thickness Margin
	{
		get => (Thickness)GetValue(MarginProperty);
		set => SetValue(MarginProperty, value);
	}

	/// <inheritdoc/>
	public Thickness Padding
	{
		get => (Thickness)GetValue(PaddingProperty);
		set => SetValue(PaddingProperty, value);
	}

	/// <inheritdoc/>
	public LayoutOptions VerticalOptions
	{
		get => (LayoutOptions)GetValue(VerticalOptionsProperty);
		set => SetValue(VerticalOptionsProperty, value);
	}

	/// <inheritdoc/>
	public LayoutOptions HorizontalOptions
	{
		get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
		set => SetValue(HorizontalOptionsProperty, value);
	}

	static LayoutOptions ConvertToLayoutOptions(Microsoft.Maui.Primitives.LayoutAlignment layoutAlignment) => layoutAlignment switch
	{
		Microsoft.Maui.Primitives.LayoutAlignment.Start => LayoutOptions.Start,
		Microsoft.Maui.Primitives.LayoutAlignment.Center => LayoutOptions.Center,
		Microsoft.Maui.Primitives.LayoutAlignment.End => LayoutOptions.End,
		Microsoft.Maui.Primitives.LayoutAlignment.Fill => LayoutOptions.Fill,
		_ => throw new NotSupportedException($"{nameof(ConvertToLayoutOptions)} does not yet support {layoutAlignment}"),
	};
}