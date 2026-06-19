using System.Collections.Generic;
using Mono.Collections.Generic;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ListBase<T> : ScreenBase where T : PickerBase
    {

        private ListView _list;

        protected ListView List
        {
            get
            {
                if (_list != null)
                    return _list;
                _list = GetListView(this.RootVisual());
                _list.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _list.makeItem = () => new Label();
                _list.bindItem = (element, i) =>
                {
                    var picker = Source[i];
                    var label = element.Q<Label>();
                    label.text = GetContent(picker);
                    label.EnableInClassList("succeeded", picker.Success);
                    label.EnableInClassList("failed", picker.Failure);
                };
                _list.itemsSource = Source;
                return _list;
            }
        }

        protected override void Initialize(VisualElement root) => _ = List;

        protected abstract ListView GetListView(VisualElement root);

        protected List<T> Source { get; } = new();

        protected abstract string GetContent(T item);

        public int Selected
        {
            get => List.selectedIndex;
            private set => List.selectedIndex = value;
        }

        protected bool HasPicked
        {
            get
            {
                foreach (var picker in Source)
                    if (picker.Success)
                        return true;
                return false;
            }
        }

        protected bool CanPick => !Equals(List.itemsSource, ReadOnlyCollection<T>.Empty) && Source is {Count: not 0};

        protected void Clear()
        {
            Source.Clear();
            Refresh();
        }

        protected void Refresh() => List.RefreshItems();

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

        public override bool Select(int index)
        {
            if (index == -1 || !CanPick || index >= Source.Count)
                return false;
            var success = Select(Source[index]);
            _refreshingState = success;
            _list.RefreshItem(index);
            _refreshingState = null;
            return success;
        }

        protected abstract bool Select(T item);

        public override void Confirm() => Select(Selected);

        public override void SetVisibility(bool visible) => List.SetVisibility(visible);

    }

}
