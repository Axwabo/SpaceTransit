using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class SpeedBasedAudioPlayer : MonoBehaviour
    {

        [SerializeField]
        private Operator op;

        [SerializeField]
        private float min;

        [SerializeField]
        private float max;

        private AudioSource _source;

        private ShipAssembly _assembly;

        private float _targetVolume;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _assembly = GetComponentInParent<ShipAssembly>();
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
            if (_targetVolume == 0 || volume != 0 || _source.loop)
                return;
            _source.Play();
            _source.time = op != Operator.Range
                ? _assembly.CurrentSpeed.Raw / _assembly.MaxSpeed * (_source.clip.length * 0.95f)
                : 0;
        }

        private void UpdateTargetVolume() => _targetVolume = _assembly.IsStationary()
            ? 0
            : op switch
            {
                Operator.Accelerating when _assembly.CurrentSpeed < _assembly.TargetSpeed => 1,
                Operator.Decelerating when _assembly.CurrentSpeed > _assembly.TargetSpeed => 1,
                Operator.Range when Mathf.Approximately(_assembly.CurrentSpeed.Raw, _assembly.TargetSpeed.Raw)
                                    && _assembly.CurrentSpeed.Raw > min
                                    && _assembly.CurrentSpeed.Raw <= max => 1,
                _ => 0
            };

        private enum Operator
        {

            Accelerating,
            Range,
            Decelerating

        }

    }

}
