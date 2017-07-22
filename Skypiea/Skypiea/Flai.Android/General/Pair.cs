
using System;
using System.Collections.Generic;

namespace Flai.General
{
    public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        public readonly T1 First;
        public readonly T2 Second;

        public Pair(T1 first, T2 second)
        {
            this.First = first;
            this.Second = second;
        }

        public bool Equals(Pair<T1, T2> other)
        {
            return this.First.Equals(other.First) && this.Second.Equals(other.Second);
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<T1, T2>)
            {
                return this.Equals((Pair<T2, T2>)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (EqualityComparer<T1>.Default.GetHashCode(this.First) * 397) ^ EqualityComparer<T2>.Default.GetHashCode(this.Second);
        }

        public static Pair<T1, T2> Create(T1 first, T2 second)
        {
            return new Pair<T1, T2>(first, second);
        }
    }

    public static class Pair
    {
        public static Pair<T1, T2> Create<T1, T2>(T1 first, T2 second)
        {
            return new Pair<T1, T2>(first, second);
        }
    }
}
