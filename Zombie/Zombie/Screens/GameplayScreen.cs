using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Zombie.Model;
using Zombie.View;

namespace Zombie.Screens
{
    public class GameplayScreen : GameScreen
    {
        private readonly Level _level;
        private readonly LevelRenderer _levelRenderer;

        public GameplayScreen()
        {
            _level = Level.Generate();
            _levelRenderer = new LevelRenderer(_level);
        }

        protected override void LoadContent(bool instancePreserved)
        {
            _level.Initialize();
            _levelRenderer.LoadContent();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _level.Update(updateContext);
            _levelRenderer.Update(updateContext);
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            _levelRenderer.Draw(graphicsContext);
        }
    }
}
