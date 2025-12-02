using System;
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

        [Header("Conjunctions")]
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

        [Header("Ships")]
        [SerializeField]
        private AudioClip passenger;

        [SerializeField]
        private AudioClip fast;

        [SerializeField]
        private AudioClip interHub;

        [SerializeField]
        private AudioClip ship;

        [Header("Common Phrases")]
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
        private AudioClip onlyAt;

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
                _announced[route] = (int) Clock.Now.TotalMinutes;
                var inter = route.Type == ServiceType.InterHub;
                _queue.Enqueue(inter ? interHubSignal : genericSignal, inter ? interHubDuration : genericDuration);
                foreach (var clip in announcement)
                    _queue.Enqueue(clip);
            }
        }

        private IEnumerable<AudioClip> GetAnnouncement(RouteDescriptor route, int index, IDeparture departure)
        {
            if ((int) Clock.Now.TotalMinutes == _announced.GetValueOrDefault(route, -1))
                return null;
            var remaining = departure.MinutesToDeparture();
            return (remaining, index) switch
            {
                (3 or 5, -1) => In(remaining, route, departure),
                (1, -1) => Immediate(route, departure),
                (1, _) => Arriving(route, index, departure),
                _ => null
            };
        }

        private IEnumerable<AudioClip> In(int deltaMinutes, RouteDescriptor route, IDeparture departure) => new[]
        {
            the,
            Type(route),
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
            Type(route),
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

        private IEnumerable<AudioClip> Arriving(RouteDescriptor route, int index, IDeparture departure)
        {
            yield return Type(route);
            yield return ship;
            yield return arrivingFrom;
            yield return route.Origin.Station.Announcement;
            yield return at;
            yield return dock;
            yield return numbersTo20[departure.DockIndex];
            yield return and;
            yield return departsFor;
            yield return route.Destination.Station.Announcement;
            var intermediateStops = route.IntermediateStops.Count;
            if (index == intermediateStops - 1)
                yield break;
            yield return shipStop;
            if (route.EveryStation)
            {
                yield return everyStation;
                yield break;
            }

            yield return onlyAt;
            for (var i = index + 1; i < intermediateStops; i++)
            {
                if (i != index + 1 && i == intermediateStops - 1)
                    yield return and;
                yield return route.IntermediateStops[i].Station.Announcement;
            }
        }

        private AudioClip Type(RouteDescriptor route) => route.Type switch
        {
            ServiceType.Passenger => passenger,
            ServiceType.Fast => fast,
            ServiceType.InterHub => interHub,
            _ => throw new ArgumentOutOfRangeException()
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
