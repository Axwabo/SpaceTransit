namespace SpaceTransit.Stations.Announcements
{

    public sealed class DepartingAnnouncement : IAnnouncement
    {

        private readonly int _dockIndex;

        public DepartingAnnouncement(int dockIndex)
        {
            _dockIndex = dockIndex;
        }

        public int Priority => 100;

        public string PlayedAnnouncement => $"A ship is departing from dock {_dockIndex + 1}, please stand back from the platform edge.";

        public bool PlayTwice => true;

        public UpdateResult UpdateQueued() => UpdateResult.PlayImmediately;

    }

}
