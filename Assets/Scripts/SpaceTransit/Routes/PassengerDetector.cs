using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [RequireComponent(typeof(Station))]
    public sealed class PassengerDetector : MonoBehaviour
    {

        private Station _station;

        private float _time;

        private void Awake() => _station = GetComponent<Station>();

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out MovementController _))
                _time = 5;
        }

        private void Update() => _station.PassengersWaiting = (_time -= Time.deltaTime) > 0;

    }

}
