using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class ShipModule : MonoBehaviour
    {

        public Rigidbody Rigidbody { get; private set; }

        public ShipAssembly Assembly { get; set; }

        private bool _first;

        private void Awake() => Rigidbody = GetComponent<Rigidbody>();

        private void Update()
        {
            if (_first)
                Rigidbody.AddRelativeForce(Vector3.forward);
        }

    }

}
