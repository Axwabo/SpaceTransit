using SpaceTransit.Interactions;
using UnityEngine;

namespace SpaceTransit.Ships.Modules.Doors
{

    [RequireComponent(typeof(MeshRenderer))]
    public sealed class DoorOpener : MonoBehaviour, IInteractable
    {

        [SerializeField]
        private DoorController controller;

        [SerializeField]
        private Material disabledMaterial;

        private bool _couldOpen = true;

        private Material _interactableMaterial;

        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _interactableMaterial = _renderer.sharedMaterial;
        }

        private void Update()
        {
            var canOpen = controller.Controller.State == ShipState.Docked && controller.IsCorrectSide;
            if (canOpen == _couldOpen)
                return;
            _couldOpen = canOpen;
            _renderer.sharedMaterial = canOpen ? _interactableMaterial : disabledMaterial;
        }

        public void OnInteracted() => controller.RequestOpen();

    }

}
