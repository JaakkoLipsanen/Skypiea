using System.Linq;
using Flai.Audio;
using Flai.Content;
using Flai.Graphics;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#if WINDOWS_PHONE
using Flai.Input;
using Flai.DataStructures;
#endif

#if WINDOWS
using System.Windows.Forms;
using Flai.Input;
using Flai.DataStructures;

#endif

namespace Flai
{
    // Tried doing "FlaiGameAttribute", so that all the games would start from the Flai and the "game projects" would have a [FlaiGame] attribute in their game class.
    // however, that doesn't work, since Flai doesn't have a reference to the game project and there is no way to even get the assembly of the game project, since it simply isn't referenced
    // anywhere.. oh well, FlaiGame.Run<T>() it is !

    public abstract class FlaiGame : Game
    {
        #region Fields

        public delegate void WindowResolutionChanged(Size oldResolution, Size newResolution);
        public event WindowResolutionChanged ResolutionChanged;

        private readonly UpdateContext _updateContext;
        private readonly GraphicsContext _graphicsContext;

        private readonly List<FlaiGameComponent> _updateableComponents = new List<FlaiGameComponent>();
        private readonly List<FlaiGameComponent> _temporaryUpdateableComponents = new List<FlaiGameComponent>();
        private readonly List<FlaiDrawableGameComponent> _drawableComponents = new List<FlaiDrawableGameComponent>();
        private readonly List<FlaiDrawableGameComponent> _temporaryDrawableComponents = new List<FlaiDrawableGameComponent>();

        protected readonly FlaiServiceContainer _serviceContainer;
        protected readonly ScreenManager _screenManager;
        protected readonly ContentProvider _contentProvider;
        protected readonly GraphicsDeviceManager _graphicsDeviceManager;
        protected readonly SoundEffectManager _soundEffectManager;

        private Color? _clearColor = Color.White;

#if WINDOWS
        private Size _minimumScreenSize;
        private Size _maximumScreenSize;

        protected readonly ReadOnlyArray<string> _commandLineArgs;
#endif

#if DEBUG
        private readonly DebugRenderer _debugRenderer = new DebugRenderer();
#endif

        // Initialion of the FlaiGameComponent's should be handled Game.

        #endregion

        #region Properties

        public bool IsLoaded { get; private set; }

        public new FlaiServiceContainer Services
        {
            get { return _serviceContainer; }
        }

        internal GraphicsDeviceManager GraphicsDeviceManager // TODO: DeleteEntity the property?
        {
            get { return _graphicsDeviceManager; }
        }

        public new FlaiContentManager Content
        {
            get { return base.Content as FlaiContentManager; }
        }

        public FontContainer FontContainer
        {
            get { return _graphicsContext.FontContainer; }
        }

        public int TargetFrameRate
        {
            set
            {
                this.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / FlaiMath.Clamp(value, 5, 120));
            }
        }

        public string WindowTitle
        {
            get { return Window.Title; }
            set { Window.Title = value ?? ""; }
        }

        public Size ScreenSize
        {
            get { return new Size(800, 480); /* base.GraphicsDevice.GetScreenSize(); */ }
        }

        public Rectangle ScreenArea
        {
            get { return base.GraphicsDevice.GetScreenArea(); }
        }

        public Size ViewportSize
        {
            get { return base.GraphicsDevice.GetViewportSize(); }
        }

        public Rectangle ViewportArea
        {
            get { return base.GraphicsDevice.GetViewportArea(); }
        }

        public InputState InputState
        {
            get { return _updateContext.InputState; }
        }

        public Color? ClearColor
        {
            get { return _clearColor; }
            protected set { _clearColor = value; }
        }

        // ... only use if *ABSOLUTELY* needed
        public GraphicsContext GraphicsContext
        {
            get { return _graphicsContext; }
        }

#if WINDOWS

        public ReadOnlyArray<string> CommandLineArgs
        {
            get { return _commandLineArgs; }
        }

        /// <summary>
        /// Sets the minimum size of the window. Note that this sets the size of XNA's backbuffer; actual window size is 16 pixels wider and 39 taller 
        /// </summary>
        public Size MinimumScreenSize
        {
            get { return _minimumScreenSize; }
            set
            {
                if (_minimumScreenSize != value)
                {
                    _minimumScreenSize = value;
                    this.WindowControl.MinimumSize = new System.Drawing.Size(_minimumScreenSize.Width + 16, _minimumScreenSize.Height + 39);
                }
            }
        }

        /// <summary>
        /// Sets the minimum size of the window. Note that this sets the size of XNA's backbuffer; actual window size is 16 pixels wider and 39 taller 
        /// </summary>
        public Size MaximumScreenSize
        {
            get { return _maximumScreenSize; }
            set
            {
                if (_maximumScreenSize != value)
                {
                    _maximumScreenSize = value;
                    this.WindowControl.MaximumSize = new System.Drawing.Size(_maximumScreenSize.Width + 16, _maximumScreenSize.Height + 39);
                }
            }
        }

