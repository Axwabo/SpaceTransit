using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
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

        [SerializeField]
        private RouteDescriptor initialRoute;

        public RouteDescriptor Route { get; private set; }

        public Stop Stop { get; private set; }

        protected override void OnInitialized()
        {
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
        }

        public void BeginRoute(RouteDescriptor descriptor)
        {
            Route = descriptor;
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
        }

    }

}
