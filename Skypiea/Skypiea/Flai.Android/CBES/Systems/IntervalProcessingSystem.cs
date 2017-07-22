using System;
using Flai.DataStructures;

namespace Flai.CBES.Systems
{
    public class IntervalProcessingSystem : ProcessingSystem
    {
        // todo: could use just "ticks" fields but still expose the interval as TimeSpan to outside
        private TimeSpan _interval;
        private TimeSpan _currentTime = TimeSpan.Zero;

        protected TimeSpan Interval
        {
            get { return _interval; }
            set
            {
                Ensure.True(value >= TimeSpan.Zero);
                _interval = value;
            }
        }

        public IntervalProcessingSystem(Aspect aspect)
            : this(aspect, TimeSpan.FromSeconds(0.5))
        {
        }

        public IntervalProcessingSystem(Aspect aspect, TimeSpan interval) 
            : base(aspect)
        {
            this.Interval = interval;
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            _currentTime += TimeSpan.FromTicks(updateContext.GameTime.DeltaTicks);
            if (_currentTime >= _interval)
            {
                _currentTime -= _interval;
                this.ProcessInner(updateContext, entities);
            }
        }

        // meh name
        protected virtual void ProcessInner(UpdateContext updateContext, ReadOnlyBag<Entity> entities) { }
    }
}
