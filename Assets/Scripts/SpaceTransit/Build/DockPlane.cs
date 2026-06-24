using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class DockPlane : MonoBehaviour
    {

        [field: SerializeField]
        public Dock Dock { get; private set; }

    }

}
