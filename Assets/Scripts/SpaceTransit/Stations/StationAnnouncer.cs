using System.Collections.Generic;
using System.Linq;
using Katie.Unity;
using SpaceTransit.Menu;
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
        private Signal genericSignal;

        [SerializeField]
        private Signal interHubSignal;

        [SerializeField]
        private PhrasePack pack;

        private QueuePlayer _queue;

        private DeparturesArrivals _cache;

        private readonly Dictionary<RouteDescriptor, int> _announced = new();

        private List<DepartureEntry> _departures;

        private List<ArrivalEntry> _arrivals;

        private string _name;

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
            _name = $"K.A.T.I.E. <color=#888>({_cache.StationId.name})</color>";
        }

        private void Update()
        {
            if (_queue.IsYapping)
                return;
            foreach (var (route, index, arrival) in _arrivals)
            {
                if (arrival.Arrival.Value < Clock.Now
                    || AnnouncementCreator.GetAnnouncement(route, index, arrival, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(route, announcement);
                break;
            }

            foreach (var (route, index, departure) in _departures)
            {
                if (departure.Departure.Value < Clock.Now
                    || AnnouncementCreator.GetAnnouncement(route, index, departure, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(route, announcement);
                return;
            }
        }

        private void OnDisable() => _queue.Clear();

        private void Announce(RouteDescriptor route, string announcement)
        {
            _announced[route] = (int) Clock.Now.TotalMinutes;
            var inter = route.Type == ServiceType.InterHub;
            var signal = inter ? interHubSignal : genericSignal;
            var duration = _queue.EnqueueAnnouncement(announcement, pack, signal);
            _queue.Delay(3);
            KatieSubtitleList.Add(_name, announcement, signal.Duration, duration.TotalSeconds);
        }

    }

}
