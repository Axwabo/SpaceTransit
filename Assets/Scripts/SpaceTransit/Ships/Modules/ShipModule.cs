using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class ShipModule : MonoBehaviour
    {

        public Rigidbody Rigidbody { get; private set; }

        public ShipAssembly Assembly { get; set; }

        private Transform _t;

        private bool _first;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _t = transform;
        }

        private void Start() => _first = Assembly.Modules[0] == this;

        private void Update()
        {
            var sample = Assembly.journey.SampleNearest(_t.position);
            Rigidbody.position = sample.location;
            Rigidbody.rotation = sample.Rotation;
        }

        private void FixedUpdate()
        {
            if (Mathf.Approximately(0, Assembly.speed))
                return;
            if (!_first)
                return;
            var forward = _t.forward;
            Rigidbody.linearVelocity = Rigidbody.linearVelocity.magnitude * forward;
            var move = InputSystem.actions["Move"].ReadValue<Vector2>();
            if (move.y > 0)
                Rigidbody.AddRelativeForce(Vector3.forward);
            if (move.y < 0)
                Rigidbody.AddRelativeForce(Vector3.back);
        }

    }

}
