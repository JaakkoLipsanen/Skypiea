using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    // this could implement some method/properties from List<T> too.. probably useful
    public abstract class ExtendableList<T> : IList<T>
    {
        protected readonly List<T> _list = new List<T>();

        #region IList<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual void Add(T item)
        {
            _list.Add(item);
        }

        public virtual void Clear()
        {
            _list.Clear();
        }

        public virtual bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(T item)
        {
            return _list.Remove(item);
        }

        public virtual int Count
        {
            get { return _list.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)_list).IsReadOnly; }
        }

        public virtual int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public virtual T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        #endregion
    }
}
