using System;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Sequences;
using SpaceTransit.Ships.Driving.Screens;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(ServiceSequence))]
    [CanEditMultipleObjects]
    public sealed class ServiceSequenceEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var rotor = (ServiceSequence) target;
            rotor.routes ??= Array.Empty<JourneyDescriptorBase>();
            var lastTime = TimeSpan.Zero;
            StationId lastStop = null;
            var lastDock = rotor.routes.Length == 0 ? 0 : rotor.routes[0].Beginning.DockIndex;
            if (!IsMidnightResetInvalid(rotor))
            {
                DefaultInspector(rotor);
                return;
            }

            foreach (var descriptor in rotor.routes)
            {
                if (IsInvalid(descriptor, lastTime, lastStop, lastDock))
                    break;
                if (descriptor is RouteDescriptor route)
                    lastTime = route.Destination.Arrival.Value;
                lastStop = descriptor.End.Station;
                lastDock = descriptor.End.DockIndex;
            }

            DefaultInspector(rotor);
        }

        private void DefaultInspector(ServiceSequence rotor)
        {
            base.OnInspectorGUI();
            GUILayout.Space(20);
            foreach (var descriptor in rotor.routes)
                GUILayout.Label($"[{descriptor.Beginning.DockIndex + 1}] {RouteList.Format(descriptor)} [{descriptor.End.DockIndex + 1}]");
        }

        private static bool IsMidnightResetInvalid(ServiceSequence rotor)
        {
            if (rotor.routes.Length == 0)
                return true;
            var origin = rotor.routes[0].Beginning;
            var destination = rotor.routes[^1].End;
            if (origin.Station != destination.Station)
            {
                EditorGUILayout.HelpBox($"Midnight reset is not possible!\n{rotor.routes[0].name} origin != {rotor.routes[^1].name} destination", MessageType.Error);
                return false;
            }

            if (origin.DockIndex != destination.DockIndex)
                EditorGUILayout.HelpBox($"Midnight reset dock mismatch!\n{rotor.routes[0].name} destination dock != {rotor.routes[^1].name} origin dock", MessageType.Warning);
            return true;
        }

        private static bool IsInvalid(JourneyDescriptorBase descriptor, TimeSpan lastTime, StationId lastStop, int lastDock)
        {
            if (descriptor.Beginning.Departure < lastTime)
            {
                EditorGUILayout.HelpBox($"Routes are not continuous!\n{descriptor.name} departure {descriptor.Beginning.Departure} < {lastTime:hh':'mm}", MessageType.Error);
                return true;
            }

            if (lastStop && lastStop != descriptor.Beginning.Station)
            {
                EditorGUILayout.HelpBox($"Routes are not continuous!\n{descriptor.name} origin != {lastStop.name}", MessageType.Error);
                return true;
            }

            if (lastDock != descriptor.Beginning.DockIndex)
                EditorGUILayout.HelpBox($"Routes continuity dock mismatch!\n{descriptor.name} origin dock != {lastDock}", MessageType.Warning);
            return false;
        }

    }

}
