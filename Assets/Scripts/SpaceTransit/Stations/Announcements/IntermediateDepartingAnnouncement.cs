using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class IntermediateDepartingAnnouncement : ScheduledAnnouncementBase<IDeparture>
    {

        private readonly VaulterController _vaulter;

        public IntermediateDepartingAnnouncement(VaulterController vaulter, RouteDescriptor route, IntermediateStop stop, IKatilect station) : base(route, stop, station)
        {
            _vaulter = vaulter;
            Priority = Priorities.IntermediateDeparting(route.Type);
        }

        public override UpdateResult UpdateQueued() => ReadyOrRemove(_vaulter && _vaulter.Target == Stop);

        protected override string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<IDeparture> context) => katilect.Departing(ref context);

    }

}
