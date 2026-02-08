using SpaceTransit.Routes;

namespace SpaceTransit
{

    public static class StationIdExtensions
    {

        public static bool IsLoaded(this StationId stationId) => Station.TryGetLoadedStation(stationId, out _);

    }

}
