using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;

namespace CommunityToolkit.Maui.Views;

/// <summary>
/// Represents a small View that pops up at front the Page.
/// </summary>
public partial class Popup : ContentView
{
	/// <remarks>Use <see cref="PopupOptions.MarginProperty"/> to set the <see cref="MarginProperty"/> for <see cref="Popup"/></remarks>
	[Obsolete($"Use {nameof(PopupOptions)} to bind to the MarginProperty of {nameof(Popup)}", true)]
	public static new readonly BindableProperty MarginProperty = View.MarginProperty;

	/// <remarks>Use <see cref="PopupOptions.PaddingProperty"/> to set the <see cref="PaddingProperty"/> for <see cref="Popup"/></remarks>
	[Obsolete($"Use {nameof(PopupOptions)} to bind to the PaddingProperty of {nameof(Popup)}", true)]
	public static new readonly BindableProperty PaddingProperty = Microsoft.Maui.Controls.Compatibility.Layout.PaddingProperty;

	/// <remarks>Use <see cref="PopupOptions.VerticalOptionsProperty"/> to set the <see cref="VerticalOptionsProperty"/> for <see cref="Popup"/></remarks>
	[Obsolete($"Use {nameof(PopupOptions)} to bind to the VerticalOptionsProperty  {nameof(Popup)}", true)]
	public static new readonly BindableProperty VerticalOptionsProperty = View.VerticalOptionsProperty;

	/// <remarks>Use <see cref="PopupOptions.HorizontalOptionsProperty"/> to set the <see cref="HorizontalOptionsProperty"/> for <see cref="Popup"/></remarks>
	[Obsolete($"Use {nameof(PopupOptions)} to bind to the HorizontalOptionsProperty of {nameof(Popup)}", true)]
	public static new readonly BindableProperty HorizontalOptionsProperty = View.HorizontalOptionsProperty;

	/// <summary>
	/// Event occurs when the Popup is opened.
	/// </summary>
	public event EventHandler? Opened;

	/// <summary>
	/// Event occurs when the Popup is closed.
	/// </summary>
	public event EventHandler? Closed;

	/// <remarks>Use <see cref="PopupOptions.Margin"/> to set the <see cref="Margin"/> for <see cref="Popup"/></remarks>
	public new Thickness Margin => base.Margin;

	/// <remarks>Use <see cref="PopupOptions.Padding"/> to set the <see cref="Padding"/> for <see cref="Popup"/></remarks>
	public new Thickness Padding => base.Padding;

	/// <remarks>Use <see cref="PopupOptions.VerticalOptions"/> to set the <see cref="VerticalOptions"/> for <see cref="Popup"/></remarks>
	public new LayoutOptions VerticalOptions => base.VerticalOptions;

	/// <remarks>Use <see cref="PopupOptions.HorizontalOptions"/> to set the <see cref="HorizontalOptions"/> for <see cref="Popup"/></remarks>
	public new LayoutOptions HorizontalOptions => base.HorizontalOptions;

	/// <summary>
	/// Close the Popup.
	/// </summary>
	public virtual Task Close(CancellationToken token = default) => GetPopupPage().Close(new PopupResult(false), token);

	internal void NotifyPopupIsOpened()
	{
		Opened?.Invoke(this, EventArgs.Empty);
	}

	internal void NotifyPopupIsClosed()
	{
		Closed?.Invoke(this, EventArgs.Empty);
	}

	private protected PopupPage GetPopupPage()
	{
		var parent = Parent;

		while (parent is not null)
		{
			if (parent.Parent is PopupPage popuppage)
			{
				return popuppage;
			}

			parent = parent.Parent;
		}

		throw new InvalidOperationException($"Unable to close popup: could not locate {nameof(PopupPage)}. {nameof(PopupExtensions.ShowPopup)} or {nameof(PopupExtensions.ShowPopupAsync)} must be called before {nameof(Close)}. If using a custom implementation of {nameof(Popup)}, override the {nameof(Close)} method");
	}
}

/// <summary>
/// Represents a small View that pops up at front the Page.
/// </summary>
public partial class Popup<T> : Popup
{
	/// <summary>
	/// Close the Popup with a result.
	/// </summary>
	/// <param name="result">Popup result</param>
	/// <param name="token"><see cref="CancellationToken"/></param>
	public virtual Task Close(T result, CancellationToken token = default) => GetPopupPage().Close(new PopupResult<T>(result, false), token);
}