using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules.Displays;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class CosmosScreen : ModuleUIComponent, ICullingListener
    {

        [SerializeField]
        private EntryListManager dockList;

        [SerializeField]
        private ExitListManager exitList;

        private Label _text;

        private ScreenBase _current;

        private VisualElement _root;

        private bool _disabled;

        private bool _exitsShown = true;

        private void OnEnable()
        {
            if (_root == null)
                return;
            _root.SetVisibility(true);
            ForceShow(_exitsShown);
        }

        private void OnDisable()
        {
            _root?.SetVisibility(false);
            dockList.enabled = false;
            exitList.enabled = false;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ForceShow(true);
        }

        protected override void Initialize(VisualElement root)
        {
            _root = root;
            _text = root.Q<Label>("Title");
        }

        public override void OnStateChanged() => Show(Assembly.FrontModule.Thruster.Tube is Dock && Controller.State is ShipState.Docked or ShipState.WaitingForDeparture);

        public void Select(int index)
        {
            _current.Select(index);
            _current.Confirm();
        }

        private void ForceShow(bool exits)
        {
            _current = exits ? exitList.Screen : dockList.Screen;
            _text.text = exits ? "Exit Towards" : "Enter Dock";
            _exitsShown = exits;
            dockList.enabled = !exits;
            exitList.enabled = exits;
        }

        public void Show(bool exits)
        {
            if (_exitsShown != exits)
                ForceShow(exits);
        }

    }

}
