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
            if (UsedBy.Count != 0 && !UsedBy.Contains(assembly))
                return false;
            UsedBy.Add(assembly);
            if (!connectTo)
                return true;
            if (assembly.Reverse)
                connectTube.Previous = connectTo;
            else
                connectTube.Next = connectTo;
            return true;
        }

        private void Update() => UsedBy.RemoveWhere(e => !e);

    }

}
