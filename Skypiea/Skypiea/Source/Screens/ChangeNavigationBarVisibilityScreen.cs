using System;
using Flai;
using Flai.ScreenManagement;

namespace Skypiea.Source.Screens
{
    public class ChangeNavigationBarVisibilityScreen : GameScreen
    {
        private readonly GameScreen _gameScreenToLoad;
        private readonly bool _navigationBarVisibility;
        private Size _screenSizeOnLoad;
        public ChangeNavigationBarVisibilityScreen(GameScreen gameScreenToLoad, bool navigationBarVisibility)
        {
            _gameScreenToLoad = gameScreenToLoad;
            _navigationBarVisibility = navigationBarVisibility;
        }

        protected override void LoadContent(bool instancePreserved)
        {
            if (SkypieaGameActivity.Instance.GetNavigationBarVisibility() != _navigationBarVisibility)
            {
                _screenSizeOnLoad = FlaiGame.Current.ScreenSize;
                SkypieaGameActivity.Instance.SetNavigationBarVisibility(_navigationBarVisibility);
            }
            else
            {
                this.LoadNextScreen();
            }
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if(FlaiGame.Current.ScreenSize != _screenSizeOnLoad || this.ScreenRunningTime.TotalSeconds > 2)
            {
                this.LoadNextScreen();
            }
        }

        private void LoadNextScreen()
        {
            this.ScreenManager.RemoveScreen(this);
            this.ScreenManager.AddScreen(_gameScreenToLoad);
        }
    }
}
