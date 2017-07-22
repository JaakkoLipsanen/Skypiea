using System;
using Microsoft.Xna.Framework;

namespace Flai
{
    #region Size

    public struct Size : IEquatable<Size>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public float AspectRatio
        {
            get { return (float)this.Width / this.Height; }
        }

        public Size(Vector2i size)
            : this(size.X, size.Y)
        {
        }

        public Size(int width, int height)
            : this()
        {
            Ensure.True(width >= 0 && height >= 0);

            this.Width = width;
            this.Height = height;
        }

        public static Vector2 operator /(Size size, float value)
        {
            return new Vector2(size.Width / value, size.Height / value);
        }

        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return size1.Width != size2.Width || size1.Height != size2.Height;
        }

        public static implicit operator Vector2i(Size size)
        {
            return new Vector2i(size.Width, size.Height);
        }

        public Vector2i ToVector2i()
        {
            return new Vector2i(this.Width, this.Height);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(0, 0, this.Width, this.Height);
        }

        public bool Equals(Size other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                return (Size)obj == this;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Width ^ this.Height;
        }

        public override string ToString()
        {
            return this.Width + ", " + this.Height;
        }

        public static readonly Size Empty = new Size(0, 0);
        public static readonly Size Invalid = new Size() { Width = -1, Height = -1 };
    }

    #endregion

    #region SizeF

    // TODO: This is currently not used anywhere. Make it used :P! (or delete this class)
    public struct SizeF : IEquatable<SizeF>
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        public float AspectRatio
        {
            get { return (float)this.Width / this.Height; }
        }

        public SizeF(Vector2 size)
            : this(size.X, size.Y)
        {
        }

        public SizeF(float width, float height)
            : this()
        {
            Ensure.True(width >= 0 && height >= 0);

            this.Width = width;
            this.Height = height;
        }

        public static bool operator ==(SizeF size1, SizeF size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        public static bool operator !=(SizeF size1, SizeF size2)
        {
            return size1.Width != size2.Width || size1.Height != size2.Height;
        }

        public static SizeF operator +(SizeF size1, SizeF size2)
        {
            return new SizeF(size1.Width + size2.Width, size1.Height + size2.Height);
        }

        public static implicit operator Vector2(SizeF size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(this.Width, this.Height);
        }

        public RectangleF ToRectangle()
        {
            return new RectangleF(0, 0, this.Width, this.Height);
        }

        public bool Equals(SizeF other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is SizeF)
            {
                return (SizeF)obj == this;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }

        public override string ToString()
        {
            return this.Width + ", " + this.Height;
        }

        public static readonly SizeF Empty = new SizeF(0, 0);
        public static readonly SizeF Invalid = new SizeF() { Width = -1, Height = -1 };
    }

    #endregion
}
