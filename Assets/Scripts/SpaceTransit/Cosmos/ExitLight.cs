using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(MeshRenderer))]
    public sealed class ExitLight : MonoBehaviour
    {

        [SerializeField]
        private Dock dock;

        [SerializeField]
        private Material stop;

        [SerializeField]
        private Material free;

        [SerializeField]
        private int materialIndex;

        private MeshRenderer _renderer;

        private Material[] _materials;

        private bool _wasFree;

        private bool _backwards;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _materials = _renderer.sharedMaterials;
            _materials[materialIndex] = stop;
            _renderer.sharedMaterials = _materials;
            _backwards = Vector3.Dot(transform.right, dock.transform.forward) < 0;
        }

        private void Update()
        {
            bool isFree;
            if (dock.Safety.IsOccupied)
            {
                var assembly = dock.Safety.Occupants.FirstFast().Assembly;
                isFree = assembly.Parent.CanProceed && assembly.Modules.Length == dock.Safety.Occupants.Count && _backwards == assembly.Reverse;
            }
            else
                isFree = false;

            if (isFree == _wasFree)
                return;
            _materials[materialIndex] = isFree ? free : stop;
            _renderer.sharedMaterials = _materials;
            _wasFree = isFree;
        }

    }

}
