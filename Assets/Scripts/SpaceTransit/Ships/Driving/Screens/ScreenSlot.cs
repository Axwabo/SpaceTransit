using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class ScreenSlot : ModuleComponentBase
    {

        [SerializeField]
        private ScreenBase dockList;

        [SerializeField]
        private ScreenBase exitList;

        [SerializeField]
        private TextMeshProUGUI text;

        private ScreenBase _current;

        private bool _disabled;

        private bool _exitsShown = true;

        protected override void Awake()
        {
            base.Awake();
            _current = exitList;
            text.text = "Exit Towards";
        }

        protected override void OnInitialized() => dockList.gameObject.SetActive(false);

        public override void OnStateChanged() => Show(Assembly.FrontModule.Thruster.Tube is Dock && Controller.State is ShipState.Docked or ShipState.WaitingForDeparture);

        public void Select(int index)
        {
            _current.Select(index);
            _current.Confirm();
        }

        public void Show(bool exits)
        {
            if (_exitsShown == exits)
                return;
            text.text = exits ? "Exit Towards" : "Enter Dock";
            _exitsShown = exits;
            dockList.gameObject.SetActive(!exits);
            exitList.gameObject.SetActive(!exits);
        }

    }

}
