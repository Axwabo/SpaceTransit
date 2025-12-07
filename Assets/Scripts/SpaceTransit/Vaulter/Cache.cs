using System.Collections.Generic;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public static class Cache
    {

        private static RouteDescriptor[] _routes;

        private static StationId[] _stations;

        public static IReadOnlyCollection<RouteDescriptor> Routes => _routes ??= Resources.LoadAll<RouteDescriptor>("Routes");

        public static IReadOnlyCollection<StationId> Stations => _stations ??= Resources.LoadAll<StationId>("Stations");

    }

}
