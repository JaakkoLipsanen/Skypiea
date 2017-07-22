using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    // Just a nicer name
    public class ReadOnlyArray<T> : ReadOnlyList<T>
    {
        public int Length
        {
            get { return this.Count; }
        }

        public ReadOnlyArray(T[] array)
            : base(array)
        {
        }
    }

    public class ReadOnlyBag<T> : IEnumerable<T>
    {
        private readonly Bag<T> _bag;

        public int Count
        {
            get { return _bag.Count; }
        }

        public ReadOnlyBag(Bag<T> bag)
        {
            _bag = bag;
        }

        public T this[int index]
        {
            get { return _bag[index]; }
        }

        public Bag<T>.Enumerator GetEnumerator()
        {
            return new Bag<T>.Enumerator(_bag);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // todo: IList<T>?
    public class ReadOnlyList<T> : IList<T>
    {
        private readonly IList<T> _innerList;
        public int Count
        {
            get { return _innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ReadOnlyList(IList<T> list)
        {
            Ensure.NotNull(list);
            _innerList = list;
        }

        public T this[int i]
        {
            get { return _innerList[i]; }
        }

        public bool Contains(T item)
        {
            return _innerList.Contains(item);
        }

        public int IndexOf(T item)
        {
            return _innerList.IndexOf(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        T IList<T>.this[int i]
        {
            get { return _innerList[i]; }
            set { throw new NotSupportedException(); }
        }

        #region Implementation of IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        #endregion
    }
}
