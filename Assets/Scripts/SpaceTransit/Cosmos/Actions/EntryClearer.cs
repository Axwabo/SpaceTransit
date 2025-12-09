using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    [RequireComponent(typeof(Dock))]
    public sealed class EntryClearer : SafetyActionBase
    {

        public Dock Dock { get; private set; }

        private void Awake() => Dock = GetComponent<Dock>();

        public override void OnEntered(ShipModule module)
        {
            if (Ensurer.Occupants.Count != module.Assembly.Modules.Count)
                return;
            var entries = module.Assembly.Reverse ? Dock.FrontEntries : Dock.BackEntries;
            foreach (var entry in entries)
                entry.Release(module.Assembly);
        }

    }

}
