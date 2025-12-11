using System.IO;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public sealed class RenderTextureExporter : EditorWindow
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
            if (!_texture || !GUILayout.Button("Save"))
                return;
            var path = EditorUtility.SaveFilePanelInProject("Export Texture", _texture.name, "png", "Export Render Texture");
            if (string.IsNullOrWhiteSpace(path))
                return;
            var active = RenderTexture.active;
            RenderTexture.active = _texture;
            var texture2D = new Texture2D(_texture.width, _texture.height);
            texture2D.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
            RenderTexture.active = active;
            File.WriteAllBytes(path, texture2D.EncodeToPNG());
        }

    }

}
