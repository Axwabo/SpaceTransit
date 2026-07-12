namespace SpaceTransit.Routes.Stops
{

    public interface ITarget
    {

        public const int Origin = -1;
        public const int Destination = int.MaxValue;

        int DockIndex { get; }

        StationId Station { get; }

    }

}
