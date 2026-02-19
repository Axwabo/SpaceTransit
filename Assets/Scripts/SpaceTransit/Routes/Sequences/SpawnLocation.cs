using SpaceTransit.Cosmos;
using SpaceTransit.Tubes;

namespace SpaceTransit.Routes.Sequences
{

    public record SpawnLocation(int StopIndex = -1)
    {

        public static SpawnLocation Origin { get; } = new();

    }

    public record TubeSpawn(TubeBase Tube, int StopIndex = -1) : SpawnLocation(StopIndex);

    public sealed record EntrySpawn(TubeBase Tube, Entry Entry, int StopIndex = -1) : TubeSpawn(Tube, StopIndex);

}
