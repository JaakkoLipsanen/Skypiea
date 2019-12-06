using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

using AndroidView = Android.Views.View;

namespace Skypiea
{
    [Activity(Label = "Skypiea"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        public static Activity1 Instance { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            Activity1.Instance = this;

            base.OnCreate(bundle);
            var g = new SkypieaGame();
            SetContentView((Android.Views.View)((Game)g).Services.GetService (typeof(Android.Views.View)));
            g.Run();
        }

        protected override void OnResume()
        {
            AndroidView decorView = this.Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable
                                          | SystemUiFlags.LayoutHideNavigation
                                          | SystemUiFlags.LayoutFullscreen
                                          | SystemUiFlags.HideNavigation
                                          | SystemUiFlags.Fullscreen
                                          | SystemUiFlags.ImmersiveSticky);
            base.OnResume();
        }
    }
}

