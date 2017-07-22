using System;
using System.Collections;
using System.Collections.Generic;
using Flai.General;

namespace Flai.DataStructures
{
    // Basically List<T>, but the inner array is exposed.
    public class DynamicArray<T> : IList<T>
    {
        private const int MiniumInitialCapacity = 4;

        private T[] _innerArray;
        private int _size = 0;

        #region Properties

        public T[] InnerArray
        {
            get { return _innerArray; }
        }

        public bool IsEmpty
        {
            get { return _size == 0; }
        }

        public int Count
        {
            get { return _size; }
        }

        public int Capacity
        {
            get { return _innerArray.Length; }
            set
            {
                Ensure.True(value >= _size, "New capacity cannot be smaller than the number of items in the array");

                if (value != _innerArray.Length)
                {
                    if (value == 0)
                    {
                        _innerArray = ArrayHelper<T>.Empty;
                    }
                    else
                    {
                        T[] tempArray = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_innerArray, 0, tempArray, 0, _size);
                        }

                        _innerArray = tempArray;
                    }
                }
            }
        }

        public T this[int index]
        {
            get
            {
                Ensure.True(index <= _size);
                return _innerArray[index];
            }
            set
            {
                Ensure.True(index < _size);
                _innerArray[index] = value;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        #endregion

        public DynamicArray()
            : this(0)
        {
        }

        public DynamicArray(int capacity)
        {
            _innerArray = capacity == null ? ArrayHelper<T>.Empty : new T[Math.Max(MiniumInitialCapacity, capacity)];
        }

        #region Implementation of IList<T>

        public void Add(T item)
        {
            if (_size == _innerArray.Length)
            {
                this.EnsureCapacity(_size + 1);
            }

            _innerArray[_size++] = item;
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_innerArray, 0, _size);
                _size = 0;
            }
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                for (int i = 0; i < this._size; i++)
                {
                    if (_innerArray[i] == null)
                    {
                        return true;
                    }
                }
                return false;
            }

            EqualityComparer<T> defaultComparer = EqualityComparer<T>.Default;
            for (int j = 0; j < _size; j++)
            {
                if (defaultComparer.Equals(_innerArray[j], item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array)
        {
            this.CopyTo(array, 0);
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_innerArray, 0, array, arrayIndex, this._size);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            Ensure.True(_size - index >= count);
            Array.Copy(_innerArray, index, array, arrayIndex, count);
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf<T>(_innerArray, item, 0, _size);
        }

        public void Insert(int index, T item)
        {
            Ensure.True(index <= _size);

            if (_size == _innerArray.Length)
            {
                this.EnsureCapacity(_size + 1);
            }

            if (index < _size)
            {
                Array.Copy(_innerArray, index, _innerArray, index + 1, _size - index);
            }

            _innerArray[index] = item;
            _size++;
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            Ensure.True(index < _size);

            _size--;
            if (index < _size)
            {
                Array.Copy(_innerArray, index + 1, _innerArray, index, _size - index);
            }

            _innerArray[_size] = default(T);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_innerArray).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public void EnsureCapacity(int minCapacity)
        {
            if (_innerArray.Length < minCapacity)
            {
                int newCapacity = this.IsEmpty ? MiniumInitialCapacity : (_innerArray.Length * 2);
                if (newCapacity < minCapacity)
                {
                    newCapacity = minCapacity;
                }
                this.Capacity = newCapacity;
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            Array.Copy(_innerArray, array, this.Count);

            return array;
        }
    }
}
