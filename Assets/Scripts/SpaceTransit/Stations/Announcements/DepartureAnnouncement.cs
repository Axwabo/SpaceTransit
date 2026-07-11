using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DepartureAnnouncement : ScheduledAnnouncementBase<IDeparture>
    {

        private const int Restart = int.MaxValue;

        public static DepartureAnnouncement In15(DepartureEntry entry, IKatilect station) => new(entry, 15, 1, station);

        public static DepartureAnnouncement In10(DepartureEntry entry, IKatilect station) => new(entry, 10, 9, station);

        public static DepartureAnnouncement In5(DepartureEntry entry, IKatilect station) => new(entry, 5, 2, station);

        public static DepartureAnnouncement In3(DepartureEntry entry, IKatilect station) => new(entry, 3, 2, station);

        public static DepartureAnnouncement Immediately(DepartureEntry entry, IKatilect station) => new(entry, 3, 1, station);

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
            _index = entry.Index;
            Priority = Priorities.Departing(entry.Route.Type);
        }

        protected override string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<IDeparture> context) => MinuteMark switch
        {
            Restart when Stop.Departure.Value <= Clock.Now => katilect.Departing(ref context),
            1 => katilect.DepartingImmediately(ref context),
            < 10 => katilect.DepartingIn(ref context, (Stop.Departure.Value - Clock.Now).Minutes + 1),
            _ => katilect.DepartsFor(ref context, _index)
        };

    }

}
