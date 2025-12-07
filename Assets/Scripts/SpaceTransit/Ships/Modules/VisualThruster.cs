using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class VisualThruster : ModuleComponentBase
    {

        private AudioSource _source;

        private bool _swap;

        [SerializeField]
        private AudioClip liftoff;

        [SerializeField]
        private AudioClip land;

        protected override void Awake()
        {
            base.Awake();
            _source = GetComponent<AudioSource>();
            _swap = liftoff != land;
        }

        public override void OnStateChanged()
        {
            if (State is not (ShipState.LiftingOff or ShipState.Landing))
                return;
            if (_swap)
                _source.clip = State == ShipState.LiftingOff ? liftoff : land;
            _source.Play();
        }

    }

}
