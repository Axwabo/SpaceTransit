using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class SpeedBasedAudioPlayer : ShipComponentBase
    {

        [SerializeField]
        private Operator op;

        private AudioSource _source;

        private float _targetVolume;

        private void Awake() => _source = GetComponent<AudioSource>();

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

        private void UpdateTargetVolume() => _targetVolume = Assembly.IsStationary()
            ? 0
            : op switch
            {
                Operator.Accelerating when Assembly.CurrentSpeed < Assembly.TargetSpeed => 1,
                Operator.TargetSpeed when Mathf.Approximately(Assembly.CurrentSpeed.Raw, Assembly.TargetSpeed.Raw) => 1,
                Operator.Decelerating when Assembly.CurrentSpeed > Assembly.TargetSpeed => 1,
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
