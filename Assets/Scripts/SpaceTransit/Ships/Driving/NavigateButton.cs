using SpaceTransit.Interactions;
using SpaceTransit.Ships.Driving.Screens;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class NavigateButton : MonoBehaviour, IInteractable
    {

        [SerializeField]
        private ScreenSlot slot;

        [SerializeField]
        private bool down;

        public void OnInteracted()
        {
            if (slot.Current)
                slot.Current.Navigate(down);
        }

    }

}
