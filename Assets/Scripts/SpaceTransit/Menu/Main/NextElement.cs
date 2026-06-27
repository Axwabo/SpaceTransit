using System.Linq;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu.Main
{

    public sealed class NextElement : AutoRegisterButton
    {

        private VisualElement _container;

        private VisualElement _previous;

        private int _index = -1;

        protected override void Start()
        {
            base.Start();
            _container = this.RootVisual().Q<VisualElement>("Container");
            Click();
        }

        protected override void Click()
        {
            if (++_index >= _container.childCount)
                _index = 0;
            _previous?.RemoveFromClassList("active");
            _previous = _container.Children().ElementAt(_index);
            _previous.AddToClassList("active");
        }

    }

}
