using Microsoft.Xna.Framework;
using System;

namespace Flai
{
    public struct Ray2D : IEquatable<Ray2D>
    {
        public Vector2 Direction;
        public Vector2 Position;

        public Ray2D(Vector2 position, Vector2 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        public Ray2D(ITransform2D transform)
            : this(transform.Position, transform.GetRotationVector())
        {           
        }

        #region Static Methods

        #region Intersects(Segment2D)

        public bool Intersects(Segment2D segment)
        {
            Vector2 intersectionPoint;
            return this.Intersects(segment, out intersectionPoint);
        }

        public bool Intersects(Segment2D segment, out Vector2 intersectionPoint)
        {
            const float Epsilon = 0.00001f;
            if (segment.IsPoint)
            {
                if (this.Intersects(segment.Start))
                {
                    intersectionPoint = segment.Start;
                    return true;
                }

                intersectionPoint = default(Vector2);
                return false;
            }

            Vector2 segmentDifference = (segment.End - segment.Start);
            float check = Vector2.Dot(GetPerpendicularVector(segmentDifference), this.Direction);

            // Ray and segment are parallel
            if (Math.Abs(check) < Epsilon)
            {
                if (this.Intersects(segment.Start))
                {
                    intersectionPoint = segment.Start;
                    return true;
                }
                else if (this.Intersects(segment.End))
                {
                    intersectionPoint = segment.End;
                    return true;
                }
                intersectionPoint = default(Vector2);
                return false;
            }

            float s = Vector2.Dot(GetPerpendicularVector(segmentDifference), segment.Start - this.Position) / check;
            float t = Vector2.Dot(GetPerpendicularVector(this.Direction), segment.Start - this.Position) / check;

            if (s >= 0 && t >= 0 && t <= 1)
            {
                intersectionPoint = segment.Start + t * segmentDifference;
                return true;
            }

            intersectionPoint = default(Vector2);
            return false;
        }

        #endregion

        #region Intersects(Vector2)

        public bool Intersects(Vector2 p)
        {
            const float Epsilon = 0.0001f;
            /*    float slope = (ray.DisplayPosition.Y - (ray.DisplayPosition.Y + ray.Direction.Y)) / (ray.DisplayPosition.X - (ray.DisplayPosition.X + ray.Direction.X));
                float yIntersect = -slope * ray.DisplayPosition.X / ray.DisplayPosition.Y;

                float check = slope * v.X * yIntersect;

            Vector2 A = ray.DisplayPosition;
            Vector2 B = ray.DisplayPosition + ray.Direction * int.MaxValue; // ...
            float normalLength = Hypotenuse(B.X - A.X, B.Y - A.Y);
            float distance = Math.Abs((P.X - A.X) * (B.Y - A.Y) - (P.Y - A.Y) * (B.Y - A.Y)) / normalLength;

            return Math.Abs(check - v.Y) < Epsilon;*/

            Vector2 pOrig = Vector2.Normalize(p - this.Position);
            return Math.Abs(this.Direction.X - pOrig.X) < Epsilon && Math.Abs(this.Direction.Y - pOrig.Y) < Epsilon;
        }

        #endregion

        #region Intersects(RectangleF)

        public bool Intersects(RectangleF rectangle)
        {
            Vector2 intersectionPoint;
            return this.Intersects(rectangle, out intersectionPoint);
        }

        // meh.. brute-force
        public bool Intersects(RectangleF rectangle, out Vector2 intersectionPoint)
        {
            intersectionPoint = default(Vector2);

            Vector2 temporaryIntersectionPoint;
            float bestIntersectionDistanceSquared = float.MaxValue;
            if (this.Intersects(rectangle.GetSideSegment(Direction2D.Left), out temporaryIntersectionPoint))
            {
                float lengthSquared = temporaryIntersectionPoint.LengthSquared();
                if (lengthSquared < bestIntersectionDistanceSquared)
                {
                    intersectionPoint = temporaryIntersectionPoint;
                    bestIntersectionDistanceSquared = lengthSquared;
                }
            }

            if (this.Intersects(rectangle.GetSideSegment(Direction2D.Right), out temporaryIntersectionPoint))
            {
                float lengthSquared = temporaryIntersectionPoint.LengthSquared();
                if (lengthSquared < bestIntersectionDistanceSquared)
                {
                    intersectionPoint = temporaryIntersectionPoint;
                    bestIntersectionDistanceSquared = lengthSquared;
                }
            }

            if (this.Intersects(rectangle.GetSideSegment(Direction2D.Up), out temporaryIntersectionPoint))
            {
                float lengthSquared = temporaryIntersectionPoint.LengthSquared();
                if (lengthSquared < bestIntersectionDistanceSquared)
                {
                    intersectionPoint = temporaryIntersectionPoint;
                    bestIntersectionDistanceSquared = lengthSquared;
                }
            }

            if (this.Intersects(rectangle.GetSideSegment(Direction2D.Down), out temporaryIntersectionPoint))
            {
                float lengthSquared = temporaryIntersectionPoint.LengthSquared();
                if (lengthSquared < bestIntersectionDistanceSquared)
                {
                    intersectionPoint = temporaryIntersectionPoint;
                    bestIntersectionDistanceSquared = lengthSquared;
                }
            }

            return bestIntersectionDistanceSquared != float.MaxValue;
        }

        #endregion

        private static Vector2 GetPerpendicularVector(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        #endregion

        #region Object Members

        public override int GetHashCode()
        {
            return this.Direction.GetHashCode() ^ this.Position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Ray2D)
            {
                return this.Equals((Ray2D)obj);
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("{DisplayPosition: {0}, Direction: {1}}", this.Position, this.Direction);
        }

        #region IEquatable<Ray2D> Members

        public bool Equals(Ray2D other)
        {
            return this.Direction == other.Direction && this.Position == other.Position;
        }

        #endregion

        #endregion
    }
}
