using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace CommunityToolkit.Maui.Sample;

[Activity(Theme = "@style/Maui.SplashTheme", ResizeableActivity = true, MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
[Register("com.microsoft.CommunityToolkit.Maui.Sample.MainActivity")]
public class MainActivity : MauiAppCompatActivity
{

}