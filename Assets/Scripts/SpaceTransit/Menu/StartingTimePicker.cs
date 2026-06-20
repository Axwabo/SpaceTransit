using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class StartingTimePicker : MonoBehaviour
    {

        private SliderInt _hours;

        private SliderInt _minutes;

        private SliderInt _seconds;

        private Label _display;

        private void Start()
        {
            var visual = this.RootVisual();
            _hours = visual.Q<SliderInt>("H");
            _minutes = visual.Q<SliderInt>("M");
            _seconds = visual.Q<SliderInt>("S");
            _display = visual.Q<Label>("Time");
            var min = new TimeSpan(_hours.lowValue, _minutes.lowValue, _seconds.lowValue);
            var max = new TimeSpan(_hours.highValue, _minutes.highValue, _seconds.highValue);
            if (Clock.StartTime < min)
                Clock.StartTime = min;
            else if (Clock.StartTime > max)
                Clock.StartTime = max;
            Apply();
            _hours.RegisterValueChangedCallback(OnValueChanged);
            _minutes.RegisterValueChangedCallback(OnValueChanged);
            _seconds.RegisterValueChangedCallback(OnValueChanged);
        }

        private void OnValueChanged(ChangeEvent<int> evt)
        {
            Clock.StartTime = new TimeSpan(_hours.value, _minutes.value, _seconds.value);
            Apply();
        }

        private void Apply()
        {
            var start = Clock.StartTime;
            _display.text = $"Starting time: {start:hh':'mm':'ss}";
            _hours.value = start.Hours;
            _minutes.value = start.Minutes;
            _seconds.value = start.Seconds;
        }

    }

}
