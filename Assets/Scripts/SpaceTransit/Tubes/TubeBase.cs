using SpaceTransit.Cosmos;
using UnityEngine;

namespace SpaceTransit.Tubes
{

    [ExecuteInEditMode]
    public abstract class TubeBase : MonoBehaviour
    {

        protected Transform Transform { get; private set; }

        [field: SerializeField]
        public float SpeedLimit { get; private set; }

        [field: SerializeField]
        public TubeBase Next { get; set; }

        public bool HasNext { get; private set; }

        public TubeBase Previous { get; set; }

        public bool HasPrevious { get; private set; }

        public SafetyEnsurer Safety { get; private set; }

        protected virtual void Awake()
        {
            Transform = transform;
            if (Application.isPlaying)
                Safety = TryGetComponent(out SafetyEnsurer ensurer)
                    ? ensurer
                    : gameObject.AddComponent<NextSegmentSafety>();
            OnValidate();
        }

        private void Start()
        {
            if (!Application.isPlaying)
                return;
            if (HasNext)
                PlaceSign(Next, 0, Quaternion.identity);
            if (HasPrevious)
                PlaceSign(Previous, Previous.Length, Quaternion.Euler(0, 180, 0));
        }

        private void PlaceSign(TubeBase tube, float distance, Quaternion rotationOffset)
        {
            if (Mathf.Approximately(SpeedLimit, tube.SpeedLimit))
                return;
            var (position, rotation) = tube.Sample(distance);
            var forwardsSign = Instantiate(World.SpeedLimitSignPrefab, Transform);
            forwardsSign.transform.SetLocalPositionAndRotation(position, rotation * rotationOffset);
            forwardsSign.Forwards.text = tube.SpeedLimit is 0 ? "--" : (tube.SpeedLimit * 3.6f).ToString("N0");
        }

        private void OnValidate()
        {
            if (!Next)
                return;
            Next.Previous = this;
            Next.HasPrevious = true;
            HasNext = true;
        }

        public abstract float Length { get; }

        public abstract (Vector3 Position, Quaternion Rotation) Sample(float distance);

        public abstract float GetDistance(Vector3 point);

    }

}
