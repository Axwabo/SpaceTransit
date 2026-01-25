using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class LockBasedSafety : NextSegmentSafety
    {

        [SerializeField]
        private Lock @lock;

        public bool IsFree => @lock.IsFree;

        public void Claim(ShipAssembly assembly) => @lock.Claim(assembly);

        public override bool CanProceed(ShipAssembly assembly) => @lock.IsUsedOnlyBy(assembly) && base.CanProceed(assembly);

    }

}
