using System;

namespace Flai
{
    public class FlaiRandom : Random
    {
        public FlaiRandom()
            : base()
        {
        }

        public FlaiRandom(int seed)
            : base(seed)
        {
        }

        public override int Next()
        {
            return base.Next();
        }

        public int Next(RangeInt range)
        {
            if (range.Length == 0)
            {
                return range.Min;
            }

            return range.Min + this.Next(range.Length + 1);
        }

        public uint NextUInt()
        {
            return (uint)(base.Sample() * uint.MaxValue);
        }

        public uint NextUInt(uint min, uint max)
        {
            return (uint)(base.Sample() * max + (max - min));
        }

        public double NextDouble(double min, double max)
        {
            return min + base.NextDouble() * (max - min);
        }

        public float NextFloat()
        {
            return (float)base.NextDouble();
        }

        public float NextFloat(Range range)
        {
            if (range.Length == 0)
            {
                return range.Min;
            }

            return (float)(range.Min + this.NextDouble() * (range.Max - range.Min));
        }

        public float NextFloat(float max)
        {
            return (float)(base.NextDouble() * max );
        }

        public float NextFloat(float min, float max)
        {
            return (float)(min + base.NextDouble() * (max - min));
        }

        public bool NextBoolean()
        {
            return base.NextDouble() > 0.5;
        }

        public byte NextByte()
        {
            return (byte)FlaiMath.Round(base.NextDouble() * 255); // 255 or 256?
        }

        public byte NextByte(byte min, byte max)
        {
            return (byte)FlaiMath.Round(min + base.NextDouble() * (max - min)); // 255 or 256?
        }

        public bool NextFromOdds(float odds)
        {
            Ensure.IsValid(odds);
            Ensure.WithinRange(odds, 0, 1);

            return base.NextDouble() < odds;
        }
    }
}
