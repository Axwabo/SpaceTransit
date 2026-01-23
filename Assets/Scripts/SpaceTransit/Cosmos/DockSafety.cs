using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(Dock))]
    public sealed class DockSafety : NextSegmentSafety
    {

        private Dock Dock => (Dock) Tube;

        public override bool CanProceed(ShipAssembly assembly)
        {
            if (!assembly.IsStationary())
                return base.CanProceed(assembly);
            var exits = assembly.Reverse ? Dock.BackExits : Dock.FrontExits;
            if (exits.Length == 0)
                return base.CanProceed(assembly);
            foreach (var exit in exits)
                if (exit.IsUsedOnlyBy(assembly))
                    return base.CanProceed(assembly);
            return false;
        }

    }

}
