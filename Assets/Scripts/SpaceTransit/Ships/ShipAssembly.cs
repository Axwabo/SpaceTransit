using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Ships.Driving;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(ShipDriver))]
    public sealed class ShipAssembly : MonoBehaviour
    {

        public ShipDriver Driver { get; private set; }

        public IReadOnlyList<ShipModule> Modules { get; private set; }

        public TubeBase startTube;

        [field: Header("Speed Controls")]
        [field: SerializeField]
        public float MaxSpeed { get; private set; }

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private float deceleration;

        private ShipSpeed _targetSpeed;

        public ShipSpeed CurrentSpeed { get; private set; }

        public ShipSpeed TargetSpeed
        {
            get => _targetSpeed;
            set => _targetSpeed = value.Clamp(MaxSpeed);
        }

        private void Awake()
        {
            Driver = GetComponent<ShipDriver>();
            Modules = this.GetComponentsInImmediateChildren<ShipModule>().ToArray();
            if (Modules.Count == 0)
                throw new MissingComponentException("Ships must have at least 1 module");
            foreach (var module in Modules)
                module.Initialize(this);
        }

        private void Update()
        {
            if (CurrentSpeed < TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Time.deltaTime * acceleration, MaxSpeed);
            else if (CurrentSpeed > TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Time.deltaTime * deceleration, MaxSpeed);
        }

        public void Reverse() => CurrentSpeed = CurrentSpeed.FlipReverse();

    }

}
