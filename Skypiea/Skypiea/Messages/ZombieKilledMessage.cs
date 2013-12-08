using Flai.CBES;

namespace Skypiea.Messages
{
    public class ZombieKilledMessage : PoolableMessage
    {
        // todo: score etc
        public Entity Zombie { get; private set; }
        public ZombieKilledMessage Initialize(Entity zombie)
        {
            this.Zombie = zombie;
            return this;
        }

        protected override void Cleanup()
        {
            this.Zombie = null;
        }
    }
}
