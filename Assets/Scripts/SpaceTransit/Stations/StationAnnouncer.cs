using System.Collections.Generic;
using System.Linq;
using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Stations
{

    [RequireComponent(typeof(QueuePlayer))]
    public sealed class StationAnnouncer : MonoBehaviour
    {

        [Header("Signals")]
        [SerializeField]
        private AudioClip genericSignal;

        [SerializeField]
        private float genericDuration;

        [SerializeField]
        private AudioClip interHubSignal;

        [SerializeField]
        private float interHubDuration;

        [SerializeField]
        private AnnouncementCreator announcementCreator;

        private QueuePlayer _queue;

        private DeparturesArrivals _cache;

        private readonly Dictionary<RouteDescriptor, int> _announced = new();

        private List<DepartureEntry> _departures;

        private List<ArrivalEntry> _arrivals;

        private void Awake()
        {
            _queue = GetComponent<QueuePlayer>();
            _cache = GetComponentInParent<DeparturesArrivals>();
        }

        private void Start()
        {
            _departures = _cache.Departures
                .Where(static e => e.Index == -1)
                .OrderBy(static e => e.Departure.Departure.Value.TotalMinutes)
                .ThenByDescending(static e => e.Route.Type)
                .ToList();
            _arrivals = _cache.Arrivals
                .OrderBy(static e => e.Arrival.Arrival.Value.TotalMinutes)
                .ThenByDescending(static e => e.Route.Type)
                .ToList();
            _queue.Delay(0.5f);
        }

        private void Update()
        {
            if (_queue.IsYapping)
                return;
            foreach (var (route, index, arrival) in _arrivals)
            {
                if (arrival.Arrival.Value < Clock.Now
                    || announcementCreator.GetAnnouncement(route, index, arrival, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(route, announcement);
                break;
            }

            foreach (var (route, index, departure) in _departures)
            {
                if (departure.Departure.Value < Clock.Now
                    || announcementCreator.GetAnnouncement(route, index, departure, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(route, announcement);
                return;
            }
        }

        private void OnDisable() => _queue.Clear();

        private void Announce(RouteDescriptor route, IEnumerable<AnnouncementClip> announcement)
        {
            _announced[route] = (int) Clock.Now.TotalMinutes;
            var inter = route.Type == ServiceType.InterHub;
            _queue.Enqueue(inter ? interHubSignal : genericSignal, inter ? interHubDuration : genericDuration);
            foreach (var clip in announcement)
                _queue.Enqueue(clip.Clip, clip.Length);
            _queue.Delay(3);
        }

    }

}
