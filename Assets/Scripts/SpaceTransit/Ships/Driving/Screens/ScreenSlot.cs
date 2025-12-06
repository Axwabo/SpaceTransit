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
        private ScreenBase um; // TODO

        public ScreenBase Current { get; private set; }

        private bool _wasDockShown;

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
            um.gameObject.SetActive(!dock);
            Current = dock ? dockList : um;
        }

    }

}
