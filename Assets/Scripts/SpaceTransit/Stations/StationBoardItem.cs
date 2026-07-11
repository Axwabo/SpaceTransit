using System;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Vaulter;
using Unity.Properties;

namespace SpaceTransit.Stations
{

    [Serializable]
    public sealed record StationBoardItem(
        TimeOnly TimeOfDay,
        [property: CreateProperty] string Type,
        [property: CreateProperty] string Time,
        [property: CreateProperty] string Station,
        [property: CreateProperty] string Dock
    )
    {

        public static implicit operator StationBoardItem(DepartureEntry entry) => new(
            entry.Time,
            entry.Route.Type.ToStringFast(),
            entry.Time.ToString(),
            entry.Route.Destination.Station.name,
            DockToString(entry.Departure)
        );

        public static implicit operator StationBoardItem(ArrivalEntry entry) => new(
            entry.Time,
            entry.Route.Type.ToStringFast(),
            entry.Time.ToString(),
            entry.Route.Origin.Station.name,
            DockToString(entry.Arrival)
        );

        private static string DockToString(IStop stop) => (stop.DockIndex + 1).ToString();

    }

}
