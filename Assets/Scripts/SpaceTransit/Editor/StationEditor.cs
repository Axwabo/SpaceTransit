using SpaceTransit.Routes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(Station))]
    [CanEditMultipleObjects]
    public sealed class StationEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set Name from ID"))
                Set();
        }

        private void Set()
        {
            Undo.RecordObjects(targets, "Set Name from ID");
            foreach (var o in targets)
            {
                var station = (Station) o;
                station.name = station.Name;
                EditorUtility.SetDirty(o);
            }
        }

    }

}
