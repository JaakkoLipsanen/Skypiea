using System;
using System.Linq;
using Flai.DataStructures;

namespace Flai.General
{
    // http://stackoverflow.com/a/1416660/925777  
    // make all the methods here internal and "overload"/"new" them in EnumHelper?
    public abstract class EnumConstraint<TSystemDotEnum> // TSystemDotEnum == System.Enum
               where TSystemDotEnum : class
    {
        // make sure no class outside of EnumHelper inherits this
        internal EnumConstraint() { }

        public static TEnum Parse<TEnum>(string name)
           where TEnum : struct, TSystemDotEnum
        {
            return InnerHelper<TEnum>.Parse(name, false);
        }

        public static TEnum Parse<TEnum>(string name, bool ignoreCase)
          where TEnum : struct, TSystemDotEnum
        {
            return InnerHelper<TEnum>.Parse(name, ignoreCase);
        }

        public static ReadOnlyArray<TEnum> GetValues<TEnum>()
            where TEnum : struct, TSystemDotEnum
        {
            // phew
            // http://stackoverflow.com/a/5879249/925777
            return InnerHelper<TEnum>.Values;
        }

        public static ReadOnlyArray<string> GetNames<TEnum>()
           where TEnum : struct, TSystemDotEnum
        {
            // phew
            // http://stackoverflow.com/a/5879249/925777
            return InnerHelper<TEnum>.Names;
        }

        public static bool IsDefined<TEnum>(TEnum value)
            where TEnum : struct , TSystemDotEnum
        {
            return InnerHelper<TEnum>.IsDefined(value);
        }

        public static string GetName<TEnum>(TEnum value)
            where TEnum : struct, TSystemDotEnum
        {
            return InnerHelper<TEnum>.GetName(value);
        }

        public static TEnum GetRandom<TEnum>(Random random)
            where TEnum : struct, TSystemDotEnum
        {
            return InnerHelper<TEnum>.Values[random.Next(0, InnerHelper<TEnum>.Values.Length)];
        }

        #region InnerHelper

        private static class InnerHelper<T>
            where T : struct, TSystemDotEnum
        {
            private static ReadOnlyArray<T> _readOnlyValues;
            private static ReadOnlyArray<string> _readOnlyNames;

            public static ReadOnlyArray<T> Values
            {
                get { return _readOnlyValues ?? (_readOnlyValues = new ReadOnlyArray<T>(CreateValues())); }
            }

            public static ReadOnlyArray<string> Names
            {
                get { return _readOnlyNames ?? (_readOnlyNames = new ReadOnlyArray<string>(CreateNames())); }
            }

            public static bool IsDefined(T value)
            {
                return Values.Contains(value);
            }

            private static T[] CreateValues()
            {
                return typeof(T).GetFields().Where(field => field.IsLiteral).Select(field => field.GetValue(typeof(T))).CastLinq<T>().ToArray();
            }

            private static string[] CreateNames()
            {
                return typeof(T).GetFields().Where(field => field.IsLiteral).Select(field => field.Name).ToArray();
            }

            public static string GetName(T value)
            {
                for (int i = 0; i < Values.Count; i++)
                {
                    if (Values[i].Equals(value))
                    {
                        return Names[i];
                    }
                }

                throw new ArgumentOutOfRangeException("invalid value");
            }

            public static T Parse(string name, bool ignoreCase)
            {
                StringComparison comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
                for (int i = 0; i < Names.Count; i++)
                {
                    if (Names[i].Equals(name, comparison))
                    {
                        return Values[i];
                    }
                }

                throw new ArgumentException("name");
            }
        }

        #endregion
    }

    public abstract class EnumHelper : EnumConstraint<Enum>
    {
        // make sure no class outside inherits this
        internal EnumHelper() { }
    }

    // -----------------------------------------------------

    // Delegate Helper 
    public abstract class DelegateConstraint<TSystemDotDelegate> // TSystemDotEnum == System.Delegate
               where TSystemDotDelegate : class
    {
        // make sure no class outside of DelegateHelper inherits this
        internal DelegateConstraint() { }
    }

    public abstract class DelegateHelper : DelegateConstraint<Delegate>
    {
        // make sure no class outside inherits this
        internal DelegateHelper() { }
    }
}
