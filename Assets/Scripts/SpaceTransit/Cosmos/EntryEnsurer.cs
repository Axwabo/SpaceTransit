using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
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

    }

}
