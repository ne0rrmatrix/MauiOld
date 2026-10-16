// Test harness used to review PR #3354 (CommunityToolkit/Maui).
// Drop into samples/CommunityToolkit.Maui.Sample/Platforms/Android/ and add this to the top of App.CreateWindow (inside #if ANDROID):
//
//   if ((activationState?.Context.Services.GetService(typeof(global::Android.App.Activity)) as global::Android.App.Activity)?.Intent?.GetStringExtra("pr3354") is string pr3354Scenario)
//   {
//       return new Window(new Pr3354HostPage(pr3354Scenario));
//   }
//
// Run a scenario:  adb shell am start -n <package>/<MainActivity> --es pr3354 <modal|popup|nested|safearea|sheet|dialog>
// The host page sets a solid red status bar, waits 1.5 s, then runs the scenario.
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CommunityToolkit.Maui.Extensions;
using Google.Android.Material.BottomSheet;
using StatusBar = CommunityToolkit.Maui.Core.Platform.StatusBar;
using AView = Android.Views.View;
using TextAlignment = Microsoft.Maui.TextAlignment;

namespace CommunityToolkit.Maui.Sample;

public class Pr3354HostPage : ContentPage
{
	readonly string scenario;
	bool started;

	public Pr3354HostPage(string scenario)
	{
		this.scenario = scenario;
		BackgroundColor = Colors.White;
		Content = new Label { Text = $"HOST PAGE ({scenario})", FontSize = 24, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (started)
		{
			return;
		}

		started = true;
		StatusBar.SetColor(Colors.Red);
		StatusBar.SetStyle(Core.StatusBarStyle.LightContent);
		await Task.Delay(1500);

		var activity = (AppCompatActivity)Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!;

		switch (scenario)
		{
			case "popup":
				await this.ShowPopupAsync(
					new Label { Text = "POPUP", FontSize = 24, Padding = 40, BackgroundColor = Colors.White },
					new PopupOptions { PageOverlayColor = Color.FromRgba(0, 0, 0, 0.6) });
				break;

			case "nested":
				await Navigation.PushModalAsync(MakeModal("MODAL A (sets blue)", Colors.LightGray, useSafeArea: true));
				await Task.Delay(1500);
				StatusBar.SetColor(Colors.Blue);
				await Task.Delay(1500);
				await Navigation.PushModalAsync(MakeModal("MODAL B (opened from blue A)", Colors.LightGreen, useSafeArea: true));
				break;

			case "modal":
				await Navigation.PushModalAsync(MakeModal("DEFAULT SAFE AREA MODAL", Colors.LightGray, useSafeArea: true));
				break;

			case "safearea":
				await Navigation.PushModalAsync(MakeModal("SafeAreaEdges=None MODAL", Colors.LightGray, useSafeArea: false));
				break;

			case "sheet":
				new Pr3354Sheet().Show(activity.SupportFragmentManager, "pr3354sheet");
				break;

			case "dialog":
				new Pr3354Dialog().Show(activity.SupportFragmentManager, "pr3354dialog");
				break;
		}
	}

	static ContentPage MakeModal(string title, Color background, bool useSafeArea)
	{
		var grid = new Grid
		{
			RowDefinitions = [new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto)],
			BackgroundColor = background,
		};
		grid.Add(new Label { Text = "TOP MARKER", BackgroundColor = Colors.Yellow, HeightRequest = 120, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.End, FontSize = 20 }, 0, 0);
		grid.Add(new Label { Text = title, FontSize = 22, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }, 0, 1);
		grid.Add(new Label { Text = "BOTTOM MARKER", BackgroundColor = Colors.Orange, HeightRequest = 120, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Start, FontSize = 20 }, 0, 2);

		var page = new ContentPage { Content = grid, BackgroundColor = background };
		if (!useSafeArea)
		{
			page.SafeAreaEdges = SafeAreaEdges.None;
			grid.SafeAreaEdges = SafeAreaEdges.None;
		}

		return page;
	}
}

class Pr3354Sheet : BottomSheetDialogFragment
{
	public override AView? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
	{
		var text = new TextView(inflater.Context)
		{
			Text = "BOTTOM SHEET\n\n\n\nLAST LINE OF SHEET",
			TextSize = 22,
			Gravity = GravityFlags.Center,
		};
		text.SetBackgroundColor(global::Android.Graphics.Color.Yellow);
		text.SetMinHeight(700);
		return text;
	}
}

class Pr3354Dialog : AppCompatDialogFragment
{
	public override global::Android.App.Dialog OnCreateDialog(Bundle? savedInstanceState) =>
		new AndroidX.AppCompat.App.AlertDialog.Builder(RequireContext())
			.SetTitle("FLOATING DIALOG")!
			.SetMessage("Floating AppCompat DialogFragment content")!
			.SetPositiveButton("OK", (s, e) => { })!
			.Create();
}
