using System.Collections.Generic;
using Mono.Collections.Generic;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ListBase<TItem, TPicker> : ScreenBase where TPicker : PickerBase, new()
    {

        private ListView _list;

        protected ListView List
        {
            get
            {
                if (_list != null)
                    return _list;
                _list = GetListView(this.RootVisual());
                _list.makeItem = () => new Label();
                _list.bindItem = (element, i) =>
                {
                    var picker = new TPicker();
                    picker.Bind(element);
                    picker.Text = GetContent(Source[i]);
                    Pickers.Add(picker);
                };
                return _list;
            }
        }

        protected override void Initialize(VisualElement root) => _ = List;

        protected abstract ListView GetListView(VisualElement root);

        protected List<TPicker> Pickers { get; } = new();

        protected abstract List<TItem> Source { get; }

        protected abstract string GetContent(TItem item);

        public int Selected
        {
            get => List.selectedIndex;
            private set => List.selectedIndex = value;
        }

        protected bool HasPicked
        {
            get
            {
                foreach (var p in Pickers)
                    if (p.Picked)
                        return true;
                return false;
            }
        }

        protected bool CanPick => Pickers.Count != 0;

        protected void Clear()
        {
            Pickers.Clear();
            List.itemsSource = ReadOnlyCollection<TItem>.Empty;
            List.RefreshItems();
        }

        protected void SetUp()
        {
            Pickers.Clear();
            Pickers.Capacity = Source.Count;
            List.itemsSource = Source;
            List.RefreshItems();
        }

        public override void Navigate(bool forwards)
        {
            if (Pickers.Count == 0 || HasPicked)
                return;
            var previous = Selected;
            var index = previous + (forwards ? 1 : -1);
            if (index >= Pickers.Count)
                index = 0;
            else if (index < 0)
                index = Pickers.Count - 1;
            if (previous != -1 && previous != index)
                Pickers[previous].Selected = false;
            Pickers[index].Selected = true;
            Selected = index;
        }

        public override bool Select(int index) => index != -1 && Pickers.Count != 0 && index < Pickers.Count && Select(Source[index], Pickers[index]);

        protected abstract bool Select(TItem item, TPicker picker);

        public override void Confirm() => Select(Selected);

        public override void SetVisibility(bool visible) => List.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

    }

}
