using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class ExitClearer : DelegatingEnsurer
    {

        [SerializeField]
        private Exit exit;

        [SerializeField]
        private bool forwards;

        [SerializeField]
        private bool backwards;

        public override void OnExited(ShipModule module)
        {
            base.OnExited(module);
            if (!ensurer.IsOccupied && module.Assembly.Reverse ? backwards : forwards)
                exit.UsedBy.Remove(module.Assembly);
        }

    }

}
