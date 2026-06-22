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

        [SerializeField]
        private Lock clearance;

        private void Start()
        {
            RefreshClearance();
            CrossSceneObject.SubscribeToSceneChanges(RefreshClearance, clearanceReference);
        }

        private void OnValidate() => clearanceReference = CrossSceneObject.GetOrCreate(clearance, gameObject, clearanceReference);

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshClearance;

        private void RefreshClearance() => clearance = CrossSceneObject.GetComponent(clearanceReference, clearance);

        public override bool IsFree => (!clearance || clearance.IsFree) && base.IsFree;

        public override bool Lock(ShipAssembly assembly)
        {
            if (!clearance)
                return base.Lock(assembly);
            if (!clearance.CanClaim(assembly))
                return false;
            clearance.Claim(assembly);
            return base.Lock(assembly);
        }

        public override void Release(ShipAssembly assembly)
        {
            base.Release(assembly);
            if (clearance)
                clearance.Release(assembly);
        }

        public override bool IsUsedOnlyBy(ShipAssembly assembly) => base.IsUsedOnlyBy(assembly) && (!clearance || clearance.IsUsedOnlyBy(assembly));

    }

}
