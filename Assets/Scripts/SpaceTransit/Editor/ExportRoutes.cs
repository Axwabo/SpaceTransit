using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Editor
{

    public class ExportRoutes : EditorWindow
    {

        [MenuItem("Window/SpaceTransit/Export Routes")]
        private static void ShowWindow()
        {
            var window = GetWindow<ExportRoutes>();
            window.titleContent = new GUIContent("Export Routes");
            window.Show();
        }

        private void OnGUI()
        {
            if (!GUILayout.Button("Export"))
                return;
            var routes = new List<string>();
            var stops = new List<string>();
            foreach (var route in Cache.Routes)
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
