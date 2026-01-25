using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class AggregateSafety : SafetyEnsurer
    {

        [SerializeField]
        private SafetyEnsurer[] others;

        public override bool CanProceed(ShipAssembly assembly)
        {
            foreach (var ensurer in others)
                if (!ensurer.CanProceed(assembly))
                    return false;
            return true;
        }

    }

}
