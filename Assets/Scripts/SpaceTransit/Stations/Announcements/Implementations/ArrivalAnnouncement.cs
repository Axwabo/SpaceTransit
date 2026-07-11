using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations.Announcements.Implementations
{

    public sealed class ArrivalAnnouncement : ScheduledAnnouncementBase<IArrival>, IInterruptable
    {

        private readonly int _index;

        public ArrivalAnnouncement(ArrivalEntry entry, int minuteMark, IKatilect station)
            : base(entry.Route, entry.Arrival, minuteMark, station)
            => _index = entry.Index;

        protected override bool ShipExists => true;

        protected override string BuildAnnouncement(IKatilect katilect, ref AnnouncementContext<IArrival> context) => _index == -1
            ? katilect.Arriving(ref context)
            : katilect.ArrivingAndDepartsFor(ref context, _index);

        public bool ShouldBeInterruptedBy(AnnouncementBase other) => other is IntermediateDepartingAnnouncement departing && departing.Stop == Stop;

    }

}
