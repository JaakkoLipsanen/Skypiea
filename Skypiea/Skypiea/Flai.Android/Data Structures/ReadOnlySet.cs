using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    // HashSet<T> doesn't implement any methods/properties that an ISet<T> wouldn't, so lets use ISet rather than HashSet<
    public class ReadOnlySet<T> : ISet<T>
    {
        private readonly ISet<T> _innerSet;
        public ReadOnlySet(ISet<T> innerSet)
        {
            Ensure.NotNull(innerSet);
            _innerSet = innerSet;
        }

        #region ISet<T> Members

        bool ISet<T>.Add(T item)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _innerSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _innerSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _innerSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _innerSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _innerSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _innerSet.SetEquals(other);
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        public bool Contains(T item)
        {
            return _innerSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerSet.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerSet.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException("This set is read-only and cannot be modified");
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _innerSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerSet.GetEnumerator();
        }

        #endregion
    }
}