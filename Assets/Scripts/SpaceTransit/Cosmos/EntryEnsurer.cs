using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class EntryEnsurer : DelegatingEnsurer
    {

        [SerializeField]
        private Station station;

        [SerializeField]
        private bool backwards;

        public override bool CanProceed(ShipAssembly assembly)
        {
            if (!base.CanProceed(assembly))
                return false;
            if (assembly.Reverse != backwards)
                return true;
            foreach (var dock in station.Docks)
            {
                var entry = assembly.Reverse ? dock.FrontEntry : dock.BackEntry;
                if (entry && entry.UsedBy.Contains(assembly))
                    return true;
            }

            return false;
        }

    }

}
