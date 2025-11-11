using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class ShipModule : MonoBehaviour
    {

        public Rigidbody Rigidbody { get; private set; }

        public ShipAssembly Assembly { get; set; }

        public float speed;

        private Transform _t;

        private bool _first;

        private float _positionInSpline;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _t = transform;
        }

        private void Start()
        {
            _first = Assembly.Modules[0] == this;
            _positionInSpline = _t.localPosition.z;
        }

        private void Update()
        {
            if (!_first || Mathf.Approximately(0, speed))
                return;
            var sample = Assembly.journey.GetSampleAtDistance(_positionInSpline);
            _positionInSpline += Time.deltaTime * speed;
            _t.position = sample.location;
            _t.rotation = sample.Rotation;
        }

    }

}
