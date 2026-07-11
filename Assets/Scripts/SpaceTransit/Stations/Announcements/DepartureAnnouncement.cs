using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DepartureAnnouncement : ScheduledAnnouncementBase<IDeparture>
    {

        private const int Restart = int.MaxValue;

        private readonly int _minuteMark;

        public static DepartureAnnouncement In15(DepartureEntry entry, IKatilect station) => new(entry, 15, 1, station);

        public static DepartureAnnouncement In10(DepartureEntry entry, IKatilect station) => new(entry, 10, 9, station);

        public static DepartureAnnouncement In5(DepartureEntry entry, IKatilect station) => new(entry, 5, 1, station);

        public static DepartureAnnouncement In3(DepartureEntry entry, IKatilect station) => new(entry, 3, 1, station);

        public static DepartureAnnouncement AfterRestart(VaulterController vaulter, DepartureEntry entry, IKatilect station)
            => new(entry, Restart, 0, station)
            {
                Cancellation = vaulter.destroyCancellationToken,
                Priority = Priorities.IntermediateDeparting(entry.Route.Type)
            };

        private readonly int _index;

        private DepartureAnnouncement(DepartureEntry entry, int minuteMark, int expiryMinutes, IKatilect station)
            : base(entry.Route, entry.Departure, minuteMark, expiryMinutes, station)
        {
            _minuteMark = minuteMark;
            _index = entry.Index;
            Priority = Priorities.Departing(entry.Route.Type);
        }

        protected override string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<IDeparture> context) => _minuteMark switch
        {
            Restart when Stop.Departure.Value <= Clock.Now => katilect.Departing(ref context),
            5 or 3 => katilect.DepartingIn(ref context, _minuteMark),
            _ => katilect.DepartsFor(ref context, _index)
        };

    }

}
