using System.Collections;
using System.Linq;
using System.Reflection;
using Flai.General;
using Flai.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Flai //.Extensions
{
    #region General C# Extensions

    public static class GeneralCSharpExtensions
    {
        /// <summary>
        /// Gets service from the service container
        /// </summary>
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }

        /// <summary>
        /// Indicates if Single is a number ( that is, it is not inifinity nor NaN )
        /// </summary>
        public static bool IsValidNumber(this Single single)
        {
            return !float.IsInfinity(single) && !float.IsNaN(single);
        }

        public static bool IsValidNumber(this double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }

        // These two are implemented in FlaiRandom
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(min + random.NextDouble() * (max - min));
        }

        public static float NextFloat(this Random random, Range range)
        {
            return (float)(range.Max + random.NextDouble() * (range.Max - range.Min));
        }

        public static string TrimIfNotNull(this string str)
        {
            return str != null ? str.Trim() : null;
        }

        public static string NullIfEmpty(this string str)
        {
            // should this trim too...?
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }

        public static string EmptyIfNull(this string str)
        {
            return str ?? "";
        }

        public static T Cast<T>(this object value)
            where T : class
        {
            // should this use 'as'?
            return (T)value;
        }

        public static IEnumerable<T> CastLinq<T>(this IEnumerable enumerable)
        {
            return Enumerable.Cast<T>(enumerable);
        }

        public static T As<T>(this object value)
            where T : class
        {
            return (value as T);
        }
    }

    #endregion

    #region BinarySerializer Extensions

    public static class BinarySerializerExtensions
    {
        public static void WriteArray(this BinaryWriter writer, byte[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (byte item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, sbyte[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (sbyte item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, short[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (short item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, ushort[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (ushort item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, int[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (int item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, uint[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (uint item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, long[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (long item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, ulong[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (ulong item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, float[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (float item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, double[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (double item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

#if WINDOWS
        public static void WriteArray(this BinaryWriter writer, decimal[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (decimal item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

#endif

        public static void WriteArray(this BinaryWriter writer, char[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (char item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, bool[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (bool item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray(this BinaryWriter writer, string[] array)
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (string item in array)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteArray<T>(this BinaryWriter writer, T[] array)
            where T : IBinarySerializable
        {
            if (array != null)
            {
                writer.Write(array.Length);
                foreach (T item in array)
                {
                    item.Write(writer);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteList<T>(this BinaryWriter writer, List<T> list)
            where T : IBinarySerializable
        {
            if (list != null)
            {
                writer.Write(list.Count);
                foreach (T item in list)
                {
                    item.Write(writer);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void WriteList(this BinaryWriter writer, List<string> list)
        {
            if (list != null)
            {
                writer.Write(list.Count);
                foreach (string item in list)
                {
                    writer.Write(item);
                }
            }
            else
            {
                writer.Write(-1);
            }
        }

        public static void Write<T>(this BinaryWriter writer, T value)
            where T : IBinarySerializable
        {
            if (value != null)
            {
                writer.Write(true);
                value.Write(writer);
            }
            else
            {
                writer.Write(false);
            }
        }

        public static void Write(this BinaryWriter writer, DateTime value)
        {
            writer.Write(value.Ticks);
        }

        public static void WriteString(this BinaryWriter writer, string value)
        {
            writer.Write(value ?? string.Empty);
        }

        public static T Read<T>(this BinaryReader reader, T value)
            where T : IBinarySerializable
        {
            if (reader.ReadBoolean())
            {
                value.Read(reader);
                return value;
            }

            return value; // value or default(T)???
        }

        public static T ReadGeneric<T>(this BinaryReader reader)
            where T : IBinarySerializable, new()
        {
            if (reader.ReadBoolean())
            {
                T result = new T();
                result.Read(reader);
                return result;
            }
            return default(T);
        }

        public static byte[] ReadByteArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                byte[] array = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadByte();
                }

                return array;
            }

            return null;
        }

        public static sbyte[] ReadSByteArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                sbyte[] array = new sbyte[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadSByte();
                }

                return array;
            }

            return null;
        }

        public static short[] ReadInt16Array(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                short[] array = new short[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadInt16();
                }

                return array;
            }

            return null;
        }

        public static ushort[] ReadUInt16Array(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                ushort[] array = new ushort[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadUInt16();
                }

                return array;
            }

            return null;
        }

        public static int[] ReadInt32Array(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                int[] array = new int[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadInt32();
                }

                return array;
            }

            return null;
        }

        public static uint[] ReadUInt32Array(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                uint[] array = new uint[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadUInt32();
                }

                return array;
            }

            return null;
        }

        public static long[] ReadInt64Array(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                long[] array = new long[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadInt64();
                }

                return array;
            }

            return null;
        }

        public static ulong[] ReadUInt64Array(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                ulong[] array = new ulong[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadUInt64();
                }

                return array;
            }

            return null;
        }

        public static float[] ReadSingleArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                float[] array = new float[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadSingle();
                }

                return array;
            }

            return null;
        }

        public static double[] ReadDoubleArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                double[] array = new double[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadDouble();
                }

                return array;
            }

            return null;
        }

#if WINDOWS
        public static decimal[] ReadDecimalArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                decimal[] array = new decimal[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadDecimal();
                }

                return array;
            }

            return null;
        }
#endif

        public static bool[] ReadBooleanArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                bool[] array = new bool[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadBoolean();
                }

                return array;
            }

            return null;
        }

        public static char[] ReadCharArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                char[] array = new char[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadChar();
                }

                return array;
            }

            return null;
        }

        public static string[] ReadStringArray(this BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                string[] array = new string[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = reader.ReadString();
                }

                return array;
            }

            return null;
        }

        public static T[] ReadArray<T>(this BinaryReader reader)
            where T : IBinarySerializable, new()
        {
            int length = reader.ReadInt32();
            if (length >= 0)
            {
                T[] array = new T[length];
                for (int i = 0; i < length; i++)
                {
                    T item = new T();
                    item.Read(reader);
                    array[i] = item;
                }

                return array;
            }

            return null;
        }

        public static List<string> ReadList(this BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count >= 0)
            {
                List<string> list = new List<string>(count);
                for (int i = 0; i < count; i++)
                {
                    list.Add(reader.ReadString());
                }
                return list;
            }

            return null;
        }

        public static List<T> ReadList<T>(this BinaryReader reader)
            where T : IBinarySerializable, new()
        {
            int count = reader.ReadInt32();
            if (count >= 0)
            {
                List<T> list = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    T item = new T();
                    item.Read(reader);
                    list.Add(item);
                }
                return list;
            }

            return null;
        }

        public static DateTime ReadDateTime(this BinaryReader reader)
        {
            long ticks = reader.ReadInt64();
            return new DateTime(ticks);
        }
    }

    #endregion

    #region Collection Extensions

    public static class CollectionExtensions
    {
        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        public static void AddAll<T>(this ICollection<T> collection, IList<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                collection.Add(items[i]);
            }
        }

        public static T[] ShuffleToNew<T>(this T[] input, Random random)
        {
            T[] tempArray = new T[input.Length];
            Array.Copy(input, tempArray, input.Length);

            for (int i = tempArray.Length - 1; i > 0; i--)
            {
                int n = random.Next(i + 1);
                tempArray.Swap(i, n);
            }

            return tempArray;
        }

        public static IList<T> ShuffleToNew<T>(this IList<T> input, Random random)
        {
            T[] tempArray = input.ToArray();
            // Array.Copy(input, tempArray, input.Count);

            for (int i = tempArray.Length - 1; i > 0; i--)
            {
                int n = random.Next(i + 1);
                tempArray.Swap(i, n);
            }

            return tempArray;
        }

        public static T[] Shuffle<T>(this T[] input, Random random)
        {
            return (input as IList<T>).Shuffle(random) as T[];
        }

        public static IList<T> Shuffle<T>(this IList<T> input, Random random)
        {
            for (int i = input.Count - 1; i > 0; i--)
            {
                int n = random.Next(i + 1);
                input.Swap(i, n);
            }

            return input;
        }

        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static T[] ToArray<T>(this ICollection<T> collection)
        {
            T[] arr = new T[collection.Count];
            if (collection.Count > 0)
            {
                collection.CopyTo(arr, 0);
            }

            return arr;
        }
    }

    #endregion

    #region Enum Extensions

    // okay DO NOT USE, too slow. make your own HasFlag/IncludeFlag for each enum type you want to use. they can use & | and whatnot
    public static class EnumExtensions
    {
        // SLOOOOOOOOOOOOOOW!! And generates lots of garbage!!

        /// <summary>
        /// Includes an enumerated type and returns the new value
        /// </summary>
        private static T IncludeFlag<T>(this Enum value, T append)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            Type type = value.GetType();

            //determine the values
            object result = value;
            _Value parsed = new _Value(append, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value) | (long)parsed.Signed; // blahh gar
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value) | (ulong)parsed.Unsigned;
            }

            //return the final value
            return (T)Enum.Parse(type, result.ToString(), false);
        }


        /// <summary>
        /// Checks if an enumerated type contains a value
        /// </summary>
        private static bool HasFlag<T>(this Enum value, T check)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            Type type = value.GetType();

            //determine the values
            object result = value;
            _Value parsed = new _Value(check, type);
            if (parsed.Signed is long)
            {
                return (Convert.ToInt64(value) & (long)parsed.Signed) == (long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                return (Convert.ToUInt64(value) & (ulong)parsed.Unsigned) == (ulong)parsed.Unsigned;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an enumerated type and returns the new value
        /// </summary>
        private static T RemoveFlag<T>(this Enum value, T remove)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            Type type = value.GetType();

            //determine the values
            object result = value;
            _Value parsed = new _Value(remove, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value) & ~(long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value) & ~(ulong)parsed.Unsigned;
            }

            //return the final value
            return (T)Enum.Parse(type, result.ToString(), false);
        }

        #region Helper Classes

        //class to simplfy narrowing values between 
        //a ulong and long since either value should
        //cover any lesser value
        private struct _Value
        {
            //cached comparisons for tye to use
            private static readonly Type _UInt64 = typeof(ulong);
            private static readonly Type _UInt32 = typeof(long);

            public long? Signed;
            public ulong? Unsigned;

            public _Value(object value, Type type)
            {
                //make sure it is even an enum to work with
                if (!type.IsEnum)
                {
                    throw new
                        ArgumentException("Value provided is not an enumerated type!");
                }

                //then check for the enumerated value
                Type compare = Enum.GetUnderlyingType(type);

                //if this is an unsigned long then the only
                //value that can hold it would be a ulong
                if (compare.Equals(_Value._UInt32) || compare.Equals(_Value._UInt64))
                {
                    this.Unsigned = Convert.ToUInt64(value);
                    this.Signed = null;
                }
                //otherwise, a long should cover anything else
                else
                {
                    this.Signed = Convert.ToInt64(value);
                    this.Unsigned = null;
                }
            }
        }

        #endregion
    }

    #endregion

    #region TimeSpan Extensions

    public static class TimeSpanExtensions
    {
        public static TimeSpan ClampToRange(this TimeSpan value, TimeSpan minimum, TimeSpan maximum)
        {
            if (value < minimum)
            {
                value = minimum;
            }

            if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }
    }

    #endregion

    #region Array Extensions

    public static class ArrayExtensions
    {
        public static void Populate<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static void Populate<T>(this T[] array, T value, int startIndex)
        {
            if (startIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            for (int i = startIndex; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static void Populate<T>(this T[] array, T value, int startIndex, int count)
        {
            if (startIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            int max = startIndex + count;
            if (max >= array.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            for (int i = startIndex; i < max; i++)
            {
                array[i] = value;
            }
        }
    }

    #endregion

    #region Stopwatch Extensions

    public static class StopwatchExtensions
    {
#if WINDOWS_PHONE
        public static void Restart(this System.Diagnostics.Stopwatch sw)
        {
            sw.Reset();
            sw.Start();
        }
#endif
    }

    #endregion

    #region List Extensions

    public static class ListExtensions
    {
        public static int BinarySearch<T>(this List<T> list, T item, Comparison<T> comparison)
        {
            return list.BinarySearch(item, new ComparisonComparer<T>(comparison));
        }

        // preserve the order (!! not sure if works !!)
        public static void StableSort<T>(this IList<T> list)
        {
            list.StableSort(Comparer<T>.Default.Compare);
        }

        public static void StableSort<T>(this IList<T> list, Comparison<T> comparison)
        {
            T[] arr = list.ToArray();
            arr.StableSort(comparison);

            for (int i = 0; i < arr.Length; i++)
            {
                list[i] = arr[i];
            }
        }

        // preserve the order (!! not sure if works !!)
        public static void StableSort<T>(this T[] arr)
        {
            arr.StableSort(Comparer<T>.Default.Compare);
        }

        public static void StableSort<T>(this T[] arr, Comparison<T> comparison)
        {
            var keys = new KeyValuePair<int, T>[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                keys[i] = new KeyValuePair<int, T>(i, arr[i]);
            }

            Array.Sort(keys, arr, new StabilizingComparer<T>(comparison));
        }

        private sealed class StabilizingComparer<T> : IComparer<KeyValuePair<int, T>>
        {
            private readonly Comparison<T> _comparison;

            public StabilizingComparer(Comparison<T> comparison)
            {
                _comparison = comparison;
            }

            public int Compare(KeyValuePair<int, T> x,
                               KeyValuePair<int, T> y)
            {
                var result = _comparison(x.Value, y.Value);
                return result != 0 ? result : x.Key.CompareTo(y.Key);
            }
        }
    }

    #endregion

    #region IComparableExtensions

    public static class IComparableExtensions
    {
        public static bool IsGreaterThanOrEqual<T, Y>(this Y comparable, T other)
            where Y : IComparable<T>
        {
            return comparable.CompareTo(other) >= 0;
        }

        public static bool IsGreaterThan<T, Y>(this Y comparable, T other)
            where Y : IComparable<T>
        {
            return comparable.CompareTo(other) > 0;
        }

        public static bool IsLessThanOrEqual<T, Y>(this Y comparable, T other)
            where Y : IComparable<T>
        {
            return comparable.CompareTo(other) <= 0;
        }

        public static bool IsLessThan<T, Y>(this Y comparable, T other)
            where Y : IComparable<T>
        {
            return comparable.CompareTo(other) < 0;
        }

        public static bool IsEqual<T, Y>(this Y comparable, T other)
            where Y : IComparable<T>
        {
            return comparable.CompareTo(other) < 0;
        }

        public static T Clamp<T>(this T value, T min, T max) 
            where T : IComparable<T>
        {
            if (value.IsLessThan(min))
            {
                return min;           
            }

            if (value.IsGreaterThan(max))
            {
                return max;
            }

            return value;
        }
    }

    #endregion

    #region Enumerable Extensions

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T value)
        {
            IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
            foreach (T item in enumerable)
            {
                if (!equalityComparer.Equals(item, value))
                {
                    yield return item;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action == null)
            {
                return;
            }

            foreach (T item in enumerable)
            {
                action(item);
            }
        }
    }

    #endregion

    #region Type Extensions

    public static class TypeExtensions
    {
        public static MethodInfo GetGenericMethod(this Type type, string name)
        {
            return type.GetGenericMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetMethods(bindingFlags).First(method => method.Name == name && method.IsGenericMethod);
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, int genericArgumentsCount)
        {
            return type.GetGenericMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, genericArgumentsCount);
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags bindingFlags, int genericArgumentsCount)
        {
            return type.GetMethods(bindingFlags).First(method => method.Name == name && method.IsGenericMethod && method.GetGenericArguments().Length == genericArgumentsCount);
        }
    }

    #endregion

    #region StringBuilder Extensions

    public static partial class StringBuilderExtensions
    {
        #region Numeric

        private static readonly char[] _digitCharacters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private static readonly uint _defaultDecimalPlaces = 5; // Matches standard .NET formatting decimal places
        private static readonly char _defaultPadCharacter = '0';

        // Not sure about the naming..
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, uint value, uint padAmount, char padCharacter, uint baseValue)
        {
            // Calculate length of integer when written out
            uint length = 0;
            uint length_calc = value;

            do
            {
                length_calc /= baseValue;
                length++;
            } while (length_calc > 0);

            // Pad out space for writing.
            stringBuilder.Append(padCharacter, (int)Math.Max(padAmount, length));

            int strpos = stringBuilder.Length;

            // We're writing backwards, one character at a time.
            while (length > 0)
            {
                strpos--;

                // Lookup from static char array, to cover hex values too
                stringBuilder[strpos] = _digitCharacters[value % baseValue];

                value /= baseValue;
                length--;
            }

            return stringBuilder;
        }

        //! Convert a given unsigned integer value to a string and concatenate onto the stringbuilder. Assume no padding and base ten.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, uint value)
        {
            stringBuilder.AppendIntelligent(value, 0, _defaultPadCharacter, 10);
            return stringBuilder;
        }

        //! Convert a given unsigned integer value to a string and concatenate onto the stringbuilder. Assume base ten.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, uint value, uint padAmount)
        {
            stringBuilder.AppendIntelligent(value, padAmount, _defaultPadCharacter, 10);
            return stringBuilder;
        }

        //! Convert a given unsigned integer value to a string and concatenate onto the stringbuilder. Assume base ten.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, uint value, uint padAmount, char padCharacter)
        {
            stringBuilder.AppendIntelligent(value, padAmount, padCharacter, 10);
            return stringBuilder;
        }

        //! Convert a given signed integer value to a string and concatenate onto the stringbuilder. Any base value allowed.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, int value, uint padAmount, char padCharacter, uint baseValue)
        {
            // Deal with negative numbers
            if (value < 0)
            {
                stringBuilder.Append('-');
                uint uint_val = uint.MaxValue - ((uint)value) + 1; //< This is to deal with Int32.MinValue
                stringBuilder.AppendIntelligent(uint_val, padAmount, padCharacter, baseValue);
            }
            else
            {
                stringBuilder.AppendIntelligent((uint)value, padAmount, padCharacter, baseValue);
            }

            return stringBuilder;
        }

        //! Convert a given signed integer value to a string and concatenate onto the stringbuilder. Assume no padding and base ten.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, int value)
        {
            stringBuilder.AppendIntelligent(value, 0, _defaultPadCharacter, 10);
            return stringBuilder;
        }

        //! Convert a given signed integer value to a string and concatenate onto the stringbuilder. Assume base ten.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, int value, uint padAmount)
        {
            stringBuilder.AppendIntelligent(value, padAmount, _defaultPadCharacter, 10);
            return stringBuilder;
        }

        //! Convert a given signed integer value to a string and concatenate onto the stringbuilder. Assume base ten.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, int value, uint padAmount, char padChar)
        {
            stringBuilder.AppendIntelligent(value, padAmount, padChar, 10);
            return stringBuilder;
        }

        //! Convert a given float value to a string and concatenate onto the stringbuilder
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, float value, uint decimalPlaces, uint padAmount, char padCharacter)
        {
            if (decimalPlaces == 0)
            {
                // No decimal places, just round up and print it as an int

                // Agh, Math.Floor() just works on doubles/decimals. Don't want to cast! Let's do this the old-fashioned way.
                int int_val;
                if (value >= 0.0f)
                {
                    // Round up
                    int_val = (int)(value + 0.5f);
                }
                else
                {
                    // Round down for negative numbers
                    int_val = (int)(value - 0.5f);
                }

                stringBuilder.AppendIntelligent(int_val, padAmount, padCharacter, 10);
            }
            else
            {
                int int_part = (int)value;

                // First part is easy, just cast to an integer
                stringBuilder.AppendIntelligent(int_part, padAmount, padCharacter, 10);

                // Decimal point
                stringBuilder.Append('.');

                // Work out remainder we need to print after the d.p.
                float remainder = Math.Abs(value - int_part);

                // Multiply up to become an int that we can print
                do
                {
                    remainder *= 10;
                    decimalPlaces--;
                } while (decimalPlaces > 0);

                // Round up. It's guaranteed to be a positive number, so no extra work required here.
                remainder += 0.5f;

                // All done, print that as an int!
                stringBuilder.AppendIntelligent((uint)remainder, 0, '0', 10);
            }
            return stringBuilder;
        }

        //! Convert a given float value to a string and concatenate onto the stringbuilder. Assumes five decimal places, and no padding.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, float value)
        {
            stringBuilder.AppendIntelligent(value, _defaultDecimalPlaces, 0, _defaultPadCharacter);
            return stringBuilder;
        }

        //! Convert a given float value to a string and concatenate onto the stringbuilder. Assumes no padding.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, float value, uint decimalPlaces)
        {
            stringBuilder.AppendIntelligent(value, decimalPlaces, 0, _defaultPadCharacter);
            return stringBuilder;
        }

        //! Convert a given float value to a string and concatenate onto the stringbuilder.
        public static StringBuilder AppendIntelligent(this StringBuilder stringBuilder, float value, uint decimalPlaces, uint padAmount)
        {
            stringBuilder.AppendIntelligent(value, decimalPlaces, padAmount, _defaultPadCharacter);
            return stringBuilder;
        }

        #endregion

        #region Format

        //! Concatenate a formatted string with arguments
        public static StringBuilder ConcatFormat<A>(this StringBuilder stringBuilder, String formatString, A arg1)
            where A : IConvertible
        {
            return stringBuilder.ConcatFormat<A, int, int, int>(formatString, arg1, 0, 0, 0);
        }

        //! Concatenate a formatted string with arguments
        public static StringBuilder ConcatFormat<A, B>(this StringBuilder stringBuilder, String formatString, A arg1, B arg2)
            where A : IConvertible
            where B : IConvertible
        {
            return stringBuilder.ConcatFormat<A, B, int, int>(formatString, arg1, arg2, 0, 0);
        }

        //! Concatenate a formatted string with arguments
        public static StringBuilder ConcatFormat<A, B, C>(this StringBuilder stringBuilder, String formatString, A arg1, B arg2, C arg3)
            where A : IConvertible
            where B : IConvertible
            where C : IConvertible
        {
            return stringBuilder.ConcatFormat<A, B, C, int>(formatString, arg1, arg2, arg3, 0);
        }

        //! Concatenate a formatted string with arguments
        public static StringBuilder ConcatFormat<A, B, C, D>(this StringBuilder stringBuilder, String formatString, A arg1, B arg2, C arg3, D arg4)
            where A : IConvertible
            where B : IConvertible
            where C : IConvertible
            where D : IConvertible
        {
            int verbatim_range_start = 0;

            for (int index = 0; index < formatString.Length; index++)
            {
                if (formatString[index] == '{')
                {
                    // Formatting bit now, so make sure the last block of the string is written out verbatim.
                    if (verbatim_range_start < index)
                    {
                        // Write out unformatted string portion
                        stringBuilder.Append(formatString, verbatim_range_start, index - verbatim_range_start);
                    }

                    uint base_value = 10;
                    uint padding = 0;
                    uint decimal_places = 5; // Default decimal places in .NET libs

                    index++;
                    char format_char = formatString[index];
                    if (format_char == '{')
                    {
                        stringBuilder.Append('{');
                        index++;
                    }
                    else
                    {
                        index++;

                        if (formatString[index] == ':')
                        {
                            // Extra formatting. This is a crude first pass proof-of-concept. It's not meant to cover
                            // comprehensively what the .NET standard library Format() can do.
                            index++;

                            // Deal with padding
                            while (formatString[index] == '0')
                            {
                                index++;
                                padding++;
                            }

                            if (formatString[index] == 'X')
                            {
                                index++;

                                // Print in hex
                                base_value = 16;

                                // Specify amount of padding ( "{0:X8}" for example pads hex to eight characters
                                if ((formatString[index] >= '0') && (formatString[index] <= '9'))
                                {
                                    padding = (uint)(formatString[index] - '0');
                                    index++;
                                }
                            }
                            else if (formatString[index] == '.')
                            {
                                index++;

                                // Specify number of decimal places
                                decimal_places = 0;

                                while (formatString[index] == '0')
                                {
                                    index++;
                                    decimal_places++;
                                }
                            }
                        }


                        // Scan through to end bracket
                        while (formatString[index] != '}')
                        {
                            index++;
                        }

                        // Have any extended settings now, so just print out the particular argument they wanted
                        switch (format_char)
                        {
                            case '0':
                                stringBuilder.ConcatFormatValue<A>(arg1, padding, base_value, decimal_places);
                                break;
                            case '1':
                                stringBuilder.ConcatFormatValue<B>(arg2, padding, base_value, decimal_places);
                                break;
                            case '2':
                                stringBuilder.ConcatFormatValue<C>(arg3, padding, base_value, decimal_places);
                                break;
                            case '3':
                                stringBuilder.ConcatFormatValue<D>(arg4, padding, base_value, decimal_places);
                                break;

                            default:
                                throw new ArgumentOutOfRangeException("Invalid format index");
                        }
                    }

                    // Update the verbatim range, start of a new section now
                    verbatim_range_start = (index + 1);
                }
            }

            // Anything verbatim to write out?
            if (verbatim_range_start < formatString.Length)
            {
                // Write out unformatted string portion
                stringBuilder.Append(formatString, verbatim_range_start, formatString.Length - verbatim_range_start);
            }

            return stringBuilder;
        }

        //! The worker method. This does a garbage-free conversion of a generic type, and uses the garbage-free Concat() to add to the stringbuilder
        private static void ConcatFormatValue<T>(this StringBuilder stringBuilder, T arg, uint padding, uint baseValue, uint decimalPlaces) where T : IConvertible
        {
            switch (arg.GetTypeCode())
            {
                case System.TypeCode.UInt32:
                    {
                        stringBuilder.AppendIntelligent(arg.ToUInt32(System.Globalization.NumberFormatInfo.CurrentInfo), padding, '0', baseValue);
                        break;
                    }

                case System.TypeCode.Int32:
                    {
                        stringBuilder.AppendIntelligent(arg.ToInt32(System.Globalization.NumberFormatInfo.CurrentInfo), padding, '0', baseValue);
                        break;
                    }

                case System.TypeCode.Single:
                    {
                        stringBuilder.AppendIntelligent(arg.ToSingle(System.Globalization.NumberFormatInfo.CurrentInfo), decimalPlaces, padding, '0');
                        break;
                    }

                case System.TypeCode.String:
                    {
                        stringBuilder.Append(arg.ToString(null));
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException("Unknown parameter type");
                    }
            }
        }

        #endregion
    }

    #endregion

    #region Dictionary Extensions

    public static class DictionaryExtensions
    {
        public static void AddOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey fromKey, TKey toKey)
        {
            TValue value = dictionary[fromKey];
            dictionary.Remove(fromKey);
            dictionary[toKey] = value;
        }

        public static Y TryGetValue<T, Y>(this Dictionary<T, Y> dictionary, T key)
            where Y : class // lets just make this class constraint, it's easier that way
        {
            Y value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return default(Y);
        }

        public static Y? TryGetValue<T, Y>(this Dictionary<T, Y?> dictionary, T key)
            where Y : struct // lets just make this class constraint, it's easier that way
        {
            Y? value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return default(Y);
        }
    }

    #endregion
}