using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ListBase<T> : ScreenBase where T : PickerBase
    {

        protected ListView List { get; private set; }

        protected List<T> Source { get; } = new();

        protected int Selected
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

        protected bool CanPick => Source.Count != 0 && !HasPicked;

        protected void Initialize()
        {
            List = GetListView(this.RootVisual());
            List.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            List.makeItem = () => new Label();
            List.bindItem = (element, i) =>
            {
                var picker = Source[i];
                var label = element.Q<Label>();
                label.text = GetContent(picker);
                label.EnableInClassList("succeeded", picker.Success);
                label.EnableInClassList("failed", picker.Failure);
            };
            List.itemsSource = Source;
        }

        protected abstract ListView GetListView(VisualElement root);

        protected abstract string GetContent(T item);

        protected void Clear()
        {
            Source.Clear();
            Refresh();
        }

        protected void Refresh() => List.RefreshItems();

        public override void Navigate(bool forwards)
        {
            if (!CanPick)
                return;
            var previous = Selected;
            var index = previous + (forwards ? 1 : -1);
            if (index >= Source.Count)
                index = 0;
            else if (index < 0)
                index = Source.Count - 1;
            Selected = index;
        }

        public override bool Select(int index)
        {
            if (index == -1 || !CanPick || index >= Source.Count)
                return false;
            var item = Source[index];
            Select(item);
            List.RefreshItem(index);
            return item.Success;
        }

        protected abstract void Select(T item);

        public override void Confirm() => Select(Selected);

        public override void SetVisibility(bool visible) => List.SetVisibility(visible);

    }

}
