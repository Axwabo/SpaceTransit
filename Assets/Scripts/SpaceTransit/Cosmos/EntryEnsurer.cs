using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Cosmos
{

    public sealed class EntryEnsurer : DelegatingEnsurer
    {

        [SerializeField]
        public Station station;

        [field: SerializeField]
        [field: FormerlySerializedAs("backwards")]
        public bool Backwards { get; private set; }

        public override bool CanProceed(ShipAssembly assembly)
        {
            if (!base.CanProceed(assembly))
                return false;
            if (assembly.Reverse != Backwards)
                return true;
            foreach (var dock in station.Docks)
            {
                var entry = assembly.Reverse ? dock.FrontEntry : dock.BackEntry;
                if (entry && entry.IsUsedOnlyBy(assembly))
                    return true;
            }

            return false;
        }

    }

}
