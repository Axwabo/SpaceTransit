using SpaceTransit.Ships;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(ShipAssembly))]
    public sealed class ShipAssemblyInspector : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var assembly = (ShipAssembly) target;
            GUILayout.Space(20);
            GUILayout.Label($"Current speed: {assembly.CurrentSpeed.RawKmh} km/h");
            GUILayout.Label($"Target speed: {assembly.TargetSpeed.RawKmh} km/h");
            if (GUILayout.Button("Reverse"))
                assembly.Reverse = !assembly.Reverse;
        }

        public override bool RequiresConstantRepaint() => true;

    }

}
