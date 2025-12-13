using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Ships.Driving
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ManualSpeedControl : MonoBehaviour
    {

        private ShipAssembly _assembly;

        private void Awake() => _assembly = GetComponent<ShipAssembly>();

        private void OnEnable() => _assembly.IsManuallyDriven = true;

        private void Update()
        {
            if (_assembly.Parent.State != ShipState.Sailing)
                return;
            var move = InputSystem.actions["Speed"].ReadValue<float>();
            if (move > 0)
                _assembly.TargetSpeed += 2;
            else if (move < 0)
                _assembly.TargetSpeed -= 2;
            var limit = _assembly.FrontModule.Thruster.Tube.SpeedLimit;
            if (!Mathf.Approximately(0, limit) && _assembly.TargetSpeed > limit)
                _assembly.SetSpeed(limit);
        }

        private void OnDisable() => _assembly.IsManuallyDriven = false;

    }

}
