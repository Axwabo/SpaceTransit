using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions
{

    public static IEnumerable<T> GetComponentsInImmediateChildren<T>(this Component parent, bool includeSelf = false)
    {
        if (includeSelf)
            foreach (var component in parent.GetComponents<T>())
                yield return component;
        var t = parent.transform;
        var childCount = t.childCount;
        for (var i = 0; i < childCount; i++)
            if (t.GetChild(i).TryGetComponent(out T component))
                yield return component;
    }

}
