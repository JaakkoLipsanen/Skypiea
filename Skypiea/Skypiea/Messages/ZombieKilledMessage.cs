using Flai.CBES;

namespace Skypiea.Messages
{
    public class ZombieKilledMessage : Message
    {
        // todo: score etc
        public Entity Zombie { get; private set; }
        public ZombieKilledMessage(Entity zombie)
        {
            this.Zombie = zombie;
        }
    }
}
