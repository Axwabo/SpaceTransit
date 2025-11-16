using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(ShipController))]
    public sealed class ShipAssembly : ShipComponentBase
    {

        public IReadOnlyList<ShipModule> Modules { get; private set; }

        public TubeBase startTube;

        [field: Header("Speed Controls")]
        [field: SerializeField]
        public float MaxSpeed { get; private set; }

        [field: SerializeField]
        public float Acceleration { get; private set; }

        [field: SerializeField]
        public float Deceleration { get; private set; }

        private ShipSpeed _targetSpeed;

        public ShipSpeed CurrentSpeed { get; private set; }

        public ShipSpeed TargetSpeed
        {
            get => _targetSpeed;
            set => _targetSpeed = value.Clamp(MaxSpeed);
        }

        public bool Reverse
        {
            get => CurrentSpeed.IsReverse;
            set
            {
                CurrentSpeed = CurrentSpeed.WithReverse(value);
                TargetSpeed = TargetSpeed.WithReverse(value);
            }
        }

        public bool IsPlayerMounted { get; private set; }

        public ShipModule FrontModule => CurrentSpeed.IsReverse ? Modules[^1] : Modules[0];

        protected override void Awake()
        {
            Modules = this.GetComponentsInImmediateChildren<ShipModule>().ToArray();
            if (Modules.Count == 0)
                throw new MissingComponentException("Ships must have at least 1 module");
        }

        private void Update()
        {
            if (CurrentSpeed < TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Clock.Delta * Acceleration, MaxSpeed);
            else if (CurrentSpeed > TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Clock.Delta * Deceleration, MaxSpeed);
            IsPlayerMounted = Modules.Any(e => e.Mount.Transform == MovementController.Current.Mount);
        }

        public void SetSpeed(float raw) => TargetSpeed = new ShipSpeed(raw, Reverse);

    }

}
