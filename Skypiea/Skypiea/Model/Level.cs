using Flai;
using Flai.CBES;
using Flai.Ui;
using Skypiea.Messages;

namespace Skypiea.Model
{
    public class Level
    {
        public event GenericEvent GameOver; 

        private readonly World _world;
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();

        public World World
        {
            get { return _world; }
        }

        public EntityWorld EntityWorld
        {
            get { return _world.EntityWorld; }
        }

        public BasicUiContainer UiContainer
        {
            get { return _uiContainer; }
        }

        private Level(World world)
        {
            _world = world;
            _world.EntityWorld.SubscribeToMessage<GameOverMessage>(this.OnGameOver);
        }

        public void Update(UpdateContext updateContext)
        {
            _world.Update(updateContext);
            _uiContainer.Update(updateContext);
        }

        private void OnGameOver(GameOverMessage message)
        {
            this.GameOver.InvokeIfNotNull();
        }

        public static Level Generate()
        {
            return new Level(WorldGenerator.Generate());
        }
    }
}
