using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ShipController : MonoBehaviour
    {

        public ShipAssembly Assembly { get; private set; }

        public ShipState State { get; private set; }

        private void Awake() => Assembly = GetComponent<ShipAssembly>();

    }

}
