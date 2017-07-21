using Flai.CBES;

namespace Skypiea.Components
{
    public class CBasicZombieAI : PoolableComponent
    {
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
