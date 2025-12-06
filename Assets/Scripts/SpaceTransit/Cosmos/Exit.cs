using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : MonoBehaviour
    {

        [field: FormerlySerializedAs("<ExitTowards>k__BackingField")]
        [field: FormerlySerializedAs("<ConnectedStation>k__BackingField")]
        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        [FormerlySerializedAs("connectExitTube")]
        [SerializeField]
        private TubeBase connectTube;

        [FormerlySerializedAs("connectExitTo")]
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