        public Control WindowControl
        {
            get { return Control.FromHandle(this.Window.Handle); }
        }

        [Obsolete("You should use Cursor.Visible instead", false)]
        public new bool IsMouseVisible
        {
            get { return base.IsMouseVisible; }
            set { base.IsMouseVisible = value; }
        }

#endif

        #endregion

        protected FlaiGame()
        {
            _instance = this;

            base.Components.ComponentAdded += this.GameComponentAdded;
            base.Components.ComponentRemoved += this.GameComponentRemoved;

            // Should these be moved to Initialize() ?
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _serviceContainer = new FlaiServiceContainer(base.Services);
            _contentProvider = new ContentProvider(this.Services);
            _screenManager = new ScreenManager(this);

            _soundEffectManager = new SoundEffectManager(this.Services) { Enabled = false }; // Sounds are by default disabled
            base.Components.Add(_soundEffectManager);

            _updateContext = new UpdateContext(this);
            _graphicsContext = new GraphicsContext(this);

            base.IsFixedTimeStep = false;
            base.InactiveSleepTime = TimeSpan.Zero;
            this.TargetFrameRate = 60;
            base.Content = _contentProvider.DefaultManager;

#if WINDOWS
            _commandLineArgs = new ReadOnlyArray<string>(Environment.GetCommandLineArgs());

            this.MinimumScreenSize = new Size(128, 128);
            this.MaximumScreenSize = new Size(4096, 4096);
#endif

#if WINDOWS_PHONE
        //  this.HookPhoneApplicationServiceEvents(); // TODO
#endif

#if WINRT        
    Windows.UI.Input.PointerVisualizationSettings.GetForCurrentView().IsContactFeedbackEnabled = false;
    Windows.UI.Input.PointerVisualizationSettings.GetForCurrentView().IsBarrelButtonFeedbackEnabled = false;
#endif

            _graphicsDeviceManager.PreparingDeviceSettings += this.OnPreparingDeviceSettings;
        }

        protected sealed override void Initialize()
        {
            this.InitializeGraphicsSettings();
            _graphicsDeviceManager.ApplyChanges();

            _screenManager.Initialize();

            // Call overridable initialize method
            this.InitializeInner();
            base.Initialize();
        }

        protected sealed override void LoadContent()
        {
            _graphicsContext.LoadContent();
            this.LoadContentInner();
            base.LoadContent();

            // On Windows Phone, AddInitialScreens() is called by GameLaunching(o, e) or GameActivated(o, e)
#if WINDOWS || WP8_MONOGAME || ANDROID
            this.AddInitialScreens();
#endif

            this.IsLoaded = true;
        }

        protected sealed override void UnloadContent()
        {
            this.UnloadContentInner();
            base.UnloadContent();
        }

        protected sealed override void Update(GameTime gameTime)
        {
            _updateContext.Update(gameTime);
            _graphicsContext.Update(gameTime);

            this.UpdateInner(_updateContext);
            this.UpdateFlaiComponents();

            base.Update(gameTime);
        }

        // todo: hmm.. maybe I should just make it so that ScreenManager.Draw is called here and maybe Pre/Post draw called before/after it, which can be overridden. same for update
        protected sealed override void Draw(GameTime gameTime)
        {
            if (_clearColor.HasValue)
            {
                this.GraphicsDevice.Clear(_clearColor.Value);
            }

            this.DrawInner(_graphicsContext);
            this.DrawFlaiComponents();

            base.Draw(gameTime);

#if DEBUG
            _debugRenderer.Flush(_graphicsContext);
#endif

#if WINDOWS
            Cursor.Draw(_graphicsContext);
#endif
        }

        protected override void EndDraw()
        {
            _graphicsContext.SpriteBatch.VerifyIsReadyForEndOfFrame();
            base.EndDraw();
        }

        private void UpdateFlaiComponents()
        {
            _temporaryUpdateableComponents.AddAll(_updateableComponents);
            foreach (FlaiGameComponent gameComponent in _temporaryUpdateableComponents.Where(component => component.Enabled))
            {
                gameComponent.Update(_updateContext);
            }

            _temporaryUpdateableComponents.Clear();
        }

        private void DrawFlaiComponents()
        {
            _temporaryDrawableComponents.AddAll(_drawableComponents);
            foreach (FlaiDrawableGameComponent drawableGameComponent in _temporaryDrawableComponents.Where(component => component.Visible))
            {
                drawableGameComponent.Draw(_graphicsContext);
            }

            _temporaryDrawableComponents.Clear();
        }

        /// <summary>
        /// Adds initial screens to the screen manager
        /// </summary>
        protected virtual void AddInitialScreens() { }

        /// <summary>
        /// Initialize all graphics related settings here
        /// </summary>
        protected virtual void InitializeGraphicsSettings() { }

