
namespace Flai.ScreenManagement.Screens
{
    public abstract class SplashScreen : GameScreen
    {
        // Splash screen cannot be pop up
        public sealed override bool IsPopup
        {
            get { return false; }
        }
    }
}
