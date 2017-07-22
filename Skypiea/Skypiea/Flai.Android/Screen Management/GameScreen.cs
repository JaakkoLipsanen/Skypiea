
using System;
using Flai.Content;
using Flai.Graphics;
using Microsoft.Xna.Framework;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Flai.ScreenManagement
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    // move these all to another .cs
    public enum FadeDirection
    {
        None,
        Left,
        Up,
        Right,
        Down,
    }

    public enum FadeType
    {
        None,
        FadeToBlack,
        FadeAlpha,
    }

    internal static class FadeDirectionExtensions
    {
        public static Vector2i ToVector2i(this FadeDirection fadeDirection)
        {
            switch (fadeDirection)
            {
                case FadeDirection.None:
                    return Vector2i.Zero;

                case FadeDirection.Left:
                    return -Vector2i.UnitX;

                case FadeDirection.Right:
                    return Vector2i.UnitX;

                case FadeDirection.Up:
                    return -Vector2i.UnitY;

                case FadeDirection.Down:
                    return Vector2i.UnitY;

                default:
                    throw new ArgumentOutOfRangeException("fadeDirection");
            }
        }
    }

    public static class GameScreenSimulator
    {
#if WINDOWS_PHONE
        public static void LoadContent(GameScreen gameScreen, bool instancePreserved)
        {
            gameScreen.LoadScreenContent(instancePreserved);
        }
#else 
        public static void LoadContent(GameScreen gameScreen)
        {
            gameScreen.LoadScreenContent();
        }
#endif

        public static void Update(UpdateContext updateContext, GameScreen gameScreen)
        {
            gameScreen.UpdateScreen(updateContext, false, false);
        }

        public static void Draw(GraphicsContext graphicsContext, GameScreen gameScreen)
        {
            gameScreen.DrawScreen(graphicsContext);
        }
    }

    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class GameScreen
    {
#if WINDOWS_PHONE
        private GestureType _enabledGestures = GestureType.None;
#endif

        private bool _isLoaded = false;
        private bool _otherScreenHasFocus;
        private readonly DateTime _screenLaunchTime = DateTime.Now;

        #region Properties

        // pretty meh. FadeType has nothing to do with FadeIn/Out (which are only positional fades).. whatever
        protected FadeDirection FadeIn { get; set; }
        protected FadeDirection FadeOut { get; set; }
        protected float? FadeInOutAmount { get; set; }
        protected FadeType FadeType { get; set; }
        // todo: visible area currently (after fading)? could be useful
        public TimeSpan ScreenRunningTime
        {
            get { return DateTime.Now - _screenLaunchTime; }
        }

        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public virtual bool IsPopup { get { return false; } }

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransitionOnTime { get; protected set; }

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime { get; protected set; }

        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition { get; protected internal set; }

        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState { get; protected internal set; }

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }

        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting { get; internal protected set; }

        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 1 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - this.TransitionPosition; }
        }

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !_otherScreenHasFocus &&
                       (this.ScreenState == ScreenState.TransitionOn || this.ScreenState == ScreenState.Active);
            }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

