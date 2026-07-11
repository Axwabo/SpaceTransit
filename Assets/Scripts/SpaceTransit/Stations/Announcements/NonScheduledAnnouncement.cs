using SpaceTransit.Ships;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class NonScheduledAnnouncement : AnnouncementBase
    {

        private const int DefaultPriority = 100;

        public static NonScheduledAnnouncement Departing(ShipAssembly assembly, int dockIndex) => new(assembly, dockIndex, "departing from");

        public static NonScheduledAnnouncement Arriving(ShipAssembly assembly, int dockIndex) => new(assembly, dockIndex, "arriving");

        public static NonScheduledAnnouncement PassingThrough(ShipAssembly assembly, int dockIndex) => new(assembly, dockIndex, "passing through")
        {
            Priority = 90,
            PlayTwice = false
        };

        private readonly ShipAssembly _assembly;

        private NonScheduledAnnouncement(ShipAssembly assembly, int dockIndex, string action)
        {
            _assembly = assembly;
            FinalAnnouncement = $"A ship is {action} dock {dockIndex + 1}, please stand back from the platform edge.";
            Priority = DefaultPriority;
            PlayTwice = true;
        }

        public override UpdateResult UpdateQueued() => _assembly ? UpdateResult.PlayImmediately : UpdateResult.Remove;

    }

}
