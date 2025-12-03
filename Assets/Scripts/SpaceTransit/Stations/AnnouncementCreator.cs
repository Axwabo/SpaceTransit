using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Stations
{

    [CreateAssetMenu(fileName = "Station", menuName = "SpaceTransit/Announcement Creator", order = 0)]
    public sealed class AnnouncementCreator : ScriptableObject
    {

        private static readonly AnnouncementClip FullStop = 0.5f;

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
        private AudioClip twenty;

        [SerializeField]
        private AudioClip thirty;

        [SerializeField]
        private AudioClip forty;

        [SerializeField]
        private AudioClip fifty;

        [SerializeField]
        private AudioClip oh;

        public IEnumerable<AnnouncementClip> GetAnnouncement(RouteDescriptor route, int index, IDeparture departure, int lastAnnounced)
        {
            if ((int) Clock.Now.TotalMinutes == lastAnnounced)
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

        private IEnumerable<AnnouncementClip> In(int deltaMinutes, RouteDescriptor route, IDeparture departure) => new[]
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
            FullStop,
            pleaseBoard
        };

        private IEnumerable<AnnouncementClip> Immediate(RouteDescriptor route, IDeparture departure) => new[]
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
            FullStop,
            stopBoarding
        };

        private IEnumerable<AnnouncementClip> Arriving(RouteDescriptor route, int index, IDeparture departure)
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
            yield return FullStop;
            yield return shipStop;
            if (route.EveryStation)
            {
                yield return everyStation;
                yield break;
            }

            yield return onlyAt;
            for (var i = index + 1; i < intermediateStops; i++)
            {
                yield return 0.3f;
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
                if (padZero && n < 10)
                    yield return oh;
                yield return numbersTo20[n - 1];
                yield break;
            }

            yield return n switch
            {
                < 30 => twenty,
                < 40 => thirty,
                < 50 => forty,
                < 60 => fifty,
                _ => throw new ArgumentOutOfRangeException(nameof(n), "Cannot parse numbers < 0 or >= 60")
            };
            yield return numbersTo20[n % 10 - 1];
        }

    }

}
