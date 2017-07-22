using System;
using System.Collections.Generic;
using System.Reflection;

namespace Flai.IO
{
    public struct TypeAttributePair<T>
        where T : Attribute
    {
        public Type Type { get; internal set; }
        public T Attribute { get; internal set; }
    }

    public static class AttributeHelper
    {
        public static TypeAttributePair<T>[] FindTypesWithAttribute<T>()
            where T : Attribute
        {
            return AttributeHelper.FindTypesWithAttribute<T>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static TypeAttributePair<T>[] FindTypesWithAttribute<T>(IEnumerable<Assembly> assembliesToSearch)
            where T : Attribute
        {
            return AttributeHelper.FindTypesWithAttributeInner<T>(assembliesToSearch).ToArray();
        }

        private static List<TypeAttributePair<T>> FindTypesWithAttributeInner<T>(IEnumerable<Assembly> assembliesToSearch)
            where T : Attribute
        {
            List<TypeAttributePair<T>> types = new List<TypeAttributePair<T>>();
            foreach (Assembly assembly in assembliesToSearch)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    var attribute = Attribute.GetCustomAttribute(type, typeof(T));
                    if (attribute != null)
                    {
                        types.Add(new TypeAttributePair<T> { Type = type, Attribute = (T)attribute });
                    }
                }
            }

            return types;
        }
    }
}