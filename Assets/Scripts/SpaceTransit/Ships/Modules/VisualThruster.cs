using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class VisualThruster : ModuleComponentBase
    {

        private AudioSource _source;

        protected override void Awake()
        {
            base.Awake();
            _source = GetComponent<AudioSource>();
        }

        public override void OnStateChanged()
        {
            if (State is ShipState.LiftingOff or ShipState.Landing)
                _source.Play();
        }

    }

}
