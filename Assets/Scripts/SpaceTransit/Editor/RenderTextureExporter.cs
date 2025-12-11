using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public class RenderTextureExporter : EditorWindow
    {

        [MenuItem("Window/SpaceTransit/Export Render Texture")]
        private static void ShowWindow()
        {
            var window = GetWindow<RenderTextureExporter>();
            window.titleContent = new GUIContent("Export Render Texture");
            window.Show();
        }

        private RenderTexture _texture;

        private void OnGUI()
        {
            _texture = EditorGUILayout.ObjectField("Texture", _texture, typeof(RenderTexture), false) as RenderTexture;
            if (!GUILayout.Button("Save"))
                return;
            var path = EditorUtility.SaveFilePanelInProject("Export Texture", "", "", "");
            if (string.IsNullOrWhiteSpace(path))
                return;
        }

    }

}
