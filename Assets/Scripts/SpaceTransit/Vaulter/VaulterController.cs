using System;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterController : ShipComponentBase
    {

        private const int OutOfService = -2;
        private const int Origin = -1;
        private const int Destination = int.MaxValue;

        private int _stopIndex = OutOfService;

        private VaulterComponentBase[] _components;

        [SerializeField]
        public RouteDescriptor initialRoute;

        [SerializeField]
        public int initialStopIndex = Origin;

        public RouteDescriptor Route { get; private set; }

        public Stop Stop { get; private set; }

        public bool IsInService => _stopIndex != OutOfService;

        public ReadOnlySpan<IntermediateStop> NextIntermediateStops => Stop is Destination
            ? ReadOnlySpan<IntermediateStop>.Empty
            : Route.IntermediateStops[(_stopIndex + 1)..];

        protected override void Awake() => _components = GetComponentsInChildren<VaulterComponentBase>(true);

        protected override void OnInitialized()
        {
            foreach (var component in _components)
                component.Initialize(this);
            if (!initialRoute)
                return;
            Stop origin = initialStopIndex == Origin ? initialRoute.Origin : initialRoute.IntermediateStops[initialStopIndex];
            if (!Station.TryGetLoadedStation(origin.Station, out var station))
                return;
            if (!Assembly.startTube)
                Assembly.startTube = station.Docks[origin.DockIndex];
            BeginRoute(initialRoute, initialStopIndex);
        }

        public void ExitService()
        {
            if (!IsInService)
                return;
            _stopIndex = OutOfService;
            Route = null;
            Stop = null;
            NotifyRouteChanged();
        }

        public void BeginRoute(RouteDescriptor descriptor, int stopIndex = Origin)
        {
            Route = descriptor;
            Assembly.Reverse = descriptor.Reverse;
            UpdateStop(stopIndex);
            NotifyRouteChanged();
        }

        private void UpdateStop(int index)
        {
            _stopIndex = index;
            Stop = index switch
            {
                OutOfService => null,
                Origin => Route.Origin,
                Destination => Route.Destination,
                _ => Route.IntermediateStops[index]
            };
            foreach (var component in _components)
                component.OnStopChanged();
        }

        public override void OnStateChanged(ShipState previousState)
        {
            if (_stopIndex is not (OutOfService or Destination)
                && Parent.State == ShipState.Sailing
                && previousState == ShipState.LiftingOff
                && Assembly.FrontModule.Thruster.Tube is Dock {Station: var station} && station.ID == Stop?.Station)
                UpdateStop(_stopIndex >= Route.IntermediateStops.Length - 1 ? Destination : _stopIndex + 1);
        }

        private void NotifyRouteChanged()
        {
            foreach (var component in _components)
                component.OnRouteChanged();
        }

    }

}
