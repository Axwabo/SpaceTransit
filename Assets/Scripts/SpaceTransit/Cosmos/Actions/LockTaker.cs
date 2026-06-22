using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    [RequireComponent(typeof(OpposingTrafficPreventiveSafety))]
    public sealed class LockTaker : SafetyActionBase
    {

        private OpposingTrafficPreventiveSafety _safety;

        private void Awake() => _safety = GetComponent<OpposingTrafficPreventiveSafety>();

        public override void OnEntered(ShipModule module) => _safety.Claim(module.Assembly);

    }

}
