using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Driving.Screens;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterScreen : VaulterComponentBase, ICullingListener
    {

        [SerializeField]
        private RouteList routes;

        [SerializeField]
        private StopList stops;

        private Label _title;

        private bool _routesVisible = true;

        private VisualElement _root;

        private void OnEnable() => _root?.SetVisibility(true);

        private void OnDisable() => _root?.SetVisibility(false);

        private void Update()
        {
            if (!_routesVisible
                && Parent.Stop is Destination
                && Controller.State == ShipState.Docked
                && Assembly.FrontModule.Thruster.Tube is Dock dock
                && dock.Station.ID == Parent.Stop.Station)
                ShowRoutes();
        }

        protected override void OnInitialized()
        {
            _root = this.RootVisual();
            _title = _root.Q<Label>("Title");
        }

        public override void OnRouteChanged()
        {
            if (!IsInService)
            {
                ShowRoutes();
                return;
            }

            _routesVisible = false;
            routes.SetVisibility(false);
            routes.enabled = false;
            stops.enabled = true;
            _title.text = $"{Parent.Route.name} {Parent.Route.Summary()}";
        }

        private void ShowRoutes()
        {
            _routesVisible = true;
            routes.enabled = true;
            stops.enabled = false;
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
