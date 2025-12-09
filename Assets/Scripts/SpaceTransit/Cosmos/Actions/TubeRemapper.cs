using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class TubeRemapper : SafetyActionBase
    {

        [SerializeField]
        public TubeBase connectTube;

        [SerializeField]
        public TubeBase connectTo;

        public override void OnEntered(ShipModule module)
        {
            if (Ensurer.Occupants.Count == 1)
                Remap();
        }

        public void Remap() => connectTube.SetNext(connectTo);

    }

}
