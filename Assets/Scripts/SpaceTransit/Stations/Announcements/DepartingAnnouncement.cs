using Katie.Unity;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DepartingAnnouncement : AnnouncementBase
    {

        private readonly VaulterController _vaulter;
        private readonly RouteDescriptor _route;
        private readonly IntermediateStop _stop;
        private readonly IKatilect _station;

        public DepartingAnnouncement(VaulterController vaulter, RouteDescriptor route, IntermediateStop stop, IKatilect station)
        {
            _vaulter = vaulter;
            _route = route;
            _stop = stop;
            _station = station;
            Priority = (int) route.Type + 10;
        }

        public override bool InterHub => _route.Type == ServiceType.InterHub;

        public override UpdateResult UpdateQueued() => _vaulter && _vaulter.Target == _stop ? UpdateResult.Ready : UpdateResult.Remove;

        public override void OnUtteranceStarting(ref PhrasePack pack)
        {
            var context = new AnnouncementContext<IDeparture>(_route, _stop, pack);
            FinalAnnouncement = _route.Katilect.Or(_station).Departing(ref context);
            pack = context.Pack;
        }

    }

}
