using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class PassingThroughAnnouncer : SafetyActionBase
    {

        [SerializeField]
        private Dock dock;

        [SerializeField]
        private bool forwards;

        [SerializeField]
        private bool backwards;

        public override void OnEntered(ShipModule module)
        {
            if (Ensurer.Occupants.Count == module.Assembly.Modules.Length && Direction(module))
                Announce(module.Assembly);
        }

        public override void OnExited(ShipModule module)
        {
            if (!Ensurer.IsOccupied && Direction(module))
                Announce(module.Assembly);
        }

        private bool Direction(ShipModule module) => module.Assembly.Reverse ? backwards : forwards;

        private void Announce(ShipAssembly assembly)
        {
            if (assembly.Parent.TryGetVaulter(out var vaulter) && vaulter.IsInService && dock.Station.ID != vaulter.Stop.Station && dock.Station.Announcer)
                dock.Station.Announcer.EnqueuePassingThrough(assembly, dock.Index);
        }

    }

}
