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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Split"))
                Split();
        }

        private void Split()
        {
            var spline = (Spline) serializedObject.FindProperty("spline").boxedValue;
            var root = spline.transform.parent;
            var nodes = spline.nodes;
            var hasTiling = spline.TryGetComponent(out SplineMeshTiling tiling);
            var next = ((SplineTube) target).Next;
            for (var i = nodes.Count - 2; i >= 0; i--)
            {
                var o = new GameObject((i + 1).ToString(), typeof(Spline), typeof(SplineMeshTiling), typeof(SplineTube))
                {
                    transform =
                    {
                        parent = root
                    }
                };
                var splineClone = o.GetComponent<Spline>();
                splineClone.nodes = nodes.Skip(i).Take(2).ToList();
                ApplyTiling(o, hasTiling, tiling);
                var tube = o.GetComponent<SplineTube>();
                tube.Next = next;
                next = tube;
            }
        }

        private static void ApplyTiling(GameObject o, bool hasTiling, SplineMeshTiling tiling)
        {
            var tilingClone = o.GetComponent<SplineMeshTiling>();
            if (!hasTiling)
            {
                DestroyImmediate(tilingClone);
                return;
            }

            tilingClone.material = tiling.material;
            tilingClone.updateInPlayMode = tilingClone.updateInPlayMode;
            tilingClone.scale = tilingClone.scale;
            tilingClone.translation = tilingClone.translation;
            tilingClone.rotation = tilingClone.rotation;
            tilingClone.generateCollider = tilingClone.generateCollider;
        }

    }

}
