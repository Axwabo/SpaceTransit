using SpaceTransit.Cosmos;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Tubes;

namespace SpaceTransit.Routes.Sequences
{

    public record SpawnLocation(int StopIndex = ITarget.Origin)
    {

        public static SpawnLocation Origin { get; } = new();

    }

    public record TubeSpawn(TubeBase Tube, int StopIndex = ITarget.Origin) : SpawnLocation(StopIndex);

    public sealed record EntrySpawn(TubeBase Tube, Entry Entry, int StopIndex = ITarget.Origin) : TubeSpawn(Tube, StopIndex);

}
