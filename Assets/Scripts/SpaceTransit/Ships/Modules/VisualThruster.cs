using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class VisualThruster : ModuleComponentBase
    {

        private AudioSource _source;

        private ParticleSystem _particles;

        private bool _swap;

        private float _emissionRate;

        [SerializeField]
        private AudioClip liftoff;

        [SerializeField]
        private AudioClip land;

        protected override void Awake()
        {
            base.Awake();
            _source = GetComponent<AudioSource>();
            _particles = GetComponentInChildren<ParticleSystem>();
            _emissionRate = _particles.emission.rateOverTimeMultiplier;
            _swap = liftoff != land;
        }

        private void Start()
        {
            var emission = _particles.emission;
            emission.rateOverTimeMultiplier = 0;
        }

        public override void OnStateChanged()
        {
            var emission = _particles.emission;
            emission.rateOverTimeMultiplier = State is ShipState.Docked or ShipState.WaitingForDeparture ? 0 : _emissionRate;
            if (State is not (ShipState.LiftingOff or ShipState.Landing))
                return;
            if (_swap)
                _source.clip = State == ShipState.LiftingOff ? liftoff : land;
            _source.Play();
        }

    }

}
