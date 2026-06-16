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

        private GameObject _routes;

        private bool _routesVisible = true;

        protected override void Awake()
        {
            base.Awake();
            _routes = routes.gameObject;
        }

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
            _routes.SetActive(false);
            stops.SetVisibility(true);
            _title.text = $"{Parent.Route.name} {Parent.Route.Summary()}";
        }

        private void ShowRoutes()
        {
            _routesVisible = true;
            _routes.SetActive(true);
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
