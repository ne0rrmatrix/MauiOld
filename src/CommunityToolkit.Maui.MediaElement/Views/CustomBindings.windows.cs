using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using Thickness = Microsoft.UI.Xaml.Thickness;
using Binding = Microsoft.UI.Xaml.Data.Binding;
using BindingMode = Microsoft.UI.Xaml.Data.BindingMode;
using Visibility = Microsoft.UI.Xaml.Visibility;
using WindowsMediaElement = Windows.Media.Playback.MediaPlayer;
using CommunityToolkit.Maui.Extensions;
using Page = Microsoft.Maui.Controls.Page;
using Application = Microsoft.Maui.Controls.Application;
using Microsoft.Maui.Platform;

namespace CommunityToolkit.Maui.Views;
class CustomBindings
{
	readonly AppWindow appWindow;
	public List<ElementData> Element { get; set; } = new();
	public CustomBindings(AppWindow appWindow)
	{
		this.appWindow = appWindow;
	}

	/// <summary>
	/// Gets the presented page.
	/// </summary>
	protected Page CurrentPage =>
		PageExtensions.GetCurrentPage(Application.Current?.MainPage ?? throw new InvalidOperationException($"{nameof(Application.Current.MainPage)} cannot be null."));

	public void SetFullScreen(FrameworkElement? element)
	{
		var currentPage = CurrentPage;
		if (Element is null || element is null)
		{
			return;
		}
		if (appWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
		{
			appWindow.SetPresenter(AppWindowPresenterKind.Default);
			foreach(var x in Element)
			{
				if(x.Element is not null)
				{
					SetItemSize(x.Element, x.Width, x.Height, x.Thickness, true);
				}
			}
			Element.Clear();
		}
		else
		{
			appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);

			GetAllElements(element);
			foreach(var x in Element)
			{
				if (x.Element is not null && x.Element.GetType() != typeof(MediaPlayerElement))
				{
						SetItemSize(x.Element, double.NaN, double.NaN, new Thickness(0, 0, 0, 0), false);
				}
			}
		}
	}
	static bool MediaElementIsDescendant(FrameworkElement? element)
	{
		if (element is null)
		{
			Debug.WriteLine("Exiting recursion");
			return false;
		}
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
		{
			var child = VisualTreeHelper.GetChild(element, i) ?? null;
			if (child is null)
			{
				return false;
			}
			var type = child.GetType();
			if (type == typeof(MediaPlayerElement))
			{
				return true;
			}
			var item = MediaElementIsDescendant(child as FrameworkElement);
			if (item)
			{
				return true;
			}
		}
		return false;
	}
	static void SetAllChildrenVisibility(FrameworkElement? view, bool visible)
	{
		view = view?.Parent as FrameworkElement ?? null;
		if (view is null)
		{
			return;
		}

		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(view); i++)
		{
			var child = VisualTreeHelper.GetChild(view, i);
			if (child is not FrameworkElement item)
			{
				return;
			}
			if (visible)
			{
				item.Visibility = Visibility.Visible;
			}
			else if (item.GetType() != typeof(MediaPlayerElement) && !MediaElementIsDescendant(item))
			{
				item.Visibility = Visibility.Collapsed;
			}
		}
	}
	void GetAllElements(FrameworkElement? element)
	{
		if (element is null)
		{
			return;
		}
		ElementData temp = new()
		{
			Element = element,
			Thickness = new Thickness(element.Margin.Left, element.Margin.Top, element.Margin.Right, element.Margin.Bottom),
			Width = GetWidth(element),
			Height = GetHeight(element)
		};
		Element.Add(temp);
		while (true)
		{
			ElementData item = new();
			element = element.Parent as FrameworkElement ?? null;
			if (element is null)
			{
				return;
			}
			item.Element = element;
			item.Thickness = new Thickness(element.Margin.Left, element.Margin.Top, element.Margin.Right, element.Margin.Bottom);
			item.Width = GetWidth(element);
			item.Height = GetHeight(element);
			Element.Add(item);
		}
	}
	static double GetWidth(FrameworkElement frameworkElement)
	{
		return frameworkElement.ActualWidth;
	}
	static double GetHeight(FrameworkElement frameworkElement)
	{
		return frameworkElement.ActualHeight;
	}
	static void BindWidth(FrameworkElement bindMe, FrameworkElement toMe)
	{
		Binding b = new()
		{
			Mode = BindingMode.OneWay,
			Source = toMe.Width
		};
		bindMe.SetBinding(FrameworkElement.WidthProperty, b);
	}
	static void BindHeight(FrameworkElement bindMe, FrameworkElement toMe)
	{
		Binding b = new()
		{
			Mode = BindingMode.OneWay,
			Source = toMe.Height
		};
		bindMe.SetBinding(FrameworkElement.HeightProperty, b);
	}
	static void SetItems(FrameworkElement frameworkElement, double width, double height)
	{
		Binding bWidth = new()
		{
			Mode = BindingMode.OneWay,
			Source = width
		};
		Binding bHeight = new()
		{
			Mode = BindingMode.OneWay,
			Source = height
		};
		frameworkElement.SetBinding(FrameworkElement.WidthProperty, bWidth);
		frameworkElement.SetBinding(FrameworkElement.HeightProperty, bHeight);
	}
	static void SetItemSize(FrameworkElement frameworkElement, double width, double height, Thickness thickness, bool visibility)
	{
		SetItems(frameworkElement, width, height);
		BindWidth(frameworkElement, frameworkElement);
		BindHeight(frameworkElement, frameworkElement);
		frameworkElement.Margin = new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
		SetAllChildrenVisibility(frameworkElement, visibility);
	}
}
