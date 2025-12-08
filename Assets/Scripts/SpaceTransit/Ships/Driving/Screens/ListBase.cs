using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ListBase<TItem, TPicker> : ScreenBase where TPicker : PickerBase
    {

        [SerializeReference]
        private TPicker prefab;

        protected List<TPicker> Pickers { get; } = new();

        protected abstract IReadOnlyList<TItem> Source { get; }

        protected abstract string GetContent(TItem item);

        public int Selected => Pickers.FindIndex(static e => e.Selected);

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

        protected void Clear()
        {
            foreach (var picker in Pickers)
                Destroy(picker.gameObject);
            Pickers.Clear();
        }

        protected void SetUp()
        {
            for (var i = 0; i < Source.Count; i++)
            {
                var clone = Instantiate(prefab, Transform);
                clone.Index = i;
                clone.Text = GetContent(Source[i]);
                Pickers.Add(clone);
            }
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
        }

        protected bool Select(int index) => index != -1 && Pickers.Count != 0 && Select(Source[index], Pickers[index]);

        protected abstract bool Select(TItem item, TPicker picker);

        public override void Confirm() => Select(Selected);

    }

}
