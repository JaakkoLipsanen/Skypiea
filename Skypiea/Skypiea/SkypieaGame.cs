using System;
using Flai;
using Flai.Graphics;
using Flai.Misc;
using Flai.Scoreloop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Leaderboards;
using Skypiea.Screens;
using Skypiea.Settings;
using Skypiea.Stats;

namespace Skypiea
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SkypieaGame : FlaiGame
    {
        private readonly ScoreloopManager _scoreloopManager;
        public SkypieaGame()
        {
            base.ClearColor = Color.Black;
            this.Components.Add(new DebugInformationComponent(this.Services) { DisplayPosition = new Vector2(9, 144), DebugInformationLevel = DebugInformationLevel.All, Visible = false });

            _serviceContainer.Add(SettingsHelper.CreateSettingsManager());
            _serviceContainer.Add(HighscoreHelper.CreateHighscoreManager());
            _serviceContainer.Add(StatsHelper.CreateStatsManager());
            _serviceContainer.Add(_scoreloopManager = LeaderboardHelper.CreateLeaderboardManager());
        }

        protected override void InitializeInner()
        {
            this.FontContainer.DefaultFont = this.FontContainer["Minecraftia.20"];
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _scoreloopManager.Close();
            base.OnExiting(sender, args);
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            _screenManager.Update(updateContext);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _screenManager.Draw(graphicsContext);
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
          //  e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = SurfaceFormat.Bgra4444;
        }
    }
}
