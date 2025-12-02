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

        [SerializeField]
        private AudioClip at;

        [SerializeField]
        private AudioClip the;

        [SerializeField]
        private AudioClip to;

        [SerializeField]
        private AudioClip from;

        [Header("Common Phrases")]
        [SerializeField]
        private AudioClip passenger;

        [SerializeField]
        private AudioClip ship;

        [SerializeField]
        private AudioClip departsFor;

        [SerializeField]
        private AudioClip dock;

        [SerializeField]
        private AudioClip shipStop;

        [SerializeField]
        private AudioClip everyStation;

        [SerializeField]
        private AudioClip departing;

        [SerializeField]
        private AudioClip immediately;

        [SerializeField]
        private AudioClip stopBoarding;

        [Header("Numbers")]
        [SerializeField]
        private AudioClip[] numbersTo20;

        [SerializeField]
        private AudioClip oh;

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
                if (GetAnnouncement(route, index, departure) is not { } announcement)
                    continue;
                _announced[route] = departure.DepartureMinutes();
                foreach (var clip in announcement)
                    _queue.Enqueue(clip);
            }
        }

        private IEnumerable<AudioClip> GetAnnouncement(RouteDescriptor route, int index, IDeparture departure)
        {
            var remaining = departure.MinutesToDeparture();
            var announcementDelta = departure.DepartureMinutes() - _announced.GetValueOrDefault(route, ushort.MaxValue);
            return (remaining, announcementDelta) switch
            {
                (0, not 0) => Immediate(route),
                _ => null
            };
        }

        private IEnumerable<AudioClip> Immediate(RouteDescriptor route)
        {
            yield return the;
            yield return passenger;
            yield return ship;
            yield return to;
            yield return route.Destination.Station.Announcement;
            yield return departing;
            yield return from;
            yield return dock;
            foreach (var clip in Number(route.Origin.DockIndex + 1))
                yield return clip;
            yield return immediately;
            yield return stopBoarding;
        }

        private IEnumerable<AudioClip> Number(int n, bool padZero = false)
        {
            if (n is >= 1 and <= 20)
            {
                yield return numbersTo20[n - 1];
                yield break;
            }
            // TODO
        }

    }

}
