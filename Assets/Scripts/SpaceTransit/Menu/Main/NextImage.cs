using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu.Main
{

    public sealed class NextImage : AutoRegisterButton
    {

        [SerializeField]
        private Sprite[] sprites;

        private VisualElement _element;

        private int _index = -1;

        protected override void Start()
        {
            base.Start();
            _element = this.RootVisual().Q<VisualElement>("Image");
            Click();
        }

        protected override void Click()
        {
            if (++_index >= sprites.Length)
                _index = 0;
            _element.style.backgroundImage = new StyleBackground(sprites[_index]);
        }

    }

}
