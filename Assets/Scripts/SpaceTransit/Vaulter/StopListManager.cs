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
            Screen.Source.RemoveAt(0);
            Screen.Refresh();
        }

        private void RefreshStops()
        {
            Screen.ResetPosition();
            Screen.Source.Clear();
            if (!IsInService)
                return;
            Screen.Source.Add(Parent.Stop);
            if (Parent.Stop is Destination)
                return;
            foreach (var stop in Parent.NextTargets)
                Screen.Source.Add(stop);
            Screen.Source.Add(Parent.Route.Destination);
        }

    }

}
