
using System;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Flai.ScreenManagement.Screens
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    public class LoadingScreen : GameScreen
    {
        #region Fields

        private static readonly List<GameScreen> _temporaryScreenList = new List<GameScreen>();

        private readonly GameScreen[] _screensToLoad;
        private readonly string _message;

        private bool _loadingIsSlow;
        private bool _otherScreensAreGone;
        
        #endregion

        #region Initialization


        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, string message, GameScreen[] screensToLoad)
        {
            _loadingIsSlow = loadingIsSlow;
            _screensToLoad = screensToLoad;
            _message = message;
        }

        public static void Load(ScreenManager screenManager, params GameScreen[] screensToLoad)
        {
            LoadingScreen.Load(screenManager, false, "Loading...", screensToLoad);
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, params GameScreen[] screensToLoad)
        {
            LoadingScreen.Load(screenManager, loadingIsSlow, "Loading...", screensToLoad);
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow, string message, params GameScreen[] screensToLoad)
        {
            bool screensHaveTransition = false;
            foreach (GameScreen screen in screenManager.Screens)
            {
                _temporaryScreenList.Add(screen);
            }

            // Tell all the current screens to transition off.
            foreach (GameScreen screen in _temporaryScreenList)
            {
                if (screen.TransitionOffTime != TimeSpan.Zero)
                {
                    screensHaveTransition = true;
                }

                screen.ExitScreen();
            }
            _temporaryScreenList.Clear();

            if (screensHaveTransition)
            {
                screenManager.AddScreen(new LoadingScreen(screenManager, loadingIsSlow, message, screensToLoad));
            }
            else
            {
                foreach (GameScreen screen in screensToLoad)
                {
                    screenManager.AddScreen(screen);
                }
            }
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(updateContext, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (_otherScreensAreGone)
            {
                base.ScreenManager.RemoveScreen(this);
                foreach (GameScreen screen in _screensToLoad)
                {
                    if (screen != null)
                    {
                        base.ScreenManager.AddScreen(screen);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                base.ScreenManager.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        protected override void Draw(GraphicsContext graphicsContext)
        {
            if ((base.ScreenState == ScreenState.Active) &&
                (base.ScreenManager.Screens.Count == 1))
            {
                _otherScreensAreGone = true;

                // Draw the screen black
                graphicsContext.SpriteBatch.Begin();
                graphicsContext.SpriteBatch.Draw(graphicsContext.BlankTexture, graphicsContext.ScreenArea, Color.Black);
                graphicsContext.SpriteBatch.End();
            }

            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.


            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (_loadingIsSlow)
            {
                SpriteFont font = graphicsContext.FontContainer.DefaultFont;

                // Center the text in the viewport.
                Viewport viewport = graphicsContext.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textPosition = (viewportSize - font.MeasureString(_message)) / 2;

                // Draw the text.
                graphicsContext.SpriteBatch.Begin();
                graphicsContext.SpriteBatch.DrawString(font, _message, textPosition, Color.White * base.TransitionAlpha);
                graphicsContext.SpriteBatch.End();
            }
        }

        #endregion
    }
}
