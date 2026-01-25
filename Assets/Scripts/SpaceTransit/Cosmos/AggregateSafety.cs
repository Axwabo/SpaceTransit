using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class AggregateSafety : SafetyEnsurer
    {

        [SerializeField]
        private SafetyEnsurer[] forwards;

        [SerializeField]
        private SafetyEnsurer[] backwards;

        public override bool CanProceed(ShipAssembly assembly)
        {
            foreach (var ensurer in assembly.Reverse ? backwards : forwards)
                if (!ensurer.CanProceed(assembly))
                    return false;
            return true;
        }

    }

}
