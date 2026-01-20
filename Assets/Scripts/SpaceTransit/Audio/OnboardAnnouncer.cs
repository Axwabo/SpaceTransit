using Katie.Unity;
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
        private Signal signal;

        [SerializeField]
        private PhrasePack pack;

        [SerializeField]
        private string announcer;

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
                _player.EnqueueWithSubtitles(announcer, $"Welcome aboard the ship to {Parent.Route.Destination.Station.name}. Please keep in mind that smoking is prohibited on the ship. We wish you a pleasant journey.", pack);
                _player.Delay(2);
                _welcomePlayed = true;
            }

            _player.EnqueueWithSubtitles(announcer, IsTerminus ? $"Next stop: {Parent.Stop.Station.name}, where this ship terminates." : $"Next stop: {Parent.Stop.Station.name}", pack);
            _player.Delay(3);
        }

        private void PlayCurrentStop()
        {
            PlaySignal();
            if (IsTerminus)
                _player.EnqueueWithSubtitles(announcer, $"{Parent.Stop.Station.name}. This ship terminates here. Thank you for choosing SpaceTransit. Goodbye.", pack);
            else
                _player.Enqueue(Parent.Stop.Station.Announcement);
            PlayDoors();
            _player.Delay(3);
            _currentStopPlayed = true;
        }

        private void PlaySignal() => _player.Enqueue(signal.Clip, signal.Duration);

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
            _player.EnqueueWithSubtitles(announcer, state == DoorsState.Left ? "Doors open on the left." : "Doors open on the right.", pack);
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
