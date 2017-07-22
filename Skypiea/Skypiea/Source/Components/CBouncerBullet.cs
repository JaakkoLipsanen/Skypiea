using System.Collections.Generic;
using Flai;
using Flai.CBES;

namespace Skypiea.Components
{
    public class CBouncerBullet : PoolableComponent
    {
        private readonly HashSet<Entity> _zombiesHits = new HashSet<Entity>(); 
        public int HitsRemaining { get; private set; }

        public void Initialize(int totalHits)
        {
            this.HitsRemaining = totalHits;
        }

        public bool HasHit(Entity zombie)
        {
            return _zombiesHits.Contains(zombie);
        }

        protected internal override void Cleanup()
        {
            this.HitsRemaining = -1;
            _zombiesHits.Clear();
        }

        public bool OnHit(Entity entityHit)
        {
            Assert.False(_zombiesHits.Contains(entityHit));

            _zombiesHits.Add(entityHit);
            return --this.HitsRemaining >= 0;
        }
    }
}
