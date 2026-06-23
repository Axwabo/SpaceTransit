namespace SpaceTransit.Routes.Stops
{

    public interface IDeparture : IStop, IExitTowards
    {

        TimeOnly Departure { get; }

    }

}
