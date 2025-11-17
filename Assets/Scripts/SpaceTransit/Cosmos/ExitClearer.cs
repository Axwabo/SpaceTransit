using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class ExitClearer : DelegatingEnsurer
    {

        [SerializeField]
        private Exit exit;

        public override void OnExited(ShipModule module)
        {
            base.OnExited(module);
            if (!ensurer.IsOccupied)
                exit.UsedBy.Remove(module.Assembly);
        }

    }

}
