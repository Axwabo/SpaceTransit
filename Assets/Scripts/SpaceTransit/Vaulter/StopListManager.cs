using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships.Driving.Screens;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(StopList))]
    public sealed class StopListManager : VaulterComponentBase
    {

        public StopList Screen { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Screen = GetComponent<StopList>();
        }

        private void OnEnable() => Screen.SetVisibility(true);

        private void OnDisable() => Screen.SetVisibility(false);

        protected override void OnInitialized() => Screen.Initialize();

        public override void OnRouteChanged()
        {
            RefreshStops();
            Screen.Refresh();
        }

        public override void OnTargetChanged()
        {
            if (Screen.Source.Count == 0)
                return;
            Screen.Source.RemoveRange(0, Screen.Source.Count == 1 || Screen.Source[1] is ExitTowards ? 2 : 1);
            Screen.Refresh();
        }

        private void RefreshStops()
        {
            Screen.ResetPosition();
            Screen.Source.Clear();
            if (!IsInService)
                return;
            Add(Parent.Target);
            if (Parent.Target is Destination)
                return;
            foreach (var stop in Parent.NextTargets)
                Add(stop);
            Add(Parent.Route.Destination);
        }

        private void Add(ITarget target)
        {
            Screen.Source.Add(new StopListEntry(target));
            if (target is IExitTowards exitTowards && exitTowards.ExitTowards)
                Screen.Source.Add(new ExitTowards(exitTowards));
        }

    }

}
