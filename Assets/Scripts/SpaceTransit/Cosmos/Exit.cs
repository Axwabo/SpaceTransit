using SpaceTransit.Loader;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : EntryOrExit
    {

        [SerializeField]
        [HideInInspector]
        private string clearanceReference;

        [field: SerializeField]
        public OpposingTrafficClearance Clearance { get; private set; }

        private void Start()
        {
            RefreshClearance();
            CrossSceneObject.SubscribeToSceneChanges(RefreshClearance, clearanceReference);
        }

        private void OnValidate() => clearanceReference = CrossSceneObject.GetOrCreate(Clearance, gameObject, clearanceReference);

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshClearance;

        private void RefreshClearance() => Clearance = CrossSceneObject.GetComponent(clearanceReference, Clearance);

        public override bool IsFree => (!Clearance || Clearance.IsFree) && base.IsFree;

        public override bool Lock(ShipAssembly assembly)
        {
            if (!Clearance)
                return base.Lock(assembly);
            if (!Clearance.CanClaim(assembly))
                return false;
            Clearance.Claim(assembly);
            return base.Lock(assembly);
        }

        public override bool IsUsedOnlyBy(ShipAssembly assembly) => base.IsUsedOnlyBy(assembly) && (!Clearance || Clearance.CanProceed(assembly));

    }

}
