using SpaceTransit.Cosmos.Actions;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public abstract class EntryOrExit : MonoBehaviour
    {

        [field: SerializeField]
        public StationId Connected { get; private set; }

        [field: SerializeField]
        protected Lock[] Locks { get; private set; }

        public TubeRemapper[] Remappers { get; private set; }

        private void Awake() => Remappers = GetComponents<TubeRemapper>();

        public virtual bool IsFree => Locks.AreFree();

        public virtual bool Lock(ShipAssembly assembly)
        {
            if (!Locks.CanClaim(assembly))
                return false;
            Locks.Claim(assembly);
            foreach (var remapper in Remappers)
                remapper.Remap();
            return true;
        }

        public void Release(ShipAssembly assembly) => Locks.Release(assembly);

        public bool IsUsedOnlyBy(ShipAssembly assembly) => Locks.AreOnlyUsedBy(assembly);

    }

}
