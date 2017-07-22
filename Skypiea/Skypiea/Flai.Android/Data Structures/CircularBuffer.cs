
using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    // Using Assert instead of Ensure here, because CircularBuffer is a high performance collection and thus all the speed possible is needed
    public class CircularBuffer<T> : IEnumerable<T>
    {
        protected readonly T[] _buffer;
        protected int _size;
        protected int _headIndex = -1; // where the current head is. -1 because when the first one is added, it will go to index = 0
        protected int _tailIndex; // where the first one is

        public int Count
        {
            get { return _size; }
        }

        public int Capacity
        {
            get { return _buffer.Length; }
        }

        public T Tail
        {
            get
            {
                Assert.True(_size > 0);
                return _buffer[_tailIndex];
            }
        }

        public T Head
        {
            get
            {
                Assert.True(_size > 0);
                return _buffer[_headIndex];
            }
        }

        public int HeadIndex
        {
            get
            {
                Assert.True(_size > 0);
                return _headIndex;
            }
        }

        public int TailIndex
        {
            get 
            {
                Assert.True(_size > 0);
                return _tailIndex;
            }
        }

        public bool IsEmpty
        {
            get { return _size == 0; }
        }

        public CircularBuffer(int size)
        {
            Ensure.True(size > 0);
            _buffer = new T[size];
        }

        public void AddLast(T value)
        {
            this.AddLast(ref value);
        }

        public void AddLast(ref T value)
        {
            if (++_headIndex == _buffer.Length)
            {
                _headIndex = 0;
            }

            _buffer[_headIndex] = value;
            if (_size == _buffer.Length)
            {
                if (++_tailIndex == _buffer.Length)
                {
                    _tailIndex = 0;
                }
            }
            _size = FlaiMath.Min(_size + 1, _buffer.Length);
        }

        public T RemoveFirst()
        {
            Assert.True(_size != 0);
            T value = _buffer[_tailIndex];

            _buffer[_tailIndex] = default(T);
            if (++_tailIndex == _buffer.Length)
            {
                _tailIndex = 0;
            }

            _size--;
            return value;
        }

        public void Get(int index, out T value)
        {
            value = _buffer[index];
        }

        public void Set(int index, ref T value)
        {
            Assert.True(index < _tailIndex + _size || index < (_tailIndex + _size - _buffer.Length));
            _buffer[index] = value;
        }

        public T this[int index]
        {
            get { return _buffer[index]; }
            set
            {
                Assert.True(index < _tailIndex + _size || index < (_tailIndex + _size - _buffer.Length));
                _buffer[index] = value;
            }
        }

        public void Clear()
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            _size = 0;
            _headIndex = 0;
            _tailIndex = 0;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0, bufferIndex = _tailIndex; i < _size; i++, bufferIndex++)
            {
                if (bufferIndex == _buffer.Length)
                {
                    bufferIndex = 0;
                }

                yield return _buffer[bufferIndex];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)(this)).GetEnumerator();
        }  
    }
}
