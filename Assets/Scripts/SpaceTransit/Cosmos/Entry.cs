using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : MonoBehaviour
    {

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        public Dock Dock { get; set; }

        [SerializeField]
        private Exit exit;

        [SerializeField]
        private TubeBase connectTube;

        [SerializeField]
        private TubeBase connectTo;

        public HashSet<ShipAssembly> UsedBy { get; } = new();

        public bool Lock(ShipAssembly assembly)
        {
            if (Dock.Tube.Safety.IsOccupied || !exit.Lock(assembly) || UsedBy.Count != 0 && !UsedBy.Contains(assembly))
                return false;
            UsedBy.Add(assembly);
            if (!connectTo)
                return true;
            connectTube.SetNext(connectTo);
            return true;
        }

        private void Update() => UsedBy.RemoveWhere(e => !e);

    }

}
