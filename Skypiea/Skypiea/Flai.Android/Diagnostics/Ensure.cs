using System;
using Flai.Diagnostics;
using Microsoft.Xna.Framework;

namespace Flai // do.not.care
{
    // No [Conditional("DEBUG)], because these are not meant to be "asserts".. though maybe check for "SKIP_ENSURE" compilation flag or something to disable them when shipping
    public static class Ensure
    { 
        public static void True(bool expression)
        {
            Ensure.True(expression, "");
        }

        public static void True(bool expression, string message)
        {
            if (!expression)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void False(bool expression)
        {
            Ensure.False(expression, "");
        }

        public static void False(bool expression, string message)
        {
            if (expression)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void Null(object value)
        {
            Ensure.Null(value, "");
        }

        public static void Null(object value, string message)
        {
            if (value != null)
            {
                throw new ArgumentException(message);
            }
        }

        public static void NotNull(object value)
        {
            Ensure.NotNull(value, "");
        }

        public static void NotNull(object value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void NotNullOrWhitespace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("");
            }
        }

        public static void WithinRange<T>(T value, T min, T max)
            where T : IComparable<T>
        {
            Ensure.WithinRange(value, min, max, "");
        }

        public static void WithinRange<T>(T value, T min, T max, string message)
            where T : IComparable<T>
        {
            if (!value.IsGreaterThanOrEqual(min) || !value.IsLessThanOrEqual(max))
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void WithinRange<T, TRange>(T value, TRange range)
           where TRange : IRange<T>
        {
            Ensure.WithinRange(value, range, "");
        }

        public static void WithinRange<T, TRange>(T value, TRange range, string message)
            where TRange : IRange<T>
        {
            if (!range.Contains(value))
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void IsValid(float value)
        {
            if (!value.IsValidNumber())
            {
                throw new ArgumentException("value");
            }
        }

        public static void IsValid(Vector2 value)
        {
            if (!value.X.IsValidNumber() || !value.Y.IsValidNumber())
            {
                throw new ArgumentException("value");
            }
        }

        public static void IsValid(double value)
        {
            if (!value.IsValidNumber())
            {
                throw new ArgumentException("value");
            }
        }

        public static void IsValid(Ray2D value)
        {
            if (!Check.IsValid(value))
            {
                throw new ArgumentException("value");
            }
        }

        public static void IsValid(Range value)
        {
            if (!Check.IsValid(value))
            {
                throw new ArgumentException("value");
            }
        }

        public static void IsValid(RectangleF value)
        {
            if (!Check.IsValid(value.X) || !Check.IsValid(value.Y) || !Check.IsValid(value.Width) || !Check.IsValid(value.Height))
            {
                throw new ArgumentException("value");
            }
        }

        public static void IsValidPath(string path)
        {
            if (!Check.IsValidPath(path))
            {
                throw new ArgumentException("path");
            }
        }

        public static void Is<T>(object value)
            where T : class
        {
            if (!Check.Is<T>(value))
            {
                throw new ArgumentException("value");
            }
        }
    }
}
