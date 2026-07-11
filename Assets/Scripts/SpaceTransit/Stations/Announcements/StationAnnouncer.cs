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

        private readonly List<(ShipController, int)> _restarting = new();

        private readonly List<(ShipController, int)> _restarted = new();

        private readonly Queue<(VaulterController Vaulter, RouteDescriptor Route, IntermediateStop Stop)> _arrivedShips = new();

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
            _announcements.RemoveAll(e => e is NonScheduledAnnouncement);
            _arrivedShips.Clear();
            _restarting.Clear();
            _restarted.Clear();
        }

        private void Start()
        {
            _cache.Station.Announcer = this;
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
            var previous = _current;
            var interrupt = UpdateQueue();
            if (interrupt != previous)
            {
                Play(interrupt);
                return;
            }

            if (_queue.IsYapping)
                return;
            _current = null;
            if (AnnounceRestarting() || AnnounceRestarted())
                return;
            if (_arrivedShips.TryDequeue(out var tuple) && tuple.Vaulter.Stop?.Station == _cache.StationId)
            {
                var context = new AnnouncementContext<IDeparture>(tuple.Route, tuple.Stop, pack);
                var announcement = tuple.Route.Katilect.Or(katilect).Departing(ref context);
                Announce(context, announcement); // TODO "the Gyuard station is departing from" LIKE HOW
                return;
            }

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

        private void Play(AnnouncementBase interrupt)
        {
            if (interrupt == null)
            {
                _queue.Clear();
                return;
            }

            var signal = interrupt.InterHub ? interHubSignal : genericSignal;
            var packToUse = pack;
            interrupt.OnUtteranceStarting(ref packToUse);
            _queue.EnqueueWithSubtitles(_name, interrupt.FinalAnnouncement, packToUse, signal);
            if (!interrupt.PlayTwice)
                return;
            _queue.Delay(3);
            _queue.EnqueueWithSubtitles(_name, interrupt.FinalAnnouncement, packToUse);
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
                    case (UpdateResult.PersistentReady, null):
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

        private bool AnnounceRestarting()
        {
            _restarting.RemoveAll(static e => !e.Item1);
            if (_restarting.Count == 0)
                return false;
            var (ship, dock) = _restarting[0];
            _restarting.RemoveAt(0);
            if (!ship.IsRestarting)
                return false;
            _queue.EnqueueWithSubtitles(_name, $"The assembly on dock {dock + 1} is being restarted. Please do not board yet.", pack, genericSignal);
            _queue.Delay(1);
            return true;
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
                _arrivedShips.Enqueue((assembly, route, intermediate));
        }

        public void EnqueueRestarting(ShipController controller, int dockIndex) => _restarting.Add((controller, dockIndex));

        public void EnqueueRestarted(ShipController controller, int dockIndex) => _restarted.Add((controller, dockIndex));

    }

}
