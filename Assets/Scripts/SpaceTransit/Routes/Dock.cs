using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class Dock : MonoBehaviour
    {

        [field: SerializeField]
        public TubeBase Tube { get; private set; }

        [field: SerializeField]
        public Exit FrontExit { get; private set; }

        [field: SerializeField]
        public Exit BackExit { get; private set; }

        [field: SerializeField]
        public bool Left { get; private set; }

        [field: SerializeField]
        public bool Right { get; private set; }

        public HashSet<ShipAssembly> UsedBy { get; } = new();

    }

}
