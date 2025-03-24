﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using FluentAssertions;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Views;

public class PopupContainerTests
{
	[Fact]
	public void Constructor_ShouldThrowArgumentNullException_WhenPopupIsNull()
	{
		// Arrange
		var popupOptions = new MockPopupOptions();
		var taskCompletionSource = new TaskCompletionSource<PopupResult>();

		// Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		Action act = () => new PopupContainer(null, popupOptions);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		// Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void Constructor_ShouldThrowArgumentNullException_WhenPopupOptionsIsNull()
	{
		// Arrange
		var view = new ContentView();
		var taskCompletionSource = new TaskCompletionSource<PopupResult>();

		// Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		Action act = () => new PopupContainer(view, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		// Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public async Task Close_ShouldSetResultAndPopModalAsync()
	{
		// Arrange
		var tcs = new TaskCompletionSource<PopupResult>();
		var view = new ContentView();
		var popupOptions = new MockPopupOptions();
		var popupContainer = new PopupContainer(view, popupOptions);
		var expectedResult = new PopupResult(false);

		popupContainer.PopupClosed += HandlePopupClosed;

		// Act
		await popupContainer.Close(expectedResult, CancellationToken.None);
		var actualResult = await tcs.Task;

		// Assert
		actualResult.Should().Be(expectedResult);

		void HandlePopupClosed(object? sender, PopupResult e)
		{
			tcs.SetResult(e);
		}
	}

	[Fact]
	public void Close_ShouldThrowOperationCanceledException_WhenTokenIsCancelled()
	{
		// Arrange
		var view = new ContentView();
		var popupOptions = new MockPopupOptions();
		var popupContainer = new PopupContainer(view, popupOptions);
		var result = new PopupResult(false);
		var cts = new CancellationTokenSource();
		cts.Cancel();

		// Act
		Func<Task> act = async () => await popupContainer.Close(result, cts.Token);

		// Assert
		act.Should().ThrowAsync<OperationCanceledException>();
	}

	[Fact]
	public void PopupContainerT_Constructor_ShouldThrowArgumentNullException_WhenPopupIsNull()
	{
		// Arrange
		var popupOptions = new MockPopupOptions();

		// Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		Action act = () => new PopupContainer<string>(null, popupOptions);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		// Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void PopupContainerT_Constructor_ShouldThrowArgumentNullException_WhenPopupOptionsIsNull()
	{
		// Arrange
		var view = new ContentView();

		// Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		Action act = () => new PopupContainer<string>(view, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		// Assert
		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public async Task PopupContainerT_Close_ShouldSetResultAndPopModalAsync()
	{
		// Arrange
		var view = new ContentView();
		var popupOptions = new MockPopupOptions();
		var taskCompletionSource = new TaskCompletionSource<PopupResult<string>>();
		var popupContainer = new PopupContainer<string>(view, popupOptions);
		var expectedResult = new PopupResult<string>("Test", false);

		popupContainer.PopupClosed += HandlePopupClosed;

		// Act
		await popupContainer.Close(expectedResult, CancellationToken.None);
		var actualResult = await taskCompletionSource.Task;

		// Assert
		actualResult.Should().Be(expectedResult);

		void HandlePopupClosed(object? sender, PopupResult e)
		{
			taskCompletionSource.SetResult((PopupResult<string>)e);
		}
	}

	[Fact]
	public void PopupContainerT_Close_ShouldThrowOperationCanceledException_WhenTokenIsCancelled()
	{
		// Arrange
		var view = new ContentView();
		var popupOptions = new MockPopupOptions();
		var popupContainer = new PopupContainer<string>(view, popupOptions);
		var result = new PopupResult<string>("Test", false);
		var cts = new CancellationTokenSource();
		cts.Cancel();

		// Act
		Func<Task> act = async () => await popupContainer.Close(result, cts.Token);

		// Assert
		act.Should().ThrowAsync<OperationCanceledException>();
	}

	sealed class MockPopupOptions : IPopupOptions
	{
		public bool CanBeDismissedByTappingOutsideOfPopup { get; set; }
		public Color BackgroundColor { get; set; } = Colors.Transparent;
		public Action? OnTappingOutsideOfPopup { get; set; }
		public IShape? Shape { get; set; }
		public Thickness Margin { get; set; }
		public Thickness Padding { get; set; }
		public LayoutOptions VerticalOptions { get; set; }
		public LayoutOptions HorizontalOptions { get; set; }
	}
}