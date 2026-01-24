using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace SpaceTransit
{

    public sealed class Clock : MonoBehaviour
    {

        public static double OffsetSeconds { get; set; }

        public static TimeSpan Now => DateTime.Today.Add(StartTime).Add(TimeSpan.FromSeconds(Time.timeSinceLevelLoadAsDouble + OffsetSeconds)).TimeOfDay;

        public static float UnscaledDelta => Mathf.Min(0.3f, Time.unscaledDeltaTime);

        public static float Delta => UnscaledDelta * Time.timeScale;

        public static float FixedDelta => Mathf.Min(0.3f, Time.fixedUnscaledDeltaTime) * Time.timeScale;

        public static TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        private TextMeshProUGUI _text;

        private int _previous;

        [SerializeField]
        private string start;

        private void Awake()
        {
            if (!TryGetComponent(out _text))
                Destroy(this);
            if (TimeSpan.TryParse(start, CultureInfo.InvariantCulture, out var startTime))
                StartTime = startTime;
        }

        private void Update()
        {
            var now = Now;
            var seconds = now.Seconds;
            if (seconds == _previous)
                return;
            _text.text = now.ToString("hh':'mm':'ss");
            _previous = seconds;
        }

    }

}
