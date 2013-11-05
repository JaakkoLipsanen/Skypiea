using Flai;
using Flai.Graphics;
using Zombie.Screens;

namespace Zombie
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZombieGame : FlaiGame
    {
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
    }
}
