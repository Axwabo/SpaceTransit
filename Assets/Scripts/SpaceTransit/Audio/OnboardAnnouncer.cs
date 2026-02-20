using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(QueuePlayer))]
    public sealed class OnboardAnnouncer : VaulterComponentBase
    {

        private const string StoppedTraffic = "The ship has stopped due to traffic reasons. We apologize for the inconvenience. Thank you.";

        [SerializeField]
        private Signal signal;

        [SerializeField]
        private PhrasePack pack;

        [SerializeField]
        public string announcer;

        private QueuePlayer _player;

        private bool _wasSailing;

        private float _delay;

        private bool _welcomePlayed;

        private bool _currentStopPlayed;

        private DoorsState _doorsState;

        private bool _wasStationary;

        protected override void Awake() => _player = GetComponent<QueuePlayer>();

        private void Update()
        {
            if (Controller.State != ShipState.Sailing || !Parent.IsInService)
            {
                _wasSailing = false;
                return;
            }

            if (!_wasSailing && !Assembly.IsStationary())
            {
                _delay = 10;
                _wasSailing = true;
            }

            if (!Assembly.IsPlayerMounted)
                return;
            if (_delay > 0 && (_delay -= Clock.Delta) <= 0)
                PlayNextStop();
            if (_welcomePlayed && !_currentStopPlayed && IsNearStation)
                PlayCurrentStop();
            if (Assembly.IsManuallyDriven || Controller.CanProceed || Assembly.IsStationary() == _wasStationary)
                return;
            _wasStationary = Assembly.IsStationary();
            if (_wasStationary)
                _player.EnqueueWithSubtitles(announcer, StoppedTraffic, pack, signal);
        }

        private void PlayNextStop()
        {
            var welcomePlayed = _welcomePlayed;
            if (!welcomePlayed)
            {
                var welcomeAnnouncement = $"Welcome aboard the ship to {Parent.Route.Destination.Station.name}. Please keep in mind that smoking is prohibited on the ship. We wish you a pleasant journey.";
                _player.EnqueueWithSubtitles(announcer, welcomeAnnouncement, pack, signal, Assembly.IsPlayerMounted);
                _player.Delay(2);
                _welcomePlayed = true;
            }

            var nextAnnouncement = IsTerminus ? $"Next stop {Parent.Stop.Station.name}, where this ship terminates." : $"Next stop {Parent.Stop.Station.name}";
            _player.EnqueueWithSubtitles(announcer, nextAnnouncement, pack, welcomePlayed ? signal : null, Assembly.IsPlayerMounted);
            _player.Delay(3);
        }

        private void PlayCurrentStop()
        {
            if (IsTerminus)
            {
                var announcement = $"{Parent.Stop.Station.name}. This ship terminates here. Thank you for choosing SpaceTransit. Goodbye!";
                _player.EnqueueWithSubtitles(announcer, announcement, pack, signal, Assembly.IsPlayerMounted);
            }
            else
            {
                _player.Enqueue(signal.Clip, signal.Duration);
                _player.Enqueue(Parent.Stop.Station.Announcement);
            }

            PlayDoors();
            _player.Delay(3);
            _currentStopPlayed = true;
        }

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
            _player.EnqueueWithSubtitles(announcer, state == DoorsState.Left ? "Doors open on the left." : "Doors open on the right.", pack, Assembly.IsPlayerMounted);
        }

        public override void OnRouteChanged()
        {
            _welcomePlayed = Parent.Stop is not Origin;
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
