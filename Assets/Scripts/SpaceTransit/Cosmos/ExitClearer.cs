using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class ExitClearer : SafetyEnsurer
    {

        [SerializeField]
        private SafetyEnsurer ensurer;

        [SerializeField]
        private Exit exit;

        public override bool CanProceed(ShipAssembly assembly) => ensurer.CanProceed(assembly);

        public override void OnEntered(ShipModule module) => ensurer.OnEntered(module);

        public override void OnExited(ShipModule module)
        {
            ensurer.OnExited(module);
            if (!ensurer.IsOccupied)
                exit.UsedBy.Remove(module.Assembly);
        }

    }

}
