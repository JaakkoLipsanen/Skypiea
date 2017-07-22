
using System;

namespace Flai
{
    public struct RangeInt : IEquatable<RangeInt>
    {
        private readonly int _min;
        private readonly int _max;

        public int Min
        {
            get { return _min; }
        }

        public int Max
        {
            get { return _max; }
        }

        // Naming?
        public int Length
        {
            get { return _max - _min; }
        }

        public RangeInt(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public bool Contains(int value)
        {
            return _min <= value && value <= _max;
        }

        public bool Intersects(RangeInt other)
        {
            return (other.Min < _max && _max < other.Max) || (other.Min < _min && _min < other.Max) || (_min < other.Min && other.Min < _max) || (_min == other.Min && _max == other.Max);
        }

        public bool Intersects(Range other)
        {
            return (other.Min < _max && _max < other.Max) || (other.Min < _min && _min < other.Max) || (_min < other.Min && other.Min < _max) || (_min == other.Min && _max == other.Max);
        }

        public static explicit operator RangeInt(int value)
        {
            return new RangeInt(value, value);
        }

        #region IEquatable<Range> Members

        public bool Equals(RangeInt other)
        {
            return _min == other.Min && _max == other.Max;
        }

        #endregion
    }
}
