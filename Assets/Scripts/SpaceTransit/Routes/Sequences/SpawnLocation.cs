using SpaceTransit.Cosmos;
using SpaceTransit.Tubes;

namespace SpaceTransit.Routes.Sequences
{

    public record SpawnLocation(int StopIndex = -1);

    public sealed record DockSpawn(int DockIndex, int StopIndex = -1) : SpawnLocation(StopIndex);

    public record TubeSpawn(TubeBase Tube, int StopIndex = -1) : SpawnLocation(StopIndex);

    public sealed record EntrySpawn(TubeBase Tube, Entry Entry, int StopIndex = -1) : TubeSpawn(Tube, StopIndex);

}
