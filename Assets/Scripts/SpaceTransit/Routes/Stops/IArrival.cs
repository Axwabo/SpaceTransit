namespace SpaceTransit.Routes.Stops
{

    public interface IArrival : IStop
    {

        TimeOnly Arrival { get; }

    }

}
