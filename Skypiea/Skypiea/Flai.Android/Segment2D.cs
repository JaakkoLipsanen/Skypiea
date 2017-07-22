using System;
using Microsoft.Xna.Framework;

namespace Flai
{
    #region Segment2Di

    public struct Segment2Di
    {
        public Vector2i Start;
        public Vector2i End;

        public bool IsPoint
        {
            get { return this.Start == this.End; }
        }

        public float Slope
        {
            get { return (this.End.Y - this.Start.Y) / (float)(this.End.X - this.Start.X); } // if start.X == end.X, the result will be NaN or PositiveInfinite
        }

        public Vector2 LeftNormal
        {
            get
            {
                int dx = this.End.X - this.Start.X;
                int dy = this.End.Y - this.Start.Y;

                return Vector2.Normalize(new Vector2(-dy, dx));
            }
        }

        public Vector2 RightNormal
        {
            get
            {
                int dx = this.End.X - this.Start.X;
                int dy = this.End.Y - this.Start.Y;

                return Vector2.Normalize(new Vector2(dy, -dx));
            }
        }

        public Segment2Di(int startX, int startY, int endX, int endY)
        {
            this.Start = new Vector2i(startX, startY);
            this.End = new Vector2i(endX, endY);
        }

        public Segment2Di(Vector2i start, Vector2i end)
        {
            this.Start = start;
            this.End = end;
        }

        public float Length()
        {
            return Vector2i.Distance(this.Start, this.End);
        }

        public float LengthSquared()
        {
            return Vector2i.DistanceSquared(this.Start, this.End);
        }

        public bool Equals(Segment2Di other)
        {
            return this.Start == other.Start && this.End == other.End;
        }

        public override bool Equals(object obj)
        {
            return obj is Segment2Di && this.Equals((Segment2Di)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Start.GetHashCode() * 397) ^ this.End.GetHashCode();
            }
        }

        public override string ToString()
        {
            return "Start: " + this.Start.ToString() + ", End: " + this.End.ToString();
        }

        public static bool operator ==(Segment2Di segment1, Segment2Di segment2)
        {
            return segment1.Start == segment2.Start && segment1.End == segment2.End;
        }

        public static bool operator !=(Segment2Di segment1, Segment2Di segment2)
        {
            return segment1.Start != segment2.Start || segment1.End != segment2.End;
        }

        public static implicit operator Segment2D(Segment2Di segment)
        {
            return new Segment2D(segment.Start, segment.End);
        }

