using System.Collections.Generic;
using System.IO;
using SpaceTransit.Routes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public static class ExportRoutes
    {

        [MenuItem("Window/SpaceTransit/Export Routes")]
        private static void Export()
        {
            var routes = new List<string>();
            var stops = new List<string>();
            foreach (var route in Resources.LoadAll<RouteDescriptor>("Routes"))
            {
                routes.Add($"({route.name}, \"{route.Type}\", {route.EveryStation.ToString().ToUpper()}, {route.Reverse.ToString().ToUpper()}, \"{route.Origin.Station.name}\", {route.Origin.DockIndex}, TIME(\"{route.Origin.Departure.Value:h':'m}\"), \"{route.Destination.Station.name}\", {route.Destination.DockIndex}, TIME(\"{route.Destination.Arrival.Value:h':'m}\")),");
                stops.Add("");
                foreach (var stop in route.IntermediateStops)
                    stops.Add($"({route.name}, \"{stop.Station.name}\", {stop.DockIndex}, TIME(\"{stop.Arrival.Value:h':'m}\"), TIME(\"{stop.Departure.Value:h':'m}\")),");
            }

            File.WriteAllLines("routes.txt", routes);
            File.WriteAllLines("stops.txt", stops);
        }

    }

}
