using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;

namespace SpaceTransit.Stations.Announcements.Implementations
{

    public abstract class StopAnnouncementBase<T> : AnnouncementBase where T : IStop
    {

        private readonly IKatilect _station;

        protected RouteDescriptor Route { get; }

        public T Stop { get; }

        protected StopAnnouncementBase(RouteDescriptor route, T stop, IKatilect station)
        {
            _station = station;
            Route = route;
            Stop = stop;
        }

        public sealed override bool InterHub => Route.Type == ServiceType.InterHub;

        public sealed override string StartUtterance(ref PhrasePack pack)
        {
            var context = new AnnouncementContext<T>(Route, Stop, pack);
            var announcement = BuildAnnouncement(Route.Katilect.Or(_station), ref context);
            pack = context.Pack;
            return announcement;
        }

        protected abstract string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<T> context);

    }

}
