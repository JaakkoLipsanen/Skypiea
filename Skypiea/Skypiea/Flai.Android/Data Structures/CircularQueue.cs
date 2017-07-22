using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    // TODO: This could have a "Dictionary<T, LinkedListNode<T>>" also, so that DeleteEntity could be O(1)
    // >> "_queue.DeleteEntity(_nodeMapper[value]); _nodeMapper.DeleteEntity(value)"
    public class CircularQueue<T> : ICustomQueue<T>
    {
        private readonly LinkedList<T> _queue = new LinkedList<T>();

        public int Count
        {
            get { return _queue.Count; }
        }

        public bool IsEmpty
        {
            get { return this.Count == 0; }
        }

        public bool Contains(T value)
        {
            return _queue.Contains(value);
        }

        public void Enqueue(T value)
        {
            _queue.AddLast(value);
        }

        public T Dequeue()
        {
            Ensure.False(this.IsEmpty, "The queue is empty!");

            var node = _queue.First;
            _queue.RemoveFirst();
            _queue.AddLast(node);

            return node.Value;
        }

        public T Peek()
        {
            Ensure.False(this.IsEmpty, "The queue is empty!");
            return _queue.First.Value;
        }

        public bool Remove(T value)
        {
            return _queue.Remove(value);
        }

        public T[] ToArray()
        {
            return _queue.ToArray();
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public void CopyTo(T[] array, int index)
        {
            _queue.CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _queue.GetEnumerator();
        }
    }
}