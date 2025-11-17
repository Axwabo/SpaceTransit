using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : MonoBehaviour
    {

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        [SerializeField]
        private TubeBase connectTube;

        [SerializeField]
        private TubeBase connectTo;

        public HashSet<ShipAssembly> UsedBy { get; } = new();

        public bool Lock(ShipAssembly assembly)
        {
            if (!UsedBy.Add(assembly) && UsedBy.Count == 1)
                return false;
            if (!connectTo)
                return true;
            if (assembly.Reverse)
                connectTo.Previous = connectTube;
            else
                connectTo.Next = connectTube;
            return true;
        }

        private void Update() => UsedBy.RemoveWhere(e => !e);

    }

}
