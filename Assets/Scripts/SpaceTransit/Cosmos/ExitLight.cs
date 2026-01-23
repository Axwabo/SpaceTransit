using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(MeshRenderer))]
    public sealed class ExitLight : MonoBehaviour
    {

        [SerializeField]
        private Material stop;

        [SerializeField]
        private Material free;

        [SerializeField]
        private int materialIndex;

        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.sharedMaterials
        }

    }

}