        /// <summary>
        /// Initialize everything inside here
        /// </summary>
        protected virtual void InitializeInner() { }

        /// <summary>
        /// Load all content inside here
        /// </summary>
        protected virtual void LoadContentInner() { }

        /// <summary>
        /// This is called when GraphicsDeviceManager.PreparingDeviceSettings is raised
        /// </summary>
        protected virtual void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e) { }

        /// <summary>
        /// Unload all content inside here
        /// </summary>
        protected virtual void UnloadContentInner() { }

        /// <summary>
        /// Update ScreenManager and other things inside here
        /// </summary>
        protected virtual void UpdateInner(UpdateContext updateContext) { }

        /// <summary>
        /// Draw ScreenManager and other things inside here
        /// </summary>
        protected virtual void DrawInner(GraphicsContext graphicsContext) { }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            _screenManager.UnloadContent();
        }

        public void ChangeResolution(Size newResolution)
        {
            this.ChangeResolution(newResolution.Width, newResolution.Height);
        }

        public void ChangeResolution(int width, int height)
        {
#if WINDOWS
            width = FlaiMath.Clamp(width, _minimumScreenSize.Width, _maximumScreenSize.Height);
            height = FlaiMath.Clamp(height, _minimumScreenSize.Width, _maximumScreenSize.Height);
#endif
            if (this.GraphicsDevice.PresentationParameters.BackBufferWidth == width && this.GraphicsDevice.PresentationParameters.BackBufferHeight == height)
            {
                return;
            }

            Size oldSize = this.ScreenSize;
            _graphicsDeviceManager.PreferredBackBufferWidth = width;
            _graphicsDeviceManager.PreferredBackBufferHeight = height;

            // If the game is exiting, ApplyChanges can throw an "ObjectDisposedException". In that case just catch and ignore it
            try
            {
                _graphicsDeviceManager.ApplyChanges();
            }
            catch { }

            var handler = this.ResolutionChanged;
            if (handler != null)
            {
                handler(oldSize, this.ScreenSize);
            }
        }

        #region Windows Phone Events
#if WINDOWS_PHONE
     /*   private void HookPhoneApplicationServiceEvents()
        {
            // Hook events on the PhoneApplicationService so we're notified of the application's life cycle
            PhoneApplicationService.Current.Launching +=
                new EventHandler<LaunchingEventArgs>(this.GameLaunching);
            PhoneApplicationService.Current.Activated +=
                new EventHandler<ActivatedEventArgs>(this.GameActivated);
            PhoneApplicationService.Current.Deactivated +=
                new EventHandler<DeactivatedEventArgs>(this.GameDeactivated);
        }

        private void GameLaunching(object sender, LaunchingEventArgs e)
        {
            this.AddInitialScreens();
        }

        private void GameActivated(object sender, ActivatedEventArgs e)
        {
            if (!_screenManager.Activate(e.IsApplicationInstancePreserved))
            {
                // If the screen manager fails to deserialize, add the initial screens
                this.AddInitialScreens();
            }
        }

        private void GameDeactivated(object sender, DeactivatedEventArgs e)
        {
            // Serialize the screen manager when the game deactivated
            _screenManager.Deactivate();
        }
        */
#endif
        #endregion

        #region Game Component Added/Removed

        private void GameComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            FlaiGameComponent gameComponent = e.GameComponent as FlaiGameComponent;
            if (gameComponent != null)
            {
                _updateableComponents.Add(gameComponent);

                FlaiDrawableGameComponent drawableGameComponent = e.GameComponent as FlaiDrawableGameComponent;
                if (drawableGameComponent != null)
                {
                    _drawableComponents.Add(drawableGameComponent);
                }
            }
        }

        private void GameComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
            FlaiGameComponent gameComponent = e.GameComponent as FlaiGameComponent;
            if (gameComponent != null)
            {
                // If component exists in updateable components, try to remove it from drawable components as well
                if (_updateableComponents.Remove(gameComponent))
                {
                    FlaiDrawableGameComponent drawableGameComponent = e.GameComponent as FlaiDrawableGameComponent;
                    if (drawableGameComponent != null)
                    {
                        _drawableComponents.Remove(drawableGameComponent);
                    }
                }
            }
        }

        #endregion

        #region Static

        private static FlaiGame _instance;

        // use this if you don't want to use the FlaiGameAttribute thingy
        public static void Run<T>()
            where T : FlaiGame, new()
        {
            using (T game = new T())
            {
                game.Run();
            }
        }

        /// <summary>
        /// Use with caution! You should pass FlaiServiceContainer and use it rather than use this. Should be used only if some API requires Game-instance to be passed
        /// </summary>
        public static FlaiGame Current
        {
            get { return _instance; }
        }

        /// <summary>
        /// Use this only when there is no way to access FlaiServiceContainer object in an OOP way
        /// </summary>
        public static FlaiServiceContainer ServiceContainer
        {
            get { return _instance.Services; }
        }

        #endregion
    }
}
