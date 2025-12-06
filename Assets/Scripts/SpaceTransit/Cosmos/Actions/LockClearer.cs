using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class LockClearer : SafetyActionBase
    {

        [SerializeField]
        private Lock[] locks;

        [SerializeField]
        private bool enter;

        [SerializeField]
        private bool exit;

        [SerializeField]
        private bool forwards;

        [SerializeField]
        private bool backwards;

        public override void OnEntered(ShipModule module)
        {
            if (exit && Ensurer.Occupants.Count == module.Assembly.Modules.Count && Direction(module))
                locks.Release(module.Assembly);
        }

        public override void OnExited(ShipModule module)
        {
            if (exit && !Ensurer.IsOccupied && Direction(module))
                locks.Release(module.Assembly);
        }

        private bool Direction(ShipModule module) => module.Assembly.Reverse ? backwards : forwards;

    }

}
