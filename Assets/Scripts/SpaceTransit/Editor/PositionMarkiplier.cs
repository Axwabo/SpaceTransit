using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public sealed class PositionMarkiplier : EditorWindow
    {

        [MenuItem("Window/SpaceTransit/Position Markiplier")]
        private static void ShowWindow()
        {
            var window = GetWindow<PositionMarkiplier>();
            window.titleContent = new GUIContent("Position Multiplier");
            window.Show();
        }

        private float _multiplier = 1;

        private void OnGUI()
        {
            _multiplier = EditorGUILayout.FloatField("Multiplier", _multiplier);
            var selection = Selection.transforms;
            if (selection.Length == 0 || !GUILayout.Button("Update"))
                return;
            foreach (var t in selection)
                t.localPosition *= _multiplier;
        }

    }

}
