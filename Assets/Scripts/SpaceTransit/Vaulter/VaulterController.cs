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
        private RouteDescriptor initialRoute;

        public RouteDescriptor Route { get; private set; }

        public Stop Stop { get; private set; }

        public bool IsInService => _stopIndex != OutOfService;

        protected override void Awake() => _components = GetComponentsInChildren<VaulterComponentBase>();

        protected override void OnInitialized()
        {
            foreach (var component in _components)
                component.Initialize(this);
            if (!initialRoute || !Station.TryGetLoadedStation(initialRoute.Origin.Station, out var station))
                return;
            Assembly.startTube = station.Docks[initialRoute.Origin.DockIndex].Tube;
            BeginRoute(initialRoute);
        }

        public void ExitService()
        {
            _stopIndex = OutOfService;
            Route = null;
            Stop = null;
            NotifyRouteChanged();
        }

        public void BeginRoute(RouteDescriptor descriptor)
        {
            Route = descriptor;
            NotifyRouteChanged();
            UpdateStop(Origin);
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
            if (_stopIndex is not (OutOfService or Destination) && Parent.State == ShipState.Sailing && previousState == ShipState.LiftingOff)
                UpdateStop(_stopIndex >= Route.IntermediateStops.Count - 1 ? Destination : _stopIndex + 1);
        }

        private void NotifyRouteChanged()
        {
            foreach (var component in _components)
                component.OnRouteChanged();
        }

    }

}
