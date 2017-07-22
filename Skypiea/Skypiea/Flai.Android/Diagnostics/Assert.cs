using System;
using System.Diagnostics;
using Flai.Diagnostics;

namespace Flai
{
    public static class Assert
    {
        [Conditional("DEBUG")]
        public static void True(bool expression)
        {
            Assert.True(expression, "");
        }

        [Conditional("DEBUG")]
        public static void True(bool expression, string message)
        {
            if (!expression)
            {
                throw new InvalidOperationException(message);
            }
        }

        [Conditional("DEBUG")]
        public static void False(bool expression)
        {
            Assert.False(expression, "");
        }

        [Conditional("DEBUG")]
        public static void False(bool expression, string message)
        {
            if (expression)
            {
                throw new InvalidOperationException(message);
            }
        }

        [Conditional("DEBUG")]
        public static void Null(object value)
        {
            Assert.Null(value, "");
        }

        [Conditional("DEBUG")]
        public static void Null(object value, string message)
        {
            if (value != null)
            {
                throw new InvalidOperationException(message);
            }
        }

        [Conditional("DEBUG")]
        public static void NotNull(object value)
        {
            Assert.NotNull(value, "");
        }

        [Conditional("DEBUG")]
        public static void NotNull(object value, string message)
        {
            if (value == null)
            {
                throw new InvalidOperationException(message);
            }
        }

        [Conditional("DEBUG")]
        public static void WithinRange<T>(T value, T min, T max)
            where T : IComparable<T>
        {
            Assert.WithinRange(value, min, max, "");
        }

        [Conditional("DEBUG")]
        public static void WithinRange<T>(T value, T min, T max, string message)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsValid(float value)
        {
            if (!Check.IsValid(value))
            {
                throw new ArgumentException("value");
            }
        }
    }
}
