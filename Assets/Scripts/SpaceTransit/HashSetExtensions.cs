using System.Collections.Generic;

namespace SpaceTransit
{

    public static class HashSetExtensions
    {

        public static T FirstFast<T>(this HashSet<T> hashSet)
        {
            using var enumerator = hashSet.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }

    }

}
