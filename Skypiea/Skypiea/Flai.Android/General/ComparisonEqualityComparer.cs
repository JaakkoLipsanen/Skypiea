using System;
using System.Collections.Generic;

namespace Flai.General
{
    public class PreciateEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _predicate;
        public PreciateEqualityComparer(Func<T, T, bool> predicate)
        {
            Ensure.NotNull(predicate);
            _predicate = predicate;
        }

        public override bool Equals(T x, T y)
        {
            if (x != null)
            {
                return (y != null) && _predicate(x, y);
            }
            else if (y != null)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode(T obj)
        {
            return 0; // ?
        }
    }
}
