using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class DockPlane : MonoBehaviour
    {

        [SerializeField]
        private DockPlane source;

        [field: SerializeField]
        public Dock Dock { get; private set; }

        private void Awake()
        {
            if (source)
                Dock = source.Dock;
            if (!Dock)
                Debug.LogError("No dock has been assigned to this DockPlane!", this);
        }

    }

}
