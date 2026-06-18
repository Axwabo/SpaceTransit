using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class TimeScaleInput : MonoBehaviour
    {

        private Label _text;

        private SliderInt _slider;

        private float _previous;

        private void Start()
        {
            var root = this.RootVisual();
            _text = root.Q<Label>("SpeedScalar");
            _slider = root.Q<SliderInt>("Speed");
            _slider.RegisterValueChangedCallback(Callback);
        }

        private static void Callback(ChangeEvent<int> evt) => Time.timeScale = evt.newValue;

        private void Update()
        {
            var scale = Time.timeScale;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_previous == scale)
                return;
            _text.text = $"Speed: {Time.timeScale:N0}x";
            _slider.value = (int) Time.timeScale;
            _previous = scale;
        }

    }

}
