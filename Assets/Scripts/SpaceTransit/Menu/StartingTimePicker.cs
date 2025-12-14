using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    public sealed class StartingTimePicker : MonoBehaviour
    {

        private static TimeSpan Time(float h, float m, float s) => new((int) h, (int) m, (int) s);

        [SerializeField]
        private Slider hours;

        [SerializeField]
        private Slider minutes;

        [SerializeField]
        private Slider seconds;

        [SerializeField]
        private TextMeshProUGUI display;

        private void Awake()
        {
            var min = Time(hours.minValue, minutes.minValue, seconds.minValue);
            var max = Time(hours.maxValue, minutes.maxValue, seconds.maxValue);
            if (Clock.StartTime < min)
                Clock.StartTime = min;
            else if (Clock.StartTime > max)
                Clock.StartTime = max;
            Apply();
            hours.onValueChanged.AddListener(OnValueChanged);
            minutes.onValueChanged.AddListener(OnValueChanged);
            seconds.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float arg0)
        {
            Clock.StartTime = Time(hours.value, minutes.value, seconds.value);
            Apply();
        }

        private void Apply()
        {
            var start = Clock.StartTime;
            display.text = $"Starting time: {start:hh':'mm':'ss}";
            hours.value = start.Hours;
            minutes.value = start.Minutes;
            seconds.value = start.Seconds;
        }

    }

}
