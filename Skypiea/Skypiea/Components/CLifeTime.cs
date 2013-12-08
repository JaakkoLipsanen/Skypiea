using Flai;
using Flai.CBES;

namespace Skypiea.Components
{
    public class CLifeTime : PoolableComponent
    {
        public float TimeRemaining { get; private set; }
        public float TotalTime { get; private set; }

        protected override void PostUpdate(UpdateContext updateContext)
        {
            this.TimeRemaining -= updateContext.DeltaSeconds;
            if (this.TimeRemaining <= 0)
            {
                this.Entity.Delete();
            }
        }

        public void Initialize(float time)
        {
            this.TotalTime = time;
            this.TimeRemaining = time;
        }

        protected override void Cleanup()
        {
            this.TimeRemaining = -1;
            this.TotalTime = -1;
        }
    }
}
