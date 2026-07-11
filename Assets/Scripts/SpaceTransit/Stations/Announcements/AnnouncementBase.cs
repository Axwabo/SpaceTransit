using Katie.Unity;

namespace SpaceTransit.Stations.Announcements
{

    public abstract class AnnouncementBase
    {

        protected static UpdateResult ReadyOrRemove(bool ready) => ready ? UpdateResult.Ready : UpdateResult.Remove;

        public int Priority { get; protected init; }

        public bool PlayTwice { get; protected init; }

        protected string FinalAnnouncement { get; init; }

        public virtual bool InterHub => false;

        public abstract UpdateResult UpdateQueued();

        public virtual string StartUtterance(ref PhrasePack pack) => FinalAnnouncement;

    }

}
