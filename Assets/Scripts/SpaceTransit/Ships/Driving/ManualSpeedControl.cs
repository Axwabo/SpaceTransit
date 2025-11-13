using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Ships.Driving
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ManualSpeedControl : MonoBehaviour
    {

        private ShipAssembly _assembly;

        private void Awake() => _assembly = GetComponent<ShipAssembly>();

        private void Update()
        {
            if (_assembly.Controller.State != ShipState.Sailing)
                return;
            var move = InputSystem.actions["Speed"].ReadValue<float>();
            if (move > 0)
                _assembly.TargetSpeed += 2;
            else if (move < 0)
                _assembly.TargetSpeed -= 2;
        }

    }

}
