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
            FindClosest(accordion.from, ref _from);
            FindClosest(accordion.to, ref _to);
            if (_applyFrom)
                accordion.fromVertices[^1] = _from;
            if (_applyTo)
                accordion.toVertices[^1] = _to;
        }

        private static void FindClosest(MeshFilter from, ref int index)
        {
            var vertices = from.sharedMesh.vertices;
            var maxDistance = float.MaxValue;
            var mouse = Event.current.mousePosition;
            var t = from.transform;
            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var point = HandleUtility.WorldToGUIPoint(t.TransformPoint(vertex));
                var distance = Vector2.Distance(mouse, point);
                if (distance < maxDistance)
                    (index, maxDistance) = (i, distance);
            }
        }

    }

}
