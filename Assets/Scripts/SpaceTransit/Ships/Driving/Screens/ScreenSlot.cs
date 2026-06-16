using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules.Displays;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class ScreenSlot : ModuleUIComponent
    {

        [SerializeField]
        private ScreenBase dockList;

        [SerializeField]
        private ScreenBase exitList;

        private Label _text;

        private ScreenBase _current;

        private bool _disabled;

        private bool _exitsShown = true;

        protected override void Awake()
        {
            base.Awake();
            _current = exitList;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            dockList.SetVisibility(false);
        }

        protected override void Initialize(VisualElement root)
        {
            _text = root.Q<Label>("Title");
            _text.text = "Exit Towards";
        }

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
            _current = exits ? exitList : dockList;
            _text.text = exits ? "Exit Towards" : "Enter Dock";
            _exitsShown = exits;
            dockList.SetVisibility(!exits);
            exitList.SetVisibility(exits);
        }

    }

}
