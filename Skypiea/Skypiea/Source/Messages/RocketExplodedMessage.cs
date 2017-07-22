using Flai.CBES;
using Flai.DataStructures;

namespace Skypiea.Messages
{
    // what :D really awful name and even more awful message (should be done some other way) but whatever...
    public class RocketExplodedMessage : PoolableMessage
    {
        private readonly Bag<Entity> _killedZombies = new Bag<Entity>();
        public ReadOnlyBag<Entity> KilledZombies { get; private set; }

        public RocketExplodedMessage()
        {
            this.KilledZombies = new ReadOnlyBag<Entity>(_killedZombies);
        }

        public void Add(Entity killedZombie)
        {
            _killedZombies.Add(killedZombie);
        }

        protected internal override void Cleanup()
        {
            _killedZombies.Clear();
        }
    }
}
