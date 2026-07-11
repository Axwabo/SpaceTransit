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
        private static readonly Predicate<AnnouncementBase> RemoveOnEnable = e => e is NonScheduledAnnouncement or IntermediateDepartingAnnouncement or RestartingAnnouncement or DepartureAnnouncement {CustomExpiry: true};

        [Header("Signals")]
        [SerializeField]
        private Signal genericSignal;

        [SerializeField]
        private Signal interHubSignal;

        [SerializeField]
        private PhrasePack pack;

        [SerializeField]
        private FormattingKatilect katilect;

        private QueuePlayer _queue;

        private DeparturesArrivals _cache;

        private AnnouncementBase _current;

        private readonly List<AnnouncementBase> _announcements = new();

        private readonly Dictionary<RouteDescriptor, int> _announced = new();

        private List<DepartureEntry> _departures;

        private List<ArrivalEntry> _arrivals;

        private int _previousDay;

        private string _name;

        private void Awake()
        {
            _queue = GetComponent<QueuePlayer>();
            _cache = GetComponentInParent<DeparturesArrivals>();
        }

        private void OnEnable()
        {
            _announcements.RemoveAll(RemoveOnEnable);
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
            _name = $"K.A.T.I.E. <color=#888>({_cache.StationId.name})</color>";
            EnqueueScheduledAnnouncements();
        }

        private void Update()
        {
            if (Clock.SecondsSinceLevelLoad < 0.5)
                return;
            if (_previousDay != Clock.Date.Day)
                EnqueueScheduledAnnouncements();
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
            foreach (var (route, index, arrival) in _arrivals)
            {
                var context = new AnnouncementContext<IArrival>(route, arrival, pack);
                if (arrival.Arrival.Value < Clock.Now
                    || katilect.GetAnnouncement(ref context, index, _announced.GetValueOrDefault(route, -1)) is not { } announcement)
                    continue;
                Announce(context, announcement);
                return;
            }
        }

        private void EnqueueScheduledAnnouncements()
        {
            var date = Clock.Date;
            var now = date.TimeOfDay;
            _previousDay = date.Day;
            foreach (var entry in _departures)
            {
                if (entry.Departure.Departure <= now)
                    continue;
                Enqueue(DepartureAnnouncement.In15(entry, katilect));
                Enqueue(DepartureAnnouncement.In10(entry, katilect));
                Enqueue(DepartureAnnouncement.In5(entry, katilect));
                Enqueue(DepartureAnnouncement.In3(entry, katilect));
                Enqueue(DepartureAnnouncement.Immediately(entry, katilect));
            }
        }

        private void Play(AnnouncementBase interrupt, AnnouncementBase previous)
        {
            _current = interrupt;
            _queue.Clear();
            if (interrupt == null)
                return;
            if (previous != null)
                _queue.Delay(2);
            var signal = interrupt.InterHub ? interHubSignal : genericSignal;
            var packToUse = pack;
            var announcement = interrupt.StartUtterance(ref packToUse);
            _queue.EnqueueWithSubtitles(_name, announcement, packToUse, signal);
            _queue.Delay(3);
            if (!interrupt.PlayTwice)
                return;
            _queue.EnqueueWithSubtitles(_name, announcement, packToUse);
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

        public void EnqueueRestarted(ShipController controller)
        {
            if (!controller.TryGetVaulter(out var vaulter) || !vaulter.IsInService || vaulter.Target.Station != _cache.StationId)
                return;
            foreach (var entry in _departures)
            {
                if (entry.Route != vaulter.Route)
                    continue;
                Enqueue(DepartureAnnouncement.AfterRestart(vaulter, entry, katilect));
                _announced.Remove(entry.Route);
                break;
            }
        }

    }

}
