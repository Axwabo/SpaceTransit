using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;

namespace SpaceTransit.Stations.Announcements
{

    public interface IKatilect
    {

        public static IKatilect Default { get; } = new DefaultKatilect();

        string DepartsFor(RouteDescriptor route, int stopIndex, IDeparture departure);

        string DepartingIn(int minutes, RouteDescriptor route, IDeparture departure);

        string DepartingImmediately(RouteDescriptor route, IDeparture departure);

        string Departing(RouteDescriptor route, IDeparture departure);

        string ArrivingAndDepartsFor(RouteDescriptor route, int stopIndex, IArrival arrival);

        string Arriving(RouteDescriptor route, IArrival arrival);

    }

}
