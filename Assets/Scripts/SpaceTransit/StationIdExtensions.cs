using SpaceTransit.Routes;

namespace SpaceTransit
{

    public static class StationIdExtensions
    {

        public static bool IsLoaded(this StationId stationId)
        {
            foreach (var line in stationId.Lines)
                if (World.IsLoaded(line))
                    return true;
            return false;
        }

    }

}
