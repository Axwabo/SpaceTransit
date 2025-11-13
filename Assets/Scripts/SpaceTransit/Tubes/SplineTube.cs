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

        public override (Vector3 Position, Quaternion Rotation) Sample(float distance)
        {
            var sample = spline.GetSampleAtDistance(distance);
            return (Transform.TransformPoint(sample.location), Transform.rotation * sample.Rotation);
        }

        public override float GetDistance(Vector3 point)
            => spline.GetProjectionSample(point).distanceInCurve;

    }

}
