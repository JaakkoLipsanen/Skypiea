using Flai.CBES;

namespace Skypiea.Components
{
    public class CRicochetBullet : PoolableComponent
    {
        public int HitsRemaining { get; private set; }

        public void Initialize(int totalHits)
        {
            this.HitsRemaining = totalHits;
        }

        public void OnEnemyHit()
        {
            this.HitsRemaining--;
        }

        protected override void Cleanup()
        {
            this.HitsRemaining = -1;
        }
    }
}
