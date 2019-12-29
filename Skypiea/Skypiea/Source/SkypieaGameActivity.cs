using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

using AndroidView = Android.Views.View;

namespace Skypiea
{
    [Activity(Label = "Final Fight Z"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class SkypieaGameActivity : AndroidGameActivity
    {
        public static SkypieaGameActivity Instance { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            SkypieaGameActivity.Instance = this;

            base.OnCreate(bundle);
            var g = new SkypieaGame();
            SetContentView((Android.Views.View)((Game)g).Services.GetService (typeof(Android.Views.View)));
            g.Run();
        }

        protected override void OnResume()
        {
            this.SetNavigationBarVisibility(true);
            base.OnResume();
        }

        public bool GetNavigationBarVisibility()
        {
            return this.Window.DecorView.SystemUiVisibility == (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LightStatusBar);
        }

        public void SetNavigationBarVisibility(bool visible)
        {
            AndroidView decorView = this.Window.DecorView;
            SystemUiFlags visibility;
            if (visible)
            {
                visibility = SystemUiFlags.LayoutStable | SystemUiFlags.LightStatusBar;
            } else
            {
                visibility = SystemUiFlags.LayoutStable
                            | SystemUiFlags.LayoutHideNavigation
                            | SystemUiFlags.LayoutFullscreen
                            | SystemUiFlags.HideNavigation
                            | SystemUiFlags.Fullscreen
                            | SystemUiFlags.ImmersiveSticky;
            }
            decorView.SystemUiVisibility = (StatusBarVisibility)visibility;
        }
    }
}

