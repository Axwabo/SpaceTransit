using SpaceTransit.Cosmos;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit
{

    public static class TubeExtensions
    {

        public static TubeBase Next(this TubeBase current, bool back) => back ? current.Previous : current.Next;

        public static bool TryGetEntryEnsurer(this TubeBase current, bool reverse, out IEntryEnsurer ensurer)
        {
            if (current.Safety is MonoBehaviour thisBehavior and IEntryEnsurer {Backwards: var thisBackwards} thisEntry && thisBackwards == reverse)
            {
                ensurer = thisEntry;
                return thisBehavior;
            }

            if ((reverse ? current.HasPrevious : current.HasNext)
                && current.Next(reverse).Safety is MonoBehaviour nextBehavior and IEntryEnsurer {Backwards: var nextBackwards} nextEntry
                && nextBackwards == reverse)
            {
                ensurer = nextEntry;
                return nextBehavior;
            }

            ensurer = null;
            return false;
        }

    }

}
