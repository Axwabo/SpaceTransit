using System;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ShipController : ShipComponentBase
    {

        [SerializeField]
        private AnimationCurve liftoff;

        [SerializeField]
        private AnimationCurve land;

        private Transform _t;

        private float _liftProgress = -1;

        private float _liftDuration;

        private ShipState _previousState;

        public ShipState State { get; private set; }

        public bool CanLand => State == ShipState.Sailing && Assembly.CurrentSpeed.Raw == 0;

        public bool CanLiftOff => State == ShipState.Docked;

        private void Awake() => _t = transform;

        private void Update()
        {
            if (_previousState == State)
                return;
            _previousState = State;
            foreach (var module in Assembly.Modules)
                module.OnStateChanged();
        }

        private void FixedUpdate()
        {
            if (_liftProgress < 0)
                return;
            var lifting = State == ShipState.LiftingOff;
            _liftProgress += Time.fixedDeltaTime;
            var curve = lifting ? liftoff : land;
            var position = _t.position;
            position.y = curve.Evaluate(_liftProgress);
            _t.position = position;
            if (_liftProgress < _liftDuration)
                return;
            _liftProgress = -1;
            State = lifting ? ShipState.Sailing : ShipState.Docked;
        }

        public void Land()
        {
            if (!CanLand)
                throw new InvalidOperationException("Cannot land while moving or not sailing");
            _liftProgress = 0;
            _liftDuration = land.Duration();
            State = ShipState.Landing;
        }

        public void LiftOff()
        {
            if (!CanLiftOff)
                throw new InvalidOperationException("Cannot lift off while not docked");
            _liftProgress = 0;
            _liftDuration = liftoff.Duration();
            State = ShipState.LiftingOff;
        }

    }

}
