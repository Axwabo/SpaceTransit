using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class LockBasedSafety : NextSegmentSafety
    {

        [SerializeField]
        private Lock @lock;

        public override bool CanProceed(ShipAssembly assembly) => @lock.IsUsedOnlyBy(assembly) && base.CanProceed(assembly);

    }

}
