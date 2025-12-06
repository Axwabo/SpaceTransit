using SpaceTransit.Cosmos;
using SpaceTransit.Ships;

namespace SpaceTransit
{

    public static class LockExtensions
    {

        public static bool CanClaim(this Lock[] locks, ShipAssembly assembly)
        {
            foreach (var @lock in locks)
                if (!@lock.CanClaim(assembly))
                    return false;
            return true;
        }

        public static void Claim(this Lock[] locks, ShipAssembly assembly)
        {
            foreach (var @lock in locks)
                @lock.Claim(assembly);
        }

        public static void Release(this Lock[] locks, ShipAssembly assembly)
        {
            foreach (var @lock in locks)
                @lock.Release(assembly);
        }

        public static bool AreOnlyUsedBy(this Lock[] locks, ShipAssembly assembly)
        {
            foreach (var @lock in locks)
                if (!@lock.IsUsedOnlyBy(assembly))
                    return false;
            return true;
        }

    }

}
