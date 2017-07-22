
using System;

namespace Flai
{
    public struct Vector3b : IEquatable<Vector3b>
    {
        public byte X;
        public byte Y;
        public byte Z;

        public Vector3b(byte x, byte y, byte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3b)
            {
                Vector3b other = (Vector3b)obj;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }
            return base.Equals(obj);
        }

        #region IEquatable<Vector3b> Members

        public bool Equals(Vector3b other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        #endregion
     
        public override int GetHashCode()
        {        
            return (int)(this.X ^ this.Y ^ this.Z);
        }

        public override string ToString()
        {
            return ("Vector3b (" + this.X + "," + this.Y + "," + this.Z + ")");
        }

        #region Operators

        public static bool operator ==(Vector3b a, Vector3b b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3b a, Vector3b b)
        {
            return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }

        public static Vector3b operator +(Vector3b a, Vector3b b)
        {
            return new Vector3b((byte)(a.X + b.X), (byte)(a.Y + b.Y), (byte)(a.Z + b.Z));
        }

        public static Vector3b operator -(Vector3b a, Vector3b b)
        {
            return new Vector3b(
                (byte)(a.X - b.X),
                (byte)(a.Y - b.Y),
                (byte)(a.Z - b.Z));
        }

        #endregion

        public static Vector3b Zero
        {
            get { return Vector3b.ZeroVector; }
        }

        public static Vector3b UnitX
        {
            get { return Vector3b.UnitXVector; }
        }

        public static Vector3b UnitY
        {
            get { return Vector3b.UnitYVector; }
        }

        public static Vector3b UnitZ
        {
            get { return Vector3b.UnitZVector; }
        }

        public static Vector3b One
        {
            get { return Vector3b.OneVector; }
        }

        private static readonly Vector3b ZeroVector = new Vector3b(0, 0, 0);
        private static readonly Vector3b UnitXVector = new Vector3b(1, 0, 0);
        private static readonly Vector3b UnitYVector = new Vector3b(0, 1, 0);
        private static readonly Vector3b UnitZVector = new Vector3b(0, 1, 0);
        private static readonly Vector3b OneVector = new Vector3b(1, 1, 1);
    }
}
