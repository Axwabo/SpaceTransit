using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public sealed class TimeScale : EditorWindow
    {

        [MenuItem("Window/SpaceTransit/Time Scale")]
        private static void ShowWindow()
        {
            var window = GetWindow<TimeScale>(true);
            window.titleContent = new GUIContent("Time Scale");
            window.Show();
        }

        private void OnGUI() => Time.timeScale = EditorGUILayout.Slider("Scale", Time.timeScale, 0.1f, 20);

    }

}
