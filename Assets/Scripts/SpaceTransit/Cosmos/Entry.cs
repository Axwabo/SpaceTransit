using SpaceTransit.Routes;
using SpaceTransit.Ships;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : EntryOrExit
    {

        public Dock Dock { get; set; }

        public bool IsUsedOnlyBy(ShipAssembly assembly) => Locks.AreOnlyUsedBy(assembly);

        public override bool Lock(ShipAssembly assembly) => !Dock.Tube.Safety.IsOccupied && base.Lock(assembly);

    }

}
