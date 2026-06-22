using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class OpposingTrafficClearance : MonoBehaviour
    {

        [SerializeField]
        private Lock forwards;

        [SerializeField]
        private Lock backwards;

        public bool CanClaim(ShipAssembly assembly) => assembly.Reverse ? forwards.IsFree : backwards.IsFree;

        public void Claim(ShipAssembly assembly)
        {
            if (CanClaim(assembly))
                (assembly.Reverse ? backwards.Claim(assembly))
        }

    }

}
