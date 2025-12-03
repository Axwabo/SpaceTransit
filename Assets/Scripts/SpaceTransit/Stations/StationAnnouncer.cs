using System.Collections.Generic;
using SpaceTransit.Audio;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
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

        private readonly List<(RouteDescriptor, int, IDeparture)> _applicableRoutes = new();

        private void Awake() => _queue = GetComponent<QueuePlayer>();

        private void Start()
        {
            var id = GetComponentInParent<Station>().ID;
            foreach (var route in World.Routes)
            {
                if (route.Origin.Station == id)
                {
                    _applicableRoutes.Add((route, -1, route.Origin));
                    continue;
                }

                for (var i = 0; i < route.IntermediateStops.Count; i++)
                {
                    var stop = route.IntermediateStops[i];
                    if (stop.Station != id)
                        break;
                    _applicableRoutes.Add((route, i, stop));
                }
            }
        }

        private void Update()
        {
            foreach (var (route, index, departure) in _applicableRoutes)
            {
                if (departure.Departure.Value > Clock.Now
                    || announcementCreator.GetAnnouncement(route, index, departure, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                _announced[route] = (int) Clock.Now.TotalMinutes;
                var inter = route.Type == ServiceType.InterHub;
                _queue.Enqueue(inter ? interHubSignal : genericSignal, inter ? interHubDuration : genericDuration);
                foreach (var clip in announcement)
                    _queue.Enqueue(clip);
            }
        }

    }

}
