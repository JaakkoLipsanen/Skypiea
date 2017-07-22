using Microsoft.Xna.Framework;

namespace Flai
{
    public interface IGameTime
    {
        float DeltaSeconds { get; }
        float TotalSeconds { get; }
        long TotalTicks { get; }
        long DeltaTicks { get; }
        double TimeScale { get; }

        GameTime XnaGameTime { get; }
    }

    internal class FlaiGameTime : IGameTime
    {
        private GameTime _gameTime = default(GameTime);
        private double _deltaSeconds = 0;
        private double _totalSeconds = 0;
        private long _totalTicks;
        private long _deltaTicks;
        private double _timeScale = 1;

        public float DeltaSeconds
        {
            get { return (float)_deltaSeconds; }
        }

        public float TotalSeconds
        {
            get { return (float)_totalSeconds; }
        }

        public long TotalTicks
        {
            get { return _totalTicks; }
        }

        public long DeltaTicks
        {
            get { return _deltaTicks; }
        }

        public double TimeScale
        {
            get { return _timeScale; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value > 0);

                _timeScale = value;
            }
        }

        // DO NOT USE THIS USUALLY!!! skips time scale etc
        public GameTime XnaGameTime
        {
            get { return _gameTime; }
        }

        internal void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _deltaSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            _deltaTicks = gameTime.ElapsedGameTime.Ticks;
            if (_timeScale != 1)
            {
                _deltaSeconds *= _timeScale;
                _deltaTicks = (long)(_deltaTicks * _timeScale);
            }

            _totalSeconds += _deltaSeconds;
            _totalTicks += _deltaTicks;
        }

        public void ResetTotalTime()
        {
            _totalSeconds = 0;
        }
    }
}
