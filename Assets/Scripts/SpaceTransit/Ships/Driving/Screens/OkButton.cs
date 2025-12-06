using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
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
