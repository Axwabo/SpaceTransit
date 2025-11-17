using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class SpecificSegmentsSafety : SafetyEnsurer
    {

        [SerializeField]
        private TubeBase[] tubes;

        public override bool CanProceed(ShipAssembly assembly)
        {
            foreach (var tube in tubes)
                if (!tube.Safety.CanProceed(assembly))
                    return false;
            return true;
        }

    }

}
