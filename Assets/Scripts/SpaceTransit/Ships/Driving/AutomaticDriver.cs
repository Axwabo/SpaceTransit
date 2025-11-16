using System;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class AutomaticDriver : VaulterComponentBase
    {

        private const int MinStaySeconds = 15;
        private const float DefaultOverscan = 15;

        private float _remainingWait;

        private bool _departed;

        private bool ShouldStop
        {
            get
            {
                if (!Station.TryGetLoadedStation(Parent.Stop.Station, out var station))
                    return false;
                var tube = station.Docks[Parent.Stop.DockIndex].Tube;
                var overscan = DefaultOverscan * Time.timeScale;
                var stopPoint = World.Current.TransformPoint(tube.Sample(Assembly.Reverse ? overscan : tube.Length - overscan).Position);
                var speed = Assembly.CurrentSpeed.Raw;
                var deceleration = Assembly.Deceleration;
                var brakingTime = speed / deceleration;
                var brakingDistance = deceleration * brakingTime * brakingTime * 0.5f;
                return Vector3.Distance(stopPoint, FrontPosition) <= brakingDistance * World.MetersToWorld;
            }
        }

        private void Update()
        {
            if (!IsInService)
                return;
            switch (Controller.State)
            {
                case ShipState.Docked:
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
                var stay = Mathf.Max(MinStaySeconds, Parent.Stop is IntermediateStop {MinStayMinutes: var minStay} ? minStay * 60 : 0);
                var departIn = Parent.Stop is IDeparture {Departure: var departure} ? departure.Value - Clock.Now : TimeSpan.Zero;
                _remainingWait = Mathf.Max((float) departIn.TotalSeconds, stay);
                _departed = false;
            }

            if ((_remainingWait -= Clock.Delta) > 0)
                return;
            Controller.MarkReady();
            _departed = false;
        }

        private void UpdateSailing()
        {
            if (!_departed)
            {
                Assembly.SetSpeed(Assembly.MaxSpeed.Limit(Assembly.NextTube().SpeedLimit));
                _departed = true;
                return;
            }

            if (Assembly.TargetSpeed.Raw != 0 && ShouldStop)
            {
                Assembly.SetSpeed(0);
                return;
            }

            if (Controller.CanLand)
            {
                Controller.Land();
                return;
            }

            if (Assembly.TargetSpeed.Raw != 0)
                Assembly.SetSpeed(Assembly.MaxSpeed.Limit(Assembly.FrontModule.Thruster.Tube.SpeedLimit));
        }

        public override void OnRouteChanged() => _departed = true;

    }

}
