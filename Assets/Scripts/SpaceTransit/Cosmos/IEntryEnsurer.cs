using System.Collections.Generic;
using SpaceTransit.Routes;

namespace SpaceTransit.Cosmos
{

    public interface IEntryEnsurer
    {

        StationId TargetStation { get; }

        bool Backwards { get; }

        List<Entry> Entries { get; }

    }

}
