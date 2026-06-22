using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    [RequireComponent(typeof(EllenmenetetMegtiltóSafety))]
    public sealed class LockTaker : SafetyActionBase
    {

        private EllenmenetetMegtiltóSafety _safety;

        private void Awake() => _safety = GetComponent<EllenmenetetMegtiltóSafety>();

        public override void OnEntered(ShipModule module) => _safety.Claim(module.Assembly);

    }

}
