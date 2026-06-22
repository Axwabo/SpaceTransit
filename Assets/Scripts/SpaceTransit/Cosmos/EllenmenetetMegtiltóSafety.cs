using SpaceTransit.Loader;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class EllenmenetetMegtiltóSafety : NextSegmentSafety
    {

        [SerializeField]
        [HideInInspector]
        private string clearanceReference;

        [field: SerializeField]
        public OpposingTrafficClearance Clearance { get; private set; }

        private void Start()
        {
            RefreshLock();
            CrossSceneObject.ScenesChanged += RefreshLock;
        }

        private void OnValidate() => clearanceReference = CrossSceneObject.GetOrCreate(Clearance, gameObject, clearanceReference);

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshLock;

        private void RefreshLock() => Clearance = CrossSceneObject.GetComponent(clearanceReference, Clearance);

        public void Claim(ShipAssembly assembly) => Clearance.Claim(assembly);

        public override bool CanProceed(ShipAssembly assembly) => Clearance.CanProceed(assembly) && base.CanProceed(assembly);

    }

}
