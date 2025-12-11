using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(MeshFilter))]
    public sealed class Accordion : MonoBehaviour
    {

        private static readonly List<Vector3> VerticesToWrite = new();
        private static readonly List<int> TrianglesToWrite = new();

        [SerializeField]
        public MeshFilter from;

        [SerializeField]
        public MeshFilter to;

        [SerializeField]
        public int[] fromVertices;

        [SerializeField]
        public int[] toVertices;

        private Transform _fromTransform;

        private Transform _toTransform;

        private Pose _fromPrevious;

        private Pose _toPrevious;

        private Vector3[] _fromVertices;

        private Vector3[] _toVertices;

        private Mesh _mesh;

        private Transform _t;

        private void Awake()
        {
            _t = transform;
            _fromTransform = from.transform;
            _toTransform = to.transform;
            _fromVertices = from.sharedMesh.vertices;
            _toVertices = to.sharedMesh.vertices;
            _mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = _mesh;
        }

        private void Update()
        {
            _fromTransform.GetLocalPositionAndRotation(out var fromPos, out var fromRot);
            _toTransform.GetLocalPositionAndRotation(out var toPos, out var toRot);
            var fromPose = new Pose(fromPos, fromRot);
            var toPose = new Pose(toPos, toRot);
            if (fromPose == toPose)
                return;
            _fromPrevious = fromPose;
            _toPrevious = toPose;
            VerticesToWrite.Clear();
            TrianglesToWrite.Clear();
            for (var i = 0; i < fromVertices.Length - 1; i++)
            {
                var vertices = VerticesToWrite.Count;
                var a = _t.InverseTransformPoint(_fromTransform.TransformPoint(_fromVertices[fromVertices[i]]));
                var b = _t.InverseTransformPoint(_fromTransform.TransformPoint(_fromVertices[fromVertices[i + 1]]));
                var c = _t.InverseTransformPoint(_toTransform.TransformPoint(_toVertices[toVertices[i]]));
                var d = _t.InverseTransformPoint(_toTransform.TransformPoint(_toVertices[toVertices[i + 1]]));
                VerticesToWrite.Add(a);
                VerticesToWrite.Add(b);
                VerticesToWrite.Add(c);
                VerticesToWrite.Add(d);
                TrianglesToWrite.Add(vertices);
                TrianglesToWrite.Add(vertices + 1);
                TrianglesToWrite.Add(vertices + 2);
                TrianglesToWrite.Add(vertices + 1);
                TrianglesToWrite.Add(vertices + 2);
                TrianglesToWrite.Add(vertices + 3);
            }

            _mesh.SetVertices(VerticesToWrite);
            _mesh.SetTriangles(TrianglesToWrite, 0);
            _mesh.normals = new Vector3[VerticesToWrite.Count]; // TODO: this is absolutely horrible, gonna implement later :33
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            DrawVertexGizmos(from, fromVertices);
            Gizmos.color = Color.green;
            DrawVertexGizmos(to, toVertices);
        }

        private static void DrawVertexGizmos(MeshFilter meshFilter, int[] indexes)
        {
            if (!meshFilter)
                return;
            var t = meshFilter.transform;
            var mesh = meshFilter.sharedMesh;
            var vertices = mesh.vertices;
            foreach (var index in indexes)
                Gizmos.DrawSphere(t.TransformPoint(vertices[index]), 0.01f);
        }

    }

}
