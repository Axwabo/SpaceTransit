using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(Dock))]
    public sealed class EntryClearer : DelegatingEnsurer
    {

        private Dock _dock;

        private void Start() => _dock = GetComponent<Dock>();

        public override void OnEntered(ShipModule module)
        {
            var count = ensurer.Occupants.Count;
            base.OnEntered(module);
            if (count != module.Assembly.Modules.Count)
                return;
            var entry = module.Assembly.Reverse ? _dock.FrontEntry : _dock.BackEntry;
            if (entry)
                entry.Clear(module.Assembly);
        }

    }

}
