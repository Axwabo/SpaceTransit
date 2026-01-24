using System;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class RouteRotor : MonoBehaviour
    {

        [SerializeField]
        private VaulterController prefab;

        [SerializeField]
        public RouteDescriptor[] routes;

        private RouteDescriptor _initial;

        private int _startingStop;

        private Entry _entry;

        private TubeBase _tube;

        private int _index;

        private VaulterController _ship;

        private float _delay;

        private State _state;

        private TimeSpan _at;

        private bool CompletedRoute => _ship.Stop is Destination && _ship.Parent.State == ShipState.Docked && _ship.Assembly.IsStationary() && !_ship.Assembly.IsManuallyDriven;

        private void Awake()
        {
            if (routes.Length == 0)
                Destroy(this);
        }

        private void Start()
        {
            for (_index = 0; _index < routes.Length; _index++)
            {
                var route = routes[_index];
                if (route.Origin.Departure > Clock.Now)
                {
                    Spawn(route);
                    return;
                }

                for (var j = 0; j < route.IntermediateStops.Length; j++)
                {
                    var stop = route.IntermediateStops[j];
                    if (!ShouldSpawn(stop))
                        continue;
                    SpawnAt(route, stop, j);
                    return;
                }
            }

            var destination = routes[^1].Destination;
            if (!Station.TryGetLoadedStation(destination.Station, out var station))
            {
                Destroy(this);
                return;
            }

            _ship = Instantiate(prefab, World.Current);
            _ship.GetComponent<ShipAssembly>().startTube = station.Docks[destination.DockIndex];
            _state = State.Completed;
            enabled = false;
        }

        private static bool ShouldSpawn(IntermediateStop stop)
            => stop.Arrival.Value - TimeSpan.FromMinutes(1) >= Clock.Now
               && stop.Departure.Value >= Clock.Now + TimeSpan.FromMinutes(stop.MinStayMinutes + 1);

        private void SpawnAt(RouteDescriptor route, IntermediateStop stop, int index)
        {
            var arrival = stop.Arrival.Value - TimeSpan.FromMinutes(1);
            if (arrival < Clock.Now)
            {
                Spawn(route);
                _ship.initialStopIndex = index;
                return;
            }

            _startingStop = index;
            _at = arrival;
            if (!Station.TryGetLoadedStation(stop.Station, out var station))
                throw new MissingComponentException($"Entry station {stop.Station.name} is not loaded");
            var dock = station.Docks[stop.DockIndex];
            var entries = route.Reverse ? dock.FrontEntries : dock.BackEntries;
            if (entries.Length == 0)
            {
                _tube = dock.Next(!route.Reverse).Next(!route.Reverse);
                return;
            }

            _entry = entries[0];
            foreach (var entry in entries)
            {
                if (entry.ConnectedStation.ID != stop.ArriveFrom)
                    continue;
                _entry = entry;
                break;
            }

            _tube = FindPreviousTube(route.Reverse);
        }

        private TubeBase FindPreviousTube(bool reverse)
        {
            var tube = _entry.Ensurer.Tube;
            return reverse ? tube.Next : tube;
        }

        private void Spawn(RouteDescriptor route)
        {
            _ship = Instantiate(prefab, World.Current);
            _ship.initialRoute = route;
            _state = State.Sailing;
        }

        private void SpawnAtEntry()
        {
            if (_entry && !_entry.IsFree)
                return;
            Spawn(routes[_index]);
            _ship.initialStopIndex = _startingStop;
            _ship.GetComponent<ShipAssembly>().startTube = _tube;
            _state = State.Ready;
            if (!_entry)
                return;
            foreach (var remapper in _entry.Remappers)
                remapper.Remap();
        }

        private void Update()
        {
            if (Clock.Now < _at)
                return;
            switch (_state)
            {
                case State.Waiting:
                    SpawnAtEntry();
                    break;
                case State.Ready:
                    if (_entry)
                        _entry.Lock(_ship.Assembly);
                    _ship.Parent.MarkReady();
                    _state = State.Sailing;
                    break;
                case State.Sailing:
                    WaitForNext();
                    break;
                case State.Rotating:
                    Cycle();
                    break;
                case State.Completed:
                    _ship.ExitService();
                    _state = State.WaitingForNextDay;
                    _at = routes[0].Origin.Departure.Value - TimeSpan.FromHours(1);
                    break;
                case State.WaitingForNextDay:
                    if (_ship.Assembly.IsManuallyDriven)
                        break;
                    _ship.BeginRoute(routes[0]);
                    _state = State.Sailing;
                    break;
            }
        }

        private void WaitForNext()
        {
            if (!CompletedRoute)
                return;
            _at = Clock.Now + TimeSpan.FromMinutes(1);
            _state = State.Rotating;
        }

        private void Cycle()
        {
            if (++_index >= routes.Length)
            {
                _state = State.Completed;
                return;
            }

            _state = State.Sailing;
            _ship.BeginRoute(routes[_index]);
        }

        private enum State
        {

            Waiting,
            Ready,
            Sailing,
            Rotating,
            Completed,
            WaitingForNextDay

        }

    }

}
