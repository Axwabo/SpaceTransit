using System;
using Katie.Unity;

namespace SpaceTransit.Stations.Announcements
{

    public abstract class AnnouncementBase : IComparable<AnnouncementBase>
    {

        protected static UpdateResult ReadyOrRemove(bool ready) => ready ? UpdateResult.Ready : UpdateResult.Remove;

        public int Priority { get; protected init; }

        public bool PlayTwice { get; protected init; }

        protected string FinalAnnouncement { get; init; }

        public virtual bool InterHub => false;

        public abstract UpdateResult UpdateQueued();

        public virtual string StartUtterance(ref PhrasePack pack) => FinalAnnouncement;

        public virtual int CompareTo(AnnouncementBase other) => ReferenceEquals(this, other)
            ? 0
            : other is null
                ? 1
                : Priority == other.Priority
                    ? CompareEqualPriority(other)
                    : Priority.CompareTo(other.Priority);

        protected virtual int CompareEqualPriority(AnnouncementBase other) => 0;

    }

}
