using Flai.DataStructures;

namespace Flai.Graphics.Particles
{
    public class ParticleBuffer : CircularBuffer<Particle>
    {
        public ParticleBuffer(int size)
            : base(size)
        {
        }

        public Iterator CreateIterator(IteratorMode iteratorMode)
        {
            return new Iterator(this, iteratorMode);
        }

        public enum IteratorMode
        {
            Normal = 1,
            Inverse = -1,
        }

        // todo: remove _currentIteration?
        public struct Iterator
        {
            private readonly ParticleBuffer _buffer;
            private readonly Particle[] _bufferArray; // use _buffer._buffer for straight array access (faster) and to skip index checks done in []
            private readonly IteratorMode _iteratorMode;

            private int _currentIteration;
            private int _currentIndex;
          
            public Particle First
            {
                get { return (_iteratorMode == IteratorMode.Normal) ? _buffer.Tail : _buffer.Head; }
            }

            internal Iterator(ParticleBuffer buffer, IteratorMode iteratorMode)
            {
                _buffer = buffer;
                _bufferArray = _buffer._buffer;
                _currentIteration = 0;
                _currentIndex = (iteratorMode == IteratorMode.Normal) ? _buffer._tailIndex : _buffer._headIndex;
                _iteratorMode = iteratorMode;
            }

            // todo: speed up? probably can't speed up any more without unsafe code. not sure if the unsafe code is really a lot faster either... but anyways, this is the slowest part of particle engine hands down.
            public bool MoveNext(ref Particle particle)
            {
                // set the particle back to the buffer            
                _bufferArray[_currentIndex] = particle;
                if (++_currentIteration >= _buffer._size)
                {
                    return false;
                }

                _currentIndex += (int) _iteratorMode;
                if (_currentIndex == -1)
                {
                    _currentIndex = _bufferArray.Length - 1;
                }
                else if(_currentIndex == _bufferArray.Length)
                {
                    _currentIndex = 0;
                }

                particle = _bufferArray[_currentIndex];
                return true;
            }

            // something like this
            //public unsafe bool MoveNext(Particle** particle)
            //{
            //    if (++_currentIteration >= _buffer._size)
            //    {
            //        return false;
            //    }

            //    _currentIndex += (int)_iteratorMode;
            //    if (_currentIndex == -1)
            //    {
            //        _currentIndex = _bufferArray.Length - 1;
            //    }
            //    else if (_currentIndex == _bufferArray.Length)
            //    {
            //        _currentIndex = 0;
            //    }

            //    fixed (Particle* arr = &_bufferArray[0])
            //    {
            //        Particle* arr2 = arr;
            //        (*particle) = *(&arr2 + _currentIndex); // (*(_bufferArray) + _currentIndex);
            //    }

            //    return true;
            //}

            public void Reset()
            {
                _currentIteration = 0;
                _currentIndex = (_iteratorMode == IteratorMode.Normal) ? _buffer._tailIndex : _buffer._headIndex;
            }
        }
    }
}
