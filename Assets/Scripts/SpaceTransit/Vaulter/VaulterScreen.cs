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

        private float _exitCooldown;

        private VisualElement _root;

        private VisualElement _confirmation;

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
            _confirmation?.SetVisibility(false);
            _exitCooldown = 0;
            routes.enabled = false;
            stops.enabled = false;
        }

        private void Update()
        {
            if (_routesShown)
                return;
            if (IsDockedAt<IDestination>())
                Show(true, false);
            if ((_exitCooldown -= Clock.Delta) <= 0)
                _confirmation.SetVisibility(false);
        }

        private bool IsDockedAt<T>() where T : ITarget => Parent.Target is T {Station: var station}
                                                          && Controller.State == ShipState.Docked
                                                          && Assembly.FrontModule.Thruster.Tube is Dock dock
                                                          && dock.Station.ID == station;

        protected override void OnInitialized()
        {
            _root = this.RootVisual();
            _title = _root.Q<Label>("Title");
            _confirmation = _root.Q<Label>("Cancel");
            Restartable = new RestartableScreen(_root);
            routes.Screen.Initialize();
            stops.Screen.Initialize();
            if (Parent.initialRoute)
                return;
            Show(!HasJourney, true);
            routes.Refresh(Assembly.startTube);
        }

        public override void OnRouteChanged() => Show(!HasJourney, false);

        public override void OnTargetChanged() => HideConfirmation();

        public override void OnRestarting() => Restartable.BeginRestart();

        public override void OnRestarted() => Restartable.EndRestart();

        private void Show(bool showRoutes, bool force)
        {
            if (!force && _routesShown == showRoutes)
                return;
            _routesShown = showRoutes;
            _current = showRoutes ? routes.Screen : stops.Screen;
            _title.text = showRoutes ? "Pick a Route" : RouteList.Format(Parent.Journey);
            routes.enabled = showRoutes;
            stops.enabled = !showRoutes;
            HideConfirmation();
        }

        private void HideConfirmation()
        {
            if (_exitCooldown <= 0)
                return;
            _confirmation.SetVisibility(false);
            _exitCooldown = 0;
        }

        public void Navigate(bool forwards)
        {
            if (Controller.IsRestarting)
                return;
            _current.Navigate(forwards);
            if (!_routesShown)
                HideConfirmation();
        }

        public void Confirm()
        {
            if (Controller.IsRestarting)
                return;
            if (!_routesShown && stops.Screen.IsAtZeroOffset && IsDockedAt<IOrigin>())
            {
                ToggleExitConfirmation();
                return;
            }

            _current.Confirm();
        }

        private void ToggleExitConfirmation()
        {
            if (_exitCooldown > 0)
            {
                HideConfirmation();
                Parent.ExitService();
                return;
            }

            _exitCooldown = 5;
            _confirmation.SetVisibility(true);
        }

    }

}
