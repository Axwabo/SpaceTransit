using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    [RequireComponent(typeof(LockBasedSafety))]
    public sealed class LockTaker : SafetyActionBase
    {

        private LockBasedSafety _safety;

        private void Awake() => _safety = GetComponent<LockBasedSafety>();

        public override void OnEntered(ShipModule module) => _safety.Claim(module.Assembly);

    }

}
