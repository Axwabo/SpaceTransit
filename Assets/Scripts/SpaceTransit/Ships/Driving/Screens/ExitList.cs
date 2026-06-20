using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(CosmosScreen))]
    public sealed class ExitList : PickableList<ExitPicker>
    {

        private CosmosScreen _screen;

        private bool _loaded;

        private Label _text;

        public string Text
        {
            set => _text.text = value;
        }

        private void Awake() => _screen = GetComponent<CosmosScreen>();

        protected override void Select(ExitPicker item)
        {
            if (HasPicked || _screen.State != ShipState.Docked)
                return;
            if (_screen.Assembly.Lock(item))
                Text = "Exiting Towards";
        }

        protected override string GetContent(ExitPicker item) => item.Exit.Connected.name;

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Exits");

        protected override void Initialize(VisualElement root)
        {
            base.Initialize(root);
            _text = root.Q<Label>("ExitTitle");
        }

        public override void SetVisibility(bool visible)
        {
            base.SetVisibility(visible);
            _text?.SetVisibility(visible);
        }

        public override void Refresh()
        {
            base.Refresh();
            SetVisibility(Source.Count != 0);
        }

    }

}
