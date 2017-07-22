using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Flai
{
    public static class Common
    {
        // Let's make these extensions and keep them in Flai namespace...
        #region Dispose/Invoke if not null

        public static void DisposeIfNotNull<T>(this T obj)
            where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
            }
        }

        public static void InvokeIfNotNull(this EventHandler eventHandler, object sender)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }

        public static void InvokeIfNotNull(this EventHandler<EventArgs> eventHandler, object sender)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }

        public static void InvokeIfNotNull<T>(this EventHandler<T> eventHandler, object sender, T args)
            where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, args);
            }
        }

        public static void InvokeIfNotNull(this PropertyChangedEventHandler eventHandler, object sender, string propertyName)
        {
            eventHandler.InvokeIfNotNull(sender, new PropertyChangedEventArgs(propertyName));
        }

        public static void InvokeIfNotNull(this PropertyChangedEventHandler eventHandler, object sender, PropertyChangedEventArgs args)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, args);
            }
        }

        public static void DoIfNotNull<T>(this T args, Action action)
            where T : class
        {
            if (args != null)
            {
                action();
            }
        }

        #endregion

        #region Flatten Index

        public static int FlattenIndex2D(int width, int x, int y)
        {
            return x + y * width;
        }

        public static int FlattenIndex2D(int width, Vector2i index)
        {
            return index.X + index.Y * width;
        }

        public static int FlattenIndex3D(int width, int height, int x, int y, int z)
        {
            return x + width * (y + height * z);
        }

        public static int FlattenIndex3D(int width, int height, Vector3i index)
        {
            return index.X + width * (index.Y + height * index.Z);
        }

        #endregion

        #region Get Month Name

        // Month enum?
        private static string[] _months;
        public static string GetMonthName(int month)
        {
            if (_months == null)
            {
                _months = new string[]
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December",
                };
            }

            Ensure.WithinRange(month, 0, 12);
            return _months[month - 1];
        }

        #endregion

        #region Character to String

        private static string[] _characterToStringArray;
        public static string CharacterToString(char c)
        {
            if (_characterToStringArray == null)
            {
                _characterToStringArray = new string[char.MaxValue];
            }

            if (_characterToStringArray[c] == null)
            {
                return (_characterToStringArray[c] = c.ToString());
            }

            return _characterToStringArray[c];
        }

        #endregion

        #region Integer to Ordinal Number ( "10th" etc. )

        public static string GetOrdinalSuffix(int number)
        {
            if (number == 0)
            {
                return "";
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }

            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static string IntegerToOrdinalNumber(byte number)
        {
            if (number == 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }

        public static string IntegerToOrdinalNumber(uint number)
        {
            if (number == 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }

        public static string IntegerToOrdinalNumber(ulong number)
        {
            if (number == 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }

        public static string IntegerToOrdinalNumber(int number)
        {
            if (number <= 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }

        public static string IntegerToOrdinalNumber(long number)
        {
            if (number <= 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "st";
                case 2:
                    return number.ToString() + "nd";
                case 3:
                    return number.ToString() + "rd";
                default:
                    return number.ToString() + "th";
            }
        }

        #endregion

        public static int CharToInt(char c)
        {
            if (!char.IsDigit(c))
            {
                throw new ArgumentException("Argument is not an digit");
            }

            return c - '0';
        }

        // http://stackoverflow.com/a/155487
        public static string AddSpaceBeforeCaps(string input)
        {
            // this could possibly be done more efficiently (and shouldn't be too complex, this seems pretty easy thing to do)
            const string RegexInput = "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))";
            return Regex.Replace(input ?? "", RegexInput, "$1 ");
        }

        public static void SwapReferences<T>(ref T val1, ref T val2)
        {
            T tmp = val1;
            val1 = val2;
            val2 = tmp;
        }

        public static IEnumerable<T> ToEnumerable<T>(T item)
        {
            yield return item;
        }
    }
}
