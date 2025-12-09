using SpaceTransit.Cosmos.Actions;
using SpaceTransit.Routes;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;

// ReSharper disable CoVariantArrayConversion

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
            var tubes = FindObjectsByType<TubeBase>(0);
            var remappers = FindObjectsByType<TubeRemapper>(0);
            foreach (var o in targets)
            {
                var tube = (StraightTube) o;
                if (tube.TryGetComponent(out Dock dock))
                    Migrate(dock, tube, tubes, remappers);
            }
        }

        private static void Migrate(Dock dock, StraightTube tube, TubeBase[] tubes, TubeRemapper[] remappers)
        {
            Undo.RecordObject(dock, "Migrate StraightTube to Dock");
            dock.SetNext(tube.Next);
            dock.SpeedLimit = tube.SpeedLimit;
            PrefabUtility.RecordPrefabInstancePropertyModifications(dock);
            Undo.RecordObjects(tubes, "Migrate StraightTube to Dock");
            Undo.RecordObjects(remappers, "Migrate StraightTube to Dock");
            foreach (var otherTube in tubes)
            {
                if (otherTube.Next != tube)
                    continue;
                otherTube.SetNext(dock);
                PrefabUtility.RecordPrefabInstancePropertyModifications(otherTube);
            }

            foreach (var remapper in remappers)
                Migrate(dock, tube, remapper);
        }

        private static void Migrate(Dock dock, StraightTube tube, TubeRemapper remapper)
        {
            var connect = remapper.connectTube == tube;
            var to = remapper.connectTo == tube;
            if (connect)
                remapper.connectTube = dock;
            if (to)
                remapper.connectTo = dock;
            if (connect || to)
                PrefabUtility.RecordPrefabInstancePropertyModifications(remapper);
        }

    }

}
