using System.Collections.Generic;

public static class CollectionExtensions
{

    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T> comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;
        for (var i = 0; i < list.Count; i++)
            if (comparer.Equals(list[i], item))
                return i;
        return -1;
    }

}
