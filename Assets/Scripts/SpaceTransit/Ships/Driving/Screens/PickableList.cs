using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class PickableList<T> : ListBase<T> where T : PickerBase
    {

        public bool HasPicked
        {
            get
            {
                foreach (var picker in Source)
                    if (picker.Success)
                        return true;
                return false;
            }
        }

        public bool CanPick => Source.Count != 0 && !HasPicked;

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

        protected override void Initialize(VisualElement root)
        {
            base.Initialize(root);
            List.makeItem = () => new Label();
        }

        public override void Navigate(bool forwards)
        {
            if (CanPick)
                base.Navigate(forwards);
        }

        protected override void BindItem(VisualElement element, int i)
        {
            var picker = Source[i];
            var label = element.Q<Label>();
            label.text = GetContent(picker);
            label.EnableInClassList("succeeded", picker.Success);
            label.EnableInClassList("failed", picker.Failure);
        }

        protected abstract string GetContent(T item);

    }

}
