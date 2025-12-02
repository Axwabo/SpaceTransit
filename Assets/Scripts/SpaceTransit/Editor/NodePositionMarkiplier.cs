using SplineMesh;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public sealed class NodePositionMarkiplier : EditorWindow
    {

        [MenuItem("Window/SpaceTransit/Node Markiplier")]
        private static void ShowWindow()
        {
            var window = GetWindow<NodePositionMarkiplier>();
            window.titleContent = new GUIContent("Spline Node Position Multiplier");
            window.Show();
        }

        private float _multiplier = 1;

        private void OnGUI()
        {
            _multiplier = EditorGUILayout.FloatField("Multiplier", _multiplier);
            var selection = Selection.gameObjects;
            if (selection.Length == 0 || !GUILayout.Button("Update"))
                return;
            foreach (var o in selection)
            {
                if (!o.TryGetComponent(out Spline spline))
                    continue;
                foreach (var node in spline.nodes)
                {
                    node.Position *= _multiplier;
                    node.Direction *= _multiplier;
                }

                spline.RefreshCurves();
            }
        }

    }

}
