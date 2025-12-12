using System;
using SpaceTransit.Vaulter;
using UnityEditor;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(RouteRotor))]
    public sealed class RouteRotorEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var rotor = (RouteRotor) target;
            var last = TimeSpan.Zero;
            foreach (var descriptor in rotor.routes)
            {
                if (!descriptor)
                    continue;
                if (descriptor.Origin.Departure < last)
                {
                    EditorGUILayout.HelpBox("Routes are not sequential!", MessageType.Error);
                    break;
                }

                last = descriptor.Destination.Arrival.Value;
            }

            base.OnInspectorGUI();
        }

    }

}
