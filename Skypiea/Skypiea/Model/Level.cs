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
        }

        public void Initialize()
        {
            _world.Initialize();
            _world.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);
        }

        public void Update(UpdateContext updateContext)
        {
            _world.Update(updateContext);
        }

        public static Level Generate(WorldType worldType)
        {
            return new Level(WorldGenerator.Generate(worldType));
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            if (message.PlayerInfo.LivesRemaining == 0)
            {
                this.GameOver.InvokeIfNotNull();
            }
        }
    }
}
