using System.Linq;
using SpaceTransit.Tubes;
using SplineMesh;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(SplineTube))]
    public sealed class SplineTubeEditor : UnityEditor.Editor
    {

        private float _offset;

        private Spline Spline => (Spline) serializedObject.FindProperty("spline").boxedValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Split"))
                Split();
            _offset = EditorGUILayout.FloatField("Offset By X", _offset);
            if (GUILayout.Button("Offset"))
                Offset();
        }

        private void Split()
        {
            var spline = Spline;
            var root = spline.transform.parent;
            var nodes = spline.nodes;
            var hasTiling = spline.TryGetComponent(out SplineMeshTiling tiling);
            var tube = (SplineTube) target;
            var next = tube.Next;
            for (var i = nodes.Count - 2; i >= 0; i--)
            {
                var o = new GameObject((i + 1).ToString(), typeof(Spline));
                var t = o.transform;
                t.SetParent(root, false);
                t.SetAsFirstSibling();
                var splineClone = o.GetComponent<Spline>();
                splineClone.nodes = nodes.Skip(i).Take(2).Select(e => new SplineNode(e.Position, e.Direction)).ToList();
                ApplyTiling(o, hasTiling, tiling);
                splineClone.RefreshCurves();
                var tubeClone = o.AddComponent<SplineTube>();
                tubeClone.Next = next;
                next = tubeClone;
                using var serialized = new SerializedObject(tubeClone);
                serialized.FindProperty("spline").boxedValue = splineClone;
                serialized.ApplyModifiedProperties();
            }

            if (tube.HasPrevious)
                tube.Previous.Next = next;
        }

        private static void ApplyTiling(GameObject o, bool hasTiling, SplineMeshTiling tiling)
        {
            if (!hasTiling)
                return;
            var tilingClone = o.AddComponent<SplineMeshTiling>();
            tilingClone.mesh = tiling.mesh;
            tilingClone.material = tiling.material;
            tilingClone.updateInPlayMode = tiling.updateInPlayMode;
            tilingClone.scale = tiling.scale;
            tilingClone.translation = tiling.translation;
            tilingClone.rotation = tiling.rotation;
            tilingClone.generateCollider = tiling.generateCollider;
            tilingClone.mode = tiling.mode;
        }

        private void Offset()
        {
            var spline = Spline;
            var offset = Vector3.right * _offset;
            foreach (var node in spline.nodes)
            {
                var transformedOffset = node.Rotation() * offset;
                node.Position += transformedOffset;
                node.Direction += transformedOffset;
            }
        }

    }

}
