
using System;
using Microsoft.Xna.Framework;

namespace Flai.Diagnostics
{
    public static class Check
    {
        public static bool IsValid(float value)
        {
            return value.IsValidNumber();
        }

        public static bool IsValid(Vector2 value)
        {
            return value.X.IsValidNumber() && value.Y.IsValidNumber();
        }

        public static bool IsValid(double value)
        {
            return value.IsValidNumber();
        }

        public static bool IsValid(Ray2D ray)
        {
            return Check.IsValid(ray.Position) && Check.IsValid(ray.Direction);
        }

        public static bool IsValid(Range value)
        {
            return Check.IsValid(value.Min) && Check.IsValid(value.Max);
        }

        public static bool IsValidPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute);
        }

        public static bool WithinRange<T>(T value, T min, T max)
            where T : IComparable<T>
        {
            return value.IsGreaterThanOrEqual(min) && value.IsLessThanOrEqual(max);
        }

        public static bool WithinRange<T, TRange>(T value, TRange range)
            where TRange : IRange<T>
        {
            return range.Contains(value);
        }

        public static bool Is<T>(object value)
            where T : class
        {
            return (value as T) != null;
        }
    }
}
