using System.IO;
using Flai.IO;

namespace Flai.Achievements
{
    public abstract class AchievementProgression : IBinarySerializable
    {
        internal event GenericEvent Unlocked;
        public abstract bool IsUnlocked { get; }

        protected void OnAchievementUnlocked()
        {
            this.Unlocked.InvokeIfNotNull();
        }

        void IBinarySerializable.Write(BinaryWriter writer)
        {
            this.WriteInner(writer);
        }

        void IBinarySerializable.Read(BinaryReader reader)
        {
            this.ReadInner(reader);
        }

        protected virtual void WriteInner(BinaryWriter writer) { }
        protected virtual void ReadInner(BinaryReader reader) { }
    }

    public abstract class PercentableAchievementProgression : AchievementProgression
    {
        public abstract float ProgressionPercent { get; } // [0, 1]
        public abstract string ProgressionDisplayString { get; }
    }

    #region BooleanProgression

    public class BooleanProgression : AchievementProgression
    {
        private bool _isUnlocked = false;
        public override bool IsUnlocked
        {
            get { return _isUnlocked; }
        }

        public BooleanProgression(bool isUnlocked)
        {
            _isUnlocked = isUnlocked;
        }

        public void Unlock()
        {
            if (!this.IsUnlocked)
            {
                _isUnlocked = true;
                this.OnAchievementUnlocked();
            }
        }

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write(_isUnlocked);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            _isUnlocked = reader.ReadBoolean();
        }
    }

    #endregion

    #region IntegerProgression

    public class IntegerProgression : PercentableAchievementProgression
    {
        private readonly int _max;
        private int _current;
        private string _progressionString;

        public int Current
        {
            get { return _current; }
            set
            {
                Ensure.WithinRange(value, _current, _max);
                if (_current != value)
                {
                    _current = value;
                    _progressionString = null;

                    if (_current >= _max)
                    {
                        this.OnAchievementUnlocked();
                    }
                }
            }
        }

        public int Max
        {
            get { return _max; }
        }

        public override bool IsUnlocked
        {
            get { return _current >= _max; }
        }

        public override float ProgressionPercent
        {
            get { return FlaiMath.Floor(_current / (float)_max); }
        }

        public override string ProgressionDisplayString
        {
            get { return _progressionString ?? (_progressionString = _current + "/" + _max);; }
        }

        public IntegerProgression(int current, int max)
        {
            Ensure.True(current <= max);
            _current = current;
            _max = max;
        }

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write(_current);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            _current = reader.ReadInt32();
        }
    }

    #endregion

    #region FloatingPointProgression

    public class FloatProgression : PercentableAchievementProgression
    {
        private readonly int _max; // let's keep max as int, yea?
        private float _current;
        private string _progressionString;

        public float Current
        {
            get { return _current; }
            set
            {
                Ensure.WithinRange(value, _current, _max);
                if (_current != value)
                {
                    _current = value;
                    _progressionString = null;

                    if (_current >= _max)
                    {
                        this.OnAchievementUnlocked();
                    }
                }
            }
        }

        public int Max
        {
            get { return _max; }
        }

        public override bool IsUnlocked
        {
            get { return _current >= _max; }
        }

        public override float ProgressionPercent
        {
            get { return FlaiMath.Floor(_current / _max); }
        }

        public override string ProgressionDisplayString
        {
            get { return _progressionString ?? (_progressionString = ((int)FlaiMath.Floor(_current)) + "/" + _max); ; }
        }

        public FloatProgression(int current, int max)
        {
            Ensure.True(current <= max);
            _current = current;
            _max = max;
        }

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write(_current);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            _current = reader.ReadSingle();
        }
    }

    #endregion
}
