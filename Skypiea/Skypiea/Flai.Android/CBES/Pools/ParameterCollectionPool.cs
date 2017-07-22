
namespace Flai.CBES.Pools
{
    // non thread safe!!
    internal class ParameterCollectionPool
    {
        private const int MaxSize = 10 + 1; // + 1 because 0 is valid size
        private readonly ParameterCollection[] _collections = new ParameterCollection[ParameterCollectionPool.MaxSize];

        public ParameterCollectionPool()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                _collections[i] = new ParameterCollection(i);
            }
        }

        #region Create overloads

        // TODO: Use "ref" ?!
        public ParameterCollection Create()
        {
            ParameterCollection collection = _collections[1];
            return collection;
        }

        public ParameterCollection Create<T1>(T1 obj1)
        {
            ParameterCollection collection = _collections[1];
            collection.Set(obj1, 0);

            return collection;
        }

        public ParameterCollection Create<T1, T2>(T1 obj1, T2 obj2)
        {
            ParameterCollection collection = _collections[2];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3)
        {
            ParameterCollection collection = _collections[3];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4>(T1 obj1, T2 obj2, T3 obj3, T4 obj4)
        {
            ParameterCollection collection = _collections[4];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4, T5>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5)
        {
            ParameterCollection collection = _collections[5];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);
            collection.Set(obj5, 4);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4, T5, T6>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6)
        {
            ParameterCollection collection = _collections[6];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);
            collection.Set(obj5, 4);
            collection.Set(obj6, 5);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4, T5, T6, T7>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7)
        {
            ParameterCollection collection = _collections[7];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);
            collection.Set(obj5, 4);
            collection.Set(obj6, 5);
            collection.Set(obj7, 6);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7, T8 obj8)
        {
            ParameterCollection collection = _collections[8];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);
            collection.Set(obj5, 4);
            collection.Set(obj6, 5);
            collection.Set(obj7, 6);
            collection.Set(obj8, 7);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7, T8 obj8, T9 obj9)
        {
            ParameterCollection collection = _collections[9];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);
            collection.Set(obj5, 4);
            collection.Set(obj6, 5);
            collection.Set(obj7, 6);
            collection.Set(obj8, 7);
            collection.Set(obj9, 8);

            return collection;
        }

        public ParameterCollection Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7, T8 obj8, T9 obj9, T10 obj10)
        {
            ParameterCollection collection = _collections[10];
            collection.Set(obj1, 0);
            collection.Set(obj2, 1);
            collection.Set(obj3, 2);
            collection.Set(obj4, 3);
            collection.Set(obj5, 4);
            collection.Set(obj6, 5);
            collection.Set(obj7, 6);
            collection.Set(obj8, 7);
            collection.Set(obj9, 8);
            collection.Set(obj10, 9);

            return collection;
        }
    }

        #endregion
}
