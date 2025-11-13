using System.Collections.Generic;
using UnityEngine;

public sealed class SqrMagnitudeComparer : IComparer<Vector3>
{

    public static SqrMagnitudeComparer Instance { get; } = new();

    public int Compare(Vector3 x, Vector3 y) => x.sqrMagnitude.CompareTo(y.sqrMagnitude);

}
