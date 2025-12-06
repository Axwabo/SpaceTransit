using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Ships.Driving
{

    public sealed class DockPicker : MonoBehaviour
    {

        private static readonly Color SelectedColor = new(1f, 1f, 0f, 0.24f);
        private static readonly Color EnteringColor = new(0, 1, 0, 0.24f);
        private static readonly Color FailedColor = new(1, 0, 0, 0.24f);

        private int _index;

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Image background;

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                text.text = $"{value + 1}";
            }
        }

        public bool Selected
        {
            get => background.color == SelectedColor;
            set => background.color = value ? SelectedColor : Color.clear;
        }

        public void Pick(bool locked) => background.color = locked ? EnteringColor : FailedColor;

    }

}
