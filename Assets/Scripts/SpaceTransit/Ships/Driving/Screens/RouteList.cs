using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class RouteList : ListBase<RouteDescriptor, PickerBase>
    {

        private readonly List<RouteDescriptor> _routes = new();

        private bool _enabled;

        protected override IReadOnlyList<RouteDescriptor> Source => _routes;

        private void OnEnable()
        {
            if (!_enabled)
            {
                _enabled = true;
                return;
            }

            _routes.Clear();
            _routes.Add(null);
            var station = Assembly.FrontModule.Thruster.Tube.GetComponentInParent<Station>();
            if (!station)
                return;
            foreach (var route in Cache.Routes)
                if (route.Origin.Station.Name == station.Name)
                    _routes.Add(route);
            Clear();
            SetUp();
        }

        protected override string GetContent(int index, RouteDescriptor item)
            => item == null
                ? "Exit Service"
                : $"{item.name} {item.Origin.Station.Name} {item.Origin.Departure.Value:hh':'mm} - {item.Destination.Station.Name} {item.Destination.Arrival.Value:hh':'mm}";

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
