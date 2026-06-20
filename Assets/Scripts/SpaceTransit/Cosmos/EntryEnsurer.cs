using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class EntryEnsurer : NextSegmentSafety
    {

        [SerializeField]
        public StationId station;

        [field: SerializeField]
        public bool Backwards { get; private set; }

        public List<Entry> Entries { get; } = new();

        public override bool CanProceed(ShipAssembly assembly)
        {
            if (!base.CanProceed(assembly))
                return false;
            if (assembly.Reverse != Backwards)
                return true;
            foreach (var entry in Entries)
                if (entry.IsUsedOnlyBy(assembly))
                    return true;
            return false;
        }

        public override void OnEntered(ShipModule module)
        {
            base.OnEntered(module);
            if (Occupants.Count != 1 || !module.Assembly.ShouldAnnounceNonScheduled(station))
                return;
            foreach (var entry in Entries)
            {
                if (!entry.IsUsedOnlyBy(module.Assembly))
                    continue;
                EnqueueArriving(module.Assembly, entry);
                break;
            }
        }

        public void EnqueueArriving(ShipAssembly assembly, Entry entry)
        {
            if (!Station.TryGetLoadedStation(station, out var actual) || !actual.Announcer)
                return;
            var exits = assembly.Reverse ? entry.Dock.BackExits : entry.Dock.FrontExits;
            foreach (var exit in exits)
                if (exit.IsUsedOnlyBy(assembly))
                {
                    actual.Announcer.EnqueuePassingThrough(assembly, entry.Dock.Index);
                    return;
                }

            actual.Announcer.EnqueueArriving(assembly, entry.Dock.Index);
        }

    }

}
