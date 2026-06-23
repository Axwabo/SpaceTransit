namespace SpaceTransit.Routes.Stops
{

    public interface ITarget
    {

        int DockIndex { get; }

        StationId Station { get; }

    }

}
