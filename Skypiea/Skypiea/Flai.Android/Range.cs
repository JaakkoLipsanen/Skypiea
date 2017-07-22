
using Flai.Diagnostics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using System;
using Color = Microsoft.Xna.Framework.Color;

namespace Flai
{
    public interface IRange<T>
    {
        T Min { get; }
        T Max { get; }

        bool Contains(T value);
        bool Intersects<TRange>(TRange other) where TRange : IRange<T>;
    }

    #region Range (float)

    public struct Range : IRange<float>, IEquatable<Range>
    {
        public static Range Zero = new Range(0, 0);
        public static Range FullRotation = new Range(-FlaiMath.Pi, FlaiMath.Pi); // meh name

        private readonly float _min;
        private readonly float _max;

        public float Min
        {
            get { return _min; }
        }

        public float Max
        {
            get { return _max; }
        }

        public float Average
        {
            get { return (_max + _min) / 2f; }
        }

        // Naming?
        public float Length
        {
            get { return _max - _min; }
        }

        public Range(float min, float max)
        {
            Ensure.IsValid(min);
            Ensure.IsValid(max);
            Ensure.True(max >= min, "Maximum can't be smaller than minimium");

            _min = min;
            _max = max;
        }

        public Range AsInflated(float amount)
        {
            return new Range(this.Min - amount, this.Max + amount * 2);
        }

        public bool Contains(float value)
        {
            return value >= _min && value <= _max;
        }

        public bool Intersects<TRange>(TRange other)
            where TRange : IRange<float>
        {
            return (other.Min < _max && _max < other.Max) || (other.Min < _min && _min < other.Max) || (_min < other.Min && other.Min < _max) || (_min == other.Min && _max == other.Max);
        }

        public bool Intersects(Range other)
        {
            return (other.Min < _max && _max < other.Max) || (other.Min < _min && _min < other.Max) || (_min < other.Min && other.Min < _max) || (_min == other.Min && _max == other.Max);
        }

        public bool Intersects(RangeInt other)
        {
            return (other.Min < _max && _max < other.Max) || (other.Min < _min && _min < other.Max) || (_min < other.Min && other.Min < _max) || (_min == other.Min && _max == other.Max);
        }

        public static implicit operator Range(float value)
        {
            return new Range(value, value);
        }

        public static Range CreateCentered(float center, float length)
        {
            Ensure.IsValid(center);
            Ensure.IsValid(length);
            Ensure.True(length > 0);

            return new Range(center - length * 0.5f, center + length * 0.5f);
        }

        #region IEquatable<Range> Members

        public bool Equals(Range other)
        {
            return _min == other.Min && _max == other.Max;
        }

        #endregion
    }

    #endregion

    #region Range<T>

    public struct Range<T> : IRange<T>, IEquatable<Range<T>>
         where T : IComparable<T>
    {
        private readonly T _min;
        private readonly T _max;

        public T Min
        {
            get { return _min; }
        }

        public T Max
        {
            get { return _max; }
        }

        public Range(T min, T max)
        {
            Ensure.True(max.IsGreaterThanOrEqual(min), "Maximum can't be smaller than minimium");

            _min = min;
            _max = max;
        }

        public bool Contains(T value)
        {
            return value.IsGreaterThanOrEqual(_min) && value.IsLessThanOrEqual(_max);
        }

        public bool Intersects<TRange>(TRange other)
            where TRange : IRange<T>
        {
            return (other.Min.IsLessThan(_max) && _max.IsLessThan(other.Max)) || (other.Min.IsLessThan(_min) && _min.IsLessThan(other.Max)) || (_min.IsLessThan(other.Min) && other.Min.IsLessThan(_max)) || (_min.IsEqual(other.Min) && _max.IsEqual(other.Max));
        }

        public bool Intersects(Range<T> other)
        {
            return (other.Min.IsLessThan(_max) && _max.IsLessThan(other.Max)) || (other.Min.IsLessThan(_min) && _min.IsLessThan(other.Max)) || (_min.IsLessThan(other.Min) && other.Min.IsLessThan(_max)) || (_min.IsEqual(other.Min) && _max.IsEqual(other.Max));
        }

        public static implicit operator Range<T>(T value)
        {
            return new Range<T>(value, value);
        }

        #region IEquatable<Range> Members

        public bool Equals(Range<T> other)
        {
            return _min.IsEqual(other.Min) && _max.IsEqual(other.Max); // don't use Equals.. or should it?
        }

        #endregion
    }

