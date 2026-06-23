using SpaceTransit.Menu;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(MapSettings))]
    public sealed class MapSettingsEditor : UnityEditor.Editor
    {

        private void OnEnable() => SceneView.duringSceneGui += DrawSceneGUI;

        private void OnDisable() => SceneView.duringSceneGui -= DrawSceneGUI;

        private void DrawSceneGUI(SceneView obj)
        {
            var settings = (MapSettings) target;
            Handles.color = Color.green;
            Handles.DrawWireCube(settings.Center, settings.Size);
        }

    }

}
