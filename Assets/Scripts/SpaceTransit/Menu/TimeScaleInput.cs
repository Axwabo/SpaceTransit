using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    public sealed class TimeScaleInput : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Slider slider;

        private float _previous;

        public void UpdateValue() => Time.timeScale = slider.value;

        private void Update()
        {
            var scale = Time.timeScale;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_previous == scale)
                return;
            text.text = Time.timeScale.ToString("N");
            _previous = scale;
        }

    }

}
