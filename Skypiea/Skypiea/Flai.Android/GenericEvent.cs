
namespace Flai
{
    public delegate void GenericEvent();
    public delegate void GenericEvent<T1>(T1 args);
    public delegate void GenericEvent<T1, T2>(T1 args1, T2 args2);
    public delegate void GenericEvent<T1, T2, T3>(T1 args1, T2 args2, T3 args3);
    public delegate void GenericEvent<T1, T2, T3, T4>(T1 args1, T2 args2, T3 args3, T4 args4);
    public delegate void GenericEvent<T1, T2, T3, T4, T5>(T1 args1, T2 args2, T3 args3, T4 args4, T5 args5);
    public delegate void GenericEvent<T1, T2, T3, T4, T5, T6>(T1 args1, T2 args2, T3 args3, T4 args4, T5 args5, T6 args6);

    public static class GenericEventExtensions
    {
        public static void InvokeIfNotNull(this GenericEvent genericEvent)
        {
            if (genericEvent != null)
            {
                genericEvent();
            }
        }

        public static void InvokeIfNotNull<T1>(this GenericEvent<T1> genericEvent, T1 args1)
        {
            if (genericEvent != null)
            {
                genericEvent(args1);
            }
        }

        public static void InvokeIfNotNull<T1, T2>(this GenericEvent<T1, T2> genericEvent, T1 args1, T2 args2)
        {
            if (genericEvent != null)
            {
                genericEvent(args1, args2);
            }
        }

        public static void InvokeIfNotNull<T1, T2, T3>(this GenericEvent<T1, T2, T3> genericEvent, T1 args1, T2 args2, T3 args3)
        {
            if (genericEvent != null)
            {
                genericEvent(args1, args2, args3);
            }
        }

        public static void InvokeIfNotNull<T1, T2, T3, T4>(this GenericEvent<T1, T2, T3, T4> genericEvent, T1 args1, T2 args2, T3 args3, T4 args4)
        {
            if (genericEvent != null)
            {
                genericEvent(args1, args2, args3, args4);
            }
        }

        public static void InvokeIfNotNull<T1, T2, T3, T4, T5>(this GenericEvent<T1, T2, T3, T4, T5> genericEvent, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5)
        {
            if (genericEvent != null)
            {
                genericEvent(args1, args2, args3, args4, args5);
            }
        }

        public static void InvokeIfNotNull<T1, T2, T3, T4, T5, T6>(this GenericEvent<T1, T2, T3, T4, T5, T6> genericEvent, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5, T6 args6)
        {
            if (genericEvent != null)
            {
                genericEvent(args1, args2, args3, args4, args5, args6);
            }
        }
    }
}
