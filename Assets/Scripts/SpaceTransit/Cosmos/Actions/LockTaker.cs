using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class LockTaker : SafetyActionBase
    {

        [SerializeField]
        private Lock @lock;

        public override void OnEntered(ShipModule module)
        {
            if (@lock.IsFree)
                @lock.Claim(module.Assembly);
        }

    }

}
