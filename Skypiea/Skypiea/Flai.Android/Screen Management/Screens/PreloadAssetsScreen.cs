using System;
using Flai.Graphics;

namespace Flai.ScreenManagement.Screens
{
    public abstract class PreloadAssetsScreen : SplashScreen
    {
        private readonly GameScreen[] _screensToLoad;

        private bool _hasDrawn = false;
        private bool _hasLoaded = false;

        protected PreloadAssetsScreen(params GameScreen[] screensToLoad)
        {
            Ensure.NotNull(screensToLoad);
            Ensure.True(screensToLoad.Length > 0);
            _screensToLoad = screensToLoad;

            base.TransitionOnTime = TimeSpan.Zero; 
            base.TransitionOffTime = TimeSpan.Zero;
            base.FadeType = FadeType.FadeToBlack;
        }

        protected sealed override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(updateContext, otherScreenHasFocus, coveredByOtherScreen);

            if (_hasDrawn && !_hasLoaded)
            {
                this.PreloadAssets();
                LoadingScreen.Load(base.ScreenManager, false, _screensToLoad);
                _hasLoaded = true;

                GC.Collect();
                base.ScreenManager.Game.ResetElapsedTime();
            }
        }

        protected sealed override void Draw(GraphicsContext graphicsContext)
        {
            if (!_hasDrawn)
            {
                _hasDrawn = true;
                this.DrawInner(graphicsContext);
            }

            base.Draw(graphicsContext);
        }

        protected abstract void PreloadAssets();
        protected abstract void DrawInner(GraphicsContext graphicsContext);
    }
}
