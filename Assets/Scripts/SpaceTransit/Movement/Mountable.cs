using UnityEngine;

namespace SpaceTransit.Movement
{

    public sealed class Mountable : MonoBehaviour
    {

        public Transform Transform { get; private set; }

        private void Awake() => Transform = transform;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MovementController controller))
                controller.Mount = Transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MovementController controller) && controller.Mount == Transform)
                controller.Mount = World.Current;
        }

    }

}
