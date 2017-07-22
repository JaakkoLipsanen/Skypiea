using System;
using System.Collections;
using System.Collections.Generic;

namespace Flai.General
{
    public class ComparisonComparer<T> : IComparer<T>, IComparer
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            Ensure.NotNull(comparison);
            _comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            return _comparison((T)x, (T)y);
        }

        #endregion
    }
}
