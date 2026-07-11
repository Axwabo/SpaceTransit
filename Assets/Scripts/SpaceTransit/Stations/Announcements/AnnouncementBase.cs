using Katie.Unity;

namespace SpaceTransit.Stations.Announcements
{

    public abstract class AnnouncementBase
    {

        public int Priority { get; protected init; }

        public bool PlayTwice { get; protected init; }

        public string FinalAnnouncement { get; protected set; }

        public virtual bool InterHub => false;

        public abstract UpdateResult UpdateQueued();

        protected UpdateResult ReadyOrRemove(bool ready) => ready ? UpdateResult.Ready : UpdateResult.Remove;

        public virtual void OnUtteranceStarting(ref PhrasePack pack)
        {
        }

    }

}
