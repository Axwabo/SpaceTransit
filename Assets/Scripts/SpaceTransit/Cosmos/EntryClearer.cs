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

        public override void OnExited(ShipModule module)
        {
            base.OnExited(module);
            if (ensurer.IsOccupied)
                return;
            var entry = module.Assembly.Reverse ? _dock.FrontEntry : _dock.BackEntry;
            if (entry)
                entry.UsedBy.Remove(module.Assembly);
        }

    }

}