    #endregion

    #region ColorRange

    public abstract class ColorRange : IRange<Color>, IEquatable<ColorRange>
    {
        Color IRange<Color>.Min
        {
            get { return this.MinInner; }
        }

        Color IRange<Color>.Max
        {
            get { return this.MaxInner; }
        }

        protected virtual Color MinInner
        {
            get { throw new NotSupportedException(""); }
        }

        protected virtual Color MaxInner
        {
            get { throw new NotSupportedException(""); }
        }

        private ColorRange() { }

        public static ColorRange FromRGB(Range<byte> redRange, Range<byte> greenRange, Range<byte> blueRange)
        {
            return new RgbColorRange(redRange, greenRange, blueRange);
        }

        public static ColorRange FromHSV(Range hRange, Range sRange, Range vRange)
        {
            return new HsvColorRange(hRange, sRange, vRange);
        }

        public static implicit operator ColorRange(Color color)
        {
            return new RgbColorRange(new Range<byte>(color.R, color.R), new Range<byte>(color.G, color.G), new Range<byte>(color.B, color.B));
        }

        public static implicit operator ColorRange(HsvColor color)
        {
            return new HsvColorRange(new Range(color.H, color.H), new Range(color.S, color.S), new Range(color.V, color.V));
        }

        public abstract Vector3 NextVector3(FlaiRandom random);
        public abstract Color NextColor(FlaiRandom random);
        public abstract bool Contains(Color value);

        public bool Equals(ColorRange other)
        {
            return this == other;
        }

        bool IRange<Color>.Intersects<TRange>(TRange other)
        {
            throw new NotImplementedException("not sure how to implement... possibly doable logically but cant think right now");
        }

        private class HsvColorRange : ColorRange
        {
            private readonly Range _h;
            private readonly Range _s;
            private readonly Range _v;

            public HsvColorRange(Range h, Range s, Range v)
            {
                Ensure.True(h.Min >= 0);
                Ensure.True(s.Min >= 0);
                Ensure.True(v.Min >= 0);

                Ensure.True(s.Max <= 1);
                Ensure.True(v.Max <= 1);

                _h = h;
                _s = s;
                _v = v;
            }

            public override Vector3 NextVector3(FlaiRandom random)
            {
                return new HsvColor(random.NextFloat(_h), random.NextFloat(_s), random.NextFloat(_v)).ToRgb().ToVector3();
            }

            public override Color NextColor(FlaiRandom random)
            {
                return new HsvColor(random.NextFloat(_h), random.NextFloat(_s), random.NextFloat(_v)).ToRgb();
            }

            public override bool Contains(Color value)
            {
                HsvColor color = HsvColor.FromRgb(value); // Not sure if this works. since "hue" can be anything over 0 (and can make "rounds"), i'm pretty sure this fails... but whatever
                return Check.WithinRange(color.H, _h) && Check.WithinRange(color.S, _s) && Check.WithinRange(color.V, _v);
            }
        }

        private class RgbColorRange : ColorRange
        {
            private readonly Range _red;
            private readonly Range _green;
            private readonly Range _blue;

            protected override Color MinInner
            {
                get { return new Color(_red.Min, _green.Min, _blue.Min); }
            }

            protected override Color MaxInner
            {
                get { return new Color(_red.Max, _green.Max, _blue.Max); }
            }

            public RgbColorRange(Range<byte> redRange, Range<byte> greenRange, Range<byte> blueRange)
            {
                _red = new Range(redRange.Min / 255f, redRange.Max / 255f);
                _green = new Range(greenRange.Min / 255f, greenRange.Max / 255f);
                _blue = new Range(blueRange.Min / 255f, blueRange.Max / 255f);
            }

            public override Vector3 NextVector3(FlaiRandom random)
            {
                return new Vector3(random.NextFloat(_red), random.NextFloat(_green), random.NextFloat(_blue));
            }

            public override Color NextColor(FlaiRandom random)
            {
                return new Color(random.NextFloat(_red), random.NextFloat(_green), random.NextFloat(_blue));
            }

            public override bool Contains(Color value)
            {
                return _red.Contains(value.R / 255f) && _green.Contains(value.G / 255f) && _blue.Contains(value.B / 255f);
            }
        }
    }

    #endregion
}
