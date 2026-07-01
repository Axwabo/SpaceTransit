using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Interactions
{

    public sealed class InteractionController : MonoBehaviour
    {

        private const float MaxDistance = 3 * World.MetersToWorld;

        private static bool Interact => TouchscreenMode.Interact || InputSystem.actions["Interact"].WasPressedThisFrame();

        private Transform _t;

        private void Awake() => _t = transform;

        private void Update()
        {
            if (Interact
                && Physics.Raycast(_t.position, _t.forward, out var hit, MaxDistance)
                && hit.collider.TryGetComponent(out IInteractable interactable))
                interactable.OnInteracted();
            TouchscreenMode.Interact = false;
        }

    }

}
