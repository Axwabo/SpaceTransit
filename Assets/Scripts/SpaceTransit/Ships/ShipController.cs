using System;
using System.Linq;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Vaulter;
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

        public bool CanLand => State == ShipState.Sailing
                               && Assembly.IsStationary()
                               && Physics.Raycast(
                                   Assembly.FrontModule.Transform.position,
                                   Vector3.down,
                                   1,
                                   LayerMask.GetMask("Dock")
                               );

        public bool CanLiftOff => State == ShipState.WaitingForDeparture
                                  && Assembly.Modules.All(e => e.CanDepart);

        public bool CanProceed { get; private set; }

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
            _liftProgress += Clock.FixedDelta;
            var curve = lifting ? liftoff : land;
            var position = _t.localPosition;
            position.y = curve.Evaluate(_liftProgress) * World.MetersToWorld;
            _t.localPosition = position;
            if (_liftProgress < _liftDuration)
                return;
            _liftProgress = -1;
            State = lifting ? ShipState.Sailing : ShipState.Docked;
        }

        private void LateUpdate()
        {
            CanProceed = Assembly.FrontModule.Thruster.Tube.Safety.CanProceed(Assembly);
            if (!CanProceed)
                Assembly.SetSpeed(0);
        }

        public void MarkReady()
        {
            if (State == ShipState.WaitingForDeparture)
                return;
            if (State != ShipState.Docked)
                throw new InvalidOperationException("Cannot depart while not docked");
            if (!TryGetExit(out var exit))
                State = ShipState.WaitingForDeparture;
            else if (exit.Lock(Assembly))
            {
                Assembly.FrontModule.ExitList.Mark(exit);
                State = ShipState.WaitingForDeparture;
            }
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
                throw new InvalidOperationException("Cannot lift off while not waiting for departure or a module prevents departure");
            _liftProgress = 0;
            _liftDuration = liftoff.Duration();
            State = ShipState.LiftingOff;
        }

        public bool TryGetVaulter(out VaulterController controller)
        {
            foreach (var component in _components)
            {
                if (component is not VaulterController vaulter)
                    continue;
                controller = vaulter;
                return true;
            }

            controller = null;
            return false;
        }

        private bool TryGetExit(out Exit exit)
        {
            var list = Assembly.FrontModule.ExitList;
            if (list.TryGetPicked(out exit))
                return true;
            if (!TryGetVaulter(out var controller)
                || !controller.IsInService
                || controller.Stop is not IDeparture {ExitTowards: var towards}
                || Assembly.FrontModule.Thruster.Tube is not Dock dock)
            {
                exit = null;
                return false;
            }

            foreach (var front in dock.FrontExits)
            {
                if (front.Connected != towards)
                    continue;
                exit = front;
                return true;
            }

            foreach (var back in dock.BackExits)
            {
                if (back.Connected != towards)
                    continue;
                exit = back;
                return true;
            }

            exit = null;
            return false;
        }

    }

}
