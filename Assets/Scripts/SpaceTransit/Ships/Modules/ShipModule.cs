using UnityEngine;

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

        private void FixedUpdate()
        {
            if (Mathf.Approximately(0, Assembly.speed))
                return;
            var sample = Assembly.journey.SampleNearest(_t.position);
            _t.position = sample.location;
            _t.rotation = sample.Rotation;
            if (!_first)
                return;
            var forward = _t.forward;
            Rigidbody.linearVelocity = Assembly.speed * forward;
            Rigidbody.AddForce(forward);
        }

    }

}
