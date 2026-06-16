using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public class PickerBase
    {

        private VisualElement _element;

        private Label _label;

        private static readonly Color SelectedColor = new(1f, 1f, 0f, 0.24f);

        public void Bind(VisualElement element)
        {
            _element = element;
            _label = element.Q<Label>();
        }

        public int Index { get; set; }

        public string Text
        {
            set => _label.text = value;
        }

        public bool Selected
        {
            get => BackgroundColor == SelectedColor;
            set => BackgroundColor = value ? SelectedColor : Color.clear;
        }

        protected Color BackgroundColor
        {
            get => _element.style.backgroundColor.value;
            set => _element.style.backgroundColor = value;
        }

        public virtual bool Picked => false;

    }

}