#if WINDOWS_PHONE
        public GestureType EnabledGestures
        {
            get { return _enabledGestures; }
            protected set
            {
                _enabledGestures = value;
                if (this.ScreenState == ScreenState.Active && _isLoaded)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }
#endif

        public FlaiServiceContainer Services
        {
            get { return this.ScreenManager == null ? FlaiGame.ServiceContainer : this.ScreenManager.Services; }
        }

        public IContentProvider ContentProvider
        {
            get { return this.ScreenManager.ContentProvider; }
        }

        public IFontContainer FontContainer
        {
            get { return this.ScreenManager.FontProvider; }
        }

        public FlaiGame Game
        {
            get { return this.ScreenManager.Game; }
        }

        #endregion

        public event GenericEvent Exited;

        protected GameScreen()
        {
            this.TransitionPosition = 1;
        }

        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        /// <param name="instancePreserved">
        /// True if the game was preserved during deactivation, false if the screen is just being added or if the game was tombstoned.
        /// On Xbox and Windows this will always be false.
        /// </param>
#if WINDOWS
        internal void LoadScreenContent()
        {
            this.LoadContent();
            _isLoaded = true;
            this.ScreenManager.Game.ResetElapsedTime();
        }
        protected virtual void LoadContent() { }
#elif WINDOWS_PHONE
        internal void LoadScreenContent(bool instancePreserved)
        {
            this.LoadContent(instancePreserved);
            _isLoaded = true;
            this.ScreenManager.Game.ResetElapsedTime();
        }
        protected virtual void LoadContent(bool instancePreserved) { }
#endif

        /// <summary>
        /// Unload content for the screen. Called when the screen is removed from the screen manager.
        /// </summary>
        internal void UnloadScreenContent()
        {
            this.UnloadContent();
        }
        protected virtual void UnloadContent() { }

#if WINDOWS_PHONE
        /// <summary>
        /// Deactivates the screen. Called when the game is being deactivated due to pausing or tombstoning.
        /// </summary>
        internal void DeactivateScreen()
        {
            this.Deactivate();
        }
        protected virtual void Deactivate() { }
#endif

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        internal void UpdateScreen(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (this.IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                this.ScreenState = ScreenState.TransitionOff;

                if (!this.UpdateTransition(updateContext, this.TransitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    this.ScreenManager.RemoveScreen(this);
                    this.Exited.InvokeIfNotNull();
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (this.UpdateTransition(updateContext, this.TransitionOffTime, 1))
                {
                    // Still busy transitioning.
                    this.ScreenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    this.ScreenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (this.UpdateTransition(updateContext, this.TransitionOnTime, -1))
                {
                    // Still busy transitioning.
                    this.ScreenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    this.ScreenState = ScreenState.Active;
                }
            }

            this.Update(updateContext, otherScreenHasFocus, coveredByOtherScreen);
        }
        protected virtual void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen) { }

        internal void PreDrawScreen(GraphicsContext graphicsContext)
        {
            this.BeginDraw(graphicsContext);
            this.PreDraw(graphicsContext);
            this.EndDraw(graphicsContext);
        }
        protected virtual void PreDraw(GraphicsContext graphicsContext) { }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        internal void DrawScreen(GraphicsContext graphicsContext)
        {
            this.BeginDraw(graphicsContext);
            this.Draw(graphicsContext);
            this.EndDraw(graphicsContext);
        }
        protected virtual void Draw(GraphicsContext graphicsContext) { }

        internal void PostDrawScreen(GraphicsContext graphicsContext)
        {
            this.BeginDraw(graphicsContext);
            this.EndDraw(graphicsContext);
            this.PostDraw(graphicsContext);
        }
        protected virtual void PostDraw(GraphicsContext graphicsContext) { }

        private void BeginDraw(GraphicsContext graphicsContext)
        {
            if (this.FadeType == FadeType.FadeAlpha && this.TransitionAlpha < 1)
            {
                graphicsContext.SpriteBatch.Alpha.Push(this.TransitionAlpha * this.TransitionAlpha);
            }

            // FadeDirection stuff
            if (this.ScreenState == ScreenState.TransitionOn || this.ScreenState == ScreenState.TransitionOff)
            {
                FadeDirection currentFade = this.ScreenState == ScreenState.TransitionOn ? this.FadeIn : this.FadeOut;
                if (currentFade == FadeDirection.None)
                {
                    return;
                }

                float offsetAmount = this.FadeInOutAmount.HasValue ?
                    this.FadeInOutAmount.Value :
                    ((currentFade == FadeDirection.Left || currentFade == FadeDirection.Right) ? graphicsContext.ScreenSize.Width / 2f : graphicsContext.ScreenSize.Height / 2f);

                offsetAmount *= this.TransitionPosition * this.TransitionPosition;
                graphicsContext.SpriteBatch.Offset.Push(currentFade.ToVector2i() * offsetAmount);
            }
        }

        private void EndDraw(GraphicsContext graphicsContext)
        {
            if (this.FadeType == FadeType.FadeToBlack)
            {
                GraphicsHelper.FadeBackBufferToColor(graphicsContext, Color.Black * this.TransitionPosition);
            }
            else if (this.FadeType == FadeType.FadeAlpha && this.TransitionAlpha < 1)
            {
                graphicsContext.SpriteBatch.Alpha.Pop();
            }

            // FadeDirection stuff
            if (this.ScreenState == ScreenState.TransitionOn || this.ScreenState == ScreenState.TransitionOff)
            {
                FadeDirection currentFade = this.ScreenState == ScreenState.TransitionOn ? this.FadeIn : this.FadeOut;
                if (currentFade == FadeDirection.None)
                {
                    return;
                }

                graphicsContext.SpriteBatch.Offset.Pop();
            }
        }

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (this.TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                this.ScreenManager.RemoveScreen(this);
                this.Exited.InvokeIfNotNull();
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                this.IsExiting = true;
            }
        }

        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        private bool UpdateTransition(UpdateContext updateContext, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;
            if (time == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(updateContext.GameTime.DeltaSeconds * 1000 / time.TotalMilliseconds);
            }

            // Update the transition position.
            this.TransitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (this.TransitionPosition <= 0)) ||
                ((direction > 0) && (this.TransitionPosition >= 1)))
            {
                this.TransitionPosition = MathHelper.Clamp(this.TransitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }
    }
}
