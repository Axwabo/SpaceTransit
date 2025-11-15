using System.IO;
using System.Linq;
using SpaceTransit.Routes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(StationId))]
    [CanEditMultipleObjects]
    public sealed class StationIdEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Assign Announcement"))
                Assign();
        }

        private void Assign()
        {
            var folder = EditorUtility.OpenFolderPanel("Path to announcement clips", null, "");
            if (string.IsNullOrEmpty(folder))
                return;
            var search = new[] {Path.Combine("Assets", Path.GetRelativePath(Application.dataPath, folder))};
            var assets = AssetDatabase.FindAssets("t:AudioClip", search)
                .Select(e => AssetDatabase.LoadAssetByGUID<AudioClip>(new GUID(e)))
                .ToDictionary(e => e.name);
            Undo.RecordObjects(targets, "Assign Announcement");
            foreach (var o in targets)
            {
                if (!assets.TryGetValue(o.name, out var clip))
                    continue;
                ((StationId) o).Announcement = clip;
                EditorUtility.SetDirty(o);
            }
        }

    }

}
