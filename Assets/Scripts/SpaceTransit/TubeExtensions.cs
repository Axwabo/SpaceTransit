using SpaceTransit.Cosmos;
using SpaceTransit.Tubes;

namespace SpaceTransit
{

    public static class TubeExtensions
    {

        public static TubeBase Next(this TubeBase current, bool back) => back ? current.Previous : current.Next;

        public static bool TryGetEntryEnsurer(this TubeBase current, bool reverse, out EntryEnsurer ensurer)
        {
            if (current.Safety is EntryEnsurer {Backwards: var thisBackwards} thisEntry && thisBackwards == reverse)
            {
                ensurer = thisEntry;
                return true;
            }

            if ((reverse ? current.HasPrevious : current.HasNext)
                && current.Next(reverse).Safety is EntryEnsurer {Backwards: var nextBackwards} nextEntry
                && nextBackwards == reverse)
            {
                ensurer = nextEntry;
                return true;
            }

            ensurer = null;
            return false;
        }

    }

}
