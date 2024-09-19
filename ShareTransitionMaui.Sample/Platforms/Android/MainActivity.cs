using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace ShareTransitionMaui.Sample;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Ativar o modo fullscreen ao iniciar
        //SetFullScreen();
    }

    public void SetFullScreen()
    {
        var window = this.Window;

        window.AddFlags(WindowManagerFlags.Fullscreen);
        window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Fullscreen |
                                              (StatusBarVisibility)SystemUiFlags.HideNavigation |
                                              (StatusBarVisibility)SystemUiFlags.ImmersiveSticky;
    }
}
