using System;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
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
            for (var i = 0; i < routes.Length; i++)
            {
                var route = routes[i];
                if (route.Origin.Departure > Clock.Now)
                {
                    Spawn(route);
                    return;
                }

                _index = i;
                for (var j = 0; j < route.IntermediateStops.Length; j++)
                {
                    var stop = route.IntermediateStops[j];
                    if (stop.Departure < Clock.Now)
                        continue;
                    SpawnAt(route, stop, j);
                    return;
                }
            }

            Destroy(this);
        }

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
            foreach (var entry in route.Reverse ? dock.FrontEntries : dock.BackEntries)
            {
                // TODO
            }
        }

        private void Spawn(RouteDescriptor route)
        {
            _ship = Instantiate(prefab, World.Current);
            _ship.initialRoute = route;
            _state = State.Sailing;
        }

        private void SpawnAtEntry()
        {
        }

        private void Update()
        {
            if (Clock.Now > _at)
                return;
            switch (_state)
            {
                case State.Waiting:
                    SpawnAtEntry();
                    break;
                case State.Sailing:
                    if (!CompletedRoute)
                        break;
                    _at = Clock.Now + TimeSpan.FromMinutes(1);
                    _state = State.Rotating;
                    break;
                case State.Rotating:
                    Cycle();
                    break;
                case State.Completed:
                    enabled = false;
                    break;
            }
        }

        private void Cycle()
        {
            if (++_index >= routes.Length)
            {
                _state = State.Completed;
                return;
            }

            _state = State.Completed;
            _ship.BeginRoute(routes[_index]);
        }

        private enum State
        {

            Waiting,
            Sailing,
            Rotating,
            Completed

        }

    }

}
