using System;
using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class AutomaticDriver : VaulterComponentBase
    {

        private const float ReversingSpeed = 20 * ShipSpeed.KmhToMps;
        private const int MinStaySeconds = 15;
        private const float DefaultOverscan = 15 * World.MetersToWorld;

        private float _remainingWait;

        private bool _departed;

        private bool _stopping;

        private bool _entryRequested;

        private bool _reversing;

        private bool ShouldStop
        {
            get
            {
                if (Parent.Target is Passthrough || !Station.TryGetLoadedStation(Parent.Target.Station, out var station))
                    return false;
                var tube = station.Docks[Parent.Target.DockIndex];
                var overscan = DefaultOverscan * Mathf.Sqrt(Time.timeScale);
                var stopPoint = World.Current.TransformPoint(tube.Sample(Assembly.Reverse != _reversing ? overscan : tube.Length - overscan).Position);
                var speed = Assembly.CurrentSpeed.Raw;
                var deceleration = Assembly.Deceleration;
                var brakingTime = speed / deceleration;
                var brakingDistance = deceleration * brakingTime * brakingTime * 0.5f;
                return Vector3.Distance(stopPoint, GetFrontPosition(_reversing)) <= brakingDistance * World.MetersToWorld;
            }
        }

        private bool IsInDockReverse => (Parent.Route.Reverse ? Assembly.BackModule : Assembly.FrontModule).Thruster.Tube is Dock;

        private void Update()
        {
            if (!IsInService)
                return;
            switch (Controller.State)
            {
                case ShipState.Docked:
                    _entryRequested = _stopping = _reversing = false;
                    Assembly.Reverse = Parent.Route.Reverse;
                    UpdateDocked();
                    break;
                case ShipState.WaitingForDeparture:
                    if (Controller.CanLiftOff)
                        Controller.LiftOff();
                    break;
                case ShipState.Sailing:
                    UpdateSailing();
                    break;
            }
        }

        private void UpdateDocked()
        {
            if (IsTerminus)
                return;
            if (_departed)
            {
                var stay = Mathf.Max(MinStaySeconds, Parent.Target is IntermediateStop {MinStayMinutes: var minStay} ? minStay * 60 : 0);
                var departIn = Parent.Target is IDeparture {Departure: var departure} ? departure.Value - Clock.Now : TimeSpan.Zero;
                Controller.TimeToDeparture = (float) departIn.TotalSeconds;
                _remainingWait = Mathf.Max(Controller.TimeToDeparture, stay);
                _departed = false;
            }

            if ((_remainingWait -= Clock.Delta) > 0)
            {
                Controller.TimeToDeparture = _remainingWait;
                return;
            }

            Controller.MarkReady();
            _departed = false;
            if (!Assembly.FrontModule.Thruster.Tube.Next(Assembly.Reverse))
                Destroy(Assembly.gameObject);
        }

        private void UpdateSailing()
        {
            if (!_departed)
            {
                if (!Controller.CanProceed)
                    return;
                Assembly.SetTargetSpeed(Assembly.MaxSpeed.Limit(Assembly.NextTube().SpeedLimit));
                _departed = true;
                return;
            }

            if (_stopping || ShouldStop)
            {
                if (!Controller.CanLand || _reversing && !IsInDockReverse)
                    StopOrReverse();
                else
                    Controller.Land();
                return;
            }

            var tube = Assembly.FrontModule.Thruster.Tube;
            if (!_entryRequested && tube.TryGetEntryEnsurer(Assembly.Reverse, out var ensurer))
                Enter(ensurer);
            if (Controller.CanProceed && !_stopping)
                Assembly.SetTargetSpeed(Assembly.MaxSpeed.Limit(tube.SpeedLimit));
        }

        private void StopOrReverse()
        {
            _stopping = true;
            var stop = _reversing ? IsInDockReverse : !Assembly.IsStationary();
            if (stop)
            {
                Assembly.SetTargetSpeed(0);
                return;
            }

            _reversing = true;
            Assembly.Reverse = !Parent.Route.Reverse;
            Assembly.SetTargetSpeed(ReversingSpeed);
        }

        private void Enter(EntryEnsurer ensurer)
        {
            if (Parent.Target == null || ensurer.station != Parent.Target.Station)
                return;
            if (LoadingProgress.Current != null && !Parent.Target.Station.IsLoaded() && !Assembly.IsPlayerMounted)
            {
                Destroy(gameObject);
                return;
            }

            var list = Assembly.FrontModule.Cosmos.EntryList;
            if (list.isActiveAndEnabled)
            {
                _entryRequested = list.SelectDock(Parent.Target.DockIndex);
                return;
            }

            _entryRequested = false;
            foreach (var entry in ensurer.Entries)
            {
                if (entry.Dock.Index != Parent.Target.DockIndex)
                    continue;
                _entryRequested = entry.Lock(Assembly);
                break;
            }
        }

        public override void OnRouteChanged() => _departed = true;

        private void OnEnable() => _departed = true;

    }

}
