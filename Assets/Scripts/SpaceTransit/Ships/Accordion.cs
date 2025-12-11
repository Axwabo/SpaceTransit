using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(MeshFilter))]
    public sealed class Accordion : MonoBehaviour
    {

        private static readonly List<Vector3> VerticesToWrite = new();
        private static readonly List<Vector3> NormalsToWrite = new();
        private static readonly List<int> TrianglesToWrite = new();

        [SerializeField]
        public MeshFilter from;

        [SerializeField]
        public MeshFilter to;

        [SerializeField]
        public int[] fromVertices;

        [SerializeField]
        public int[] toVertices;

        [SerializeField]
        private bool flipNormals;

        private Transform _fromTransform;

        private Transform _toTransform;

        private Pose _fromPrevious;

        private Pose _toPrevious;

        private Vector3[] _fromVertices;

        private Vector3[] _toVertices;

        private Vector3[] _fromNormals;

        private Mesh _mesh;

        private Transform _t;

        private void Awake()
        {
            var fromMesh = from.sharedMesh;
            var toMesh = to.sharedMesh;
            _t = transform;
            _fromTransform = from.transform;
            _toTransform = to.transform;
            _fromVertices = fromMesh.vertices;
            _toVertices = toMesh.vertices;
            _fromNormals = fromMesh.normals;
            _mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = _mesh;
        }

        private void LateUpdate()
        {
            _fromTransform.GetLocalPositionAndRotation(out var fromPos, out var fromRot);
            _toTransform.GetLocalPositionAndRotation(out var toPos, out var toRot);
            var fromPose = new Pose(fromPos, fromRot);
            var toPose = new Pose(toPos, toRot);
            if (fromPose == _fromPrevious && toPose == _toPrevious)
                return;
            _fromPrevious = fromPose;
            _toPrevious = toPose;
            VerticesToWrite.Clear();
            NormalsToWrite.Clear();
            TrianglesToWrite.Clear();
            for (var i = 0; i < fromVertices.Length; i++)
            {
                AddNormal(i);
                var vertices = VerticesToWrite.Count;
                var a = _t.InverseTransformPoint(_fromTransform.TransformPoint(_fromVertices[fromVertices[i]]));
                var c = _t.InverseTransformPoint(_toTransform.TransformPoint(_toVertices[toVertices[i]]));
                VerticesToWrite.Add(a);
                VerticesToWrite.Add(c);
                TrianglesToWrite.Add(vertices);
                TrianglesToWrite.Add(vertices + 1);
                TrianglesToWrite.Add(vertices + 2);
                TrianglesToWrite.Add(vertices + 2);
                TrianglesToWrite.Add(vertices + 1);
                TrianglesToWrite.Add(vertices + 3);
            }

            VerticesToWrite.Add(_t.InverseTransformPoint(_fromTransform.TransformPoint(_fromVertices[fromVertices[0]])));
            VerticesToWrite.Add(_t.InverseTransformPoint(_toTransform.TransformPoint(_toVertices[toVertices[0]])));
            AddNormal(0);
            _mesh.SetVertices(VerticesToWrite);
            _mesh.SetNormals(NormalsToWrite);
            _mesh.SetTriangles(TrianglesToWrite, 0);
        }

        private void AddNormal(int i)
        {
            var normal = _fromNormals[fromVertices[i]];
            var flipped = flipNormals ? -normal : normal;
            NormalsToWrite.Add(flipped);
            NormalsToWrite.Add(flipped);
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
