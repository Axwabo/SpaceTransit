using System;
using System.Collections.Generic;
using System.Linq;
using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Stations.Announcements
{

    [RequireComponent(typeof(QueuePlayer))]
    public sealed class StationAnnouncer : MonoBehaviour
    {

        private static readonly Comparison<AnnouncementBase> PriorityComparison = (a, b) => b.Priority - a.Priority;
        private static readonly Predicate<AnnouncementBase> RemoveOnEnable = e => e is NonScheduledAnnouncement or IntermediateDepartingAnnouncement or RestartingAnnouncement;

        [Header("Signals")]
        [SerializeField]
        private Signal genericSignal;

        [SerializeField]
        private Signal interHubSignal;

        [SerializeField]
        private PhrasePack pack;

        private QueuePlayer _queue;

        private DeparturesArrivals _cache;

        private AnnouncementBase _current;

        private readonly List<AnnouncementBase> _announcements = new();

        private readonly Dictionary<RouteDescriptor, int> _announced = new();

        private readonly List<(ShipController, int)> _restarted = new();

        private List<DepartureEntry> _departures;

        private List<ArrivalEntry> _arrivals;

        private string _name;

        [SerializeField]
        private FormattingKatilect katilect;

        private void Awake()
        {
            _queue = GetComponent<QueuePlayer>();
            _cache = GetComponentInParent<DeparturesArrivals>();
        }

        private void OnEnable()
        {
            _announcements.RemoveAll(RemoveOnEnable);
            _restarted.Clear();
        }

        private void Start()
        {
            _cache.Station.Announcer = this;
            _departures = _cache.Departures
                .Where(e => e.Index == -1)
                .OrderBy(e => e.Departure.Departure.Value.TotalMinutes)
                .ToList();
            _arrivals = _cache.Arrivals
                .OrderBy(e => e.Arrival.Arrival.Value.TotalMinutes)
                .ToList();
            _queue.Delay(0.5f);
            _name = $"K.A.T.I.E. <color=#888>({_cache.StationId.name})</color>";
        }

        private void Update()
        {
            var previous = _current;
            var interrupt = UpdateQueue();
            if (interrupt != previous)
            {
                Play(interrupt, previous);
                return;
            }

            if (_queue.IsYapping)
                return;
            _current = null;
            if (AnnounceRestarted())
                return;
            foreach (var (route, index, arrival) in _arrivals)
            {
                var context = new AnnouncementContext<IArrival>(route, arrival, pack);
                if (arrival.Arrival.Value < Clock.Now
                    || katilect.GetAnnouncement(ref context, index, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(context, announcement);
                return;
            }

            foreach (var (route, index, departure) in _departures)
            {
                var context = new AnnouncementContext<IDeparture>(route, departure, pack);
                if (departure.Departure.Value < Clock.Now
                    || katilect.GetAnnouncement(ref context, index, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(context, announcement);
                return;
            }
        }

        private void Play(AnnouncementBase interrupt, AnnouncementBase previous)
        {
            _queue.Clear();
            if (interrupt == null)
                return;
            if (previous != null)
                _queue.Delay(2);
            var signal = interrupt.InterHub ? interHubSignal : genericSignal;
            var packToUse = pack;
            interrupt.OnUtteranceStarting(ref packToUse);
            _queue.EnqueueWithSubtitles(_name, interrupt.FinalAnnouncement, packToUse, signal);
            _queue.Delay(3);
            if (!interrupt.PlayTwice)
                return;
            _queue.EnqueueWithSubtitles(_name, interrupt.FinalAnnouncement, packToUse);
            _queue.Delay(1);
        }

        private AnnouncementBase UpdateQueue()
        {
            var interrupt = _current;
            _announcements.Sort(PriorityComparison);
            for (var i = 0; i < _announcements.Count; i++)
            {
                var announcement = _announcements[i];
                switch (announcement.UpdateQueued(), interrupt)
                {
                    case (UpdateResult.Idle, _):
                        break;
                    case (UpdateResult.Remove, _):
                        _announcements.RemoveAt(i--);
                        break;
                    case (UpdateResult.PlayImmediately, {Priority: var priority}) when priority < announcement.Priority:
                        interrupt = announcement;
                        _announcements[i] = _current;
                        _queue.Clear();
                        break;
                    case (UpdateResult.Ready or UpdateResult.PlayImmediately, null):
                        _announcements.RemoveAt(i--);
                        interrupt = announcement;
                        break;
                }
            }

            return interrupt;
        }

        private void OnDisable()
        {
            _queue.Clear();
            _current = null;
        }

        private void Announce<T>(AnnouncementContext<T> context, string announcement) where T : IStop
        {
            _announced[context.Route] = (int) Clock.Now.TotalMinutes;
            var inter = context.Type == ServiceType.InterHub;
            var signal = inter ? interHubSignal : genericSignal;
            _queue.EnqueueWithSubtitles(_name, announcement, context.Pack, signal);
            _queue.Delay(3);
        }

        private bool AnnounceRestarted()
        {
            _restarted.RemoveAll(static e => !e.Item1);
            if (_restarted.Count == 0)
                return false;
            var ship = _restarted[0].Item1;
            _restarted.RemoveAt(0);
            if (!ship.TryGetVaulter(out var vaulter) || !vaulter.IsInService || vaulter.Target.Station != _cache.StationId)
                return false;
            foreach (var (route, index, departure) in _departures)
            {
                if (route != vaulter.Route)
                    continue;
                var context = new AnnouncementContext<IDeparture>(route, departure, pack);
                var targetKatilect = route.Katilect.Or(katilect);
                var announcement = departure.Departure < Clock.Now
                    ? targetKatilect.Departing(ref context)
                    : targetKatilect.DepartsFor(ref context, index);
                Announce(context, announcement);
                _announced.Remove(route);
                return true;
            }

            return false;
        }

        private void Enqueue(AnnouncementBase announcement) => _announcements.Add(announcement);

        public void EnqueueDeparting(ShipAssembly assembly, int dockIndex) => Enqueue(NonScheduledAnnouncement.Departing(assembly, dockIndex));

        public void EnqueuePassingThrough(ShipAssembly assembly, int dockIndex) => Enqueue(NonScheduledAnnouncement.PassingThrough(assembly, dockIndex));

        public void EnqueueArriving(ShipAssembly assembly, int dockIndex) => Enqueue(NonScheduledAnnouncement.Arriving(assembly, dockIndex));

        public void EnqueueArrived(VaulterController assembly, RouteDescriptor route, Stop stop)
        {
            if (stop is IntermediateStop intermediate && intermediate.Arrival.Value != intermediate.Departure.Value)
                Enqueue(new IntermediateDepartingAnnouncement(assembly, route, intermediate, katilect));
        }

        public void EnqueueRestarting(ShipController controller, int dockIndex) => Enqueue(new RestartingAnnouncement(controller, dockIndex));

        public void EnqueueRestarted(ShipController controller, int dockIndex) => _restarted.Add((controller, dockIndex));

    }

}
