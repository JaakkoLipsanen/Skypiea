using System;

namespace Flai.DataStructures
{
    /// <summary>
    /// Priority queue used for A* OPEN-list
    /// </summary>
    public abstract class PriorityQueue<T>
        where T : IComparable<T>
    {
        private readonly PriorityDirection _priorityDirection;

        public PriorityDirection PriorityDirection
        {
            get { return _priorityDirection; }
        }

        protected PriorityQueue()
            : this(PriorityDirection.Lowest)
        {
        }

        protected PriorityQueue(PriorityDirection priorityDirection)
        {
            _priorityDirection = priorityDirection;
        }

        /// <summary>
        /// If Compare returns negative, then the first value has lower priority than the other. If it returns positive, the first value has higher priority and if it returns zero, then both values have same priority
        /// </summary>
        protected virtual int PriorityCompare(T value1, T value2)
        {
            // If priority direction is Lowest, then the object which is greater than the other, is prioritized below the other one
            if (_priorityDirection == PriorityDirection.Lowest)
            {
                return value1.CompareTo(value2) * -1; // return 0 if value1 == value2, > 0 if value2 > value1 and < 0 if value2 < value1
            }
            return value1.CompareTo(value2); // return 0 if value1 == value2, > 0 if value1 > value2 and < 0 if value1 < value2
        }

        /// <summary>
        /// Returns true if the queue has elements, otherwise false
        /// </summary>
        public abstract bool HasNext { get; }

        /// <summary>
        /// Peeks the highest priority value from the queue. Returns true if queue has values in it, otherwise false
        /// </summary>
        /// <returns></returns>
        public abstract T Peek();

        /// <summary>
        /// Enqueues the highest priority value from the queue. Returns true if value was succesfully enqueued, otherwise false
        /// </summary>
        /// <returns></returns>
        public abstract T Dequeue();

        /// <summary>
        /// Adds a value to the priority queue
        /// </summary>
        public abstract void Add(T value);

        /// <summary>
        /// Returns true if queue contains the item, false otherwise
        /// </summary>
        /// <param name="value">Value to search</param>
        /// <returns></returns>
        public abstract bool Contains(T value);
    }
}
