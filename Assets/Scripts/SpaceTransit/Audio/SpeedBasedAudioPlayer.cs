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

        [SerializeField]
        private bool restart;

        [SerializeField]
        private bool restartOnEnable;

        private AudioSource _source;

        private ShipAssembly _assembly;

        private float _targetVolume;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _assembly = GetComponentInParent<ShipAssembly>();
        }

        private void OnEnable()
        {
            if (!restartOnEnable)
                return;
            _source.time = 0;
            UpdateTargetVolume();
            if (_targetVolume != 0)
                _source.Play();
        }

        private void Update()
        {
            UpdateTargetVolume();
            UpdateSource();
        }

        private void UpdateSource()
        {
            var volume = _source.volume;
            _source.volume = Mathf.MoveTowards(volume, _targetVolume, Clock.UnscaledDelta);
            if (_targetVolume == 0 || volume != 0 || _source.loop)
                return;
            _source.Play();
            _source.time = restart
                ? 0
                : op switch
                {
                    Operator.Accelerating => _assembly.CurrentSpeed.Raw / _assembly.MaxSpeed * (_source.clip.length * 0.95f),
                    Operator.Decelerating => (1 - _assembly.CurrentSpeed.Raw / _assembly.MaxSpeed) * (_source.clip.length * 0.95f),
                    _ => 0
                };
        }

        private void UpdateTargetVolume() => _targetVolume = _assembly.IsStationary() || _assembly.CurrentSpeed.Raw <= min || _assembly.CurrentSpeed.Raw > max
            ? 0
            : op switch
            {
                Operator.Accelerating when _assembly.CurrentSpeed < _assembly.TargetSpeed => 1,
                Operator.Decelerating when _assembly.CurrentSpeed > _assembly.TargetSpeed => 1,
                Operator.RangeAtTarget when Mathf.Approximately(_assembly.CurrentSpeed.Raw, _assembly.TargetSpeed.Raw) => 1,
                Operator.Range => 1,
                _ => 0
            };

        private enum Operator
        {

            Accelerating,
            RangeAtTarget,
            Decelerating,
            Range

        }

    }

}
