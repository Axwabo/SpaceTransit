using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class ExitClearer : SafetyActionBase
    {

        [SerializeField]
        private Exit exit;

        [SerializeField]
        private bool forwards;

        [SerializeField]
        private bool backwards;

        public override void OnExited(ShipModule module)
        {
            if (!Ensurer.IsOccupied && (module.Assembly.Reverse ? backwards : forwards))
                exit.Release(module.Assembly);
        }

    }

}
