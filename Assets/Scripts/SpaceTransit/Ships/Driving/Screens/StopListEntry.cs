using SpaceTransit.Routes.Stops;

namespace SpaceTransit.Ships.Driving.Screens
{

    public record StopListEntry(ITarget Target);

    public sealed record ExitTowards(IExitTowards Exit) : StopListEntry(Exit);

}
