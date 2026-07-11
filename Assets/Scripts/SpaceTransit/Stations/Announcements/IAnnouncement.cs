namespace SpaceTransit.Stations.Announcements
{

    public interface IAnnouncement
    {

        int Priority { get; }

        string PlayedAnnouncement { get; }

        bool PlayTwice => false;

        UpdateResult UpdateQueued();

    }

}
