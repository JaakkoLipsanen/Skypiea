
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Flai.Content;
using Flai.DataStructures;
using Flai.General;
using Flai.Graphics;
using Flai.ScreenManagement.Screens;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Flai.ScreenManagement
{
    public class Delay
    {
        public float TimeRemaining { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsCancelled { get; private set; }
        public bool HasFinished { get { return this.TimeRemaining <= 0; }}

        public Delay(float delay)
        {
            this.TimeRemaining = delay;
        }

        public void Update(UpdateContext updateContext)
        {
            // is cancelled here?
            if (!this.IsPaused)
            {
                this.TimeRemaining -= updateContext.DeltaSeconds;
                if (this.TimeRemaining < 0)
                {
                    this.TimeRemaining = 0;
                }
            }
        }

        public void Continue()
        {
            this.IsPaused = false;
        }

        public void Pause()
        {
            this.IsPaused = true;
        }

        public void Cancel()
        {
            this.IsCancelled = true;
        }
    }

    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : IScreenManager
    {
        #region Fields

        private readonly FlaiGame _game;

        private readonly List<GameScreen> _screens = new List<GameScreen>();
        private readonly List<GameScreen> _tempScreensList = new List<GameScreen>();
        private readonly Bag<Pair<Delay, GameScreen>> _screensToBeAddedWithDelay = new Bag<Pair<Delay, GameScreen>>();  // PS. it'd be great if the whole engine had some general "delay" stuff. for example FlaiGame.WithDelay(0.25f, () => { });

        private readonly ReadOnlyCollection<GameScreen> _readOnlyScreenList;

        private bool _isInitialized = false;

        #endregion

        #region Properties

        public FlaiGame Game
        {
            get { return _game; }
        }

        public FlaiServiceContainer Services
        {
            get { return _game.Services; }
        }

        public IContentProvider ContentProvider
        {
            get { return this.Services.Get<IContentProvider>(); }
        }

        public IFontContainer FontProvider
        {
            get { return this.Services.Get<IFontContainer>(); }
        }

        /// <summary>
        /// Returns copy of ScreenManager's screen list. Does not return the original list!
        /// </summary>
        public ReadOnlyCollection<GameScreen> Screens
        {
            get { return _readOnlyScreenList; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        internal ScreenManager(FlaiGame game)
        {
            _game = game;
            _game.Services.Add<IScreenManager>(this);

            _readOnlyScreenList = new ReadOnlyCollection<GameScreen>(_screens);
        }


        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        internal void Initialize()
        {
            _isInitialized = true;
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        internal void LoadContent()
        {
            // Tell each of the screens to load their content.
            foreach (GameScreen screen in _screens)
            {
                if (!screen.IsLoaded)
                {
#if WINDOWS
                    screen.LoadScreenContent();
#elif WINDOWS_PHONE
                    screen.LoadScreenContent(false);
#endif
                }
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        internal void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in _screens)
            {
                screen.UnloadScreenContent();
            }
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public void Update(UpdateContext updateContext)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _tempScreensList.Clear();

            foreach (GameScreen screen in _screens)
            {
                _tempScreensList.Add(screen);
            }

            bool otherScreenHasFocus = !this.Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (_tempScreensList.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = _tempScreensList[_tempScreensList.Count - 1];

                _tempScreensList.RemoveAt(_tempScreensList.Count - 1);

                // Update the screen.
                screen.UpdateScreen(updateContext, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        // Input should be handled in the Update method
                        //   screen.HandleScreenInput(updateContext);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }
            }

            for(int i = 0; i < _screensToBeAddedWithDelay.Count; i++)
            {
                Pair<Delay, GameScreen> pair = _screensToBeAddedWithDelay[i];
                pair.First.Update(updateContext);
                if (pair.First.HasFinished)
                {
                    _screensToBeAddedWithDelay.RemoveAt(i);
                    this.AddScreen(pair.Second);
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public void Draw(GraphicsContext graphicsContext)
        {
            // pre draw
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                screen.PreDrawScreen(graphicsContext);
            }

            // draw
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                screen.DrawScreen(graphicsContext);
            }

            // post draw
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                screen.PostDrawScreen(graphicsContext);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;
            _screens.Add(screen);

#if WINDOWS_PHONE
            // update the TouchPanel to respond to gestures this screen is interested in
            TouchPanel.EnabledGestures = screen.EnabledGestures;
#endif

            // If we have a graphics device, tell the screen to load content.
            if (_isInitialized)
            {
#if WINDOWS
                screen.LoadScreenContent();
#elif WINDOWS_PHONE
                screen.LoadScreenContent(false);
#endif
            }
        }

        public void AddScreen(GameScreen screen, Delay delay)
        {
            Ensure.NotNull(screen);
            Ensure.NotNull(delay);

            _screensToBeAddedWithDelay.Add(Pair.Create(delay, screen));
        }

        // feels a bit meh
        public void LoadScreen(GameScreen gameScreen)
        {
            LoadingScreen.Load(this, false, gameScreen);
        }

        public void LoadScreen(GameScreen gameScreen1, GameScreen gameScreen2)
        {
            LoadingScreen.Load(this, false, new GameScreen[] { gameScreen1, gameScreen2 });
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (_isInitialized)
            {
                screen.UnloadScreenContent();
            }

            _screens.Remove(screen);
            _tempScreensList.Remove(screen);

#if WINDOWS_PHONE
            // if there is a screen still in the manager, update TouchPanel
            // to respond to gestures that screen is interested in.
            if (_screens.Count > 0)
            {
                TouchPanel.EnabledGestures = _screens[_screens.Count - 1].EnabledGestures;
            }
#endif
        }

        public void RemoveAllScreens()
        {
            for (int i = _screens.Count - 1; i >= 0; i--)
            {
                this.RemoveScreen(_screens[i]);
            }
        }

        public void ExitAllScreens()
        {
            for (int i = _screens.Count - 1; i >= 0; i--)
            {
               _screens[i].ExitScreen();
            }
        }

#if WINDOWS_PHONE
        internal bool Activate(bool instancePreserved)
        {
            // If the game instance was preserved, the game wasn't dehydrated so our screens still exist.
            // We just need to activate them and we're ready to go.
            if (instancePreserved)
            {
                // Make a copy of the master screen list, to avoid confusion if
                // the process of activating one screen adds or removes others.
                _tempScreensList.Clear();

                foreach (GameScreen screen in _screens)
                {
                    _tempScreensList.Add(screen);
                }

                foreach (GameScreen screen in _tempScreensList)
                {
                    screen.LoadScreenContent(true);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Informs the screen manager to serialize its state to disk.
        /// </summary>
        internal void Deactivate()
        {
            _tempScreensList.Clear();
            foreach (GameScreen screen in _screens)
            {
                _tempScreensList.Add(screen);
            }

            foreach (GameScreen screen in _tempScreensList)
            {
                screen.DeactivateScreen();
            }

            _tempScreensList.Clear();
        }
#endif

        #endregion
    }
}
