#nullable enable

using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;

namespace SpaceTransit.Stations.Announcements
{

    public ref struct AnnouncementContext<T> where T : IStop
    {

        public readonly RouteDescriptor Route;

        public readonly T Stop;

        public PhrasePack Pack;

        public ServiceType Type => Route.Type;

        public int Dock => Stop.DockIndex + 1;

        public string Origin => Route.Origin.Station.name;

        public string Destination => Route.Destination.Station.name;

        public AnnouncementContext(RouteDescriptor route, T stop, PhrasePack pack)
        {
            Route = route;
            Stop = stop;
            Pack = pack;
        }

    }

}
