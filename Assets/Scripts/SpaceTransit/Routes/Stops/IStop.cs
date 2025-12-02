namespace SpaceTransit.Routes.Stops
{

    public interface IStop
    {

        int DockIndex { get; }
        StationId Station { get; }

    }

}
