using SpaceTransit.Menu;
using SpaceTransit.Movement;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Audio
{

    public sealed class ClosingDoorsAnnouncer : ShipComponentBase
    {

        [SerializeField]
        private AudioSource source;

        private Transform _t;

        private bool _played;

        protected override void Awake() => _t = source.transform;

        public override void OnStateChanged(ShipState previousState)
        {
            if (Parent.State == ShipState.Docked)
                _played = false;
            else if (!_played && Parent.State == ShipState.WaitingForDeparture && !Parent.ModulesReadyForDeparture)
                Play();
        }

        private void Update()
        {
            if (!_played && !Assembly.IsManuallyDriven && Parent.State == ShipState.Docked && Parent.WillBeDeparting && !Parent.ModulesReadyForDeparture)
                Play();
        }

        private void Play()
        {
            if (Parent.TryGetVaulter(out var vaulter) && Vector3.Distance(_t.position, MovementController.Current.LastPosition) < 10)
                KatieSubtitleList.Add(vaulter.Announcer, "Please stand clear of the closing doors.", 0, source.clip.length);
            _played = true;
            source.time = 0;
            source.Play();
        }

    }

}
