using System;

namespace Flai.DataStructures
{
    public interface IObjectPool<T>
    {
        T Fetch();
        void Store(T item);
    }

    public class ObjectPool<T> : IObjectPool<T> 
        where T : class
    {
        private const int DefaultCapacity = 16;
        private const int DefaultPreInitializeAmount = 8;

        private readonly Bag<T> _availableObjects = new Bag<T>();
        private readonly Func<T> _createNewFunction;
        private readonly Action<T> _initializeFunction;
        private readonly Action<T> _deinitializeFunction;

        public ObjectPool(Func<T> createNewFunction)
            : this(createNewFunction, ObjectPool<T>.DefaultPreInitializeAmount, null, null)
        {
        }

        public ObjectPool(Func<T> createNewFunction, int preInitializeAmount)
            : this(createNewFunction, preInitializeAmount, null, null)
        {
        }

        public ObjectPool(Func<T> createNewFunction, int preInitializeAmount, Action<T> initializeFunction, Action<T> deinitializeFunction)
        {
            Ensure.True(preInitializeAmount >= 0, "Pre-initialize amount must be positive");

            _availableObjects = new Bag<T>(Math.Max(ObjectPool<T>.DefaultCapacity, preInitializeAmount));
            _createNewFunction = createNewFunction;
            _initializeFunction = initializeFunction;
            _deinitializeFunction = deinitializeFunction;

            for (int i = 0; i < preInitializeAmount; i++)
            {
                _availableObjects.Add(_createNewFunction());
            }
        }

        public T Fetch()
        {
            T item = _availableObjects.Count > 0 ? _availableObjects.RemoveLast() : _createNewFunction();
            if (_initializeFunction != null)
            {
                _initializeFunction(item);
            }

            return item;
        }

        public void Store(T item)
        {
            if (_deinitializeFunction != null)
            {
                _deinitializeFunction(item);
            }

            _availableObjects.Add(item);
        }
    }
}
