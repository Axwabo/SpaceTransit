using System;
using System.Collections.Generic;

namespace SpaceTransit
{

    public static class SpanExtensions
    {

        public static bool Contains<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T> comparer)
        {
            foreach (var t in span)
                if (comparer.Equals(t, value))
                    return true;
            return false;
        }

    }

}
