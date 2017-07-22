
namespace Flai.CBES
{
    // PrefabParameterCollection?
    public sealed class ParameterCollection
    {
        private readonly object[] _innerArray;
        public int Length
        {
            get { return _innerArray.Length; }
        }

        internal ParameterCollection(int capacity)
        {
            _innerArray = new object[capacity];
        }

        public bool HasIndex(int index)
        {
            return this.Length > index;
        }

        public T Get<T>(int index)
        {
            Assert.True(_innerArray[index] is T);
            return (T)_innerArray[index];
        }

        public T GetOrDefault<T>(int index)
        {
            if (index < this.Length)
            {
                Assert.True(_innerArray[index] is T);
                return (T)_innerArray[index];
            }

            return default(T);
        }

        public T GetOrDefault<T>(int index, T defaultValue)
        {
            if (index < this.Length)
            {
                Assert.True(_innerArray[index] is T);
                return (T)_innerArray[index];
            }

            return defaultValue;
        }

        // dont use indexer since that would cause an additional boxing.. or would it be additional? actually no :P it would just get boxed once. well whatever. use this still
        internal void Set<T>(T value, int index)
        {
            _innerArray[index] = value;
        }

        public object this[int index]
        {
            get { return _innerArray[index]; }
        } 
    }
}
