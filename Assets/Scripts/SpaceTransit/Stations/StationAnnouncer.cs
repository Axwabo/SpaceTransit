using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Audio;
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

        private readonly Dictionary<RouteDescriptor, int> _announced = new();

        private List<DepartureEntry> _departures;

        private void Awake() => _queue = GetComponent<QueuePlayer>();

        private void Start()
        {
            var cache = GetComponentInParent<DeparturesArrivals>();
            _departures = cache.Departures.OrderBy(static e => e.Departure.Departure.Value.TotalMinutes)
                .ThenBy(static e => e.Route.Type)
                .ToList();
        }

        private void Update()
        {
            foreach (var (route, index, departure) in _departures)
            {
                if (departure.Departure.Value < Clock.Now
                    || announcementCreator.GetAnnouncement(route, index, departure, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                _announced[route] = (int) Clock.Now.TotalMinutes;
                var inter = route.Type == ServiceType.InterHub;
                _queue.Enqueue(inter ? interHubSignal : genericSignal, inter ? interHubDuration : genericDuration);
                foreach (var clip in announcement)
                    _queue.Enqueue(clip.Clip, clip.Length);
                _queue.Delay(3);
            }
        }

    }

}
