namespace SpaceTransit.Routes.Stops
{

    public interface IExitTowards : ITarget
    {

        StationId ExitTowards { get; }

    }

}
