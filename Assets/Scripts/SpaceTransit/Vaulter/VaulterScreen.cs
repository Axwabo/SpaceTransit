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

        public RestartableScreen Restartable { get; private set; }

        private void OnEnable()
        {
            if (_root == null)
                return;
            _root.SetVisibility(true);
            Show(_routesShown, true);
        }

        private void OnDisable()
        {
            _root?.SetVisibility(false);
            routes.enabled = false;
            stops.enabled = false;
        }

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
            Restartable = new RestartableScreen(_root);
            routes.Screen.Initialize();
            stops.Screen.Initialize();
            if (Parent.initialRoute)
                return;
            Show(!IsInService, true);
            routes.Refresh(Assembly.startTube);
        }

        public override void OnRouteChanged() => Show(!IsInService, false);

        public override void OnRestarting() => Restartable.BeginRestart();

        public override void OnRestarted() => Restartable.EndRestart();

        private void Show(bool showRoutes, bool force)
        {
            if (!force && _routesShown == showRoutes)
                return;
            _routesShown = showRoutes;
            _current = showRoutes ? routes.Screen : stops.Screen;
            _title.text = showRoutes ? "Pick a Route" : $"{Parent.Route.name} {Parent.Route.Summary()}";
            routes.enabled = showRoutes;
            stops.enabled = !showRoutes;
        }

        public void Navigate(bool forwards)
        {
            if (!Controller.IsRestarting)
                _current.Navigate(forwards);
        }

        public void Confirm()
        {
            if (!Controller.IsRestarting)
                _current.Confirm();
        }

    }

}
