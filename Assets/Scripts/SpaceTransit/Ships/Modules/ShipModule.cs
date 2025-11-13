using SpaceTransit.Audio;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public sealed class ShipModule : MonoBehaviour
    {

        [field: SerializeField]
        public ModuleAudioBounds AudioBounds { get; private set; }

        public ShipAssembly Assembly { get; set; }

        private TubeBase _tube;

        private Transform _t;

        private float _distance;

        private void Awake()
        {
            _t = transform;
            _distance = _t.position.z;
        }

        private void Start() => _tube = Assembly.startTube;

        private void FixedUpdate()
        {
            var target = _distance + Assembly.CurrentSpeed * Time.fixedDeltaTime;
            if (target >= _tube.Length)
            {
                if (_tube.HasNext)
                {
                    _distance = _tube.Length - target;
                    _tube = _tube.Next;
                }
            }
            else if (target <= 0)
            {
                if (_tube.HasPrevious)
                {
                    _distance = target + _tube.Length;
                    _tube = _tube.Previous;
                }
            }
            else
                _distance = target;

            var sample = _tube.Sample(_distance);
            _t.position = sample.Position;
            _t.rotation = sample.Rotation;
        }

    }

}
