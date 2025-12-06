using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    [RequireComponent(typeof(Dock))]
    public sealed class EntryClearer : SafetyActionBase
    {

        private Dock _dock;

        private void Awake() => _dock = GetComponent<Dock>();

        public override void OnEntered(ShipModule module)
        {
            if (Ensurer.Occupants.Count != module.Assembly.Modules.Count)
                return;
            var entry = module.Assembly.Reverse ? _dock.FrontEntry : _dock.BackEntry;
            if (entry)
                entry.Release(module.Assembly);
        }

    }

}
