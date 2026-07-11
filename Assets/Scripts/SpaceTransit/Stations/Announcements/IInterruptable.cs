namespace SpaceTransit.Stations.Announcements
{

    public interface IInterruptable
    {

        bool ShouldBeInterruptedBy(AnnouncementBase other);

    }

}
