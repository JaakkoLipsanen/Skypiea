using Flai;
using Flai.Graphics;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombie.Screens;

namespace Zombie
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZombieGame : FlaiGame
    {
        public ZombieGame()
        {
            this.Components.Add(new DebugInformationComponent(this.Services) { DisplayPosition = new Vector2(9, 72), ShowExtras = false });    
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
            _graphicsDeviceManager.PreferMultiSampling = true;
        }

        protected override void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
        }
    }
}
