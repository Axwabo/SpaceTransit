using System;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;
using UnityEditor;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(RouteRotor))]
    [CanEditMultipleObjects]
    public sealed class RouteRotorEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var rotor = (RouteRotor) target;
            var lastTime = TimeSpan.Zero;
            StationId lastStop = null;
            foreach (var descriptor in rotor.routes)
            {
                if (!descriptor)
                    continue;
                if (IsInvalid(descriptor, lastTime, lastStop))
                    break;
                lastTime = descriptor.Destination.Arrival.Value;
                lastStop = descriptor.Destination.Station;
            }

            base.OnInspectorGUI();
        }

        private static bool IsInvalid(RouteDescriptor descriptor, TimeSpan lastTime, StationId lastStop)
        {
            if (descriptor.Origin.Departure < lastTime)
            {
                EditorGUILayout.HelpBox($"Routes are not continuous!\n{descriptor.name} departure {descriptor.Origin.Departure.Value:hh':'mm} < {lastTime:hh':'mm}", MessageType.Error);
                return true;
            }

            if (!lastStop || lastStop == descriptor.Origin.Station)
                return false;
            EditorGUILayout.HelpBox($"Routes are not continuous!\n{descriptor.name} origin != {lastStop.name}", MessageType.Error);
            return true;
        }

    }

}
