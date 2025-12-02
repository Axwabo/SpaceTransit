using SpaceTransit.Routes.Stops;

namespace SpaceTransit
{

    public static class RouteExtensions
    {

        public static int MinutesToDeparture(this IDeparture departure) => (int) (departure.Departure.Value - Clock.Now).TotalMinutes;

        public static int DepartureMinutes(this IDeparture departure) => (int) departure.Departure.Value.TotalMinutes;

    }

}
