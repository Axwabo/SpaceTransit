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

        [Header("Signals")]
        [SerializeField]
        private Signal genericSignal;

        [SerializeField]
        private Signal interHubSignal;

        [SerializeField]
        private PhrasePack pack;

        private QueuePlayer _queue;

        private DeparturesArrivals _cache;

        private readonly Dictionary<RouteDescriptor, int> _announced = new();

        private readonly List<(ShipAssembly, int)> _departing = new();

        private readonly List<(ShipAssembly, int)> _passingThrough = new();

        private readonly List<(ShipAssembly, int)> _arriving = new();

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
            _departing.Clear();
            _passingThrough.Clear();
            _arriving.Clear();
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
            if (_queue.IsYapping || AnnounceDeparting() || AnnouncePassingThrough() || AnnounceArriving() || AnnounceRestarting() || AnnounceRestarted())
                return;
            if (_arrivedShips.TryDequeue(out var tuple) && tuple.Vaulter.Stop?.Station == _cache.StationId)
            {
                var context = new AnnouncementContext<IDeparture>(tuple.Route, tuple.Stop, pack);
                var announcement = tuple.Route.Katilect.Or(katilect).Departing(ref context);
                Announce(context, announcement);
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

        private void OnDisable()
        {
            _queue.Clear();
            _passingThrough.Clear();
        }

        private void Announce<T>(AnnouncementContext<T> context, string announcement) where T : IStop
        {
            _announced[context.Route] = (int) Clock.Now.TotalMinutes;
            var inter = context.Type == ServiceType.InterHub;
            var signal = inter ? interHubSignal : genericSignal;
            _queue.EnqueueWithSubtitles(_name, announcement, context.Pack, signal);
            _queue.Delay(3);
        }

        private bool AnnounceDeparting() => AnnounceTwice(_departing, "departing from");

        private bool AnnouncePassingThrough()
        {
            _passingThrough.RemoveAll(static e => !e.Item1);
            if (_passingThrough.Count == 0)
                return false;
            _queue.EnqueueWithSubtitles(_name, $"A ship is passing through dock {_passingThrough[0].Item2 + 1}, please stand back from the platform edge.", pack, genericSignal);
            _queue.Delay(1);
            _passingThrough.RemoveAt(0);
            return true;
        }

        private bool AnnounceArriving() => AnnounceTwice(_arriving, "arriving at");

        private bool AnnounceTwice(List<(ShipAssembly, int)> list, string action)
        {
            list.RemoveAll(static e => !e.Item1);
            if (list.Count == 0)
                return false;
            var announcement = $"A ship is {action} dock {list[0].Item2 + 1}, please stand back from the platform edge.";
            _queue.EnqueueWithSubtitles(_name, announcement, pack, genericSignal);
            _queue.Delay(3);
            _queue.EnqueueWithSubtitles(_name, announcement, pack);
            _queue.Delay(1);
            list.RemoveAt(0);
            return true;
        }

        private bool AnnounceRestarting()
        {
            _restarting.RemoveAll(static e => !e.Item1);
            if (_restarting.Count == 0)
                return false;
            var ship = _restarting[0].Item1;
            _restarting.RemoveAt(0);
            if (!ship.IsRestarting)
                return false;
            _queue.EnqueueWithSubtitles(_name, $"The assembly on dock {_restarting[0].Item2 + 1} is being restarted. Please do not board yet.", pack, genericSignal);
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

        public void EnqueueDeparting(ShipAssembly assembly, int dockIndex) => _departing.Add((assembly, dockIndex));

        public void EnqueuePassingThrough(ShipAssembly assembly, int dockIndex) => _passingThrough.Add((assembly, dockIndex));

        public void EnqueueArriving(ShipAssembly assembly, int dockIndex) => _arriving.Add((assembly, dockIndex));

        public void EnqueueArrived(VaulterController assembly, RouteDescriptor route, Stop stop)
        {
            if (stop is IntermediateStop intermediate && intermediate.Arrival.Value != intermediate.Departure.Value)
                _arrivedShips.Enqueue((assembly, route, intermediate));
        }

        public void EnqueueRestarting(ShipController controller, int dockIndex) => _restarting.Add((controller, dockIndex));

        public void EnqueueRestarted(ShipController controller, int dockIndex) => _restarted.Add((controller, dockIndex));

    }

}
