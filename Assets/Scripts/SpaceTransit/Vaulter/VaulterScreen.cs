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
        private RouteListManager routes;

        [SerializeField]
        private StopListManager stops;

        private Label _title;

        private ScreenBase _current;

        private bool _routesShown = true;

        private VisualElement _root;

        private void OnEnable()
        {
            if (_root == null)
                return;
            _root.SetVisibility(true);
            Show(_routesShown, true);
        }

        private void OnDisable() => _root?.SetVisibility(false);

        private void Update()
        {
            if (!_routesShown
                && Parent.Stop is Destination
                && Controller.State == ShipState.Docked
                && Assembly.FrontModule.Thruster.Tube is Dock dock
                && dock.Station.ID == Parent.Stop.Station)
                Show(true, false);
        }

        protected override void OnInitialized()
        {
            _root = this.RootVisual();
            _title = _root.Q<Label>("Title");
        }

        public override void OnRouteChanged()
        {
            Show(IsInService, false);
            if (!IsInService)
            {
                ShowRoutes();
                return;
            }

            _routesShown = false;
            routes.enabled = false;
            stops.enabled = true;
            _title.text = $"{Parent.Route.name} {Parent.Route.Summary()}";
            _current = stops.Screen;
        }

        private void Show(bool showRoutes, bool force)
        {
            if (!force && _routesShown == showRoutes)
                return;
            _routesShown = showRoutes;
            _current = showRoutes ? routes.Screen : stops.Screen;
            _title.text = showRoutes ? "Pick a  Route" : $"{Parent.Route.name} {Parent.Route.Summary()}";
            routes.enabled = showRoutes;
            stops.enabled = !showRoutes;
        }

        private void ShowRoutes()
        {
            routes.enabled = true;
            stops.enabled = false;
            _title.text = "Pick a Route";
            _current = routes.Screen;
        }

        public void Navigate(bool forwards) => _current.Navigate(forwards);

        public void Confirm() => _current.Confirm();

    }

}
