using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    public class ReadOnlyQueue<T> : ICustomQueue<T>
    {
        #region Queue Views

        private interface IQueueView : IEnumerable<T>
        {
            int Count { get; }

            bool Contains(T item);
            void CopyTo(T[] array, int arrayIndex);
           
            T Peek();
            T[] ToArray();
        }

        private class QueueView : IQueueView
        {
            private readonly Queue<T> _innerQueue;
            public QueueView(Queue<T> queue)
            {
                _innerQueue = queue;
            }

            #region IQueueView<T> Members

            public int Count
            {
                get { return _innerQueue.Count; }
            }

            public bool Contains(T item)
            {
                return _innerQueue.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _innerQueue.CopyTo(array, arrayIndex);
            }

            public T Peek()
            {
                return _innerQueue.Peek();
            }

            public T[] ToArray()
            {
                return _innerQueue.ToArray();
            }

            #endregion

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

        private class CustomQueueView : IQueueView
        {
            private readonly ICustomQueue<T> _innerQueue;
            public CustomQueueView(ICustomQueue<T> queue)
            {
                _innerQueue = queue;
            }

            #region IQueueView<T> Members

            public int Count
            {
                get { return _innerQueue.Count; }
            }

            public bool Contains(T item)
            {
                return _innerQueue.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _innerQueue.CopyTo(array, arrayIndex);
            }

            public T Peek()
            {
                return _innerQueue.Peek();
            }

            public T[] ToArray()
            {
                return _innerQueue.ToArray();
            }

            #endregion

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

        #endregion

        private readonly IQueueView _innerQueue;
        public ReadOnlyQueue(Queue<T> queue)
        {
            Ensure.NotNull(queue);
            _innerQueue = new QueueView(queue);
        }

        // TODO: Make "IQueue<T>" interface for all custom queues?
        public ReadOnlyQueue(ICustomQueue<T> queue)
        {
            Ensure.NotNull(queue);
            _innerQueue = new CustomQueueView(queue);
        }

        public int Count { get; private set; }
        public bool IsEmpty { get; private set; }

        public bool Contains(T value)
        {
            return _innerQueue.Contains(value);
        }

        T ICustomQueue<T>.Dequeue()
        {
            throw new NotImplementedException();
        }

        void ICustomQueue<T>.Enqueue(T value)
        {
            throw new NotImplementedException();
        }

        public T Peek()
        {
            return _innerQueue.Peek();
        }

        public T[] ToArray()
        {
            return _innerQueue.ToArray();
        }

        void ICustomQueue<T>.Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int index)
        {
            _innerQueue.CopyTo(array, index);
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
