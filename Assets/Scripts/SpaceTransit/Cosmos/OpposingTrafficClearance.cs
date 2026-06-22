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

        public bool CanProceed(ShipAssembly assembly) => assembly.Reverse ? _forwards.Count == 0 : _backwards.Count == 0;

        public bool CanClaim(ShipAssembly assembly)
        {
            if (!CanProceed(assembly))
                return false;
            var collection = assembly.Reverse ? _forwards : _backwards;
        }

        public void Claim(ShipAssembly assembly)
        {
            if (CanClaim(assembly))
                (assembly.Reverse ? _backwards : _forwards).Add(assembly);
        }

        private void Update()
        {
            _forwards.RemoveDestroyed();
            _backwards.RemoveDestroyed();
        }

        public void Release(ShipAssembly assembly)
        {
            _forwards.Remove(assembly);
            _backwards.Remove(assembly);
        }

    }

}
