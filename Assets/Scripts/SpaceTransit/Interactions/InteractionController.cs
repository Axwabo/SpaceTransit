using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Interactions
{

    public sealed class InteractionController : MonoBehaviour
    {

        private Transform _t;

        private void Awake() => _t = transform;

        private void Update()
        {
            if (InputSystem.actions["Interact"].WasPressedThisFrame()
                && Physics.Raycast(_t.position, _t.forward, out var hit, 3)
                && hit.collider.TryGetComponent(out IInteractable interactable))
                interactable.OnInteracted();
        }

    }

}
