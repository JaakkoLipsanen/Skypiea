using Flai;
#if WINDOWS_PHONE && !WP8_MONOGAME
using System.Linq;

namespace System.Collections.Generic
{
    // From https://gist.github.com/tpetrina/4581209
    public interface ISet<T> : ICollection<T>
    {
        bool Add(T item);
        void ExceptWith(IEnumerable<T> other);
        void IntersectWith(IEnumerable<T> other);
        bool IsProperSubsetOf(IEnumerable<T> other);
        bool IsProperSupersetOf(IEnumerable<T> other);
        bool IsSubsetOf(IEnumerable<T> other);
        bool IsSupersetOf(IEnumerable<T> other);
        bool Overlaps(IEnumerable<T> other);
        bool SetEquals(IEnumerable<T> other);
        void SymmetricExceptWith(IEnumerable<T> other);
        void UnionWith(IEnumerable<T> other);
    }

    public class HashSet<T> : ISet<T>
    {
        private readonly Dictionary<T, bool> _data;
        public int Count
        {
            get { return _data.Count; }
        }

        public IEqualityComparer<T> Comparer
        {
            get { return _data.Comparer; }
        }

        #region Implementation of public methods

        public HashSet()
        {
            _data = new Dictionary<T, bool>();
        }

        public HashSet(IEnumerable<T> collection)
        {
            Ensure.NotNull(collection);

            _data = new Dictionary<T, bool>();
            this.AddRange(collection);
        }

        public HashSet(IEqualityComparer<T> comparer)
        {
            _data = new Dictionary<T, bool>(comparer);
        }

        public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _data = new Dictionary<T, bool>(comparer);
            this.AddRange(collection);
        }

        public bool Add(T item)
        {
            if (_data.ContainsKey(item))
            {
                return false;
            }

            _data.Add(item, true);
            return true;
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(T item)
        {
            return _data.ContainsKey(item);
        }

        public void CopyTo(T[] array)
        {
            Ensure.NotNull(array);
            _data.Keys.CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Ensure.NotNull(array);
            Ensure.WithinRange(arrayIndex, 0, array.Length - 1);
           
            _data.Keys.CopyTo(array, arrayIndex);
        }

        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            throw new NotImplementedException("");
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            Ensure.NotNull(other);
            foreach (var item in other)
            {
                _data.Remove(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            Ensure.NotNull(other);

            IEnumerable<T> result = other.Where(this.Contains);
            this.Clear();
            this.AddRange(result);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            Ensure.NotNull(other);

            HashSet<T> otherSet = new HashSet<T>(other);
            return (this.Count < otherSet.Count && _data.Keys.All(otherSet.Contains));
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            Ensure.NotNull(other);

            if (this.Count == 0)
            {
                return false;
            }

            var otherSet = new HashSet<T>(other);
            return (this.Count > otherSet.Count && otherSet.All(Contains));
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            Ensure.NotNull(other);
            if (this.Count == 0)
            {
                return true;
            }

            var otherSet = new HashSet<T>(other);
            return (this.Count <= otherSet.Count && _data.Keys.All(otherSet.Contains));
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            Ensure.NotNull(other);
            return other.All(this.Contains);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            Ensure.NotNull(other);
            return other.Any(this.Contains);
        }

        public bool Remove(T item)
        {
            if (!_data.ContainsKey(item))
            {
                return false;
            }

            _data.Remove(item);
            return true;
        }

        public int RemoveWhere(Predicate<T> match)
        {
            Ensure.NotNull(match);

            var result = _data.Keys.Where(i => match(i)).ToArray();
            var removedCount = (this.Count - result.Length);
            foreach (T removed in result)
            {
                this.Remove(removed);
            }

            return removedCount;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            Ensure.NotNull(other);

            var otherSet = new HashSet<T>();
            foreach (var item in other)
            {
                if (!this.Contains(item))
                {
                    return false;
                }

                otherSet.Add(item);
            }

            return (otherSet.Count == Count);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            Ensure.NotNull(other);
            foreach (var item in other.Where(this.Contains))
            {
                this.Remove(item);
            }
        }

        public void TrimExcess()
        {
            throw new NotImplementedException(); // throw exception or just do nothing?
        }

        public void UnionWith(IEnumerable<T> other)
        {
            Ensure.NotNull(other);
            this.AddRange(other);
        }

        #endregion

        private void AddRange(IEnumerable<T> items)
        {
            Ensure.NotNull(items);
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        #region Remaining interface implementations

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        #endregion
    }
}
#endif