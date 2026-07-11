using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;

namespace SpaceTransit.Stations.Announcements
{

    public abstract class StopAnnouncementBase<T> : AnnouncementBase where T : IStop
    {

        private readonly IKatilect _station;

        protected RouteDescriptor Route { get; }

        protected T Stop { get; }

        protected StopAnnouncementBase(RouteDescriptor route, T stop, IKatilect station)
        {
            _station = station;
            Route = route;
            Stop = stop;
        }

        public sealed override bool InterHub => Route.Type == ServiceType.InterHub;

        public sealed override void OnUtteranceStarting(ref PhrasePack pack)
        {
            var context = new AnnouncementContext<T>(Route, Stop, pack);
            FinalAnnouncement = BuildAnnouncement(Route.Katilect.Or(_station), ref context);
            pack = context.Pack;
        }

        protected abstract string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<T> context);

    }

}
