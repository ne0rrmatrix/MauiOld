using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AVKit;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Handlers;
using UIKit;

namespace CommunityToolkit.Maui.Core.Views;

/// <summary>
/// The user-interface element that represents the <see cref="MediaElement"/> on iOS and macOS.
/// </summary>
public class MauiMediaElement : UIView
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MauiMediaElement"/> class.
	/// </summary>
	/// <param name="playerViewController">The <see cref="AVPlayerViewController"/> that acts as the platform media player.</param>
	/// <param name="virtualView">The <see cref="MediaElement"/> used as the VirtualView for this <see cref="MauiMediaElement"/>.</param>
	/// <exception cref="NullReferenceException">Thrown when <paramref name="playerViewController"/><c>.View</c> is <see langword="null"/>.</exception>
	public MauiMediaElement(AVPlayerViewController playerViewController, MediaElement virtualView)
	{
		ArgumentNullException.ThrowIfNull(playerViewController.View);
		playerViewController.View.Frame = Bounds;
#if IOS16_0_OR_GREATER || MACCATALYST16_1_OR_GREATER
		// On iOS 16+ and macOS 13+ the AVPlayerViewController has to be added to a parent ViewController, otherwise the transport controls won't be displayed.

		UIViewController? viewController;

		// If any of the Parents in the VisualTree of MediaElement uses a UIViewController for their PlatformView, use it as the child ViewController
		// This enables support for UI controls like CommunityToolkit.Maui.Popup whose PlatformView is a UIViewController (e.g. `public class MauiPopup : UIViewController`)
		// To find the UIViewController, we traverse `MediaElement.Parent` until a Parent using UIViewController is located
		if (virtualView.TryFindParentPlatformView(out UIViewController? parentUIViewController))
		{
			viewController = parentUIViewController;
		}
		// If none of the Parents in the VisualTree of MediaElement use a UIViewController, we can use the ViewController in the PageHandler
		// To find the PageHandler, we traverse `MediaElement.Parent` until the Page is located
		else if (virtualView.TryFindParent<Page>(out var page)
			&& page.Handler is PageHandler { ViewController: not null } pageHandler)
		{
			viewController = pageHandler.ViewController;
		}
		// If the parent Page cannot be found, MediaElement is being used inside a DataTemplate. I.e. The MediaElement is being used inside a CarouselView or a CollectionView
		// The top-most parent is null when MediaElement is placed in a DataTemplate because DataTemplates defer loading until they are about to be displayed on the screen 
		// When the MediaElement is used inside a DataTemplate, we must retrieve its CarouselViewHandler / CollectionViewHandler
		// To retrieve its CarouselViewHandler / CollectionViewHandler, we must traverse all VisualElements on the current page
		else
		{
			ArgumentNullException.ThrowIfNull(virtualView);

			if (!TryGetCurrentPage(out var currentPage))
			{
				throw new InvalidOperationException("Cannot find current page");
			}

			// look for an ItemsView (e.g. CarouselView or CollectionView) on page 
			if (TryGetItemsViewOnPage(currentPage, out var itemsView))
			{
				var parentViewController = itemsView.Handler switch
				{
					CarouselViewHandler carouselViewHandler => carouselViewHandler.ViewController ?? GetInternalController(carouselViewHandler),
					CollectionViewHandler collectionViewHandler => collectionViewHandler.ViewController ?? GetInternalController(collectionViewHandler),
					null => throw new InvalidOperationException("Handler cannot be null"),
					_ => throw new NotSupportedException($"{itemsView.Handler.GetType()} not yet supported")
				};

				viewController = parentViewController;

				// The Controller we need is a `protected internal` property in the ItemsViewController class: https://github.com/dotnet/maui/blob/cf002538cb73db4bf187a51e4786d7478a7025ee/src/Controls/src/Core/Handlers/Items/ItemsViewHandler.iOS.cs#L39
				// In this method, we must use reflection to get the value of its backing field 
				static ItemsViewController<TItemsView> GetInternalController<TItemsView>(ItemsViewHandler<TItemsView> handler) where TItemsView : ItemsView
				{
					var nonPublicInstanceFields = typeof(ItemsViewHandler<TItemsView>).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

					var controllerProperty = nonPublicInstanceFields.Single(x => x.FieldType == typeof(ItemsViewController<TItemsView>));
					return (ItemsViewController<TItemsView>)(controllerProperty.GetValue(handler) ?? throw new InvalidOperationException($"Unable to get the value for the Controller property on {handler.GetType()}"));
				}
			}
			// If we don't find an ItemsView, default to the current UIViewController
			else
			{
				viewController = Platform.GetCurrentUIViewController();
			}
		}


		if (viewController?.View is not null)
		{
			// Zero out the safe area insets of the AVPlayerViewController
			UIEdgeInsets insets = viewController.View.SafeAreaInsets;
			playerViewController.AdditionalSafeAreaInsets =
				new UIEdgeInsets(insets.Top * -1, insets.Left, insets.Bottom * -1, insets.Right);

			// Add the View from the AVPlayerViewController to the parent ViewController
			viewController.AddChildViewController(playerViewController);
		}
#endif
		AddSubview(playerViewController.View);
	}

	static bool TryGetItemsViewOnPage(Page currentPage, [NotNullWhen(true)] out ItemsView? itemsView)
	{
		itemsView = null;

		if (currentPage == null)
		{
			return false;
		}

		var itemsViewsOnPage = ((IElementController)currentPage).LogicalChildren.OfType<ItemsView>().ToList();

		if (itemsViewsOnPage.Count == 1)
		{
			itemsView = itemsViewsOnPage[0];
			return true;
		}

		if (itemsViewsOnPage.Count > 1)
		{
			// Attempt to find the ItemsView that contains the MediaElement
			foreach (var view in itemsViewsOnPage)
			{
				if (ContainsMediaElement(view, out var mediaElement))
				{
					itemsView = view;
					return true;
				}
			}

			// If we cannot determine which ItemsView contains the MediaElement, return false
			return false;
		}

		return false;
	}

	static bool ContainsMediaElement(Element element, [NotNullWhen(true)] out MediaElement? mediaElement)
	{
		mediaElement = null;

		if (element is MediaElement media)
		{
			mediaElement = media;
			return true;
		}

		foreach (var child in ((IElementController)element).LogicalChildren)
		{
			if (ContainsMediaElement(child, out mediaElement))
			{
				return true;
			}
		}

		return false;
	}


	static bool TryGetCurrentPage([NotNullWhen(true)] out Page? currentPage)
	{
		currentPage = null;

		if (Application.Current?.Windows is null || Application.Current.Windows.Count == 0)
		{
			return false;
		}

		if (Application.Current.Windows.Count > 1)
		{
			// Attempt to find the window that contains the MediaElement
			foreach (var currentWindow in Application.Current.Windows)
			{
				if (currentWindow.Page is not null && ContainsMediaElement(currentWindow.Page, out _))
				{
					currentPage = currentWindow.Page;
					return true;
				}
			}

			// If we cannot determine which Window contains the MediaElement, return false
			return false;
		}


		var window = Application.Current.Windows[0];

		// If using Shell, return the current page
		if (window.Page is Shell { CurrentPage: not null } shell)
		{
			currentPage = shell.CurrentPage;
			return true;
		}

		// If not using Shell, use the ModalStack to check for any pages displayed modally
		if (TryGetModalPage(window, out var modalPage))
		{
			currentPage = modalPage;
			return true;
		}

		// If not using Shell or a Modal Page, return the visible page in the (non-modal) NavigationStack
		if (window.Navigation?.NavigationStack?.LastOrDefault() is Page page)
		{
			currentPage = page;
			return true;
		}

		return false;

		static bool TryGetModalPage(Window window, [NotNullWhen(true)] out Page? page)
		{
			page = window.Navigation?.ModalStack?.LastOrDefault();
			return page is not null;
		}
	}
}