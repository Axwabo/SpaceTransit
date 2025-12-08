using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Ships;

namespace SpaceTransit
{

    public static class DockExtensions
    {

        public static bool LockEntry(this Dock dock, ShipAssembly assembly, EntryEnsurer ensurer)
        {
            foreach (var entry in ensurer.Entries)
                if (entry.Dock == dock)
                    return entry.Lock(assembly);
            return false;
        }

    }

}