        // TODO: All algo's from Segment2D
    }

    #endregion

    #region Segment2D

    public struct Segment2D
    {
        public Vector2 Start;
        public Vector2 End;

        public bool IsPoint
        {
            get { return this.Start == this.End; }
        }

        public float Slope
        {
            get { return (this.End.Y - this.Start.Y) / (this.End.X - this.Start.X); } // if start.X == end.X, the result will be NaN or PositiveInfinite
        }

        public Vector2 LeftNormal
        {
            get
            {
                float dx = this.End.X - this.Start.X;
                float dy = this.End.Y - this.Start.Y;

                return Vector2.Normalize(new Vector2(-dy, dx));
            }
        }

        public Vector2 RightNormal
        {
            get
            {
                float dx = this.End.X - this.Start.X;
                float dy = this.End.Y - this.Start.Y;

                return Vector2.Normalize(new Vector2(dy, -dx));
            }
        }

        // normalized
        public Vector2 Direction
        {
            get
            {
                if (this.Start == this.End)
                {
                    return Vector2.Zero;
                }

                return Vector2.Normalize(this.End - this.Start);
            }
        }

        public static Segment2D Zero
        {
            get { return new Segment2D(); }
        }

        public Segment2D(float startX, float startY, float endX, float endY)
        {
            this.Start = new Vector2(startX, startY);
            this.End = new Vector2(endX, endY);
        }

        public Segment2D(Vector2 start, Vector2 end)
        {
            this.Start = start;
            this.End = end;
        }

        public float Length()
        {
            return Vector2.Distance(this.Start, this.End);
        }

        public float LengthSquared()
        {
            return Vector2.DistanceSquared(this.Start, this.End);
        }

        public bool Equals(Segment2D other)
        {
            return this.Start == other.Start && this.End == other.End;
        }

        public override bool Equals(object obj)
        {
            return obj is Segment2D && this.Equals((Segment2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Start.GetHashCode() * 397) ^ this.End.GetHashCode();
            }
        }

        public override string ToString()
        {
            return "Start: " + this.Start.ToString() + ", End: " + this.End.ToString();
        }

        public static bool operator ==(Segment2D segment1, Segment2D segment2)
        {
            return segment1.Start == segment2.Start && segment1.End == segment2.End;
        }

        public static bool operator !=(Segment2D segment1, Segment2D segment2)
        {
            return segment1.Start != segment2.Start || segment1.End != segment2.End;
        }

        public static bool Intersects(Segment2D segment, Circle circle)
        {
            return Segment2D.MinimumDistance(segment, circle.Position) < circle.Radius;
        }

        public static bool Intersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersectionPoint)
        {
            return Segment2D.Intersects(new Segment2D(p1, p2), new Segment2D(p3, p4), out intersectionPoint);
        }

        public static bool Intersects(Segment2D segment1, Segment2D segment2)
        {
            Vector2 intersectionPoint;
            return Segment2D.Intersects(segment1, segment2, out intersectionPoint);
        }

        public static bool Intersects(Segment2D segment1, Segment2D segment2, out Vector2 intersectionPoint)
        {
            /*  const float Epsilon = 0.00001f;

              Vector2 cmp = new Vector2(segment2.Start.X - segment1.Start.X, segment2.Start.Y - segment1.Start.Y);
              Vector2 r = new Vector2(segment1.End.X - segment1.Start.X, segment1.End.Y - segment1.Start.Y);
              Vector2 s = new Vector2(segment2.End.X - segment2.Start.X, segment2.End.Y - segment2.Start.Y);

              float cmPxR = cmp.X * r.Y - cmp.Y * r.X;
              float cmPxS = cmp.X * s.Y - cmp.Y * s.X;
              float RxS = r.X * s.Y - r.Y * s.X;

              if (Math.Abs(cmPxR) < Epsilon)
              {
                  intersectionPoint = Vector2.Zero;
                  return true;
              }

              if (Math.Abs(RxS) < Epsilon)
              {
                  intersectionPoint = default(Vector2);
                  return false;
              }

              float rXsR = 1f / RxS;
              float t = cmPxS * rXsR;
              float u = cmPxR * rXsR;

              if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
              {
                  intersectionPoint = segment1.Start + r * t;
                  return true;
              }

              intersectionPoint = default(Vector2);
              return false; */

            const float Epsilon = 0.00001f;

            float ua = (segment2.End.X - segment2.Start.X) * (segment1.Start.Y - segment2.Start.Y) - (segment2.End.Y - segment2.Start.Y) * (segment1.Start.X - segment2.Start.X);
            float ub = (segment1.End.X - segment1.Start.X) * (segment1.Start.Y - segment2.Start.Y) - (segment1.End.Y - segment1.Start.Y) * (segment1.Start.X - segment2.Start.X);
            float denominator = (segment2.End.Y - segment2.Start.Y) * (segment1.End.X - segment1.Start.X) - (segment2.End.X - segment2.Start.X) * (segment1.End.Y - segment1.Start.Y);

            if (Math.Abs(denominator) <= Epsilon)
            {
                if (Math.Abs(ua) <= Epsilon && Math.Abs(ub) <= Epsilon)
                {
                    intersectionPoint = (segment1.Start + segment1.End + segment2.Start + segment2.End) / 4f;
                    return true;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersectionPoint = new Vector2
                    {
                        X = segment1.Start.X + ua * (segment1.End.X - segment1.Start.X),
                        Y = segment1.Start.Y + ua * (segment1.End.Y - segment1.Start.Y)
                    };
                    return true;
                }
            }

            intersectionPoint = default(Vector2);
            return false;
        }

        // http://stackoverflow.com/a/100165/925777
        public static bool Intersects(Segment2D segment, RectangleF rectangle)
        {
            float minX = Math.Min(segment.Start.X, segment.End.X);
            float maxX = Math.Max(segment.Start.X, segment.End.X);

            if (maxX > rectangle.Right)
            {
                maxX = rectangle.Right;
            }

            if (minX < rectangle.Left)
            {
                minX = rectangle.Left;
            }

            if (minX > maxX)
            {
                return false;
            }

            float minY = Math.Min(segment.Start.Y, segment.End.Y);
            float maxY = Math.Max(segment.Start.Y, segment.End.Y);

            float dx = segment.End.X - segment.Start.X;
            if (Math.Abs(dx) > 0.0000001f)
            {
                float a = (segment.End.Y - segment.Start.Y) / dx;
                float b = segment.Start.Y - a * segment.Start.X;
                minY = a * minX + b;
                maxY = a * maxX + b;
            }

            if (minY > maxY)
            {
                float tmp = maxY;
                maxY = minY;
                minY = tmp;
            }

            if (maxY > rectangle.Bottom)
            {
                maxY = rectangle.Bottom;
            }

            if (minY < rectangle.Top)
            {
                minY = rectangle.Top;
            }

            if (minY > maxY)
            {
                return false;
            }

            return true;
        }

        public static float MinimumDistance(Segment2D segment, Vector2 point)
        {
            Vector2 minimumDistancePoint;
            return Segment2D.MinimumDistance(segment, point, out minimumDistancePoint);
        }

        public static float MinimumDistance(Segment2D segment, Vector2 point, out Vector2 minimumDistancePoint)
        {
            // Return minimum distance between line segment vw and point p
            float lengthSquared = segment.LengthSquared();  // i.e. |w-v|^2 -  avoid a sqrt
            if (lengthSquared == 0.0f)
            {
                minimumDistancePoint = segment.Start;
                return Vector2.Distance(point, segment.Start);   // v == w case
            }

            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            float t = Vector2.Dot(point - segment.Start, segment.End - segment.Start) / lengthSquared;
            if (t < 0.0)
            {
                minimumDistancePoint = segment.Start;
                return Vector2.Distance(point, segment.Start);       // Beyond the 'v' end of the segment
            }
            else if (t > 1.0)
            {
                minimumDistancePoint = segment.End;
                return Vector2.Distance(point, segment.End);  // Beyond the 'w' end of the segment
            }

            Vector2 projection = segment.Start + t * (segment.End - segment.Start);  // Projection falls on the segment

            minimumDistancePoint = projection;
            return Vector2.Distance(point, projection);
        }

        // yeah... umm, this isn't accurate. and its very slow/brute-force. but whatever, at least it somehow works
        public static float MinimumDistance(Segment2D segment, RectangleF rectangle)
        {
            if (Segment2D.Intersects(segment, rectangle))
            {
                return 0;
            }

            return FlaiMath.Min(
                Segment2D.MinimumDistance(segment, rectangle.TopLeft), Segment2D.MinimumDistance(segment, rectangle.TopRight),
                Segment2D.MinimumDistance(segment, rectangle.BottomLeft), Segment2D.MinimumDistance(segment, rectangle.BottomRight));
        }

        public static float MinimumDistance(Segment2D segment, Circle circle)
        {
            if (Segment2D.Intersects(segment, circle))
            {
                return 0;
            }

            // okay this Max *should be* unnecessary, but lets keep it just for sure
            return FlaiMath.Max(0, Segment2D.MinimumDistance(segment, circle.Position) - circle.Radius); 
        }

    //  public static float MinimumDistance(Segment2D segment, RectangleF rectangle, out Vector2 minimumDistancePoint)

        // is this correct? i guess so...
        public static float MaximumDistance(Segment2D segment, Vector2 point)
        {
            return Math.Max(Vector2.Distance(segment.Start, point), Vector2.Distance(segment.End, point));
        }

        // is this accurate? could be...
        public static float MaximumDistance(Segment2D segment, RectangleF rectangle)
        {
            if (Segment2D.Intersects(segment, rectangle))
            {
                return 0;
            }

            return FlaiMath.Max(
                Segment2D.MaximumDistance(segment, rectangle.TopLeft), Segment2D.MaximumDistance(segment, rectangle.TopRight),
                Segment2D.MaximumDistance(segment, rectangle.BottomLeft), Segment2D.MaximumDistance(segment, rectangle.BottomRight));
        }

        public static Segment2D Clamp(Segment2D segment, RectangleF area)
        {
            segment.Start = Vector2.Clamp(segment.Start, area.TopLeft, area.BottomRight);
            segment.End = Vector2.Clamp(segment.End, area.TopLeft, area.BottomRight);

            return segment;
        }
    }

    #endregion
}
