using SpaceTransit.Interactions;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
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
