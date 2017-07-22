using System;
using System.Reflection;

namespace Flai.IO
{
    // okay this has nothing to do with IO but fuck it :P
    public static class ReflectionHelper
    {
        public static PropertyInfo GetProperty<T>(string propertyName)
        {
            return typeof (T).GetProperty(propertyName);
        }

        // no idea if these work. lets hope they do
        public static Func<T1> CompileMethodToFunc<T1>(object instance, MethodInfo methodInfo)
        {
            return (Func<T1>)Delegate.CreateDelegate(typeof(Func<T1>), instance, methodInfo);
        }

        public static Func<T1, T2> CompileMethodToFunc<T1, T2>(object instance, MethodInfo methodInfo)
        {
            return (Func<T1, T2>)Delegate.CreateDelegate(typeof(Func<T1, T2>), instance, methodInfo);
        }

        public static Func<T1, T2, T3> CompileMethodToFunc<T1, T2, T3>(object instance, MethodInfo methodInfo)
        {
            return (Func<T1, T2, T3>)Delegate.CreateDelegate(typeof(Func<T1, T2, T3>), instance, methodInfo);
        }

        // this one works \o/
        public static TDelegate CompileMethod<TDelegate>(object instance, MethodInfo methodInfo)
           where TDelegate : class //, Delegate
        {
            // !! <Check if TDelegate inherits from Delegate> !!
            return Delegate.CreateDelegate(typeof(TDelegate), instance, methodInfo) as TDelegate;
        }

        public static object CompileMethod(Type type, object instance, MethodInfo methodInfo)
        {
            // !! <Check if TDelegate inherits from Delegate> !!
            return Delegate.CreateDelegate(type, instance, methodInfo);
        }
    }
}
