using SpaceTransit.Loader.References;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class LockBasedSafety : NextSegmentSafety
    {

        [SerializeField]
        [HideInInspector]
        private string lockReference;

        [SerializeField]
        private Lock @lock;

        public bool IsFree => @lock.IsFree;

        private void Start() => @lock = CrossSceneObject.GetComponent(lockReference, @lock);

        private void OnValidate() => lockReference = CrossSceneObject.GetOrCreate(@lock, this) ?? lockReference;

        public void Claim(ShipAssembly assembly) => @lock.Claim(assembly);

        public override bool CanProceed(ShipAssembly assembly) => @lock.IsUsedOnlyBy(assembly) && base.CanProceed(assembly);

    }

}
