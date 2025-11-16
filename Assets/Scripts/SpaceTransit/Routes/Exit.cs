using System.Collections.Generic;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class Exit : MonoBehaviour
    {

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        public HashSet<ShipAssembly> UsedBy { get; } = new();

    }

}
