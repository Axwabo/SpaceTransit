using System.Collections.Generic;
using System.Linq;
using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
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

        private readonly Queue<(VaulterController, RouteDescriptor, IntermediateStop)> _arrivedShips = new();

        private List<DepartureEntry> _departures;

        private List<ArrivalEntry> _arrivals;

        private string _name;

        private IKatilect _katilect;

        private void Awake()
        {
            _queue = GetComponent<QueuePlayer>();
            _cache = GetComponentInParent<DeparturesArrivals>();
            _katilect = IKatilect.Default;
        }

        private void OnEnable()
        {
            _departing.Clear();
            _passingThrough.Clear();
            _arriving.Clear();
            _arrivedShips.Clear();
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
            if (_queue.IsYapping || AnnounceDeparting() || AnnouncePassingThrough() || AnnounceArriving())
                return;
            if (_arrivedShips.TryDequeue(out var tuple) && tuple.Item1.Stop?.Station == _cache.StationId)
            {
                Announce(tuple.Item2, _katilect.Departing(tuple.Item2, tuple.Item3));
                return;
            }

            foreach (var (route, index, arrival) in _arrivals)
            {
                if (arrival.Arrival.Value < Clock.Now
                    || _katilect.GetAnnouncement(route, index, arrival, _announced.GetValueOrDefault(route, -1)) is not ({ } announcement))
                    continue;
                Announce(route, announcement);
                return;
            }

            foreach (var (route, index, departure) in _departures)
            {
                if (departure.Departure.Value < Clock.Now
                    || _katilect.GetAnnouncement(route, index, departure, _announced.GetValueOrDefault(route, -1)) is not ({ } announcement))
                    continue;
                Announce(route, announcement);
                return;
            }
        }

        private void OnDisable()
        {
            _queue.Clear();
            _passingThrough.Clear();
        }

        private void Announce(RouteDescriptor route, string announcement)
        {
            _announced[route] = (int) Clock.Now.TotalMinutes;
            var inter = route.Type == ServiceType.InterHub;
            var signal = inter ? interHubSignal : genericSignal;
            _queue.EnqueueWithSubtitles(_name, announcement, pack, signal);
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

        public void EnqueueDeparting(ShipAssembly assembly, int dockIndex) => _departing.Add((assembly, dockIndex));

        public void EnqueuePassingThrough(ShipAssembly assembly, int dockIndex) => _passingThrough.Add((assembly, dockIndex));

        public void EnqueueArriving(ShipAssembly assembly, int dockIndex) => _arriving.Add((assembly, dockIndex));

        public void EnqueueArrived(VaulterController assembly, RouteDescriptor route, Stop stop)
        {
            if (stop is IntermediateStop intermediate && intermediate.Arrival.Value != intermediate.Departure.Value)
                _arrivedShips.Enqueue((assembly, route, intermediate));
        }

    }

}
