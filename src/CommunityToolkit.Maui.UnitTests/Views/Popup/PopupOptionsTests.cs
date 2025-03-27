﻿using Microsoft.Maui.Controls.Shapes;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Views;

public class PopupOptionsTests : BaseTest
{
	[Fact]
	public void CanBeDismissedByTappingOutsideOfPopup_DefaultValue_ShouldBeTrue()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.CanBeDismissedByTappingOutsideOfPopup, popupOptions.CanBeDismissedByTappingOutsideOfPopup);
	}

	[Fact]
	public void CanBeDismissedByTappingOutsideOfPopup_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		popupOptions.CanBeDismissedByTappingOutsideOfPopup = false;
		Assert.False(popupOptions.CanBeDismissedByTappingOutsideOfPopup);
	}
	
	[Fact]
	public void PopupBackgroundColor_DefaultValue_ShouldBeWhite()
	{
		Assert.Equal(PopupOptionsDefaults.PopupBackgroundColor, Colors.White);
	}
	
	[Fact]
	public void PageOverlayColor_DefaultValue_ShouldBeDefaultColor()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.PageOverlayColor, popupOptions.PageOverlayColor);
	}

	[Fact]
	public void PageOverlayColor_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		var color = Colors.Red;
		popupOptions.PageOverlayColor = color;
		Assert.Equal(color, popupOptions.PageOverlayColor);
	}
	
	[Fact]
	public void BorderStroke_DefaultValue_ShouldBeDefaultStroke()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.BorderStroke, popupOptions.BorderStroke);
	}

	[Fact]
	public void BorderStroke_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		var color = Colors.Red;
		popupOptions.BorderStroke = color;
		Assert.Equal(color, popupOptions.BorderStroke);
	}

	[Fact]
	public void OnTappingOutsideOfPopup_DefaultValue_ShouldBeNull()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.OnTappingOutsideOfPopup, popupOptions.OnTappingOutsideOfPopup);
	}

	[Fact]
	public void OnTappingOutsideOfPopup_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		Action action = () => { };
		popupOptions.OnTappingOutsideOfPopup = action;
		Assert.Equal(action, popupOptions.OnTappingOutsideOfPopup);
	}

	[Fact]
	public void Shape_DefaultValue_ShouldBeNull()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.Shape, popupOptions.Shape);
	}

	[Fact]
	public void Shape_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		IShape shape = new Rectangle();
		popupOptions.Shape = shape;
		Assert.Equal(shape, popupOptions.Shape);
	}

	[Fact]
	public void Margin_DefaultValue_ShouldBeDefaultThickness()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.Margin, popupOptions.Margin);
	}

	[Fact]
	public void Margin_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		var thickness = new Thickness(10);
		popupOptions.Margin = thickness;
		Assert.Equal(thickness, popupOptions.Margin);
	}

	[Fact]
	public void Padding_DefaultValue_ShouldBeDefaultThickness()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.Padding, popupOptions.Padding);
	}

	[Fact]
	public void Padding_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		var thickness = new Thickness(10);
		popupOptions.Padding = thickness;
		Assert.Equal(thickness, popupOptions.Padding);
	}

	[Fact]
	public void VerticalOptions_DefaultValue_ShouldBeDefaultLayoutOptions()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.VerticalOptions, popupOptions.VerticalOptions);
	}

	[Fact]
	public void VerticalOptions_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		var layoutOptions = LayoutOptions.Center;
		popupOptions.VerticalOptions = layoutOptions;
		Assert.Equal(layoutOptions, popupOptions.VerticalOptions);
	}

	[Fact]
	public void HorizontalOptions_DefaultValue_ShouldBeDefaultLayoutOptions()
	{
		var popupOptions = new PopupOptions();
		Assert.Equal(PopupOptionsDefaults.HorizontalOptions, popupOptions.HorizontalOptions);
	}

	[Fact]
	public void HorizontalOptions_SetValue_ShouldBeUpdated()
	{
		var popupOptions = new PopupOptions();
		var layoutOptions = LayoutOptions.Center;
		popupOptions.HorizontalOptions = layoutOptions;
		Assert.Equal(layoutOptions, popupOptions.HorizontalOptions);
	}

	[Fact]
	public void CanBeDismissedByTappingOutsideOfPopup_ShouldReturnTrue()
	{
		var options = new MockPopupOptions { CanBeDismissedByTappingOutsideOfPopup = true };
		Assert.True(options.CanBeDismissedByTappingOutsideOfPopup);
	}

	[Fact]
	public void CanBeDismissedByTappingOutsideOfPopup_ShouldReturnFalse()
	{
		var options = new MockPopupOptions { CanBeDismissedByTappingOutsideOfPopup = false };
		Assert.False(options.CanBeDismissedByTappingOutsideOfPopup);
	}
	
	[Fact]
	public void BorderStroke_ShouldReturnCorrectColor()
	{
		var color = Colors.Red;
		var options = new MockPopupOptions { BorderStroke = color };
		Assert.Equal(color, options.BorderStroke);
	}
	
	[Fact]
	public void PageOverylayColor_ShouldReturnCorrectColor()
	{
		var color = Colors.Red;
		var options = new MockPopupOptions { PageOverlayColor = color };
		Assert.Equal(color, options.PageOverlayColor);
	}

	[Fact]
	public void OnTappingOutsideOfPopup_ShouldInvokeAction()
	{
		bool actionInvoked = false;
		var options = new MockPopupOptions { OnTappingOutsideOfPopup = () => actionInvoked = true };
		options.OnTappingOutsideOfPopup?.Invoke();
		Assert.True(actionInvoked);
	}

	[Fact]
	public void Shape_ShouldReturnCorrectShape()
	{
		var shape = new Rectangle();
		var options = new MockPopupOptions { Shape = shape };
		Assert.Equal(shape, options.Shape);
	}

	[Fact]
	public void Margin_ShouldReturnCorrectThickness()
	{
		var margin = new Thickness(10);
		var options = new MockPopupOptions { Margin = margin };
		Assert.Equal(margin, options.Margin);
	}

	[Fact]
	public void Padding_ShouldReturnCorrectThickness()
	{
		var padding = new Thickness(5);
		var options = new MockPopupOptions { Padding = padding };
		Assert.Equal(padding, options.Padding);
	}

	[Fact]
	public void VerticalOptions_ShouldReturnCorrectLayoutOptions()
	{
		var verticalOptions = LayoutOptions.Center;
		var options = new MockPopupOptions { VerticalOptions = verticalOptions };
		Assert.Equal(verticalOptions, options.VerticalOptions);
	}

	[Fact]
	public void HorizontalOptions_ShouldReturnCorrectLayoutOptions()
	{
		var horizontalOptions = LayoutOptions.End;
		var options = new MockPopupOptions { HorizontalOptions = horizontalOptions };
		Assert.Equal(horizontalOptions, options.HorizontalOptions);
	}

	sealed class MockPopupOptions : IPopupOptions
	{
		public bool CanBeDismissedByTappingOutsideOfPopup { get; set; }
		public Color PageOverlayColor { get; set; } = Colors.Transparent;
		public Action? OnTappingOutsideOfPopup { get; set; }
		public IShape? Shape { get; set; }
		public Brush? BorderStroke { get; set; } = null;
		public Thickness Margin { get; set; }
		public Thickness Padding { get; set; }
		public LayoutOptions VerticalOptions { get; set; }
		public LayoutOptions HorizontalOptions { get; set; }
	}
}
