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

        [SerializeField]
        private bool flipNormals;

        [SerializeField]
        private bool crossConnect;

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
            var fromMesh = from.sharedMesh;
            var toMesh = to.sharedMesh;
            _t = transform;
            _fromTransform = from.transform;
            _toTransform = to.transform;
            _fromVertices = fromMesh.vertices;
            _toVertices = toMesh.vertices;
            var normals = fromMesh.normals;
            var newNormals = new Vector3[normals.Length + 1];
            for (var i = 0; i < fromVertices.Length; i++)
                newNormals[i] = normals[fromVertices[i]];
            newNormals[^1] = normals[fromVertices[0]];
            _mesh = new Mesh
            {
                name = "Accordion",
                normals = newNormals
            };
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
            TrianglesToWrite.Clear();
            for (var i = 0; i < fromVertices.Length; i++)
            {
                var ta = VerticesToWrite.Count;
                var tb = ta + 1;
                var tc = ta + 2;
                var td = ta + 3;
                var va = _t.InverseTransformPoint(_fromTransform.TransformPoint(_fromVertices[fromVertices[i]]));
                var vc = _t.InverseTransformPoint(_toTransform.TransformPoint(_toVertices[toVertices[crossConnect ? ^(i + 1) : i]]));
                VerticesToWrite.Add(va);
                VerticesToWrite.Add(vc);
                AddTriangle(ta, tb, crossConnect ? td : tc);
                if (crossConnect)
                    AddTriangle(td, ta, tc);
                else
                    AddTriangle(tc, tb, td);
            }

            VerticesToWrite.Add(_t.InverseTransformPoint(_fromTransform.TransformPoint(_fromVertices[fromVertices[0]])));
            VerticesToWrite.Add(_t.InverseTransformPoint(_toTransform.TransformPoint(_toVertices[crossConnect ? ^1 : toVertices[0]])));
            _mesh.SetVertexBufferData(); // this is too complicated
            _mesh.SetVertices(VerticesToWrite);
            _mesh.SetTriangles(TrianglesToWrite, 0);
        }

        private void AddTriangle(int a, int b, int c)
        {
            if (flipNormals)
                (a, c) = (c, a);
            TrianglesToWrite.Add(a);
            TrianglesToWrite.Add(b);
            TrianglesToWrite.Add(c);
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
