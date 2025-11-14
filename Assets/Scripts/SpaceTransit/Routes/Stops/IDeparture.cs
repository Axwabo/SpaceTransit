namespace SpaceTransit.Routes.Stops
{

    public interface IDeparture
    {

        TimeOnly Departure { get; }

        StationId ExitTowards { get; }

    }

}
