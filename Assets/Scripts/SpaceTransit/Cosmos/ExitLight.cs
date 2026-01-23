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
            var isFree = CanProceed;
            if (isFree == _wasFree)
                return;
            _materials[materialIndex] = isFree ? free : stop;
            _renderer.sharedMaterials = _materials;
            _wasFree = isFree;
        }

        private bool CanProceed
        {
            get
            {
                if (!dock.Safety.IsOccupied)
                    return false;
                var assembly = dock.Safety.Occupants.FirstFast().Assembly;
                if (!assembly.Parent.CanProceed || assembly.Modules.Length != dock.Safety.Occupants.Count || _backwards != assembly.Reverse)
                    return false;
                var exits = _backwards ? dock.BackExits : dock.FrontExits;
                foreach (var exit in exits)
                    if (exit.IsUsedOnlyBy(assembly))
                        return true;
                return false;
            }
        }

    }

}
