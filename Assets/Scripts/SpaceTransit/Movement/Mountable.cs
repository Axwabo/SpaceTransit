using UnityEngine;

namespace SpaceTransit.Movement
{

    public sealed class Mountable : MonoBehaviour
    {

        private Transform _t;

        private void Awake() => _t = transform;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MovementController controller))
                controller.Mount = _t;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MovementController controller) && controller.Mount == _t)
                controller.Mount = null;
        }

    }

}
