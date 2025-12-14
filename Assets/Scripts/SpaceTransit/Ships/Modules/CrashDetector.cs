using SpaceTransit.Menu;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CrashDetector : MonoBehaviour
    {

        private float _wait = 3;

        private void Update() => _wait -= Clock.UnscaledDelta;

        private void OnTriggerEnter(Collider other)
        {
            if (_wait < 0 && other.TryGetComponent(out CrashDetector _))
                CrashDisplay.DisplayCrash(transform.position);
        }

    }

}
