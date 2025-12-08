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
        public Exit[] FrontExits { get; private set; }

        [field: SerializeField]
        public Exit[] BackExits { get; private set; }

        [field: SerializeField]
        public Entry[] FrontEntries { get; private set; }

        [field: SerializeField]
        public Entry[] BackEntries { get; private set; }

        [field: SerializeField]
        public bool Left { get; private set; }

        [field: SerializeField]
        public bool Right { get; private set; }

        public int Index { get; set; }

        public HashSet<ShipAssembly> UsedBy { get; } = new();

        private void Awake()
        {
            foreach (var entry in FrontEntries)
                entry.Dock = this;
            foreach (var entry in BackEntries)
                entry.Dock = this;
        }

    }

}
