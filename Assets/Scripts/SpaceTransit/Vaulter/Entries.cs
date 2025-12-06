using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;

namespace SpaceTransit.Stations
{

    public sealed record DepartureEntry(RouteDescriptor Route, int Index, IDeparture Departure);

    public sealed record ArrivalEntry(RouteDescriptor Route, int Index, IArrival Departure);

}
