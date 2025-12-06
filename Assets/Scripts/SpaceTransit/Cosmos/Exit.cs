using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : EntryOrExit
    {

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

    }

}
