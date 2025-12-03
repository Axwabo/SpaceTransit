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
            if (!HasNext)
                return;
            var (position, rotation) = Next.Sample(0);
            var forwardsSign = Instantiate(World.SpeedLimitSignPrefab, position, rotation, Transform);
            forwardsSign.transform.SetLocalPositionAndRotation(position, rotation);
            forwardsSign.Forwards.text = Next.SpeedLimit is 0 ? "--" : (Next.SpeedLimit * 3.6f).ToString("N0");
            var backwardsSign = Instantiate(World.SpeedLimitSignPrefab, position, rotation * Quaternion.Euler(0, 180, 0), Transform);
            backwardsSign.Forwards.text = SpeedLimit is 0 ? "--" : (SpeedLimit * 3.6f).ToString("N0");
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
