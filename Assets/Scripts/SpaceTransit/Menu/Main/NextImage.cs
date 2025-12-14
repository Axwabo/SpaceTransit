using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu.Main
{

    public sealed class NextImage : AutoRegisterButton
    {

        [SerializeField]
        private Image image;

        [SerializeField]
        private Sprite[] sprites;

        private int _index = -1;

        private void Start() => Click();

        protected override void Click()
        {
            if (++_index >= sprites.Length)
                _index = 0;
            image.sprite = sprites[_index];
        }

    }

}
