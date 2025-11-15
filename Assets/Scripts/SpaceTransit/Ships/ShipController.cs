using System;
using System.Linq;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ShipController : MonoBehaviour
    {

        [SerializeField]
        private AnimationCurve liftoff;

        [SerializeField]
        private AnimationCurve land;

        private Transform _t;

        private ShipComponentBase[] _components;

        private float _liftProgress = -1;

        private float _liftDuration;

        private ShipState _previousState;

        public ShipState State { get; private set; }

        public ShipAssembly Assembly { get; private set; }

        public bool CanLand => State == ShipState.Sailing && Assembly.CurrentSpeed.Raw == 0;

        public bool CanLiftOff => State == ShipState.Docked;

        private void Start()
        {
            _t = transform;
            _components = this.GetComponentsInImmediateChildren<ShipComponentBase>(true).ToArray();
            Assembly = _components.OfType<ShipAssembly>().First();
            foreach (var component in _components)
                component.Initialize(this);
        }

        private void Update()
        {
            if (_previousState == State)
                return;
            foreach (var component in _components)
                component.OnStateChanged(_previousState);
            _previousState = State;
        }

        private void FixedUpdate()
        {
            if (_liftProgress < 0)
                return;
            var lifting = State == ShipState.LiftingOff;
            _liftProgress += Time.fixedDeltaTime;
            var curve = lifting ? liftoff : land;
            var position = _t.position;
            position.y = curve.Evaluate(_liftProgress) * World.MetersToWorld;
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
