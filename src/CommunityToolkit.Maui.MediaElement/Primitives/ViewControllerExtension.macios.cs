using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AVFoundation;
using AVKit;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CoreGraphics;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Controls.Handlers.Items2;
using Microsoft.Maui.Handlers;
using UIKit;

namespace CommunityToolkit.Maui.Core.Primitives;

static class ViewControllerExtensions
{
    public static UIViewController? GetViewController(AVPlayerViewController playerViewController, MediaElement virtualView)
    {
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

			if (!ViewControllerExtensions.TryGetCurrentPage(out var currentPage))
			{
				throw new InvalidOperationException("Cannot find current page");
			}

			// look for an ItemsView (e.g. CarouselView or CollectionView) on page 
			if (ViewControllerExtensions.TryGetItemsViewOnPage(currentPage, out var itemsView))
			{
				var parentViewController = itemsView.Handler switch
				{
					CarouselViewHandler carouselViewHandler => carouselViewHandler.ViewController ?? GetInternalControllerForItemsView(carouselViewHandler),
					CarouselViewHandler2 carouselViewHandler2 => carouselViewHandler2.ViewController ?? GetInternalControllerForItemsView2(carouselViewHandler2),
					CollectionViewHandler collectionViewHandler => collectionViewHandler.ViewController ?? GetInternalControllerForItemsView(collectionViewHandler),
					CollectionViewHandler2 collectionViewHandler2 => collectionViewHandler2.ViewController ?? GetInternalControllerForItemsView2(collectionViewHandler2),
					null => throw new InvalidOperationException("Handler cannot be null"),
					_ => throw new NotSupportedException($"{itemsView.Handler.GetType()} not yet supported")
				};

				viewController = parentViewController;

				// The Controller we need is a `protected internal` property called ItemsViewController in the ItemsViewHandler class: https://github.com/dotnet/maui/blob/cf002538cb73db4bf187a51e4786d7478a7025ee/src/Controls/src/Core/Handlers/Items/ItemsViewHandler.iOS.cs#L39
				// In this method, we must use reflection to get the value of its backing field 
				static ItemsViewController<TItemsView> GetInternalControllerForItemsView<TItemsView>(ItemsViewHandler<TItemsView> handler) where TItemsView : ItemsView
				{
					var nonPublicInstanceFields = typeof(ItemsViewHandler<TItemsView>).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

					var controllerProperty = nonPublicInstanceFields.Single(x => x.FieldType == typeof(ItemsViewController<TItemsView>));
					return (ItemsViewController<TItemsView>)(controllerProperty.GetValue(handler) ?? throw new InvalidOperationException($"Unable to get the value for the Controller property on {handler.GetType()}"));
				}

				// The Controller we need is a `protected internal` property called ItemsViewController in the ItemsViewHandler2 class: https://github.com/dotnet/maui/blob/70e8ddfd4bd494bc71aa7afb812cc09161cf0c72/src/Controls/src/Core/Handlers/Items2/ItemsViewHandler2.iOS.cs#L64
				// In this method, we must use reflection to get the value of its backing field 
				static ItemsViewController<TItemsView> GetInternalControllerForItemsView2<TItemsView>(ItemsViewHandler2<TItemsView> handler) where TItemsView : ItemsView
				{
					var nonPublicInstanceFields = typeof(ItemsViewHandler2<TItemsView>).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

					var controllerProperty = nonPublicInstanceFields.Single(x => x.FieldType == typeof(ItemsViewController2<TItemsView>));
					return (ItemsViewController<TItemsView>)(controllerProperty.GetValue(handler) ?? throw new InvalidOperationException($"Unable to get the value for the Controller property on {handler.GetType()}"));
				}
			}
			// If we don't find an ItemsView, default to the current UIViewController
			else
			{
				viewController = Platform.GetCurrentUIViewController() ?? throw new InvalidOperationException();
			}
		}


	
        return viewController;
    }
    public static bool TryGetItemsViewOnPage(Page currentPage, [NotNullWhen(true)] out ItemsView? itemsView)
        {
            var itemsViewsOnPage = ((IElementController)currentPage).Descendants().OfType<ItemsView>().ToList();
            switch (itemsViewsOnPage.Count)
            {
                case > 1:
                    // We are unable to determine which ItemsView contains the MediaElement when multiple ItemsView are being used in the same page
                    // TODO: Add support for MediaElement in an ItemsView on a Page containing multiple ItemsViews 
                    throw new NotSupportedException("MediaElement does not currently support pages containing multiple ItemsViews (eg multiple CarouselViews + CollectionViews)");
                case 1:
                    itemsView = itemsViewsOnPage[0];
                    return true;
                case <= 0:
                    itemsView = null;
                    return false;
            }
        }
        
        static bool TryGetCurrentPage([NotNullWhen(true)] out Page? currentPage)
        {
            currentPage = null;

            if (Application.Current?.Windows is null)
            {
                return false;
            }

            if (Application.Current.Windows.Count is 0)
            {
                throw new InvalidOperationException("Unable to find active Window");
            }

            if (Application.Current.Windows.Count > 1)
            {
                // We are unable to determine which Window contains the ItemsView that contains the MediaElement when multiple ItemsView are being used in the same page
                // TODO: Add support for MediaElement in an ItemsView in a multi-window application
                throw new NotSupportedException("MediaElement is not currently supported in multi-window applications");
            }

            var window = Application.Current.Windows[0];

            // If using Shell, return the current page
            if (window.Page is Shell { CurrentPage: not null } shell)
            {
                currentPage = shell.CurrentPage;
                return true;
            }

            // If not using Shell, use the ModelNavigationStack to check for any pages displayed modally
            if (TryGetModalPage(window, out var modalPage))
            {
                currentPage = modalPage;
                return true;
            }

            // If not using Shell or a Modal Page, return the visible page in the (non-modal) NavigationStack
            if (window.Navigation.NavigationStack[^1] is Page page)
            {
                currentPage = page;
                return true;
            }

            return false;
        }
        static bool TryGetModalPage(Window window, [NotNullWhen(true)] out Page? page)
        {
            page = window.Navigation.ModalStack.LastOrDefault();
            return page is not null;
        }
}