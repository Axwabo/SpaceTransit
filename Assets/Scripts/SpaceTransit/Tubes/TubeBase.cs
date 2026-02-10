using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using UnityEngine;

namespace SpaceTransit.Tubes
{

    [ExecuteInEditMode]
    public abstract class TubeBase : MonoBehaviour
    {

        [SerializeField]
        [HideInInspector]
        private string nextReference;

        public Transform Transform { get; private set; }

        [field: SerializeField]
        public float SpeedLimit { get; set; }

        [field: SerializeField]
        public TubeBase Next { get; private set; }

        public bool HasNext { get; private set; }

        public TubeBase Previous { get; private set; }

        public bool HasPrevious { get; private set; }

        public SafetyEnsurer Safety { get; private set; }

        protected virtual void Awake()
        {
            Transform = transform;
            if (Application.isPlaying)
                Safety = TryGetComponent(out SafetyEnsurer ensurer)
                    ? ensurer
                    : AddDefaultSafety(gameObject);
            OnValidate();
        }

        private void Start()
        {
            if (!Application.isPlaying)
                return;
            RefreshNext();
            CrossSceneObject.SubscribeToSceneChanges(RefreshNext, nextReference);
            if (HasNext)
                PlaceSign(Next, 0, Quaternion.identity);
            if (HasPrevious)
                PlaceSign(Previous, Previous.Length, Quaternion.Euler(0, 180, 0));
        }

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshNext;

        private void RefreshNext()
        {
            var next = CrossSceneObject.GetComponent(nextReference, Next);
            if (next != Next)
                SetNext(next);
        }

        private void PlaceSign(TubeBase tube, float distance, Quaternion rotationOffset)
        {
            if (Mathf.Approximately(SpeedLimit, tube.SpeedLimit))
                return;
            var (position, rotation) = tube.Sample(distance);
            var forwardsSign = Instantiate(World.SpeedLimitSignPrefab, World.Current, false);
            var t = forwardsSign.transform;
            t.SetLocalPositionAndRotation(position, rotation * rotationOffset);
            t.parent = Transform;
            forwardsSign.Forwards.text = tube.SpeedLimit is 0 ? "--" : (tube.SpeedLimit * 3.6f).ToString("N0");
        }

        private void OnValidate()
        {
            nextReference = CrossSceneObject.GetOrCreate(Next, gameObject, nextReference);
            if (!Next)
                return;
            Next.Previous = this;
            Next.HasPrevious = true;
            HasNext = true;
        }

        public abstract float Length { get; }

        public abstract (Vector3 Position, Quaternion Rotation) Sample(float distance);

        public abstract float GetDistance(Vector3 point);

        protected virtual SafetyEnsurer AddDefaultSafety(GameObject o) => o.AddComponent<NextSegmentSafety>();

        public void SetNext(TubeBase tube)
        {
            Next = tube;
            OnValidate();
        }

    }

}
