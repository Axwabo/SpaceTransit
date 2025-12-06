using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public class DelegatingEnsurer : SafetyEnsurer
    {

        [SerializeField]
        protected SafetyEnsurer ensurer;

        public override bool IsOccupied => ensurer.IsOccupied;

        public override bool CanProceed(ShipAssembly assembly) => ensurer.CanProceed(assembly);

        public override void OnEntered(ShipModule module) => ensurer.OnEntered(module);

        public override void OnExited(ShipModule module) => ensurer.OnExited(module);

    }

}
