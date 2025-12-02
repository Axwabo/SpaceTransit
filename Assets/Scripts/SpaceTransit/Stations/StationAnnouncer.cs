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

        [SerializeField]
        private AudioClip @in;

        [SerializeField]
        private AudioClip and;

        [Header("Common Phrases")]
        [SerializeField]
        private AudioClip passenger;

        [SerializeField]
        private AudioClip ship;

        [SerializeField]
        private AudioClip departsFor;

        [SerializeField]
        private AudioClip arrivingFrom;

        [SerializeField]
        private AudioClip dock;

        [SerializeField]
        private AudioClip shipStop;

        [SerializeField]
        private AudioClip everyStation;

        [SerializeField]
        private AudioClip departing;

        [SerializeField]
        private AudioClip pleaseBoard;

        [SerializeField]
        private AudioClip minutes;

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
            var announcementDelta = departure.DepartureMinutes() - _announced.GetValueOrDefault(route, ushort.MaxValue);
            if (announcementDelta == 0)
                return null;
            var remaining = departure.MinutesToDeparture();
            return (remaining, index) switch
            {
                (1, -1) => Immediate(route, departure),
                (3 or 5, -1) => In(remaining, route, departure),
                (1, _) => Arriving(route, index, departure),
                _ => null
            };
        }

        private IEnumerable<AudioClip> In(int deltaMinutes, RouteDescriptor route, IDeparture departure) => new[]
        {
            the,
            passenger,
            ship,
            to,
            route.Destination.Station.Announcement,
            departing,
            from,
            dock,
            numbersTo20[departure.DockIndex],
            @in,
            numbersTo20[deltaMinutes - 1],
            minutes,
            pleaseBoard
        };

        private IEnumerable<AudioClip> Immediate(RouteDescriptor route, IDeparture departure) => new[]
        {
            the,
            passenger,
            ship,
            to,
            route.Destination.Station.Announcement,
            departing,
            from,
            dock,
            numbersTo20[departure.DockIndex],
            immediately,
            stopBoarding
        };

        private IEnumerable<AudioClip> Arriving(RouteDescriptor route, int index, IDeparture departure) => new[]
        {
            passenger,
            ship,
            arrivingFrom,
            route.Origin.Station.Announcement,
            at,
            dock,
            numbersTo20[departure.DockIndex],
            and,
            departsFor,
            route.Destination.Station.Announcement,
            shipStop,
            (route.EveryStation ? everyStation : null) //TODO,
        };

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
