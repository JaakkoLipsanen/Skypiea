using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Flai.IO
{
    public static class TypeHelper
    {
        // FindType (IEnumerable<Assembly>) ??
        public static Type FindType(string name)
        {
            return Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(type => type.Name == name); // type.Name == name correct?
        }

        public static Type FindType(Assembly assembly, string name)
        {
            return assembly.GetTypes().FirstOrDefault(type => type.Name == name); // type.Name == name correct?
        }

        public static Type[] FindTypesInheritingFrom<T>(IEnumerable<Assembly> assemblies)
        {
            return TypeHelper.FindTypesInheritingFrom<T>(assemblies, true);
        }

        public static Type[] FindTypesInheritingFrom<T>(IEnumerable<Assembly> assemblies, bool allowAbstract)
        {
            return TypeHelper.FindTypesInheritingFrom(assemblies, typeof(T), allowAbstract);
        }

        public static Type[] FindTypesInheritingFrom(IEnumerable<Assembly> assemblies, Type targetType)
        {
            return TypeHelper.FindTypesInheritingFrom(assemblies, targetType, true);
        }

        public static Type[] FindTypesInheritingFrom(IEnumerable<Assembly> assemblies, Type targetType, bool allowAbstract)
        {
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type != targetType && targetType.IsAssignableFrom(type) && (allowAbstract || !type.IsAbstract))
                    {
                        types.Add(type);
                    }
                }
            }

            return types.ToArray();
        }
    }
}
