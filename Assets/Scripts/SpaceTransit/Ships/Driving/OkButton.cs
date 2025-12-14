using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class OkButton : ModuleComponentBase, IInteractable
    {

        [SerializeField]
        private VaulterScreen slot;

        public void OnInteracted() => slot.Confirm();

    }

}
