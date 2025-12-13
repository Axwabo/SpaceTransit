using System.Collections.Generic;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Lock : MonoBehaviour
    {

        private readonly HashSet<ShipAssembly> _usedBy = new();

        public bool IsFree => _usedBy.Count == 0;

        public bool IsUsedOnlyBy(ShipAssembly assembly) => _usedBy.Count == 1 && _usedBy.Contains(assembly);

        public bool CanClaim(ShipAssembly assembly) => IsFree || IsUsedOnlyBy(assembly);

        public void Claim(ShipAssembly assembly)
        {
            if (CanClaim(assembly))
                _usedBy.Add(assembly);
        }

        public void Release(ShipAssembly assembly) => _usedBy.Remove(assembly);

        private void Update() => _usedBy.RemoveWhere(static e => !e);

    }

}
