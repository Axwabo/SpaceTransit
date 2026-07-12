using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit
{

    public static class RouteExtensions
    {

        public static int MinutesToDeparture(this IDeparture departure) => (int) (departure.Departure.Value - Clock.Now).TotalMinutes + 1;

        public static int MinutesToArrival(this IArrival arrival) => (int) (arrival.Arrival.Value - Clock.Now).TotalMinutes + 1;

        public static string ToStringFast(this ServiceType serviceType) => serviceType switch
        {
            ServiceType.Passenger => nameof(ServiceType.Passenger),
            ServiceType.Fast => nameof(ServiceType.Fast),
            ServiceType.InterHub => nameof(ServiceType.InterHub),
            _ => "???"
        };

        public static string Summary(this RouteDescriptor route)
            => $"{route.Origin.Station.name} {route.Origin.Departure} - {route.Destination.Station.name} {route.Destination.Arrival}";

        public static (string, Color) GetAbbreviation(this RouteDescriptor route) => route?.Type switch
        {
            ServiceType.Fast => ("F", Color.orangeRed),
            ServiceType.InterHub => ("IH", Color.darkBlue),
            ServiceType.Passenger => ("P", Color.deepSkyBlue),
            _ => ("-", Color.gray)
        };

        public static int StopIndex(this RouteDescriptor route, StationId id)
        {
            if (route.Origin.Station == id)
                return ITarget.Origin;
            if (route.Destination.Station == id)
                return ITarget.Destination;
            var stops = route.IntermediateStops;
            for (var i = 0; i < stops.Length; i++)
                if (stops[i].Station == id)
                    return i;
            throw new KeyNotFoundException($"{id} is not a part of {route}");
        }

    }

}
