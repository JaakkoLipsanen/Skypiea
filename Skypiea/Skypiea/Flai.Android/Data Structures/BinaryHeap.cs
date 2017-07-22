using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Flai.DataStructures
{
    public class BinaryHeap<T> : PriorityQueue<T>, IEnumerable<T>
        where T : IComparable<T>, IEquatable<T>
    {
        protected T[] _heap;
        protected int _lastUsedIndex = -1;

        public int Count
        {
            get { return _lastUsedIndex + 1; }
        }

        public override bool HasNext
        {
            // If last used index is not -1, this heap contains values
            get { return _lastUsedIndex != -1; }
        }

        public BinaryHeap()
            : this(16, HeapType.MinHeap)
        {

        }

        public BinaryHeap(int capacity, HeapType heapType)
            : base(heapType == HeapType.MinHeap ? PriorityDirection.Lowest : PriorityDirection.Highest)
        {
            _heap = new T[capacity];
        }

        public override void Add(T value)
        {
            if (_lastUsedIndex + 1 >= _heap.Length)
            {
                Array.Resize(ref _heap, _heap.Length * 2);
            }
            _heap[++_lastUsedIndex] = value;

            int index = _lastUsedIndex;
            while (index != 0)
            {
                int parentIndex = (index - 1) / 2;

                if (this.PriorityCompare(value, _heap[parentIndex]) < 0)// value.TotalCost >= _heap[parentIndex].TotalCost)
                {
                    break;
                }

                _heap.Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        /// <summary>
        ///   Returns highest priorirty value in the queue. NOTE! Before calling this, check that there is values in the queue by calling HasNext -property!
        /// </summary>
        public override T Dequeue()
        {
            Ensure.True(this.HasNext, "Cannot dequeue from empty heap");
            T minimum = _heap[0];

            // Swap the minimum ( = highest priority ) node with the last node in the heap           
            _heap[0] = _heap[_lastUsedIndex];
            _heap[_lastUsedIndex] = default(T);
            _lastUsedIndex--;

            int index = 0;
            while (true)
            {
                int left = 2 * index + 1;
                int right = left + 1;

                int newIndex;
                if (right <= _lastUsedIndex && this.PriorityCompare(_heap[index], _heap[right]) < 0) // _heap[right].TotalCost < _heap[newIndex].TotalCost)
                {
                    newIndex = this.PriorityCompare(_heap[left], _heap[right]) > 0 ? left : right; // (_heap[left].TotalCost < _heap[right].TotalCost) ? left : right;
                }
                else if (left <= _lastUsedIndex && this.PriorityCompare(_heap[index], _heap[left]) < 0) //_heap[left].TotalCost < _heap[newIndex].TotalCost)
                {
                    newIndex = left;
                }
                else
                {
                    break;
                }

                _heap.Swap(index, newIndex);
                index = newIndex;
            }

            return minimum;
        }

        public override T Peek()
        {
            return _lastUsedIndex == -1 ? default(T) : _heap[0];
        }

        public override bool Contains(T value)
        {
            for (int i = 0; i < _heap.Length; i++)
            {
                if (_heap[i] == null)
                {
                    break;
                }
                else if (_heap[i].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            Array.Clear(_heap, 0, _heap.Length);
            _lastUsedIndex = -1;
        }

        public void Clear(T defaultValue)
        {
            for (int i = 0; i < _heap.Length; i++)
            {
                _heap[i] = defaultValue;
            }

            _lastUsedIndex = -1;
        }

        public override string ToString()
        {
            if (_lastUsedIndex != -1)
            {
                StringBuilder builder = new StringBuilder(Math.Min(500, _lastUsedIndex * 5));

                builder.Append("[");

                for (int e = 0; e < Math.Min(_lastUsedIndex, 100); e++)
                {
                    builder.Append(_heap[e].ToString());
                    builder.Append(", ");
                }

                builder.Append(_heap[_lastUsedIndex].ToString());
                builder.Append("]");

                return builder.ToString();
            }

            return "Empty BinaryHeap<" + typeof(T).Name + ">";
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_heap).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
