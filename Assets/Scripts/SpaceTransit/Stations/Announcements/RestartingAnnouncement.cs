using SpaceTransit.Ships;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class RestartingAnnouncement : AnnouncementBase
    {

        private readonly ShipController _controller;

        public RestartingAnnouncement(ShipController controller, int dockIndex)
        {
            _controller = controller;
            Priority = Priorities.Restarting;
            FinalAnnouncement = $"The assembly on dock {dockIndex + 1} is being restarted. Please do not board yet.";
        }

        public override UpdateResult UpdateQueued() => ReadyOrRemove(_controller && _controller.IsRestarting);

    }

}
