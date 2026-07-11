using SpaceTransit.Ships;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class NonScheduledAnnouncement : AnnouncementBase
    {

        public static NonScheduledAnnouncement Departing(ShipAssembly assembly, int dockIndex) => new(assembly, dockIndex, "departing from");

        public static NonScheduledAnnouncement Arriving(ShipAssembly assembly, int dockIndex) => new(assembly, dockIndex, "arriving at");

        public static NonScheduledAnnouncement PassingThrough(ShipAssembly assembly, int dockIndex) => new(assembly, dockIndex, "passing through")
        {
            Priority = Priorities.PassingThrough,
            PlayTwice = false
        };

        private readonly ShipAssembly _assembly;

        private NonScheduledAnnouncement(ShipAssembly assembly, int dockIndex, string action)
        {
            _assembly = assembly;
            PlayTwice = true;
            Priority = Priorities.NonScheduled;
            FinalAnnouncement = $"A ship is {action} dock {dockIndex + 1}, please stand back from the platform edge.";
        }

        public override UpdateResult UpdateQueued() => _assembly ? UpdateResult.PlayImmediately : UpdateResult.Remove;

    }

}
