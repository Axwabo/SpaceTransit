using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DepartingAnnouncement : ScheduledAnnouncementBase<IDeparture>
    {

        private readonly VaulterController _vaulter;

        public DepartingAnnouncement(VaulterController vaulter, RouteDescriptor route, IntermediateStop stop, IKatilect station) : base(route, stop, station)
        {
            _vaulter = vaulter;
            Priority = (int) route.Type + 10;
        }

        public override UpdateResult UpdateQueued() => _vaulter && _vaulter.Target == Stop ? UpdateResult.Ready : UpdateResult.Remove;

        protected override string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<IDeparture> context) => katilect.Departing(ref context);

    }

}
