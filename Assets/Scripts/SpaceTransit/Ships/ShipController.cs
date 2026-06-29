using System;
using System.Linq;
using System.Threading;
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

        [SerializeField]
        private RestartSequence restart;

        private Transform _t;

        private ShipComponentBase[] _components;

        private float _liftProgress = -1;

        private float _liftDuration;

        private ShipState _previousState;

        public ShipState State { get; private set; }

        public ShipAssembly Assembly { get; private set; }

        public bool CanLand => State == ShipState.Sailing
                               && Assembly.IsStationary()
                               && Assembly.FrontModule.Thruster.Tube is Dock;

        public bool CanLiftOff => State == ShipState.WaitingForDeparture && ModulesReadyForDeparture;

        public bool CanProceed { get; private set; }

        public bool ModulesReadyForDeparture => Assembly.Modules.All(e => e.CanDepart);

        public bool StopRequested => Assembly.Modules.Any(e => e.StopRequested);

        public float TimeToDeparture { get; set; }

        public bool WillBeDeparting => TimeToDeparture is > 0 and < 5 && (!TryGetExit(out var exit) || exit.IsFree || exit.IsUsedOnlyBy(Assembly));

        public bool IsRestarting { get; private set; }

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
            if (!lifting)
                NotifyArrival();
        }

        private void LateUpdate()
        {
            CanProceed = Assembly.FrontModule.Thruster.Tube.Safety.CanProceed(Assembly);
            if (!CanProceed)
                Assembly.SetTargetSpeed(0);
        }

        public async Awaitable RestartAsync(CancellationToken token)
        {
            if (!restart)
                return;
            if (!token.CanBeCanceled)
                token = Assembly.destroyCancellationToken;
            IsRestarting = true;
            foreach (var component in _components)
                component.OnRestarting();
            await restart.Execute(this, token);
            IsRestarting = false;
        }

        public void MarkReady()
        {
            if (State == ShipState.WaitingForDeparture || IsRestarting)
                return;
            if (State != ShipState.Docked)
                throw new InvalidOperationException("Cannot depart while not docked");
            NotifyDeparture();
            if (!TryGetExit(out var exit))
            {
                State = ShipState.WaitingForDeparture;
                TimeToDeparture = 0;
            }
            else if (!exit.Connected.IsLoaded() && !Assembly.IsPlayerMounted)
                Destroy(gameObject);
            else if (exit.Lock(Assembly))
            {
                Assembly.FrontModule.Cosmos.ExitList.Mark(exit);
                State = ShipState.WaitingForDeparture;
                TimeToDeparture = 0;
            }
        }

        private void NotifyArrival()
        {
            if (TryGetVaulter(out var vaulter) && vaulter.IsInService && Assembly.FrontModule.Thruster.Tube is Dock dock && dock.Station.ID == vaulter.Stop.Station && dock.Station.Announcer)
                dock.Station.Announcer.EnqueueArrived(vaulter, vaulter.Route, vaulter.Stop);
        }

        private void NotifyDeparture()
        {
            if (Assembly.FrontModule.Thruster.Tube is Dock dock && Assembly.ShouldAnnounceNonScheduled(dock.Station.ID) && dock.Station.Announcer)
                dock.Station.Announcer.EnqueueDeparting(Assembly, dock.Index);
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
            var list = Assembly.FrontModule.Cosmos.ExitList;
            if (list.TryGetPicked(out exit))
                return true;
            if (!TryGetVaulter(out var controller)
                || !controller.IsInService
                || controller.Target is not IExitTowards {ExitTowards: var towards}
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
