using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    public class SizedQueue<T> : ICustomQueue<T>
    {
        private readonly Queue<T> _innerQueue;
        private int _capacity = -1;

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                Ensure.True(value >= 0);
                int previousCapacity = _capacity;
                _capacity = value;

                if (_capacity < previousCapacity)
                {
                    while (_innerQueue.Count > _capacity)
                    {
                        _innerQueue.Dequeue();
                    }
                    _innerQueue.TrimExcess();
                }
            }
        }

        public int Count
        {
            get { return _innerQueue.Count; }
        }

        public bool IsEmpty
        {
            get { return _innerQueue.Count == 0; }
        }

        public bool IsFull
        {
            get { return _innerQueue.Count == _capacity; }
        }

        public SizedQueue(int size)
        {
            Ensure.True(size >= 0);

            _capacity = size;
            _innerQueue = new Queue<T>(_capacity);
        }

        public bool Contains(T item)
        {
            return _innerQueue.Contains(item);
        }

        public void Enqueue(T item)
        {
            while (_innerQueue.Count >= this.Capacity)
            {
                _innerQueue.Dequeue();
            }

            _innerQueue.Enqueue(item);
        }

        public T Dequeue()
        {
            return _innerQueue.Dequeue();
        }

        public T Peek()
        {
            return _innerQueue.Peek();
        }

        public void Clear()
        {
            _innerQueue.Clear();
        }

        public void CopyTo(T[] array, int index)
        {
            _innerQueue.CopyTo(array, index);
        }

        public T[] ToArray()
        {
            return _innerQueue.ToArray();
        }

        public override string ToString()
        {
            return string.Join(", ", _innerQueue);
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _innerQueue.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerQueue.GetEnumerator();
        }

        #endregion
    }
}