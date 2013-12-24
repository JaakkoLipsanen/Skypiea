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
            this.Components.Add(new DebugInformationComponent(this.Services) { DisplayPosition = new Vector2(9, 104), DebugInformationLevel = DebugInformationLevel.DetailedFPSAndMemory });
            base.IsFixedTimeStep = false;
        }

        protected override void InitializeInner()
        {
            this.FontContainer.DefaultFont = this.FontContainer["Minecraftia.20"];
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
            _screenManager.AddScreen(new MenuBackgroundScreen());
            _screenManager.AddScreen(new MainMenuScreen());
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
