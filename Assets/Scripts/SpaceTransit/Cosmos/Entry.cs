using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : MonoBehaviour
    {

        [SerializeField]
        private Lock @lock;

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        [SerializeField]
        private TubeBase connectTube;

        [SerializeField]
        private TubeBase connectTo;

        public Dock Dock { get; set; }

        public bool IsUsedOnlyBy(ShipAssembly assembly) => @lock.IsUsedOnlyBy(assembly);

        public bool Lock(ShipAssembly assembly)
        {
            if (Dock.Tube.Safety.IsOccupied || !@lock.Claim(assembly))
                return false;
            if (!connectTo)
                return true;
            connectTube.SetNext(connectTo);
            return true;
        }

        public void Release(ShipAssembly assembly) => @lock.Release(assembly);

    }

}
