using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(EntryListManager))]
    public sealed class EntryList : PickableList<EntryPicker>
    {

        private EntryListManager _manager;

        private void Awake() => _manager = GetComponent<EntryListManager>();

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
            if (!HasPicked)
                _manager.Assembly.Lock(item);
        }

        protected override string GetContent(EntryPicker item) => $"{item.Entry.Dock.Index + 1}";

    }

}
