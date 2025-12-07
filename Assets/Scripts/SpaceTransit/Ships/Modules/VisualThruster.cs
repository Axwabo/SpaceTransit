using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class VisualThruster : ModuleComponentBase
    {

        private const float MovementRate = 5;

        private AudioSource _source;

        private ParticleSystem _particles;

        private bool _swap;

        private float _emissionRate;

        private float _extensionTime;

        private Vector3 _targetPosition;

        private Quaternion _targetRotation;

        [SerializeField]
        private AudioClip liftoff;

        [SerializeField]
        private AudioClip land;

        [SerializeField]
        private Vector3 hoverPosition;

        [SerializeField]
        private Quaternion hoverRotation;

        [SerializeField]
        private Vector3 forwardsPosition;

        [SerializeField]
        private Quaternion forwardsRotation;

        [SerializeField]
        private Vector3 backwardsPosition;

        [SerializeField]
        private Quaternion backwardsRotation;

        protected override void Awake()
        {
            base.Awake();
            _source = GetComponent<AudioSource>();
            _particles = GetComponentInChildren<ParticleSystem>();
            _emissionRate = _particles.emission.rateOverTimeMultiplier;
            _swap = liftoff != land;
        }

        private void Start() => Emit(0);

        private void Emit(float rate)
        {
            var emission = _particles.emission;
            emission.rateOverTimeMultiplier = rate;
        }

        private void Play(AudioClip clip)
        {
            if (_swap)
                _source.clip = clip;
            _source.Play();
        }

        public override void OnStateChanged()
        {
            switch (State)
            {
                case ShipState.Docked:
                    Emit(0);
                    (_targetPosition, _targetRotation) = (Vector3.zero, hoverRotation);
                    _extensionTime = 0.5f;
                    break;
                case ShipState.WaitingForDeparture:
                    Emit(_emissionRate * 0.1f);
                    break;
                case ShipState.LiftingOff:
                    Play(liftoff);
                    Emit(_emissionRate * 2);
                    (_targetPosition, _targetRotation) = (hoverPosition, hoverRotation);
                    break;
                case ShipState.Sailing:
                    Emit(_emissionRate);
                    (_targetPosition, _targetRotation) = Assembly.Reverse
                        ? (backwardsPosition, backwardsRotation)
                        : (hoverPosition, hoverRotation);
                    break;
                case ShipState.Landing:
                    Play(land);
                    Emit(_emissionRate * 0.5f);
                    (_targetPosition, _targetRotation) = (hoverPosition, hoverRotation);
                    break;
            }
        }

        private void Update()
        {
            if (State is ShipState.Docked or ShipState.WaitingForDeparture && (_extensionTime -= Clock.Delta) < 0)
                return;
            if (State == ShipState.Sailing)
                UpdateTargetsBasedOnSpeed();
            Transform.GetLocalPositionAndRotation(out var position, out var rotation);
            var t = Clock.Delta * MovementRate;
            var currentEuler = rotation.eulerAngles;
            var targetEuler = _targetRotation.eulerAngles;
            var newRotation = Quaternion.Euler(
                Mathf.LerpAngle(currentEuler.x, targetEuler.x, t),
                Mathf.LerpAngle(currentEuler.y, targetEuler.y, t),
                Mathf.LerpAngle(currentEuler.z, targetEuler.z, t)
            );
            Transform.SetLocalPositionAndRotation(
                Vector3.Lerp(position, _targetPosition, t),
                newRotation
            );
            var main = _particles.main;
            main.startRotationX = new ParticleSystem.MinMaxCurve(newRotation.x);
        }

        private void UpdateTargetsBasedOnSpeed() => (_targetPosition, _targetRotation) = Assembly.IsStationary()
            ? (hoverPosition, hoverRotation)
            : Assembly.TargetSpeed < Assembly.CurrentSpeed != Assembly.Reverse
                ? (backwardsPosition, backwardsRotation)
                : (forwardsPosition, forwardsRotation);

    }

}
