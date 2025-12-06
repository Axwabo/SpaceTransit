using System;
using SpaceTransit.Routes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(RouteDescriptor))]
    public sealed class RouteDescriptorEditor : UnityEditor.Editor
    {

        private int _offset;

        public override void OnInspectorGUI()
        {
            _offset = EditorGUILayout.IntField("Offset By", _offset);
            if (GUILayout.Button("Offset"))
                Offset();
            GUILayout.Space(20);
            base.OnInspectorGUI();
        }

        private void Offset()
        {
            var offset = TimeSpan.FromMinutes(_offset);
            var route = (RouteDescriptor) target;
            route.Origin.Departure = route.Origin.Departure.Value + offset;
            route.Destination.Arrival = route.Destination.Arrival.Value + offset;
            foreach (var stop in route.IntermediateStops)
            {
                stop.Arrival = stop.Arrival.Value + offset;
                stop.Departure = stop.Departure.Value + offset;
            }
        }

    }

}
