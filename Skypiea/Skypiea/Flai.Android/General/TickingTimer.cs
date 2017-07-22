
namespace Flai.General
{
    // todo: pause/stop/start/reset/whatever?
    public class TickingTimer
    {
        public event GenericEvent Tick;

        private float _tickTime;
        private float _currentTime = 0f;
        private bool _isPaused = false;

        public float ElapsedTime
        {
            get { return _tickTime - _currentTime; }
        }

        public float NormalizedElapsedTime
        {
            get { return this.ElapsedTime / _tickTime; }
        }

        public float TickTime
        {
            get { return _tickTime; }
        }

        public bool IsTicked { get; private set; }

        public TickingTimer(float period)
        {
            this.SetTickTime(period);
            _currentTime = period;
        }

        public void Update(UpdateContext updateContext)
        {
            this.Update(updateContext.DeltaSeconds);
        }

        public void Update(float deltaSeconds)
        {
            if (_isPaused)
            {
                return;
            }

            if (this.IsTicked)
            {
                this.IsTicked = false;
            }

            _currentTime -= deltaSeconds;
            if (_currentTime < 0)
            {
                this.IsTicked = true;
                this.Tick.InvokeIfNotNull();
                _currentTime += _tickTime;
            }
        }

        public void Continue()
        {
            Ensure.True(_isPaused);
            _isPaused = false;
        }

        public void Pause()
        {
            Ensure.False(_isPaused);
            _isPaused = true;
        }

        public void SetTickTime(float period)
        {
            Ensure.IsValid(period);
            Ensure.True(period > 0);

            _tickTime = period;
            if (_currentTime > period)
            {
                _currentTime = period;
            }
        }
    }
}
