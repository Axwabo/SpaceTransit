using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class ScreenSlot : ModuleComponentBase
    {

        [SerializeField]
        private ScreenBase dockList;

        [SerializeField]
        private ScreenBase routes;

        public ScreenBase Current { get; private set; }

        private bool _wasDockShown = true;

        private bool _routesDisabled;

        protected override void Awake()
        {
            base.Awake();
            Current = dockList;
        }

        public override void OnStateChanged()
        {
            var dock = State != ShipState.Docked || Controller.TryGetVaulter(out var vaulter) && vaulter.Stop is not Destination;
            if (_wasDockShown == dock)
                return;
            _wasDockShown = dock;
            dockList.gameObject.SetActive(dock);
            if (_routesDisabled)
                routes.gameObject.SetActive(!dock);
            Current = dock ? dockList : routes;
        }

        private void Update()
        {
            if (_routesDisabled)
                return;
            _routesDisabled = true;
            routes.gameObject.SetActive(!_wasDockShown);
        }

    }

}
