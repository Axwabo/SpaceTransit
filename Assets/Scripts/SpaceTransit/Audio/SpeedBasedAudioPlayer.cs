using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class SpeedBasedAudioPlayer : MonoBehaviour
    {

        [SerializeField]
        private Operator op;

        private ShipAssembly _assembly;

        private AudioSource _source;

        private float _targetVolume;

        private void Awake()
        {
            _assembly = GetComponentInParent<ShipAssembly>();
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            UpdateTargetVolume();
            UpdateSource();
        }

        private void UpdateSource()
        {
            var volume = _source.volume;
            _source.volume = Mathf.MoveTowards(volume, _targetVolume, Time.deltaTime);
            if (_targetVolume == 0 || volume != 0)
                return;
            _source.time = 0;
            _source.Play();
        }

        private void UpdateTargetVolume() => _targetVolume = _assembly.IsStationary()
            ? 0
            : op switch
            {
                Operator.Accelerating when _assembly.CurrentSpeed < _assembly.TargetSpeed => 1,
                Operator.TargetSpeed when Mathf.Approximately(_assembly.CurrentSpeed.Raw, _assembly.TargetSpeed.Raw) => 1,
                Operator.Decelerating when _assembly.CurrentSpeed > _assembly.TargetSpeed => 1,
                _ => 0
            };

        private enum Operator
        {

            Accelerating,
            TargetSpeed,
            Decelerating

        }

    }

}
