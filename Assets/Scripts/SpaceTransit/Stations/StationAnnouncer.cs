using System;
using System.Collections.Generic;
using SpaceTransit.Audio;
using SpaceTransit.Routes;
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

        private readonly HashSet<RouteDescriptor> _alreadyAnnounced = new();

        private void Awake() => _queue = GetComponent<QueuePlayer>();

        private void Update()
        {
            var nextMinute = Clock.Now + TimeSpan.FromMinutes(1);
            foreach (var route in World.Routes)
            {
                if (route.Origin.Departure.Value > nextMinute || !_alreadyAnnounced.Add(route))
                    continue;
                _queue.Enqueue(the); // TODO
                _queue.Enqueue(passenger); // TODO
                _queue.Enqueue(ship);
                _queue.Enqueue(to);
                // _queue.Enqueue(departsFor);
                _queue.Enqueue(route.Destination.Station.Announcement);
                _queue.Enqueue(departing);
                _queue.Enqueue(from);
                _queue.Enqueue(dock);
                EnqueueNumber(route.Origin.DockIndex + 1);
                _queue.Enqueue(immediately);
                _queue.Enqueue(stopBoarding);
                // _queue.Enqueue(at);
            }
        }

        private void EnqueueNumber(int n, bool padZero = false)
        {
            if (n is >= 1 and <= 20)
            {
                _queue.Enqueue(numbersTo20[n - 1]);
                return;
            }
            // TODO
        }

    }

}
