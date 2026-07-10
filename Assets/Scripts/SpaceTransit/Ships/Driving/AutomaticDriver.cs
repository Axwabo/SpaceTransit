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

        private const float ConditionalSpeed = 40 * ShipSpeed.KmhToMps;
        private const float ReversingSpeed = 20 * ShipSpeed.KmhToMps;
        private const int MinStaySeconds = 15;
        private const int RestartedStaySeconds = 60;
        private const float DefaultOverscan = 15 * World.MetersToWorld;

        private float _remainingWait;

        private float _readyWait;

        private bool _departed;

        private bool _stopping;

        private bool _entryRequested;

        private bool _exitRequested;

        private bool _reversing;

        private bool ShouldStop
        {
            get
            {
                if (_exitRequested && Parent.Target is Passthrough || !Station.TryGetLoadedStation(Parent.Target.Station, out var station))
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

        private bool IsFullyInDock => Assembly.FrontModule.Thruster.Tube is Dock && Assembly.BackModule.Thruster.Tube is Dock;

        private bool IsFarFromStation => !Station.TryGetLoadedStation(Parent.Target.Station, out var station)
                                         || Vector3.Distance(station.Position, Assembly.FrontModule.Thruster.Transform.position) > 3000 * World.MetersToWorld;

        private bool SlowDownOnly => Parent.Target is IntermediateStop {Conditional: true, Station: var conditional} && Parent.SkipConditionalStop(conditional);

        private void Update()
        {
            if (!HasJourney)
                return;
            switch (Controller.State)
            {
                case ShipState.Docked:
                    _entryRequested = _exitRequested = _stopping = _reversing = false;
                    Assembly.Reverse = Parent.Journey.Reverse;
                    UpdateDocked();
                    break;
                case ShipState.WaitingForDeparture:
                    if ((_readyWait -= Clock.Delta) > 0 || !Controller.CanLiftOff)
                        break;
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
                var minStaySeconds = Controller.IsRestarting ? RestartedStaySeconds : MinStaySeconds;
                var stay = Mathf.Max(minStaySeconds, Parent.Target is IntermediateStop {MinStayMinutes: var minStay} ? minStay * 60 : 0);
                var departIn = Parent.Target is IDeparture {Departure: var departure} ? departure.Value - Clock.Now : TimeSpan.Zero;
                Controller.TimeToDeparture = (float) departIn.TotalSeconds;
                _remainingWait = Mathf.Max(Controller.TimeToDeparture, stay);
                _departed = false;
            }

            if ((_remainingWait -= Clock.Delta) > 0)
            {
                if (!Controller.IsRestarting)
                    Controller.TimeToDeparture = _remainingWait;
                return;
            }

            Controller.MarkReady();
            _departed = false;
            _readyWait = Parent.Target is Passthrough ? 10 : 0;
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
                if (!Controller.CanLand || _reversing && !IsFullyInDock)
                    StopOrReverse();
                else
                    Controller.Land();
                return;
            }

            var tube = Assembly.FrontModule.Thruster.Tube;
            if (!_entryRequested && tube.TryGetEntryEnsurer(Assembly.Reverse, out var ensurer))
                Enter(ensurer);
            if (_entryRequested && !_exitRequested && Parent.Target is Passthrough passthrough)
                Exit(passthrough);
            if (Controller.CanProceed && !_stopping)
                Assembly.SetTargetSpeed(Assembly.MaxSpeed.Limit(tube.SpeedLimit));
        }

        private void StopOrReverse()
        {
            _stopping = true;
            var stop = _reversing ? IsFullyInDock : !Assembly.IsStationary();
            if (stop)
            {
                Assembly.SetTargetSpeed(SlowDownOnly ? ConditionalSpeed : 0);
                return;
            }

            _reversing = true;
            Assembly.Reverse = !Parent.Journey.Reverse;
            Assembly.SetTargetSpeed(ReversingSpeed);
        }

        private void Enter(IEntryEnsurer ensurer)
        {
            if (Parent.Target == null || ensurer.TargetStation != Parent.Target.Station)
                return;
            if (LoadingProgress.Current != null && !Parent.Target.Station.IsLoaded() && !Assembly.IsPlayerMounted)
            {
                Destroy(gameObject);
                return;
            }

            if (IsFarFromStation && Assembly.FrontModule.Thruster.Tube.Safety is not IEntryEnsurer)
                return;

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

        private void Exit(Passthrough passthrough)
        {
            if (!Station.TryGetLoadedStation(passthrough.Station, out var station))
                return;
            var dock = station.Docks[passthrough.DockIndex];
            var exits = Parent.Journey.Reverse ? dock.BackExits : dock.FrontExits;
            if (exits.Length == 0)
            {
                _exitRequested = CanEnterPassthrough(dock);
                return;
            }

            _exitRequested = false;
            var list = Assembly.FrontModule.Cosmos.ExitList;
            foreach (var exit in exits)
            {
                if (exit.Connected != passthrough.ExitTowards)
                    continue;
                _exitRequested = exit.Lock(Assembly);
                if (_exitRequested && list.isActiveAndEnabled)
                    list.Mark(exit);
                return;
            }
        }

        private bool CanEnterPassthrough(Dock dock)
        {
            foreach (var entry in dock.FrontEntries)
                if (!entry.IsFree && !entry.IsUsedOnlyBy(Assembly))
                    return false;
            foreach (var entry in dock.BackEntries)
                if (!entry.IsFree && !entry.IsUsedOnlyBy(Assembly))
                    return false;
            return dock.Safety.IsFreeFor(Assembly);
        }

        public override void OnRouteChanged() => _departed = true;

        public override void OnTargetChanged() => _entryRequested = _exitRequested = _stopping = false;

        public override void OnRestarting() => Controller.TimeToDeparture = float.MaxValue;

        public override void OnRestarted()
        {
            if (Parent.Target is not Origin)
                return;
            _remainingWait = Mathf.Max(_remainingWait, RestartedStaySeconds);
            Controller.TimeToDeparture = _remainingWait;
        }

        private void OnEnable() => _departed = true;

    }

}
