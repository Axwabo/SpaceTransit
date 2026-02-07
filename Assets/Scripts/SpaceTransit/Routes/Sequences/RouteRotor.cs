using System;
using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using SpaceTransit.Vaulter;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceTransit.Routes.Sequences
{

    public sealed class RouteRotor
    {

        public ServiceSequence Sequence { get; }

        public RouteRotor(ServiceSequence sequence) => Sequence = sequence;

        public bool Destroyed { get; private set; }

        private RouteDescriptor _initial;

        private int _startingStop;

        private Entry _entry;

        private TubeBase _tube;

        private int _index;

        private VaulterController _ship;

        private float _delay;

        private State _state;

        private TimeSpan _at;

        private int _day;

        private bool CompletedRoute => _ship.Stop is Destination && _ship.Parent.State == ShipState.Docked && _ship.Assembly.IsStationary() && !_ship.Assembly.IsManuallyDriven;

        public void Initialize()
        {
            _day = Clock.Date.Day;
            for (_index = 0; _index < Sequence.routes.Length; _index++)
            {
                var route = Sequence.routes[_index];
                if (route.Origin.Departure > Clock.Now && route.Origin.Station.IsLoaded())
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

            var destination = Sequence.routes[^1].Destination;
            if (!Station.TryGetLoadedStation(destination.Station, out var station))
            {
                Destroyed = true;
                return;
            }

            _ship = Object.Instantiate(Sequence.prefab, World.Current);
            _ship.GetComponent<ShipAssembly>().startTube = station.Docks[destination.DockIndex];
            _state = State.Completed;
        }

        private static bool ShouldSpawn(IntermediateStop stop)
            => stop.Station.IsLoaded()
               && stop.Arrival.Value - TimeSpan.FromMinutes(1) >= Clock.Now
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
            if (!Station.TryGetLoadedStation(stop.Station, out var station))
                throw new MissingComponentException($"Entry station {stop.Station.name} is not loaded");
            var dock = station.Docks[stop.DockIndex];
            var entries = route.Reverse ? dock.FrontEntries : dock.BackEntries;
            if (entries.Length == 0)
            {
                _tube = dock.Next(!route.Reverse).Next(!route.Reverse);
                _at = arrival;
                return;
            }

            _entry = entries[0];
            foreach (var entry in entries)
            {
                if (entry.Connected != stop.ArriveFrom)
                    continue;
                _entry = entry;
                break;
            }

            _tube = FindPreviousTube(route.Reverse);
            if (_tube)
            {
                _at = arrival;
                return;
            }

            _entry = null;
            Spawn(route);
            _ship.initialStopIndex = index;
        }

        private TubeBase FindPreviousTube(bool reverse)
        {
            if (!_entry || !_entry.Ensurer)
                return null;
            var tube = _entry.Ensurer.Tube;
            return !tube ? null : reverse ? tube.Next : tube;
        }

        private void Spawn(RouteDescriptor route)
        {
            _ship = Object.Instantiate(Sequence.prefab, World.Current);
            _ship.initialRoute = route;
            _state = State.Sailing;
        }

        private void SpawnAtEntry()
        {
            if (_entry && !_entry.IsFree || _tube.Safety is LockBasedSafety {IsFree: false})
                return;
            Spawn(Sequence.routes[_index]);
            var assembly = _ship.GetComponent<ShipAssembly>();
            assembly.startTube = _tube;
            _ship.initialStopIndex = _startingStop;
            _state = State.Ready;
            if (_tube.Safety is LockBasedSafety lockBasedSafety)
                lockBasedSafety.Claim(assembly);
            if (!_entry)
                return;
            foreach (var remapper in _entry.Remappers)
                remapper.Remap();
        }

        public void Update()
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
                    _at = Sequence.routes[0].Origin.Departure.Value - TimeSpan.FromHours(1);
                    break;
                case State.WaitingForNextDay:
                    if (_ship.Assembly.IsManuallyDriven || _day == Clock.Date.Day)
                        break;
                    _ship.BeginRoute(Sequence.routes[0]);
                    _state = State.Sailing;
                    break;
            }
        }

        private void WaitForNext()
        {
            if (Unload() || !CompletedRoute)
                return;
            _at = Clock.Now + TimeSpan.FromMinutes(1);
            _state = State.Rotating;
        }

        private bool Unload()
        {
            if (LoadingProgress.Current != null)
                return false;
            if (!_ship)
            {
                Destroyed = true;
                return true;
            }

            if (_ship.Assembly.IsPlayerMounted
                || _ship.IsInService && _ship.Stop.Station.IsLoaded()
                || _ship.Assembly.FrontModule.Thruster.Tube is Dock dock && dock.Station.ID.IsLoaded())
                return false;
            Destroyed = true;
            Object.Destroy(_ship.gameObject);
            return true;
        }

        private void Cycle()
        {
            if (++_index >= Sequence.routes.Length)
            {
                _state = State.Completed;
                return;
            }

            _state = State.Sailing;
            _ship.BeginRoute(Sequence.routes[_index]);
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
