
namespace Flai.General
{
    // struct or class..? hmm...
    // "ReadOnlyTimer"? could be useful.. or not.. or just ITimer? with only HasFinished maybe?
    // todo: pause/stop/start/reset/whatever?
    public class Timer
    {
        private float _tickTime;
        private float _currentTime = 0f;
        private bool _isPaused = false;

        public float ElapsedTime
        {
            get { return _tickTime - _currentTime; }
        }

        public float NormalizedElapsedTime
        {
            get { return this.ElapsedTime/_tickTime; }
        }

        public bool HasFinished
        {
            get { return _currentTime <= 0; }
        }

        public float TickTime
        {
            get { return _tickTime; }
        }

        public Timer(float period)
        {
            Ensure.IsValid(period);
            Ensure.True(period > 0);

            _tickTime = period;
            _currentTime = _tickTime;
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

            _currentTime -= deltaSeconds;
            if (_currentTime < 0)
            {
                _currentTime = 0;
            }
        }

        public void ForceFinish()
        {
            _currentTime = 0;
        }

        public void Restart()
        {
            _currentTime = _tickTime;
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
