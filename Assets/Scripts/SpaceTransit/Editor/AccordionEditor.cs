using SpaceTransit.Ships;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(Accordion))]
    public sealed class AccordionEditor : UnityEditor.Editor
    {

        private int _from;
        private int _to;

        private bool _applyFrom;
        private bool _applyTo;

        public override bool RequiresConstantRepaint() => true;

        public override void OnInspectorGUI()
        {
            GUILayout.Label($"From Raycast Index: {_from}");
            GUILayout.Label($"To Raycast Index: {_to}");
            _applyFrom = GUILayout.Toggle(_applyFrom, "Apply to last From");
            _applyTo = GUILayout.Toggle(_applyTo, "Apply to last To");
            GUILayout.Space(10);
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            if (target is not Accordion accordion)
                return;
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Raycast(ray, accordion.from, ref _from);
            Raycast(ray, accordion.to, ref _to);
            if (_applyFrom)
                accordion.fromVertices[^1] = _from;
            if (_applyTo)
                accordion.toVertices[^1] = _to;
        }

        private static void Raycast(Ray ray, MeshFilter from, ref int index)
        {
            if (!from.TryGetComponent(out MeshCollider collider)
                || !collider.Raycast(ray, out var hit, 50))
                return;
            var vertices = from.sharedMesh.vertices;
            var triangles = from.sharedMesh.triangles;
            var t = collider.transform;
            var ia = triangles[hit.triangleIndex];
            var ib = triangles[hit.triangleIndex] + 1;
            var ic = triangles[hit.triangleIndex] + 2;
            var a = t.TransformPoint(vertices[ia]);
            var b = t.TransformPoint(vertices[ib]);
            var c = t.TransformPoint(vertices[ic]);
            var da = Vector3.Distance(hit.point, a);
            var db = Vector3.Distance(hit.point, b);
            var dc = Vector3.Distance(hit.point, c);
            index = ia;
            var minDistance = da;
            if (db < minDistance)
                (index, minDistance) = (ib, db);
            if (dc < minDistance)
                index = ic;
        }

    }

}
