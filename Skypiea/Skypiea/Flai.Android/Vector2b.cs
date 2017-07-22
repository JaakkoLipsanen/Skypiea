
using System;

namespace Flai
{
    public struct Vector2b : IEquatable<Vector2b>
    {
        public byte X;
        public byte Y;

        public Vector2b(byte value)
        {
            this.X = value;
            this.Y = value;
        }
        public Vector2b(byte x, byte y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2b)
            {
                Vector2b other = (Vector2b)obj;
                return this.X == other.X && this.Y == other.Y;
            }
            return base.Equals(obj);
        }

        public bool Equals(Vector2b other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
       
        public override int GetHashCode()
        {         
            return (int)(X ^ Y);
        }

        public override string ToString()
        {
            return "Vector2b (" + X + "," + Y + ")";
        }

        #region Operators

        public static bool operator ==(Vector2b a, Vector2b b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2b a, Vector2b b)
        {
            return !(a.X == b.X && a.Y == b.Y);
        }

        public static Vector2b operator +(Vector2b a, Vector2b b)
        {
            return new Vector2b((byte)(a.X + b.X), (byte)(a.Y + b.Y));
        }


        public static Vector2b operator -(Vector2b a, Vector2b b)
        {
            return new Vector2b((byte)(a.X - b.X), (byte)(a.Y - b.Y));
        }

        #endregion

        public static Vector2b Zero
        {
            get { return Vector2b.ZeroVector; }
        }

        public static Vector2b UnitX
        {
            get { return Vector2b.UnitXVector; }
        }

        public static Vector2b UnitY
        {
            get { return Vector2b.UnitYVector; }
        }

        public static Vector2b One
        {
            get { return Vector2b.OneVector; }
        }

        private static readonly Vector2b ZeroVector = new Vector2b(0);
        private static readonly Vector2b UnitXVector = new Vector2b(1, 0);
        private static readonly Vector2b UnitYVector = new Vector2b(0, 1);
        private static readonly Vector2b OneVector = new Vector2b(1, 1);
    }
}
