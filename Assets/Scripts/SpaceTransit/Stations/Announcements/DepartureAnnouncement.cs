using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DepartureAnnouncement : ScheduledAnnouncementBase<IDeparture>
    {

        public static DepartureAnnouncement In15(DepartureEntry entry, IKatilect station) => new(entry, 15, 1, station);

        public static DepartureAnnouncement In10(DepartureEntry entry, IKatilect station) => new(entry, 10, 9, station);

        private readonly int _index;

        private DepartureAnnouncement(DepartureEntry entry, int minuteMark, int expiryMinutes, IKatilect station)
            : base(entry.Route, entry.Departure, minuteMark, expiryMinutes, station)
            => _index = entry.Index;

        protected override string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<IDeparture> context)
            => katilect.DepartsFor(ref context, _index);

    }

}
