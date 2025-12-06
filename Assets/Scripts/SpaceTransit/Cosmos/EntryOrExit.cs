using SpaceTransit.Cosmos.Actions;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Cosmos
{

    public abstract class EntryOrExit : MonoBehaviour
    {

        [field: SerializeField]
        [field: FormerlySerializedAs("locks")]
        protected Lock[] Locks { get; private set; }

        protected TubeRemapper[] Remappers { get; private set; }

        private void Awake() => Remappers = GetComponents<TubeRemapper>();

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

    }

}
