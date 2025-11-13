using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Ships
{

    public sealed class ShipAssembly : MonoBehaviour
    {

        public IReadOnlyList<ShipModule> Modules { get; private set; }

        public TubeBase startTube;

        [field: Header("Speed Controls")]
        [field: SerializeField]
        public float MaxSpeed { get; private set; }

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private float deceleration;

        public ShipSpeed CurrentSpeed { get; private set; }

        public ShipSpeed TargetSpeed { get; private set; }

        private void Awake()
        {
            Modules = this.GetComponentsInImmediateChildren<ShipModule>().ToArray();
            if (Modules.Count == 0)
                throw new MissingComponentException("Ships must have at least 1 module");
            foreach (var module in Modules)
                module.Initialize(this);
        }

        private void Update()
        {
            var move = InputSystem.actions["Speed"].ReadValue<float>();
            if (move > 0)
                TargetSpeed += 2;
            else if (move < 0)
                TargetSpeed -= 2;
            if (CurrentSpeed < TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Time.deltaTime * acceleration, MaxSpeed);
            else if (CurrentSpeed > TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Time.deltaTime * deceleration, MaxSpeed);
        }

        public void Reverse() => CurrentSpeed = CurrentSpeed.FlipReverse();

    }

}
