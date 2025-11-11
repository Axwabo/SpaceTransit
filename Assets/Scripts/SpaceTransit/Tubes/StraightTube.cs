using UnityEngine;

namespace SpaceTransit.Tubes
{

    public sealed class StraightTube : TubeBase
    {

        private Transform _t;

        private float _length;

        private Quaternion _rotation;

        private void Awake()
        {
            _t = transform;
            _length = _t.lossyScale.z;
        }

        public override float Length => _length;

        public override (Vector3 Position, Quaternion Rotation) Sample(float distance)
        {
            var position = _t.TransformPoint((Mathf.Clamp01(_length / distance) - 0.5f) * Vector3.forward);
            return (position, _rotation);
        }

        public override float GetDistance(Vector3 point)
        {
            var inverse = _t.InverseTransformPoint(point);
            return (inverse.z + 0.5f) * _length;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_t.TransformPoint(Vector3.back * 0.5f), _t.TransformPoint(Vector3.forward * 0.5f));
        }

    }

}
