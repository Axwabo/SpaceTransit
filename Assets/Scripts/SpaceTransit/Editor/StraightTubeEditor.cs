using SpaceTransit.Routes;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(StraightTube))]
    [CanEditMultipleObjects]
    public sealed class StraightTubeEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!GUILayout.Button("Migrate to Dock"))
                return;
            var objects = FindObjectsByType<TubeBase>(0);
            foreach (var o in targets)
            {
                var tube = (StraightTube) o;
                if (!tube.TryGetComponent(out Dock dock))
                    continue;
                dock.SetNext(tube.Next);
                foreach (var otherTube in objects)
                    if (otherTube.Next == tube)
                        otherTube.SetNext(dock);
            }
        }

    }

}
