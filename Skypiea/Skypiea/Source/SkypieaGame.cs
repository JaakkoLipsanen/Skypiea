using System;
using Flai;
using Flai.Graphics;
using Flai.IO;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Leaderboards;
using Skypiea.Screens;
using Skypiea.Settings;
using Skypiea.Stats;
using Skypiea.View;

namespace Skypiea
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SkypieaGame : FlaiGame
    {
        private readonly ScoreloopManager _scoreloopManager;
        private readonly SkypieaSettingsManager _settingsManager;
        private RenderTarget2D r;

        public SkypieaGame()
        {
            base.ClearColor = Color.Black;
            this.Components.Add(new DebugInformationComponent(this.Services) { DisplayPosition = new Vector2(9, 144), DebugInformationLevel = DebugInformationLevel.All, Visible = true });

            _serviceContainer.Add(_settingsManager = SettingsHelper.CreateSettingsManager());
            _serviceContainer.Add(HighscoreHelper.CreateHighscoreManager());
            _serviceContainer.Add(StatsHelper.CreateStatsManager());
            _serviceContainer.Add(_scoreloopManager = LeaderboardHelper.CreateLeaderboardManager());

            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferredBackBufferWidth = 800;
            _graphicsDeviceManager.PreferredBackBufferHeight = 480;
            _graphicsDeviceManager.PreferMultiSampling = false;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            _graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            _graphicsDeviceManager.ApplyChanges();
        }

        protected override void InitializeInner()
        {
            this.FontContainer.DefaultFont = this.FontContainer["Minecraftia.20"];
            r = new RenderTarget2D(GraphicsDevice, 800, 480, false, SurfaceFormat.Color, DepthFormat.Depth24, 4, RenderTargetUsage.DiscardContents);
            Flai.Input.InputState.TouchLocationScale = SkypieaViewConstants.RenderScale;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _scoreloopManager.Close();
            _settingsManager.Save();
            base.OnExiting(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            _settingsManager.Save();
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            _screenManager.Update(updateContext);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
        //  GraphicsDevice.SetRenderTarget(r);
            _screenManager.Draw(graphicsContext);
        //  GraphicsDevice.SetRenderTarget(null);

        // graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
        // graphicsContext.SpriteBatch.DrawFullscreen(r);
        //  graphicsContext.SpriteBatch.End();
        }

        protected override void AddInitialScreens()
        {
            _screenManager.AddScreen(new SkypieaSplashScreen());
        }

        protected override void InitializeGraphicsSettings()
        {
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferredBackBufferWidth = 800;
            _graphicsDeviceManager.PreferredBackBufferHeight = 480;
            _graphicsDeviceManager.PreferMultiSampling = false;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            _graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;

            // use 4-bit (per channel) color format for WP7. Atleast Omnia 7 has a horrible banding with SurfaceFormat.Color.
            // Lumia's don't probably have but whatever
            if (OperatingSystemHelper.Version == WindowsPhoneVersion.WP7)
            {
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = SurfaceFormat.Bgra4444;
            }
        }
    }
}