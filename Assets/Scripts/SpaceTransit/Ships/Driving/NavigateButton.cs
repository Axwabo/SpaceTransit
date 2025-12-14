using SpaceTransit.Interactions;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class NavigateButton : MonoBehaviour, IInteractable
    {

        [SerializeField]
        private VaulterScreen slot;

        [SerializeField]
        private bool down;

        public void OnInteracted() => slot.Navigate(down);

    }

}
