
using System;
using System.IO;
using Flai.IO;
using Microsoft.Xna.Framework;

namespace Flai
{
    public struct Vector2i : IEquatable<Vector2i>, IBinarySerializable
    {
        public int X;
        public int Y;

        public Vector2i(int value)
        {
            this.X = value;
            this.Y = value;
        }

        public Vector2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2i(Vector2 vector2)
        {
            this.X = (int)vector2.X;
            this.Y = (int)vector2.Y;
        }

        #region IEquatable<Vector2i> Members

        public bool Equals(Vector2i other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is Vector2i)
            {
                Vector2i other = (Vector2i)obj;
                return this.X == other.X && this.Y == other.Y;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.X ^ this.Y;
        }

        public override string ToString()
        {
            return this.X + ", " + this.Y;
        }

        #region Static Methods

        public static int DistanceSquared(Vector2i value1, Vector2i value2)
        {
            int x = value1.X - value2.X;
            int y = value1.Y - value2.Y;

            return (x * x) + (y * y);
        }

        public static float Distance(Vector2i value1, Vector2i value2)
        {
            return FlaiMath.Sqrt(Vector2i.DistanceSquared(value1, value2));
        }

        public static Vector2i Min(Vector2i value1, Vector2i value2)
        {
            return new Vector2i(FlaiMath.Min(value1.X, value2.X), FlaiMath.Min(value1.Y, value2.Y));
        }

        public static Vector2i Max(Vector2i value1, Vector2i value2)
        {
            return new Vector2i(FlaiMath.Max(value1.X, value2.X), FlaiMath.Max(value1.Y, value2.Y));
        }

        public static Vector2i Clamp(Vector2i value, Vector2i min, Vector2i max)
        {
            return new Vector2i(FlaiMath.Clamp(value.X, min.X, max.X), FlaiMath.Clamp(value.Y, min.Y, max.Y));
        }

        #endregion

        #region Operators

        public static bool operator ==(Vector2i a, Vector2i b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2i a, Vector2i b)
        {
            return !(a.X == b.X && a.Y == b.Y);
        }

        public static Vector2i operator +(Vector2i a, Vector2i b)
        {
            return new Vector2i(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2i operator -(Vector2i a, Vector2i b)
        {
            return new Vector2i(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2i operator *(Vector2i a, Vector2i multiplier)
        {
            return new Vector2i(a.X * multiplier.X, a.Y * multiplier.Y);
        }

        public static Vector2i operator *(Vector2i a, int multiplier)
        {
            return new Vector2i(a.X * multiplier, a.Y * multiplier);
        }

        public static Vector2 operator *(Vector2i a, float multiplier)
        {
            return new Vector2(a.X * multiplier, a.Y * multiplier);
        }

        public static Vector2i operator /(Vector2i a, int divider)
        {
            return new Vector2i(a.X / divider, a.Y / divider);
        }

        public static Vector2 operator /(Vector2i a, float divider)
        {
            return new Vector2(a.X / divider, a.Y / divider);
        }

        public static Vector2i operator /(Vector2i a, Vector2i divider)
        {
            return new Vector2i(a.X / divider.X, a.Y / divider.Y);
        }

        public static Vector2i operator -(Vector2i a)
        {
            return new Vector2i(-a.X, -a.Y);
        }

        public static Vector2i operator +(Vector2i a, Vector2b b)
        {
            return new Vector2i(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2i operator -(Vector2i a, Vector2b b)
        {
            return new Vector2i(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2i operator *(Vector2i a, Vector2b b)
        {
            return new Vector2i(a.X * b.X, a.Y * b.Y);
        }

        public static implicit operator Vector2(Vector2i v)
        {
            return new Vector2 { X = v.X, Y = v.Y };
        }

        public static explicit operator Vector2i(Vector2 v)
        {
            return new Vector2i { X = (int)v.X, Y = (int)v.Y };
        }

        public static Vector2i Round(Vector2 v)
        {
            return new Vector2i((int)FlaiMath.Round(v.X), (int)FlaiMath.Round(v.Y));
        }

        #endregion

        public Vector2 ToVector2()
        {
            return new Vector2(this.X, this.Y);
        }

        public static Vector2i Zero
        {
            get { return Vector2i.ZeroVector; }
        }

        public static Vector2i UnitX
        {
            get { return Vector2i.UnitXVector; }
        }

        public static Vector2i UnitY
        {
            get { return Vector2i.UnitYVector; }
        }

        public static Vector2i One
        {
            get { return Vector2i.OneVector; }
        }

        public static Vector2i MinValue
        {
            get { return Vector2i.MinVector; }
        }

        public static Vector2i MaxValue
        {
            get { return Vector2i.MaxVector; }
        }

        private static readonly Vector2i ZeroVector = new Vector2i(0);
        private static readonly Vector2i UnitXVector = new Vector2i(1, 0);
        private static readonly Vector2i UnitYVector = new Vector2i(0, 1);
        private static readonly Vector2i OneVector = new Vector2i(1, 1);
        private static readonly Vector2i MinVector = new Vector2i(int.MinValue, int.MinValue);
        private static readonly Vector2i MaxVector = new Vector2i(int.MaxValue, int.MaxValue);

        #region IBinarySerializable Members

        void IBinarySerializable.Write(BinaryWriter writer)
        {
            writer.Write(this.X);
            writer.Write(this.Y);
        }

        void IBinarySerializable.Read(BinaryReader reader)
        {
            this.X = reader.ReadInt32();
            this.Y = reader.ReadInt32();
        }

        #endregion
    }
}