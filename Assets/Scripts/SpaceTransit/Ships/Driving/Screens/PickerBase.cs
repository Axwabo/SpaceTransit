using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class PickerBase : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Image background;

        private static readonly Color SelectedColor = new(1f, 1f, 0f, 0.24f);

        public int Index { get; set; }

        public string Text
        {
            set => text.text = value;
        }

        public bool Selected
        {
            get => background.color == SelectedColor;
            set => background.color = value ? SelectedColor : Color.clear;
        }

        protected Color BackgroundColor
        {
            get => background.color;
            set => background.color = value;
        }

        public virtual bool Picked => false;

    }

}
