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

        public override void OnRouteChanged()
        {
            RefreshStops();
            Screen.Refresh();
        }

        public override void OnStopChanged()
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
            foreach (var stop in Parent.NextIntermediateStops)
                Screen.Source.Add(stop);
            Screen.Source.Add(Parent.Route.Destination);
        }

    }

}
