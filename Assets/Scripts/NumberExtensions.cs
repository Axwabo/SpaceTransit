using UnityEngine;

public static class NumberExtensions
{

    public static float Limit(this float f, float max) => max == 0 ? f : Mathf.Min(f, max);

}
