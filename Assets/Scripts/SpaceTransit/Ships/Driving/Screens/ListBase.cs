using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ListBase<TItem, TPicker> : ScreenBase where TPicker : PickerBase
    {

        [SerializeField]
        private TPicker prefab;

        private readonly List<TPicker> _list = new();

        protected abstract IReadOnlyList<TItem> Source { get; }

        protected abstract string GetContent(int index, TItem item);

        public int Selected => _list.FindIndex(static e => e.Selected);

        protected void Clear()
        {
            foreach (var picker in _list)
                Destroy(picker.gameObject);
            _list.Clear();
        }

        protected void SetUp()
        {
            for (var i = 0; i < Source.Count; i++)
            {
                var clone = Instantiate(prefab, Transform);
                clone.Index = i;
                clone.Text = GetContent(i, Source[i]);
                _list.Add(clone);
            }
        }

        public override void Navigate(bool forwards)
        {
            if (_list.Count == 0)
                return;
            var previous = Selected;
            var index = previous + (forwards ? 1 : 0);
            if (index >= _list.Count)
                index = 0;
            else if (index < 0)
                index = _list.Count - 1;
            if (previous != -1 && previous != index)
                _list[previous].Selected = false;
            _list[index].Selected = true;
        }

        public bool Select(int index) => index != -1 && _list.Count != 0 && Select(Source[index], _list[index]);

        protected abstract bool Select(TItem item, TPicker picker);

        public override void Confirm() => Select(Selected);

    }

}
