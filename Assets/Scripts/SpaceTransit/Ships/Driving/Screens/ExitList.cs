using SpaceTransit.Cosmos;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(CosmosScreen))]
    public sealed class ExitList : PickableList<ExitPicker>
    {

        private bool _isEnabled = true;

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
            if (!HasPicked && _screen.Assembly.Lock(item))
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
            _isEnabled = visible;
        }

        public void Mark(Exit exit)
        {
            if (!_isEnabled || Source.Count == 0)
                return;
            for (var i = 0; i < Source.Count; i++)
            {
                var picker = Source[i];
                if (picker.Exit != exit)
                    continue;
                picker.Success = true;
                List.RefreshItem(i);
                Text = "Exiting Towards";
                break;
            }
        }

        public bool TryGetPicked(out Exit exit)
        {
            if (!_isEnabled)
            {
                exit = null;
                return false;
            }

            foreach (var picker in Source)
            {
                if (!picker.Success)
                    continue;
                exit = picker.Exit;
                return true;
            }

            if (Selected != -1)
            {
                exit = Source[Selected].Exit;
                return true;
            }

            exit = null;
            return false;
        }

    }

}
