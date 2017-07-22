using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Flai
{
    public static class FlaiAlgorithms
    {
        /// <summary>
        /// Bresenham's algorithm
        /// </summary>
        public static IEnumerable<Vector2i> GetPointsOnSegmentBresenham(Vector2i start, Vector2i end)
        {
            return FlaiAlgorithms.GetPointsOnSegmentBresenham(start.X, start.Y, end.X, end.Y);
        }

        /// <summary>
        /// Bresenham's algorithm
        /// </summary>
        public static IEnumerable<Vector2i> GetPointsOnSegmentBresenham(int xStart, int yStart, int xEnd, int yEnd)
        {
            bool steep = Math.Abs(yEnd - yStart) > Math.Abs(xEnd - xStart);
            if (steep)
            {
                Common.SwapReferences(ref xStart, ref yStart);
                Common.SwapReferences(ref xEnd, ref yEnd);
            }

            if (xStart > xEnd)
            {
                Common.SwapReferences(ref xStart, ref xEnd);
                Common.SwapReferences(ref yStart, ref yEnd);
            }

            int dX = (xEnd - xStart);
            int dY = Math.Abs(yEnd - yStart);
            int err = dX / 2;
            int ystep = (yEnd > yStart ? 1 : -1);
            int y = yStart;

            for (int x = xStart; x <= xEnd; x++)
            {
                yield return (steep ? new Vector2i(y, x) : new Vector2i(x, y));
                err = err - dY;
                if (err < 0)
                {
                    y += ystep;
                    err += dX;
                }
            }
        }

        /* FLOATING POINT VERSION */
        /// <summary>
        /// Bresenham's algorithm
        /// </summary>
        public static IEnumerable<Vector2i> GetPointsOnSegmentBresenham(Vector2 start, Vector2 end)
        {
            return FlaiAlgorithms.GetPointsOnSegmentBresenham(start.X, start.Y, end.X, end.Y);
        }

        /// <summary>
        /// Bresenham's algorithm
        /// </summary>
        public static IEnumerable<Vector2i> GetPointsOnSegmentBresenham(float xStart, float yStart, float xEnd, float yEnd)
        {
            bool steep = Math.Abs(yEnd - yStart) > Math.Abs(xEnd - xStart);
            if (steep)
            {
                Common.SwapReferences(ref xStart, ref yStart);
                Common.SwapReferences(ref xEnd, ref yEnd);
            }

            if (xStart > xEnd)
            {
                Common.SwapReferences(ref xStart, ref xEnd);
                Common.SwapReferences(ref yStart, ref yEnd);
            }

            float dX = (xEnd - xStart);
            float dY = Math.Abs(yEnd - yStart);
            float err = dX / 2;
            float ystep = (yEnd > yStart ? 1 : -1);
            float y = yStart;

            for (float x = xStart; x <= xEnd; x++)
            {
                yield return (steep ? new Vector2i((int)y, (int)x) : new Vector2i((int)x, (int)y));
                err = err - dY;
                if (err < 0)
                {
                    y += ystep;
                    err += dX;
                }
            }
        }

        public static IEnumerable<Vector2i> GetPointsOnSegment(float startX, float startY, float endX, float endY)
        {
            return FlaiAlgorithms.GetPointsOnSegment(new Segment2D(new Vector2(startX, startY), new Vector2(endX, endY)));
        }

        public static IEnumerable<Vector2i> GetPointsOnSegment(Vector2 start, Vector2 end)
        {
            return FlaiAlgorithms.GetPointsOnSegment(new Segment2D(start, end));
        }

        public static IEnumerable<Vector2i> GetPointsOnSegment(Segment2D segment)
        {
            yield return new Vector2i(segment.Start);
            if (segment.IsPoint || new Vector2i(segment.Start) == new Vector2i(segment.End))
            {
                yield break;
            }


            float slope = segment.Slope;
            if (float.IsInfinity(slope)) // Segment is straight line in x-axis
            {
                int x = (int)segment.Start.X;

                int start = (int)FlaiMath.Min(segment.Start.Y, segment.End.Y);
                int end = (int)FlaiMath.Max(segment.Start.Y, segment.End.Y);
                for (int y = start + 1; y <= end; y++)
                {
                    yield return new Vector2i(x, y);
                }
            }
            else
            {
                Vector2 normalizedStep = Vector2.Normalize(segment.End - segment.Start);

                float xStep = normalizedStep.X;
                float yStep = normalizedStep.Y;

                Vector2i previousCellIndex = Vector2i.One * int.MaxValue;
                Vector2 currentPosition = segment.Start;
                while (true)
                {
                    float nextXDistance = float.MaxValue;
                    if (xStep > 0)
                    {
                        float xDist = FlaiMath.Floor(currentPosition.X + 1) - currentPosition.X;
                        nextXDistance = xDist / xStep;
                    }
                    else if (xStep < 0)
                    {
                        float xDist = FlaiMath.Floor(currentPosition.X) - currentPosition.X;
                        nextXDistance = xDist / xStep;
                    }

                    if (nextXDistance == 0)
                    {
                        nextXDistance = float.MaxValue;
                    }

                    float nextYDistance = float.MaxValue;
                    if (yStep > 0)
                    {
                        float yDist = FlaiMath.Floor(currentPosition.Y + 1) - currentPosition.Y;
                        nextYDistance = yDist / yStep;
                    }
                    else if (yStep < 0)
                    {
                        float yDist = FlaiMath.Floor(currentPosition.Y) - currentPosition.Y;
                        nextYDistance = yDist / yStep;
                    }

                    if (nextYDistance == 0)
                    {
                        nextYDistance = float.MaxValue;
                    }

                    // Vector2i previousCellIndex = new Vector2i(currentPosition);
                    if (FlaiMath.Distance(nextXDistance, nextYDistance) < 0.0001f)
                    {
                        float newX = xStep > 0 ? (int)currentPosition.X + 1 : FlaiMath.Decrement((int)currentPosition.X);
                        float newY = yStep > 0 ? (int)currentPosition.Y + 1 : FlaiMath.Decrement((int)currentPosition.Y);
                        currentPosition = new Vector2(newX, newY);
                    }
                    else if (nextXDistance < nextYDistance)
                    {
                        if (xStep > 0)
                        {
                            currentPosition = new Vector2((int)currentPosition.X + 1, currentPosition.Y + normalizedStep.Y * nextXDistance);
                        }
                        else
                        {
                            currentPosition = new Vector2(FlaiMath.Decrement((int)currentPosition.X), currentPosition.Y + normalizedStep.Y * nextXDistance);
                        }
                    }
                    else
                    {
                        if (yStep > 0)
                        {
                            currentPosition = new Vector2(currentPosition.X + normalizedStep.X * nextYDistance, (int)currentPosition.Y + 1);
                        }
                        else
                        {
                            currentPosition = new Vector2(currentPosition.X + normalizedStep.X * nextYDistance, FlaiMath.Decrement((int)currentPosition.Y));
                        }
                    }

                    currentPosition = Vector2.Clamp(currentPosition, Vector2.Min(segment.Start, segment.End), Vector2.Max(segment.Start, segment.End));

                    Vector2i currentCellIndex = new Vector2i(currentPosition);
                    yield return currentCellIndex;

                    if (currentCellIndex == new Vector2i(segment.End) || Vector2.Distance(segment.Start, currentPosition) > Vector2.Distance(segment.Start, segment.End) || currentCellIndex == previousCellIndex)
                    {
                        yield break;
                    }

                    previousCellIndex = currentCellIndex;
                }
            }
        }

        // these could be put somewhere else.. maybe RandomUtils, FlaiRandom itself or.. dunno
        public static Vector2 GenerateRandomUnitVector2()
        {
            return FlaiAlgorithms.GenerateRandomUnitVector2(Global.Random);
        }

        public static Vector2 GenerateRandomUnitVector2(Random random)
        {
            float radians = random.NextFloat(-FlaiMath.Pi, FlaiMath.Pi);
            return FlaiMath.GetAngleVector(radians);
        }

        public static Vector2 GenerateRandomVector2(Range horizontalRange, Range verticalRange)
        {
            return FlaiAlgorithms.GenerateRandomVector2(Global.Random, horizontalRange, verticalRange);
        }

        public static Vector2 GenerateRandomVector2(Random random, Range horizontalRange, Range verticalRange)
        {
            return new Vector2(random.NextFloat(horizontalRange.Min, horizontalRange.Max), random.NextFloat(verticalRange.Min, verticalRange.Max));
        }

        public static Vector2 GenerateRandomVector2(Range horizontalRange, Range verticalRange, Vector2 evadePosition, float minDistance)
        {
            return FlaiAlgorithms.GenerateRandomVector2(Global.Random, horizontalRange, verticalRange, evadePosition, minDistance);
        }

        // "evadePosition"? Lol.. but anyways, basically makes sure that the distance from the generated position to the evadePosition is more than the minDistance
        public static Vector2 GenerateRandomVector2(Random random, Range horizontalRange, Range verticalRange, Vector2 evadePosition, float minDistance)
        {
            // todo: very inefficient.. could be done better
            float minDistanceSquared = minDistance * minDistance;
            while (true)
            {
                Vector2 generated = FlaiAlgorithms.GenerateRandomVector2(random, horizontalRange, verticalRange);
                if (Vector2.DistanceSquared(evadePosition, generated) > minDistanceSquared)
                {
                    return generated;
                }
            }
        }

        // this doesn't probably work if targetAngle and originalAngle are further than PI away from each other. should do some modulus wrapping or something probably
        // http://stackoverflow.com/a/2708740/925777
        public static float RotateAngle(float originalAngle, float targetAngle, float amount)
        {
            float difference = FlaiMath.Abs(targetAngle - originalAngle);
            if (difference > 180)
            {
                // We need to add on to one of the values.
                if (targetAngle > originalAngle)
                {
                    // We'll add it on to start...
                    originalAngle += 360;
                }
                else
                {
                    // Add it on to end.
                    targetAngle += 360;
                }
            }

            // Interpolate it.
            float value = (originalAngle + ((targetAngle - originalAngle) * amount));
            if (value >= 0 && value <= 360)
            {
                return value;
            }

            return (value % 360);
        }
    }
}
