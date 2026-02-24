using System;
using System.IO;
using SpaceTransit.Routes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(RouteDescriptor))]
    [CanEditMultipleObjects]
    public sealed class RouteDescriptorEditor : UnityEditor.Editor
    {

        private static int _offset;

        public override void OnInspectorGUI()
        {
            _offset = EditorGUILayout.IntField("Offset By", _offset);
            if (GUILayout.Button("Offset"))
                Offset();
            if (GUILayout.Button("Clone"))
                Clone();
            GUILayout.Space(20);
            base.OnInspectorGUI();
        }

        private void Offset()
        {
            var offset = TimeSpan.FromMinutes(_offset);
            foreach (var o in targets)
                Offset((RouteDescriptor) o, offset);
        }

        private static void Offset(RouteDescriptor route, TimeSpan offset)
        {
            route.Origin.Departure = route.Origin.Departure.Value + offset;
            route.Destination.Arrival = route.Destination.Arrival.Value + offset;
            foreach (var stop in route.IntermediateStops)
            {
                stop.Arrival = stop.Arrival.Value + offset;
                stop.Departure = stop.Departure.Value + offset;
            }

            EditorUtility.SetDirty(route);
        }

        private void Clone()
        {
            var original = (RouteDescriptor) target;
            if (!int.TryParse(original.name, out var id))
                return;
            var path = AssetDatabase.GetAssetPath(original);
            var newPath = Path.Combine(Path.GetDirectoryName(path) ?? path, (id + 2).ToString());
            if (!AssetDatabase.CopyAsset(path, newPath))
                return;
            var copy = AssetDatabase.LoadAssetAtPath<RouteDescriptor>(newPath);
            Offset(copy, TimeSpan.FromMinutes(_offset));
            Selection.activeObject = copy;
        }

    }

}
