using Flai;
using Flai.Graphics;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Screens;

namespace Skypiea
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SkypieaGame : FlaiGame
    {
        public SkypieaGame()
        {
            base.ClearColor = Color.Black;
            this.Components.Add(new DebugInformationComponent(this.Services) { Font = "SegoeWP.24", DisplayPosition = new Vector2(9, 80), DebugInformationLevel = DebugInformationLevel.All });
            base.IsFixedTimeStep = false;
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
            _screenManager.AddScreen(new GameplayScreen());
        }

        protected override void InitializeGraphicsSettings()
        {
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferredBackBufferWidth = 800;
            _graphicsDeviceManager.PreferredBackBufferHeight = 480;
            _graphicsDeviceManager.PreferMultiSampling = false;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
        }

        protected override void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
        }
    }
}
