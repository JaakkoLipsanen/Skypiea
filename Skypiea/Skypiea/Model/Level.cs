using Flai;
using Skypiea.Messages;

namespace Skypiea.Model
{
    public class Level
    {
        public event GenericEvent GameOver; 

        private readonly World _world;
        public World World
        {
            get { return _world; }
        }

        private Level(World world)
        {
            _world = world;
            _world.EntityWorld.SubscribeToMessage<GameOverMessage>(this.OnGameOver);
        }

        public void Update(UpdateContext updateContext)
        {
            _world.Update(updateContext);
        }

        private void OnGameOver(GameOverMessage message)
        {
            this.GameOver.InvokeIfNotNull();
        }

        public static Level Generate(WorldType worldType)
        {
            return new Level(WorldGenerator.Generate(worldType));
        }
    }
}
