using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public static class Cache
    {

        private static RouteDescriptor[] _routes;

        private static StationId[] _stations;

        public static RouteDescriptor[] Routes => _routes ??= Resources.LoadAll<RouteDescriptor>("Routes");

        public static StationId[] Stations => _stations ??= Resources.LoadAll<StationId>("Stations");

    }

}
