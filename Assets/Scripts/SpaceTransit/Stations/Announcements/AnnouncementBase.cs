namespace SpaceTransit.Stations.Announcements
{

    public abstract class AnnouncementBase
    {

        public int Priority { get; protected init; }

        public bool PlayTwice { get; protected init; }

        public string FinalAnnouncement { get; protected set; }

        public abstract UpdateResult UpdateQueued();

    }

}
