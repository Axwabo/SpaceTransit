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
                if (dock.UsedBy.Count == 0)
                    return false;
                var assembly = dock.UsedBy.FirstFast();
                if (dock.Safety.Occupants.Count != 0 && !dock.Safety.Occupants.Contains(assembly.FrontModule) || !dock.Safety.CanProceed(assembly))
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
