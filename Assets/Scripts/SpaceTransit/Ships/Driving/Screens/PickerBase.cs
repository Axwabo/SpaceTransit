using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class PickerBase : MonoBehaviour
    {

        private int _index;

        [SerializeField]
        protected TextMeshProUGUI text;

        [SerializeField]
        private Image background;

        private static readonly Color SelectedColor = new(1f, 1f, 0f, 0.24f);

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                Text = $"{value + 1}";
            }
        }

        public bool Selected
        {
            get => background.color == SelectedColor;
            set => background.color = value ? SelectedColor : Color.clear;
        }

        protected Color BackgroundColor
        {
            set => background.color = value;
        }

        protected abstract string Text { set; }

    }

}
