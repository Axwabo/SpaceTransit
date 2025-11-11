using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(Rigidbody))]
    public sealed class ShipModule : MonoBehaviour
    {

        public Rigidbody Rigidbody { get; private set; }

        public ShipAssembly Assembly { get; set; }

        private TubeBase _tube;

        private Transform _t;

        private bool _first;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _t = transform;
        }

        private void Start()
        {
            _first = Assembly.Modules[0] == this;
            _tube = Assembly.startTube;
        }

        private void Update()
        {
            var distance = _tube.GetDistance(_t.position);
            var target = distance + Time.deltaTime * Assembly.speed;
            if (target >= _tube.Length)
            {
                if (_tube.HasNext)
                {
                    distance = _tube.Length - target;
                    _tube = _tube.Next;
                }
            }
            else if (target <= 0)
            {
                if (_tube.HasPrevious)
                {
                    distance = target + _tube.Length;
                    _tube = _tube.Previous;
                }
            }
            else
                distance = target;

            var sample = _tube.Sample(distance);
            Rigidbody.position = sample.Position;
            Rigidbody.rotation = sample.Rotation;
        }

    }

}
