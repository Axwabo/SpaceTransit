using UnityEngine;

namespace SpaceTransit.Tubes
{

    public sealed class StraightTube : TubeBase
    {

        private float _length;

        private Quaternion _rotation;

        protected override void Awake()
        {
            base.Awake();
            _length = Transform.lossyScale.z;
            _rotation = Transform.rotation;
        }

        public override float Length => _length;

        public override (Vector3 Position, Quaternion Rotation) Sample(float distance)
        {
            var position = Transform.TransformPoint((Mathf.Clamp01(distance / _length) - 0.5f) * Vector3.forward);
            return (position, _rotation);
        }

        public override float GetDistance(Vector3 point)
        {
            var inverse = Transform.InverseTransformPoint(point);
            return (inverse.z + 0.5f) * _length;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.orangeRed;
            Gizmos.DrawSphere(Sample(0).Position, 0.01f);
            Gizmos.color = Color.greenYellow;
            Gizmos.DrawSphere(Sample(Length).Position, 0.01f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Transform.TransformPoint(Vector3.back * 0.5f), Transform.TransformPoint(Vector3.forward * 0.5f));
        }

    }

}
