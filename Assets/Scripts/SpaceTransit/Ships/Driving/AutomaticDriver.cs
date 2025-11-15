using System;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class AutomaticDriver : VaulterComponentBase
    {

        private static TimeSpan Now => DateTime.Now.TimeOfDay;

        private float _remainingWait;

        private bool _departed;

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
            if ((_remainingWait -= Time.deltaTime) > 0)
                return;
            Controller.MarkReady();
            _departed = false;
        }

        private void UpdateSailing()
        {
            if (!_departed)
            {
                Assembly.SetSpeed(Assembly.MaxSpeed);
                _departed = true;
            }
            else if (Assembly.TargetSpeed.Raw != 0 && ShouldStop)
                Assembly.SetSpeed(0);
            else if (Controller.CanLand)
                Controller.Land();
        }

        private bool ShouldStop
        {
            get
            {
                if (!Station.TryGetLoadedStation(Parent.Stop.Station, out var station))
                    return false;
                var tube = station.Docks[Parent.Stop.DockIndex].Tube;
                var brakingDistance = Mathf.Pow(Assembly.CurrentSpeed.Raw, 2) * Mathf.Pow(Assembly.Deceleration, 2) * 0.5f;
                var stopPoint = World.Current.TransformPoint(tube.Sample(Assembly.Reverse ? 10 : tube.Length - 10).Position);
                return Vector3.Distance(stopPoint, FrontPosition) <= brakingDistance * World.MetersToWorld;
            }
        }

    }

}
