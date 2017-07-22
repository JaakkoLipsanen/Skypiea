namespace Flai.CBES.Components
{
    public class CLifeTime : PoolableComponent
    {
        public float TimeRemaining { get; private set; }
        public float TotalTime { get; private set; }

        public float NormalizedTimeRemaining
        {
            get { return this.TimeRemaining / this.TotalTime; }
        }

        public CLifeTime()
        {
        }

        public CLifeTime(float time)
        {
            this.Initialize(time);
        }

        public void Initialize(float time)
        {
            this.TotalTime = time;
            this.TimeRemaining = time;
        }

        protected internal override void PostUpdate(UpdateContext updateContext)
        {
            this.TimeRemaining -= updateContext.DeltaSeconds;
            if (this.TimeRemaining <= 0)
            {
                this.Entity.Delete();
            }
        }

        protected internal override void Cleanup()
        {
            this.TimeRemaining = -1;
            this.TotalTime = -1;
        }
    }
}
