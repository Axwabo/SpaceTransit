using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class TubeRemapper : DelegatingEnsurer
    {

        [SerializeField]
        private TubeBase connectTube;

        [SerializeField]
        private TubeBase connectTo;

        public override void OnEntered(ShipModule module)
        {
            if (!ensurer.IsOccupied)
                Remap(module.Assembly.Reverse);
            base.OnEntered(module);
        }

        private void Remap(bool reverse)
        {
            if (reverse)
                connectTube.Previous = connectTo;
            else
                connectTube.Next = connectTo;
        }

    }

}
