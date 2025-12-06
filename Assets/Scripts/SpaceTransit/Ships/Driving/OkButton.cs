using SpaceTransit.Interactions;
using SpaceTransit.Ships.Driving.Screens;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class OkButton : ModuleComponentBase, IInteractable
    {

        [SerializeField]
        private ScreenSlot slot;

        public void OnInteracted()
        {
            if (slot.Current)
                slot.Current.Confirm();
        }

    }

}
