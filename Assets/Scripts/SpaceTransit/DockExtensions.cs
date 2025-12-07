using SpaceTransit.Routes;
using SpaceTransit.Ships;

namespace SpaceTransit
{

    public static class DockExtensions
    {

        public static bool LockEntry(this Dock dock, ShipAssembly assembly)
        {
            var entry = assembly.Reverse ? dock.FrontEntry : dock.BackEntry;
            return entry && entry.Lock(assembly);
        }

    }

}
