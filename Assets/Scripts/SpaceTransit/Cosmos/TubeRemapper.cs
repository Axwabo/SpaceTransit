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
                connectTube.SetNext(connectTo);
            base.OnEntered(module);
        }

    }

}
