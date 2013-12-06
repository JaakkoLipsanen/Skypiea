using Flai.CBES;

namespace Skypiea.Components
{
    public class CBasicZombieAI : PoolableComponent
    {
        // todo: state? (wandering around, walking towards player etc)
        public float Speed { get; private set; }

        public void Initialize(float speed)
        {
            this.Speed = speed;
        }

        protected override void Cleanup()
        {
            this.Speed = 0;
        }
    }
}
