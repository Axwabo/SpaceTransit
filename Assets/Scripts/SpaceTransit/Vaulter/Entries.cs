using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;

namespace SpaceTransit.Vaulter
{

    public abstract record StopEntry(RouteDescriptor Route, int Index, TimeOnly Time);

    public sealed record DepartureEntry(RouteDescriptor Route, int Index, IDeparture Departure) : StopEntry(Route, Index, Departure.Departure);

    public sealed record ArrivalEntry(RouteDescriptor Route, int Index, IArrival Arrival) : StopEntry(Route, Index, Arrival.Arrival);

}
