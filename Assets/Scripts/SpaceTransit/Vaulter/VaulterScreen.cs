using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Driving.Screens;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterScreen : VaulterComponentBase
    {

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private RouteList routes;

        [SerializeField]
        private StopList stops;

        private GameObject _routes;

        private GameObject _stops;

        private bool _routesVisible = true;

        protected override void Awake()
        {
            base.Awake();
            _routes = routes.gameObject;
            _stops = stops.gameObject;
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

        public override void OnRouteChanged()
        {
            if (!IsInService)
            {
                ShowRoutes();
                return;
            }

            _routesVisible = false;
            _routes.SetActive(false);
            _stops.SetActive(true);
            title.text = $"{Parent.Route.name} {Parent.Route.Summary()}";
        }

        private void ShowRoutes()
        {
            _routesVisible = true;
            _routes.SetActive(true);
            _stops.SetActive(false);
            title.text = "Pick a Route";
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
