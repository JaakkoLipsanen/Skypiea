using System;
using Microsoft.Xna.Framework;

namespace Flai
{
    public struct Circle : IEquatable<Circle>
    {
        public float X;
        public float Y;

        public float Radius;

        public float Left
        {
            get { return this.X - this.Radius; }
        }

        public float Right
        {
            get { return this.X + this.Radius; }
        }

        public float Top
        {
            get { return this.Y - this.Radius; }
        }

        public float Bottom
        {
            get { return this.Y + this.Radius; }
        }

        public RectangleF SurroundingArea
        {
            get { return new RectangleF(this.X - this.Radius, this.Y - this.Radius, this.Radius * 2, this.Radius * 2); }
        }

        public Vector2 Position
        {
            get { return new Vector2(this.X, this.Y); }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public Circle(float x, float y, float radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;
        }

        public Circle(Vector2 position, float radius)
        {
            this.X = position.X;
            this.Y = position.Y;
            this.Radius = radius;
        }

        public bool Intersects(Circle other)
        {
            float totalRadius = this.Radius + other.Radius;
            float xDiff = this.X - other.X;
            float yDiff = this.Y - other.Y;

            return totalRadius * totalRadius >= xDiff * xDiff + yDiff * yDiff;
        }

        // probably not working (corners etc). should be only false-negatives, no false-positives
        public bool Intersects(RectangleF rectangle)
        {
            float circleDistanceX = Math.Abs(this.X - rectangle.X);
            float circleDistanceY = Math.Abs(this.Y - rectangle.Y);

            if ((circleDistanceX > rectangle.Width / 2 + this.Radius) || (circleDistanceY > rectangle.Width / 2 + this.Radius) || circleDistanceX <= rectangle.Width / 2 || (circleDistanceY <= rectangle.Height / 2))
            {
                return false;
            }

            float distanceX = circleDistanceX - rectangle.Width / 2;
            float distanceY = circleDistanceY - rectangle.Height / 2;

            return (distanceX * distanceX + distanceY * distanceY) <= this.Radius * this.Radius;
        }

        public bool Contains(Circle circle)
        {
            float distance = Vector2.Distance(this.Position, circle.Position); // squared would be faster
            return this.Radius >= distance + circle.Radius; // does this even work? probably...
        }

        public bool Contains(Vector2 point)
        {
            return Vector2.DistanceSquared(this.Position, point) < this.Radius * this.Radius; // correct?
        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); // meh.. can't use X/Y/Radius since they are not readonly.. make them readonly?
        }

        public override string ToString()
        {
            return string.Format("{ X = {0} Y = {1} Radius = {2}", this.X, this.Y, this.Radius);
        }

        public override bool Equals(object obj)
        {
            if (obj is Circle)
            {
                return this.Equals((Circle)obj);
            }
            return base.Equals(obj);
        }

        #region IEquatable<Circle> Members

        public bool Equals(Circle other)
        {
            return this.X == other.X && this.Y == other.Y && this.Radius == other.Radius;
        }

        #endregion

        public static bool operator ==(Circle first, Circle second)
        {
            return first.X == second.X && first.Y == second.Y && first.Radius == second.Radius;
        }

        public static bool operator !=(Circle first, Circle second)
        {
            return first.X != second.X || first.Y != second.Y || first.Radius == second.Radius;
        }

        public float MinDistance(Vector2 position)
        {
            if (this.Contains(position))
            {
                return 0;
            }

            return Vector2.Distance(this.Position, position) - this.Radius;
        }
    }
}
