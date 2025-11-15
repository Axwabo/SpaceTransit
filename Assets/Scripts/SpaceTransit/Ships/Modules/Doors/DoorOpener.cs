using SpaceTransit.Interactions;
using UnityEngine;

namespace SpaceTransit.Ships.Modules.Doors
{

    public sealed class DoorOpener : MonoBehaviour, IInteractable
    {

        [SerializeField]
        private DoorController controller;

        public void OnInteracted() => controller.RequestOpen();

    }

}
