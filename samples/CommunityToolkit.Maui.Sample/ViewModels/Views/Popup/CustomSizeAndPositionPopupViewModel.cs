using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Sample.Views.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommunityToolkit.Maui.Sample.ViewModels.Views;

public partial class CustomSizeAndPositionPopupViewModel : BaseViewModel
{
	public IReadOnlyList<string> FlowDirectionOptions { get; } = [.. Enum.GetNames<FlowDirection>()];

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial double Height { get; set; } = 100;

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial double Width { get; set; } = 100;

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsStartHorizontalOptionSelected { get; set; } = true;

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsCenterHorizontalOptionSelected { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsEndHorizontalOptionSelected { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsFillHorizontalOptionSelected { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsStartVerticalOptionSelected { get; set; } = true;

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsCenterVerticalOptionSelected { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsEndVerticalOptionSelected { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial bool IsFillVerticalOptionSelected { get; set; }

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(ExecuteShowButtonCommand))]
	public partial int FlowDirectionSelectedIndex { get; set; }

	[RelayCommand(CanExecute = nameof(CanShowButtonExecute))]
	public Task ExecuteShowButton(CancellationToken token)
	{
		if (!IsFlowDirectionSelectionValid(FlowDirectionSelectedIndex, FlowDirectionOptions.Count))
		{
			throw new IndexOutOfRangeException("Invalid FlowDirection Selected");
		}

		Microsoft.Maui.Primitives.LayoutAlignment? verticalOptions = null, horizontalOptions = null;

		if (IsStartVerticalOptionSelected)
		{
			verticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Start;
		}
		if (IsCenterVerticalOptionSelected)
		{
			verticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Center;
		}
		if (IsEndVerticalOptionSelected)
		{
			verticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.End;
		}
		if (IsFillVerticalOptionSelected)
		{
			verticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Fill;
		}

		ArgumentNullException.ThrowIfNull(verticalOptions);

		if (IsStartHorizontalOptionSelected)
		{
			horizontalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Start;
		}
		if (IsCenterHorizontalOptionSelected)
		{
			horizontalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Center;
		}
		if (IsEndHorizontalOptionSelected)
		{
			horizontalOptions = Microsoft.Maui.Primitives.LayoutAlignment.End;
		}
		if (IsFillHorizontalOptionSelected)
		{
			horizontalOptions = Microsoft.Maui.Primitives.LayoutAlignment.Fill;
		}

		ArgumentNullException.ThrowIfNull(horizontalOptions);

		var popup = new FlowDirectionPopup((FlowDirection)FlowDirectionSelectedIndex)
		{
			Size = new Size(Width, Height),
			VerticalOptions = verticalOptions.Value,
			HorizontalOptions = horizontalOptions.Value
		};

		return Shell.Current.ShowPopupAsync(popup, token);
	}

	static bool IsFlowDirectionSelectionValid(int flowDirectionSelection, int flowDirectionOptionsCount)
	{
		return flowDirectionSelection < flowDirectionOptionsCount
				&& flowDirectionSelection >= 0;
	}

	// Ensure at least one Horizontal Option is selected, one Vertical Option is selected, Height > 0, Width > 0, and FlowDirection is valid
	bool CanShowButtonExecute() => (IsStartHorizontalOptionSelected || IsCenterHorizontalOptionSelected || IsEndHorizontalOptionSelected || IsFillHorizontalOptionSelected)
		&& (IsStartVerticalOptionSelected || IsCenterVerticalOptionSelected || IsEndVerticalOptionSelected || IsFillVerticalOptionSelected)
		&& Height > 0
		&& Width > 0
		&& IsFlowDirectionSelectionValid(FlowDirectionSelectedIndex, FlowDirectionOptions.Count);

	partial class FlowDirectionPopup : RedBlueBoxPopup
	{
		readonly FlowDirection flowDirection;

		public FlowDirectionPopup(FlowDirection flowDirection)
		{
			this.flowDirection = flowDirection;

			if (Content is not null)
			{
				Content.FlowDirection = flowDirection;
			}
		}

		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == ContentProperty.PropertyName && Content is not null)
			{
				Content.FlowDirection = flowDirection;
			}
		}
	}
}