using SpaceTransit.Routes;
using SpaceTransit.Ships;

namespace SpaceTransit
{

    public static class ExitExtensions
    {

        public static bool IsFreeFor(this Exit exit, ShipAssembly assembly)
            => exit.UsedBy.Count == 1
                ? exit.UsedBy.Contains(assembly)
                : exit.UsedBy.Count == 0;

    }

}
