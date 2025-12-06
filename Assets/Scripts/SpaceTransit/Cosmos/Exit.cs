using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : MonoBehaviour
    {

        [SerializeField]
        private Lock @lock;

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

        public bool Lock(ShipAssembly assembly)
        {
            if (!@lock.Claim(assembly))
                return false;
            if (!connectTo)
                return true;
            connectTube.SetNext(connectTo);
            return true;
        }

        public void Release(ShipAssembly assembly) => @lock.Release(assembly);

    }

}
