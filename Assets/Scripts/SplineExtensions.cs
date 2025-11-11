using SplineMesh;
using UnityEngine;

public static class SplineExtensions
{

    public static CurveSample SampleNearest(this Spline spline, Vector3 position)
        => spline.GetSampleAtDistance(spline.GetProjectionSample(position).distanceInCurve);

}
