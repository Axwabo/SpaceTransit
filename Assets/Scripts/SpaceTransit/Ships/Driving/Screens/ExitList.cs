using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(ExitListManager))]
    public sealed class ExitList : PickableList<ExitPicker>
    {

        private ExitListManager _manager;

        private bool _loaded;

        private Label _text;

        private void Awake() => _manager = GetComponent<ExitListManager>();

        protected override void Select(ExitPicker item)
        {
            if (HasPicked || _manager.State != ShipState.Docked)
                return;
            if (_manager.Assembly.Lock(item))
                _text.text = "Exiting Towards";
        }

        protected override string GetContent(ExitPicker item) => item.Exit.Connected.name;

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Exits");

        protected override void Initialize(VisualElement root)
        {
            base.Initialize(root);
            _text = root.Q<Label>("ExitTitle");
        }

        public override void Refresh()
        {
            base.Refresh();
            _text.text = "Exit Towards";
        }

        public override void SetVisibility(bool visible)
        {
            base.SetVisibility(visible);
            _text?.SetVisibility(visible);
        }

    }

}
