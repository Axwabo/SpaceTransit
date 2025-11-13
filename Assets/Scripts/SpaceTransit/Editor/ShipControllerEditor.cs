using SpaceTransit.Ships;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(ShipController))]
    public sealed class ShipControllerEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var controller = (ShipController) target;
            GUILayout.Space(20);
            GUILayout.Label($"State: {controller.State}");
            if (GUILayout.Button("Land"))
                controller.Land();
            if (GUILayout.Button("Lift Off"))
                controller.LiftOff();
        }

        public override bool RequiresConstantRepaint() => true;

    }

}
