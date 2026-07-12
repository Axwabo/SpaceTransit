using System;
using SpaceTransit.Routes.Stops;
using Unity.Properties;

namespace SpaceTransit.Menu
{

    [Serializable]
    public sealed record RouteStopItem(
        [property: CreateProperty] string Station,
        [property: CreateProperty] string Arrival,
        [property: CreateProperty] string Departure,
        [property: CreateProperty] string DockIndex
    )
    {

        public static implicit operator RouteStopItem(Stop stop) => new(
            stop.Station.name,
            (stop as IArrival)?.Arrival.ToString(),
            (stop as IDeparture)?.Departure.ToString(),
            (stop.DockIndex + 1).ToString()
        );

    }

}
