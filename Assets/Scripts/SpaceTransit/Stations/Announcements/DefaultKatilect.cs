using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using static SpaceTransit.Stations.Announcements.AnnouncementCreator;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DefaultKatilect : IKatilect
    {

        private static string DepartingCore(RouteDescriptor route, IDeparture departure)
            => $"The {route.Type} ship to {route.Destination.Station.name} is departing from dock {departure.DockIndex + 1}";

        public string DepartsFor(RouteDescriptor route, int stopIndex, IDeparture departure) => Departs(route, stopIndex, departure);

        public string DepartingIn(int minutes, RouteDescriptor route, IDeparture departure)
            => $"The {route.Type} ship to {route.Destination.Station.name} is departing from dock {departure.DockIndex + 1} in {minutes} minutes. {PleaseBoard}";

        public string DepartingImmediately(RouteDescriptor route, IDeparture departure)
            => $"{DepartingCore(route, departure)} immediately. Please stop boarding.";

        public string Departing(RouteDescriptor route, IDeparture departure)
            => $"{DepartingCore(route, departure)}. {PleaseBoard}";

        public string ArrivingAndDepartsFor(RouteDescriptor route, int stopIndex, IArrival arrival) => ArrivingAndDeparts(route, stopIndex, arrival);

        public string Arriving(RouteDescriptor route, IArrival arrival)
            => $"{route.Type} ship is arriving from {route.Origin.Station.name} at dock {arrival.DockIndex + 1}.";

    }

}
