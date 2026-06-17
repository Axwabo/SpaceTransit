using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Driving.Screens;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterScreen : VaulterComponentBase
    {

        [SerializeField]
        private RouteList routes;

        [SerializeField]
        private StopList stops;

        private Label _title;

        private bool _routesVisible = true;

        private void Update()
        {
            if (!_routesVisible
                && Parent.Stop is Destination
                && Controller.State == ShipState.Docked
                && Assembly.FrontModule.Thruster.Tube is Dock dock
                && dock.Station.ID == Parent.Stop.Station)
                ShowRoutes();
        }

        protected override void OnInitialized() => _title = this.RootVisual().Q<Label>("Title");

        public override void OnRouteChanged()
        {
            if (!IsInService)
            {
                ShowRoutes();
                return;
            }

            _routesVisible = false;
            routes.SetVisibility(false);
            stops.SetVisibility(true);
            stops.Enable();
            _title.text = $"{Parent.Route.name} {Parent.Route.Summary()}";
        }

        private void ShowRoutes()
        {
            _routesVisible = true;
            routes.SetVisibility(true);
            routes.Enable();
            stops.SetVisibility(false);
            _title.text = "Pick a Route";
        }

        public void Navigate(bool forwards)
        {
            if (_routesVisible)
                routes.Navigate(forwards);
            else
                stops.Navigate(forwards);
        }

        public void Confirm()
        {
            if (_routesVisible)
                routes.Confirm();
            else
                stops.ResetPosition();
        }

    }

}
