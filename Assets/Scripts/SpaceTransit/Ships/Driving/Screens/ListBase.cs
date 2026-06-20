using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ListBase<T> : ScreenBase
    {

        protected ListView List { get; private set; }

        public List<T> Source { get; } = new();

        public int Selected
        {
            get => List.selectedIndex;
            private set => List.selectedIndex = value;
        }

        protected override void Initialize(VisualElement root)
        {
            List = GetListView(root);
            List.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            List.itemsSource = Source;
            List.bindItem = BindItem;
        }

        public override void Navigate(bool forwards)
        {
            var previous = Selected;
            var index = previous + (forwards ? 1 : -1);
            if (index >= Source.Count)
                index = 0;
            else if (index < 0)
                index = Source.Count - 1;
            Selected = index;
        }

        public override void SetVisibility(bool visible) => List?.SetVisibility(visible);

        protected abstract ListView GetListView(VisualElement root);

        protected abstract void BindItem(VisualElement element, int i);

        public void Clear()
        {
            if (Source.Count == 0)
                return;
            Source.Clear();
            Refresh();
        }

        public void Refresh() => List.RefreshItems();

    }

}
