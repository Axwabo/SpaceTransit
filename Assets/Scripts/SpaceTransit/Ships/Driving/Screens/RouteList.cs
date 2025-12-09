using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class RouteList : ListBase<RouteDescriptor, PickerBase>
    {

        private static readonly HashSet<string> AvailableExits = new();

        private bool _enabled;

        protected override List<RouteDescriptor> Source { get; } = new();

        private void OnEnable()
        {
            if (!_enabled)
            {
                _enabled = true;
                return;
            }

            Source.Clear();
            Source.Add(null);
            Clear();
            if (Assembly.FrontModule.Thruster.Tube is not Dock dock)
                return;
            var stationName = dock.Station.name;
            CacheExits(dock.Station);
            foreach (var route in Cache.Routes)
                if (route.Origin.Station.name == stationName && AvailableExits.Contains(route.Origin.ExitTowards.name))
                    Source.Add(route);
            SetUp();
        }

        private static void CacheExits(Station station)
        {
            AvailableExits.Clear();
            foreach (var dock in station.Docks)
            {
                foreach (var exit in dock.FrontExits)
                    AvailableExits.Add(exit.ConnectedStation.Name);
                foreach (var exit in dock.BackExits)
                    AvailableExits.Add(exit.ConnectedStation.Name);
            }
        }

        protected override string GetContent(RouteDescriptor item)
            => item == null ? "Exit Service" : RouteDisplay.Format(item);

        protected override bool Select(RouteDescriptor item, PickerBase picker)
        {
            if (!Controller.TryGetVaulter(out var vaulter))
                return false;
            if (item)
                vaulter.BeginRoute(item);
            else
                vaulter.ExitService();
            return true;
        }

    }

}
