using SpaceTransit.Tubes;

namespace SpaceTransit
{

    public static class TubeExtensions
    {

        public static TubeBase Next(this TubeBase current, bool back) => back ? current.Previous : current.Next;

    }

}
