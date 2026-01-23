using SpaceTransit.Routes;
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

        [SerializeField]
        private AudioClip doorsLeft;

        [SerializeField]
        private AudioClip doorsRight;

        private QueuePlayer _player;

        private bool _wasSailing;

        private float _delay;

        private bool _welcomePlayed;

        private bool _currentStopPlayed;

        private DoorsState _doorsState;

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

            if (_delay > 0 && (_delay -= Clock.Delta) <= 0)
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
                _player.Delay(2);
                _welcomePlayed = true;
            }

            _player.Enqueue(nextStop);
            _player.Enqueue(Parent.Stop.Station.Announcement);
            if (IsTerminus)
                _player.Enqueue(nextTerminus);
            _player.Delay(3);
        }

        private void PlayCurrentStop()
        {
            PlaySignal();
            _player.Enqueue(Parent.Stop.Station.Announcement);
            if (IsTerminus)
                _player.Enqueue(goodbye);
            PlayDoors();
            _player.Delay(3);
            _currentStopPlayed = true;
        }

        private void PlaySignal() => _player.Enqueue(signal, 3);

        private void PlayDoors()
        {
            if (!Station.TryGetLoadedStation(Parent.Stop.Station, out var station))
                return;
            var dock = station.Docks[Parent.Stop.DockIndex];
            var state = (dock.Left != Assembly.Reverse, dock.Right != Assembly.Reverse) switch
            {
                (true, true) => DoorsState.Both,
                (false, true) => DoorsState.Right,
                (true, false) => DoorsState.Left,
                (false, false) => DoorsState.None
            };
            if (state == _doorsState)
                return;
            _doorsState = state;
            if (state is DoorsState.None or DoorsState.Both)
                return;
            _player.Delay(1);
            _player.Enqueue(state == DoorsState.Left ? doorsLeft : doorsRight);
        }

        public override void OnRouteChanged()
        {
            _welcomePlayed = false;
            _doorsState = DoorsState.None;
        }

        public override void OnStopChanged() => _currentStopPlayed = false;

        private enum DoorsState
        {

            None,
            Left,
            Right,
            Both

        }

    }

}
