using UnityEngine;

namespace SpaceTransit.Ships
{

    public sealed class Accordion : MonoBehaviour
    {

        [SerializeField]
        public MeshFilter from;

        [SerializeField]
        public MeshFilter to;

        [SerializeField]
        private int[] fromVertices;

        [SerializeField]
        private int[] toVertices;

        private Transform _fromTransform;

        private Transform _toTransform;

        private Pose _fromPrevious;

        private Pose _toPrevious;

        private void Awake()
        {
            _fromTransform = from.transform;
            _toTransform = to.transform;
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
            // TODO: modify mesh
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
