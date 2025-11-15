using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(QueuePlayer))]
    public sealed class OnboardAnnouncer : MonoBehaviour
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

        private VaulterController _controller;

        private QueuePlayer _player;

        private bool _wasSailing;

        private float _delay;

        private bool _welcomePlayed;

        private bool _currentStopPlayed;

        private bool IsNearStation => Station.TryGetLoadedStation(_controller.Stop.Station, out var station)
                                      && Vector3.Distance(
                                          station.Docks[_controller.Stop.DockIndex].transform.position,
                                          _controller.Assembly.FrontModule.transform.position
                                      ) < 100; // TODO: optimize position getters

        private bool IsTerminus => _controller.Stop == _controller.Route.Destination;

        private void Awake()
        {
            _controller = GetComponentInParent<VaulterController>();
            _player = GetComponent<QueuePlayer>();
        }

        private void Update()
        {
            if (_controller.Controller.State != ShipState.Sailing || !_controller.IsInService)
            {
                _wasSailing = false;
                return;
            }

            if (!_wasSailing && !_controller.Assembly.IsStationary())
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
            _player.Enqueue(signal, 3);
            if (!_welcomePlayed)
            {
                _player.Enqueue(welcomeStart);
                _player.Enqueue(_controller.Route.Destination.Station.Announcement);
                _player.Enqueue(welcomeEnd);
                _welcomePlayed = true;
            }

            _player.Enqueue(nextStop);
            _player.Enqueue(_controller.Stop.Station.Announcement);
            if (IsTerminus)
                _player.Enqueue(nextTerminus);
        }

        private void PlayCurrentStop()
        {
            _player.Enqueue(_controller.Stop.Station.Announcement);
            if (IsTerminus)
                _player.Enqueue(goodbye);
            _currentStopPlayed = true;
        }

    }

}
