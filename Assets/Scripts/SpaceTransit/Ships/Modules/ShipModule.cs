using SpaceTransit.Audio;
using SpaceTransit.Movement;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public sealed class ShipModule : MonoBehaviour
    {

        [field: SerializeField]
        public ModuleAudioBounds AudioBounds { get; private set; }

        [field: SerializeField]
        public Mountable Mount { get; private set; }

        public ShipAssembly Assembly { get; set; }

        private TubeBase _tube;

        private Transform _t;

        private float _distance;

        private void Awake()
        {
            _t = transform;
            _distance = _t.localPosition.z;
        }

        private void Start()
        {
            _tube = Assembly.startTube;
            UpdateLocation();
        }

        private void FixedUpdate()
        {
            if (Assembly.CurrentSpeed.Raw == 0)
                return;
            UpdateDistance();
            UpdateLocation();
        }

        private void UpdateLocation() => (_t.position, _t.rotation) = _tube.Sample(_distance);

        private void UpdateDistance()
        {
            var target = _distance + Assembly.CurrentSpeed * Time.fixedDeltaTime;
            if (target > _tube.Length)
            {
                if (!_tube.HasNext)
                    return;
                _distance = target - _tube.Length;
                _tube = _tube.Next;
            }
            else if (target < 0)
            {
                if (!_tube.HasPrevious)
                    return;
                _distance = target + _tube.Length;
                _tube = _tube.Previous;
            }
            else
                _distance = target;
        }

    }

}
