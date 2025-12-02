namespace SpaceTransit.Routes.Stops
{

    public interface IDeparture : IStop
    {

        TimeOnly Departure { get; }

        StationId ExitTowards { get; }

    }

}
