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

        public bool IsPlayerMounted { get; private set; }

        public ShipModule FrontModule => CurrentSpeed.IsReverse ? Modules[^1] : Modules[0];

        private void Awake()
        {
            Modules = this.GetComponentsInImmediateChildren<ShipModule>().ToArray();
            if (Modules.Count == 0)
                throw new MissingComponentException("Ships must have at least 1 module");
        }

        private void Update()
        {
            if (CurrentSpeed < TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Time.deltaTime * acceleration, MaxSpeed);
            else if (CurrentSpeed > TargetSpeed)
                CurrentSpeed = CurrentSpeed.MoveTowards(TargetSpeed.Raw, Time.deltaTime * deceleration, MaxSpeed);
            IsPlayerMounted = Modules.Any(e => e.Mount.Transform == MovementController.Current.Mount);
        }

        public void Reverse() => CurrentSpeed = CurrentSpeed.FlipReverse();

    }

}
