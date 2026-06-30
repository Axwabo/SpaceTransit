using System;
using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Audio;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterController : ShipComponentBase
    {

        private const int OutOfService = -2;
        private const int Origin = -1;
        private const int Destination = int.MaxValue;

        private int _targetIndex = OutOfService;

        private VaulterComponentBase[] _components;

        private ITarget[] _targets = Array.Empty<ITarget>();

        private StationId _passingThroughDock;

        [SerializeField]
        public RouteDescriptor initialRoute;

        [SerializeField]
        public int initialStopIndex = Origin;

        public RouteDescriptor Route { get; private set; }

        public Stop Stop { get; private set; }

        public ITarget Target { get; private set; }

        public bool IsInService => _targetIndex != OutOfService;

        public ReadOnlySpan<ITarget> NextTargets => _targetIndex == Destination
            ? ReadOnlySpan<ITarget>.Empty
            : _targets[(_targetIndex + 1)..];

        public OnboardAnnouncer Announcer => _components.OfType<OnboardAnnouncer>().First();

        public string AnnouncerName => Announcer.announcer;

        public bool SkipConditionalStop(StationId conditional) => !Parent.StopRequested && (!Station.TryGetLoadedStation(conditional, out var station) || !station.PassengersWaiting);

        protected override void Awake() => _components = GetComponentsInChildren<VaulterComponentBase>(true);

        protected override void OnInitialized()
        {
            foreach (var component in _components)
                component.Initialize(this);
            if (!initialRoute)
                return;
            Stop origin = initialStopIndex == Origin ? initialRoute.Origin : initialRoute.IntermediateStops[initialStopIndex];
            if (!Station.TryGetLoadedStation(origin.Station, out var station))
                return;
            if (!Assembly.startTube)
                Assembly.startTube = station.Docks[origin.DockIndex];
            BeginRoute(initialRoute, initialStopIndex);
        }

        public void ExitService()
        {
            if (!IsInService)
                return;
            _targetIndex = OutOfService;
            Route = null;
            Stop = null;
            Target = null;
            NotifyRouteChanged();
        }

        public void BeginRoute(RouteDescriptor descriptor, int stopIndex = Origin)
        {
            Route = descriptor;
            Assembly.Reverse = descriptor.Reverse;
            var targets = new List<ITarget>();
            foreach (var stop in descriptor.IntermediateStops)
                targets.Add(stop);
            var passthroughList = descriptor.Passthrough;
            for (var i = passthroughList.Length - 1; i >= 0; i--)
            {
                var passthrough = passthroughList[i];
                targets.Insert(targets.FindIndex(e => e.Station == passthrough.AfterStop) + 1, passthrough);
            }

            _targets = targets.ToArray();
            UpdateTarget(stopIndex == Origin ? Origin : targets.IndexOf(descriptor.IntermediateStops[stopIndex]));
            NotifyRouteChanged();
        }

        private void UpdateTarget(int index)
        {
            var stopChanged = false;
            _targetIndex = index;
            Target = index switch
            {
                OutOfService => null,
                Origin => Route.Origin,
                Destination => Route.Destination,
                _ => _targets[index]
            };
            Stop = Target as Stop ?? index switch
            {
                OutOfService => null,
                Origin => Route.Origin,
                _ => NextStop(index, out stopChanged)
            };
            foreach (var component in _components)
                component.OnTargetChanged();
            if (!stopChanged)
                return;
            foreach (var component in _components)
                component.OnStopChanged();
        }

        private Stop NextStop(int index, out bool stopChanged)
        {
            for (var i = index; i < _targets.Length; i++)
                if (_targets[i] is Stop stop)
                {
                    stopChanged = Stop != stop;
                    return stop;
                }

            stopChanged = Stop != Route.Destination;
            return Route.Destination;
        }

        public override void OnStateChanged(ShipState previousState)
        {
            if (_targetIndex is not (OutOfService or Destination)
                && Parent.State is ShipState.LiftingOff or ShipState.Sailing
                && Assembly.FrontModule.Thruster.Tube is Dock {Station: var station} && station.ID == Target?.Station)
                AdvanceTarget();
        }

        public override void OnRestarting()
        {
            foreach (var component in _components)
                component.OnRestarting();
        }

        public override void OnRestarted()
        {
            foreach (var component in _components)
                component.OnRestarted();
        }

        private void AdvanceTarget() => UpdateTarget(_targetIndex >= _targets.Length - 1 ? Destination : _targetIndex + 1);

        private void NotifyRouteChanged()
        {
            foreach (var component in _components)
                component.OnRouteChanged();
        }

        private void Update()
        {
            switch (Target)
            {
                case IntermediateStop {Conditional: true, Station: var conditionalStation} when Parent.State == ShipState.Sailing && !Assembly.IsStationary():
                {
                    if (Assembly.FrontModule.Thruster.Tube is Dock dock && dock.Station.ID == conditionalStation && SkipConditionalStop(conditionalStation))
                        AdvanceTarget();
                    break;
                }
                case Passthrough passthrough:
                {
                    var tube = Assembly.FrontModule.Thruster.Tube;
                    if (tube is Dock dock)
                    {
                        if (dock.Station.ID == passthrough.Station)
                            _passingThroughDock = passthrough.Station;
                        return;
                    }

                    if (!_passingThroughDock)
                        return;
                    if (_passingThroughDock == passthrough.Station)
                        AdvanceTarget();
                    _passingThroughDock = null;
                    break;
                }
            }
        }

    }

}
