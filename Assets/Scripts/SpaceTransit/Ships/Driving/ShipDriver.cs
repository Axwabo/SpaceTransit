using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ShipDriver : MonoBehaviour
    {

        public ShipAssembly Assembly { get; private set; }

        public DriverState State { get; private set; }

        private void Awake() => Assembly = GetComponent<ShipAssembly>();

    }

}
