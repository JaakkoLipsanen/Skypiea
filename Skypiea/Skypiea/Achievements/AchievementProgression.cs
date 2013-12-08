using Flai;

namespace Skypiea.Achievements
{
    public interface IAchievementProgression
    {
        bool IsUnlocked { get; }
    }

    // ugh!!!!! super awful name
    public interface IPercentableProgression : IAchievementProgression
    {
        int ProgressionPercent { get; }
        string ProgressionDisplayString { get; }
    }

    public class BooleanProgression : IAchievementProgression
    {
        public bool IsUnlocked { get; private set; }

        public BooleanProgression(bool isUnlocked)
        {
            this.IsUnlocked = isUnlocked;
        }

        public void Unlock()
        {
            this.IsUnlocked = true;
        }
    }

    public class IntProgression : IPercentableProgression
    {  
        private readonly int _max;
        private int _current;
        private string _progressionString;

        public int Current
        {
            get { return _current; }
            set
            {
                Ensure.WithinRange(value, 0, _max);
                if (_current != value)
                {
                    _current = value;
                    _progressionString = null;
                }
            }
        }

        public int Max
        {
            get { return _max; }
        }

        public bool IsUnlocked
        {
            get { return _current >= _max; }
        }

        int IPercentableProgression.ProgressionPercent
        {
            get { return (int)FlaiMath.Floor(_current / (float)_max); }
        }

        string IPercentableProgression.ProgressionDisplayString
        {
            get { return _progressionString ?? (_progressionString = _current + "/" + _max); } // oookaayy!!!
        }

        public IntProgression(int current, int max)
        {
            Ensure.True(current <= max);
            _current = current;
            _max = max;
        }
    }
}
