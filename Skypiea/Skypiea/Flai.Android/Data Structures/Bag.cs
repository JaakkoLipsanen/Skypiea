using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flai.General;

namespace Flai.DataStructures
{
    // idea from Artemis CBES library. basically list, but setting via [] can grow the capacity and unordered
    // removing from anywhere is O(1), otherwise should be about same as List<T>
    // todo: public method to change the capacity to smaller? or bigger too. EnsureCapacity which can take smaller values than Count?

    public class Bag<T> : IList<T>
    {
        protected T[] _items; // protected so that inheriting classes can possibly even expose the array for faster access
        public int Capacity
        {
            get { return _items.Length; }
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }

        public Bag()
            : this(4)
        {
        }

        public Bag(int capacity)
        {
            _items = (capacity == 0) ? ArrayHelper<T>.Empty : new T[capacity];
        }

        public Bag(IEnumerable<T> items)
        {
            _items = items.ToArray();
        }

        public void Add(T item)
        {
            if (this.Count == this.Capacity)
            {
                this.Grow();
            }

            _items[this.Count++] = item;
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index != -1)
            {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        public T RemoveAt(int index)
        {
            T item = _items[index];
            --this.Count;

            // move the last item with this
            _items[index] = _items[this.Count];
            _items[this.Count] = default(T);

            return item;
        }

        public T RemoveLast()
        {
            if (this.Count > 0)
            {
                T result = _items[--this.Count];
                _items[this.Count] = default(T);

                return result;
            }

            return default(T);
        }

        public bool TryRemoveLast(out T result)
        {
            if (this.Count > 0)
            {
                result = _items[--this.Count];
                _items[this.Count] = default(T);

                return true;
            }

            result = default(T);
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                _items[i] = default(T);
            }

            this.Count = 0;
        }

        // not recommended to use if 'T' is a reference type
        public void ClearRaw()
        {
            // no setting items [0, this.Count - 1] to default(T)
            this.Count = 0;
        }

        public bool Contains(T item)
        {
            for (int index = this.Count - 1; index >= 0; --index)
            {
                if (item.Equals(_items[index]))
                {
                    return true;
                }
            }

            return false;
        }

        public int IndexOf(T item)
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (_items[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /* Get/SetRaw = fast gets/sets without any checks */
        public T GetRaw(int index)
        {
            return _items[index];
        }

        public void SetRaw(int index, T value)
        {
            _items[index] = value;
        }

        public void GetRaw(int index, out T value)
        {
            value = _items[index];
        }

        public void SetRaw(int index, ref T value)
        {
            _items[index] = value;
        }

        public T this[int index]
        {
            get
            {
                if (index >= this.Count)
                {
                    return default(T);
                }

                // ehm... should I return default(T) or return it and expand the array? idk.. 

                //if (index >= this.Count)
                //{
                //    this.Count = index + 1;
                //    if (index >= this.Capacity)
                //    {
                //        this.EnsureCapacity(index * 2);
                //    }

                //    return default(T);
                //}

                return _items[index];
            }
            set
            {
                if (index >= this.Count)
                {
                    this.Count = index + 1;
                    if (index >= this.Capacity)
                    {
                        this.EnsureCapacity(index * 2);
                    }
                }

                _items[index] = value;
            }
        }

        private void Grow()
        {
            this.EnsureCapacity((int)(this.Count * 1.5f) + 1);
        }

        public void EnsureCapacity(int capacity)
        {
            // could make "EnsureCapacityInner" for the calls from inside the Bag<T>.cs which wouldn't have these if checks
            if (capacity < this.Capacity)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            else if (capacity == this.Capacity)
            {
                return;
            }

            Array.Resize(ref _items, capacity);
        }

        // does this work right? 
        public void TrimExcess()
        {
            if (this.Count >= this.Capacity * 0.9)
            {
                return;
            }

            int newCapacity = FlaiMath.Max(4, this.Count);
            Array.Resize(ref _items, newCapacity); // not 100% if works, but I think it does
        }

        #region Implementation of IList<T>

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, this.Count - arrayIndex); // okay i'm not sure if this works
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] _innerArray;
            private readonly int _count;
            private int _index;

            public T Current
            {
                get { return _innerArray[_index]; }
            }

            internal Enumerator(Bag<T> bag)
            {
                _innerArray = bag._items;
                _index = -1;
                _count = bag.Count;
            }

            public bool MoveNext()
            {
                return (++_index < _count);
            }

            public void Reset()
            {
                _index = -1;
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public void Dispose()
            {
            }
        }
    }
}
