using System.Collections.Generic;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class OpposingTrafficClearance : MonoBehaviour
    {

        private readonly HashSet<ShipAssembly> _forwards = new();

        private readonly HashSet<ShipAssembly> _backwards = new();

        public bool IsFree => _forwards.Count == 0 && _backwards.Count == 0;

        public bool CanProceed(ShipAssembly assembly) => CanClaim(assembly) && (assembly.Reverse ? _backwards : _forwards).Contains(assembly);

        public bool CanClaim(ShipAssembly assembly) => CanClaim(assembly.Reverse);

        public bool CanClaim(bool reverse) => reverse ? _forwards.Count == 0 : _backwards.Count == 0;

        public void Claim(ShipAssembly assembly) => Claim(assembly, assembly.Reverse);

        public void Claim(ShipAssembly assembly, bool reverse)
        {
            if (CanClaim(reverse))
                (reverse ? _backwards : _forwards).Add(assembly);
        }

        public void Release(ShipAssembly assembly)
        {
            _forwards.Remove(assembly);
            _backwards.Remove(assembly);
        }

        private void Update()
        {
            _forwards.RemoveDestroyed();
            _backwards.RemoveDestroyed();
        }

    }

}
