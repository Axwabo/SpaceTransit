using SpaceTransit.Cosmos.Actions;
using SpaceTransit.Loader;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(LockTaker))]
    public sealed class LockBasedSafety : NextSegmentSafety
    {

        [SerializeField]
        [HideInInspector]
        private string lockReference;

        [SerializeField]
        private Lock @lock;

        public bool IsFree => @lock.IsFree;

        private void Start()
        {
            RefreshLock();
            CrossSceneObject.ScenesChanged += RefreshLock;
        }

        private void OnValidate() => lockReference = CrossSceneObject.GetOrCreate(@lock, gameObject, lockReference);

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshLock;

        private void RefreshLock() => @lock = CrossSceneObject.GetComponent(lockReference, @lock);

        public void Claim(ShipAssembly assembly) => @lock.Claim(assembly);

        public override bool CanProceed(ShipAssembly assembly) => @lock.IsUsedOnlyBy(assembly) && base.CanProceed(assembly);

    }

}
