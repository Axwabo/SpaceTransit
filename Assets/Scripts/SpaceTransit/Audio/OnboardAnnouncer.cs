using SpaceTransit.Ships;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(QueuePlayer))]
    public sealed class OnboardAnnouncer : VaulterComponentBase
    {

        [SerializeField]
        private AudioClip signal;

        [SerializeField]
        private AudioClip welcomeStart;

        [SerializeField]
        private AudioClip welcomeEnd;

        [SerializeField]
        private AudioClip nextStop;

        [SerializeField]
        private AudioClip nextTerminus;

        [SerializeField]
        private AudioClip goodbye;

        private QueuePlayer _player;

        private bool _wasSailing;

        private float _delay;

        private bool _welcomePlayed;

        private bool _currentStopPlayed;

        protected override void Awake() => _player = GetComponent<QueuePlayer>();

        private void Update()
        {
            if (Parent.Parent.State != ShipState.Sailing || !Parent.IsInService)
            {
                _wasSailing = false;
                return;
            }

            if (!_wasSailing && !Parent.Assembly.IsStationary())
            {
                _delay = 10;
                _wasSailing = true;
            }

            if (_delay > 0 && (_delay -= Time.deltaTime) <= 0)
                PlayNextStop();
            if (_welcomePlayed && !_currentStopPlayed && IsNearStation)
                PlayCurrentStop();
        }

        private void PlayNextStop()
        {
            PlaySignal();
            if (!_welcomePlayed)
            {
                _player.Enqueue(welcomeStart);
                _player.Enqueue(Parent.Route.Destination.Station.Announcement);
                _player.Enqueue(welcomeEnd);
                _welcomePlayed = true;
            }

            _player.Enqueue(nextStop);
            _player.Enqueue(Parent.Stop.Station.Announcement);
            if (IsTerminus)
                _player.Enqueue(nextTerminus);
        }

        private void PlayCurrentStop()
        {
            PlaySignal();
            _player.Enqueue(Parent.Stop.Station.Announcement);
            if (IsTerminus)
                _player.Enqueue(goodbye);
            _currentStopPlayed = true;
        }

        private void PlaySignal() => _player.Enqueue(signal, 3);

        public override void OnRouteChanged() => _welcomePlayed = false;

        public override void OnStopChanged() => _currentStopPlayed = false;

    }

}
