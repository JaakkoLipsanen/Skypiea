using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Flai.DataStructures
{
    public class QuaternaryHeap<T> : PriorityQueue<T>, IEnumerable<T>
        where T : IComparable<T>, IEquatable<T>
    {
        protected int _lastUsedIndex = -1;
        protected T[] _heap;

        public int Count
        {
            get { return _lastUsedIndex + 1; }
        }

        public override bool HasNext
        {
            // If last used index is not -1, this heap contains values
            get { return _lastUsedIndex != -1; }
        }

        public QuaternaryHeap()
            : this(16, HeapType.MinHeap)
        {
        }

        public QuaternaryHeap(int capacity, HeapType heapType)
            : base(heapType == HeapType.MinHeap ? PriorityDirection.Lowest : PriorityDirection.Highest)
        {
            _heap = new T[capacity];
        }

        #region Priority Queue -methods

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
                int parentIndex = (index - 1) / 4;

                if (this.PriorityCompare(value, _heap[parentIndex]) < 0) // value.TotalCost >= _heap[parentIndex].TotalCost)
                {
                    break;
                }

                // Swap the node with it's parent node
                _heap.Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        /// <summary>
        /// Returns highest priorirty value in the queue. NOTE! Before calling this, check that there is values in the queue by calling HasNext -property!
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
                int first = 4 * index + 1;
                int second = first + 1;
                int third = second + 1;
                int fourth = third + 1;

                int newIndex;
                if (fourth <= _lastUsedIndex)
                {
                    int best12 = this.PriorityCompare(_heap[first], _heap[second]) > 0 ? first : second;
                    int best34 = this.PriorityCompare(_heap[third], _heap[fourth]) > 0 ? third : fourth;
                    newIndex = this.PriorityCompare(_heap[best12], _heap[best34]) > 0 ? best12 : best34;
                }
                else if (third <= _lastUsedIndex)
                {
                    int best12 = this.PriorityCompare(_heap[first], _heap[second]) > 0 ? first : second;
                    newIndex = this.PriorityCompare(_heap[best12], _heap[third]) > 0 ? best12 : third;
                }
                else if (second <= _lastUsedIndex)
                {
                    newIndex = this.PriorityCompare(_heap[first], _heap[second]) > 0 ? first : second;
                }
                else if (first <= _lastUsedIndex)
                {
                    newIndex = first;
                }
                else
                {
                    break;
                }

                if (this.PriorityCompare(_heap[newIndex], _heap[index]) < 0) // _heap[newIndex].TotalCost >= _heap[index].TotalCost)
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
            for (int e = 0; e < _heap.Length; e++)
            {
                if (_heap[e] == null)
                {
                    break;
                }
                else if (_heap[e].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        public void Clear()
        {
            Array.Clear(_heap, 0, _heap.Length);
            _lastUsedIndex = -1;
        }

        public void Clear(T defaultValue)
        {
            for (int e = 0; e < _heap.Length; e++)
            {
                _heap[e] = defaultValue;
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

            return "Empty QuaternaryHeap<" + typeof(T).Name + ">";
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

