using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(CosmosScreen))]
    public sealed class EntryList : PickableList<EntryPicker>
    {

        private CosmosScreen _manager;

        private Label _text;

        public string Text
        {
            set => _text.text = value;
        }

        private void Awake() => _manager = GetComponent<CosmosScreen>();

        protected override void Initialize(VisualElement root)
        {
            base.Initialize(root);
            _text = root.Q<Label>("EnterTitle");
        }

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Entries");

        public override bool Select(int index)
        {
            for (var i = 0; i < Source.Count; i++)
                if (Source[i].Entry.Dock.Index == index)
                    return base.Select(i);
            return false;
        }

        protected override void Select(EntryPicker item)
        {
            if (HasPicked || !_manager.Assembly.Lock(item))
                return;
            Text = "Entering Dock";
            _manager.OnEntrySelected(item.Entry);
        }

        protected override string GetContent(EntryPicker item) => $"{item.Entry.Dock.Index + 1}";

        public override void SetVisibility(bool visible)
        {
            base.SetVisibility(visible);
            _text?.SetVisibility(visible);
            enabled = visible;
        }

        public bool SelectDock(int dockIndex) => CanPick && isActiveAndEnabled && Select(dockIndex);

    }

}
