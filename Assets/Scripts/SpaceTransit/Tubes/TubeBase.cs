using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
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
                AssignSafety();
            OnValidate();
        }

        private void AssignSafety()
        {
            if (!TryGetComponent(out SafetyEnsurer ensurer))
                Safety = AddDefaultSafety(gameObject);
            else if (ensurer is EntryEnsurer entryEnsurer && TryGetComponent(out EllenmenetetMegtiltóSafety second))
                Safety = gameObject.AddComponent<CombinedSafety>().Init(entryEnsurer, second);
            else
                Safety = ensurer;
        }

        private void Start()
        {
            if (!Application.isPlaying)
                return;
            OnStart();
            RefreshNext();
            CrossSceneObject.SubscribeToSceneChanges(RefreshNext, nextReference);
            if (HasNext)
                PlaceSign(Next, 0, Quaternion.identity);
            if (HasPrevious)
                PlaceSign(Previous, Previous.Length, Quaternion.Euler(0, 180, 0));
        }

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshNext;

        protected virtual void OnStart()
        {
        }

        private void RefreshNext() => SetNext(CrossSceneObject.GetComponent(nextReference, Next));

        private void PlaceSign(TubeBase tube, float distance, Quaternion rotationOffset)
        {
            if (Mathf.Approximately(SpeedLimit, tube.SpeedLimit))
                return;
            var (position, rotation) = tube.Sample(distance);
            var forwardsSign = Instantiate(World.SpeedLimitSignPrefab, World.Current, false);
            var t = forwardsSign.transform;
            t.SetLocalPositionAndRotation(position, rotation * rotationOffset);
            t.parent = Transform;
            forwardsSign.Forwards.text = tube.SpeedLimit is 0 ? "--" : (tube.SpeedLimit * ShipSpeed.MpsToKmh).ToString("N0");
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

        private sealed class CombinedSafety : SafetyEnsurer, IEntryEnsurer, IOpposingTrafficSafety
        {

            private EntryEnsurer _a;

            private EllenmenetetMegtiltóSafety _b;

            public StationId TargetStation => _a.TargetStation;

            public bool Backwards => _a.Backwards;

            public List<Entry> Entries => _a.Entries;

            public OpposingTrafficClearance Clearance => _b.Clearance;

            public CombinedSafety Init(EntryEnsurer a, EllenmenetetMegtiltóSafety b)
            {
                _a = a;
                _b = b;
                return this;
            }

            public override bool CanProceed(ShipAssembly assembly) => _a.CanProceed(assembly) && _b.CanProceed(assembly);

            public override void OnEntered(ShipModule module)
            {
                _b.Occupants.Add(module);
                Occupants.Add(module);
                _a.OnEntered(module);
            }

            public override void OnExited(ShipModule module)
            {
                _b.Occupants.Remove(module);
                Occupants.Remove(module);
                _a.OnExited(module);
            }

        }

    }

}
