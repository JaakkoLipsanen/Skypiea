using System;
using System.Collections.Generic;

namespace Flai.DataStructures
{
    public interface IValueStack<T>
    {
        T CurrentValue { get; }

        void Push(T value);
        T Pop();
    }

    // todo: cache CurrentValue?
    public class ValueStack<T> : IValueStack<T>
    {
        protected readonly Stack<T> _stack = new Stack<T>();
        protected readonly T _defaultValue;

        public virtual T CurrentValue
        {
            get { return _stack.Count > 0 ? _stack.Peek() : _defaultValue; }
        }

        public bool IsEmpty
        {
            get { return _stack.Count == 0; }
        }

        public ValueStack(T defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public virtual void Push(T value)
        {
            _stack.Push(value);
        }

        public virtual T Pop()
        {
            return _stack.Pop();
        }
    }

    public class ValueStackAggregator<T> : ValueStack<T>
    {
        private readonly Func<T, T, T> _aggregatorFunction;
        private readonly Stack<T> _currentValueStack = new Stack<T>();

        public override T CurrentValue
        {
            get { return _currentValueStack.Peek(); }
        }

        public ValueStackAggregator(T defaultValue, Func<T, T, T> aggregatorFunction)
            : base(defaultValue)
        {
            Ensure.NotNull(aggregatorFunction);
            _aggregatorFunction = aggregatorFunction;
            _currentValueStack.Push(defaultValue);
        }

        public override void Push(T value)
        {
            base.Push(value);
            _currentValueStack.Push(_aggregatorFunction(_currentValueStack.Peek(), value));
        }

        public override T Pop()
        {
            T poppedValue = base.Pop();
            _currentValueStack.Pop();
            return poppedValue;
        }
    }

    public class ValueStackWithPredicate<T> : ValueStack<T>
    {
        private readonly Predicate<T> _predicate;
        public ValueStackWithPredicate(T defaultValue, Predicate<T> predicate)
            : base(defaultValue)
        {
            Ensure.NotNull(predicate);
            _predicate = predicate;
        }

        public override void Push(T value)
        {
            Ensure.True(_predicate(value));
            base.Push(value);
        }
    }

    public class ValueStackAggregatorWithPredicate<T> : ValueStackAggregator<T>
    {
        private readonly Predicate<T> _predicate;
        public ValueStackAggregatorWithPredicate(T defaultValue, Func<T, T, T> aggregatorFunction, Predicate<T> predicate)
            : base(defaultValue, aggregatorFunction)
        {
            _predicate = predicate;
        }

        public override void Push(T value)
        {
            Ensure.True(_predicate(value));
            base.Push(value);
        }
    }
}
