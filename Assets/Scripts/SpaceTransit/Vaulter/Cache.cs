using System;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public static class Cache
    {

        private static RouteDescriptor[] _routes;

        private static StationId[] _stations;

        public static ReadOnlySpan<RouteDescriptor> Routes => _routes ??= Resources.LoadAll<RouteDescriptor>("Routes");

        public static ReadOnlySpan<StationId> Stations => _stations ??= Resources.LoadAll<StationId>("Stations");

    }

}
