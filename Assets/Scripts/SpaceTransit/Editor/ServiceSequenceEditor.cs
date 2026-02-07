using System;
using SpaceTransit.Routes;
using UnityEditor;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(ServiceSequence))]
    [CanEditMultipleObjects]
    public sealed class ServiceSequenceEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var rotor = (ServiceSequence) target;
            rotor.routes ??= Array.Empty<RouteDescriptor>();
            var lastTime = TimeSpan.Zero;
            StationId lastStop = null;
            var lastDock = rotor.routes.Length == 0 ? 0 : rotor.routes[0].Origin.DockIndex;
            if (!IsMidnightResetInvalid(rotor))
            {
                base.OnInspectorGUI();
                return;
            }

            foreach (var descriptor in rotor.routes)
            {
                if (!descriptor)
                    continue;
                if (IsInvalid(descriptor, lastTime, lastStop, lastDock))
                    break;
                lastTime = descriptor.Destination.Arrival.Value;
                lastStop = descriptor.Destination.Station;
                lastDock = descriptor.Destination.DockIndex;
            }

            base.OnInspectorGUI();
        }

        private static bool IsMidnightResetInvalid(ServiceSequence rotor)
        {
            if (rotor.routes.Length == 0)
                return true;
            var origin = rotor.routes[0].Origin;
            var destination = rotor.routes[^1].Destination;
            if (origin.Station != destination.Station)
            {
                EditorGUILayout.HelpBox($"Midnight reset is not possible!\n{rotor.routes[0].name} origin != {rotor.routes[^1].name} destination", MessageType.Error);
                return false;
            }

            if (origin.DockIndex != destination.DockIndex)
                EditorGUILayout.HelpBox($"Midnight reset dock mismatch!\n{rotor.routes[0].name} destination dock != {rotor.routes[^1].name} origin dock", MessageType.Warning);
            return true;
        }

        private static bool IsInvalid(RouteDescriptor descriptor, TimeSpan lastTime, StationId lastStop, int lastDock)
        {
            if (descriptor.Origin.Departure < lastTime)
            {
                EditorGUILayout.HelpBox($"Routes are not continuous!\n{descriptor.name} departure {descriptor.Origin.Departure.Value:hh':'mm} < {lastTime:hh':'mm}", MessageType.Error);
                return true;
            }

            if (lastStop && lastStop != descriptor.Origin.Station)
            {
                EditorGUILayout.HelpBox($"Routes are not continuous!\n{descriptor.name} origin != {lastStop.name}", MessageType.Error);
                return true;
            }

            if (lastDock != descriptor.Origin.DockIndex)
                EditorGUILayout.HelpBox($"Routes continuity dock mismatch!\n{descriptor.name} origin dock != {lastDock}", MessageType.Warning);
            return false;
        }

    }

}
