
using System.Collections.Generic;

namespace Flai.DataStructures
{
    public interface ICustomQueue<T> : IEnumerable<T>
    {
        int Count { get; }
        bool IsEmpty { get; }

        bool Contains(T value);
        T Dequeue();
        void Enqueue(T value);
        T Peek();
        T[] ToArray();
        void Clear();
        void CopyTo(T[] array, int index);
    }
}
