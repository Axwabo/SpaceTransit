using SpaceTransit.Interactions;
using SpaceTransit.Ships.Driving.Screens;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class IndexButton : ModuleComponentBase, IInteractable
    {

        [SerializeField]
        private ScreenSlot slot;

        [SerializeField]
        private int index;

        public void OnInteracted() => slot.Select(index);

    }

}
