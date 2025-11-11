using SplineMesh;
using UnityEngine;

namespace SpaceTransit.Tubes
{

    public sealed class SplineTube : TubeBase
    {

        [SerializeField]
        private Spline spline;

        [SerializeField]
        private float length;

        public override float Length => length <= 0 ? spline.Length : length;

        public override (Vector3, Quaternion) Sample(float distance)
        {
            var sample = spline.GetSampleAtDistance(distance);
            return (sample.location, sample.Rotation);
        }

        public override float GetDistance(Vector3 point)
            => spline.GetProjectionSample(point).timeInCurve * Length;

    }

}
