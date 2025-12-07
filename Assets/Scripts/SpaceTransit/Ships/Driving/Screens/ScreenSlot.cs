using System;
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

        [SerializeField]
        private ScreenBase exitList;

        public ScreenBase Current { get; private set; }

        private Slot _previous = Slot.Exits;

        private bool _disabled;

        protected override void Awake()
        {
            base.Awake();
            Current = exitList;
        }

        public override void OnStateChanged() => Show(
            State != ShipState.Docked
                ? Slot.Docks
                : Controller.TryGetVaulter(out var vaulter) && (!vaulter.IsInService || vaulter.Stop is not Destination)
                    ? Slot.Exits
                    : Slot.Routes
        );

        public void Show(Slot slot)
        {
            if (_previous == slot)
                return;
            Current = slot switch
            {
                Slot.Docks => dockList,
                Slot.Routes => routes,
                Slot.Exits => exitList,
                _ => throw new ArgumentOutOfRangeException()
            };
            _previous = slot;
            exitList.gameObject.SetActive(slot == Slot.Exits);
            if (!_disabled)
                return;
            routes.gameObject.SetActive(_previous == Slot.Routes);
            dockList.gameObject.SetActive(_previous == Slot.Docks);
        }

        private void Update()
        {
            if (_disabled)
                return;
            _disabled = true;
            routes.gameObject.SetActive(_previous == Slot.Routes);
            dockList.gameObject.SetActive(_previous == Slot.Docks);
            enabled = false;
        }

        public enum Slot
        {

            Docks,
            Routes,
            Exits

        }

    }

}
