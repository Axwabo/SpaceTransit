using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : MonoBehaviour
    {

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        public HashSet<ShipAssembly> UsedBy { get; } = new();

    }

}
