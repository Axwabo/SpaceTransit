using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace SpaceTransit
{

    public sealed class Clock : MonoBehaviour
    {

        private static TimeSpan _startTime = DateTime.Now.TimeOfDay;

        public static TimeSpan Now => _startTime.Add(TimeSpan.FromSeconds(Time.timeSinceLevelLoadAsDouble));

        public static float UnscaledDelta => Mathf.Min(0.3f, Time.unscaledDeltaTime);

        public static float Delta => UnscaledDelta * Time.timeScale;

        public static float FixedDelta => Mathf.Min(0.3f, Time.fixedUnscaledDeltaTime) * Time.timeScale;

        private TextMeshProUGUI _text;

        [SerializeField]
        private string start;

        private void Awake()
        {
            if (!TryGetComponent(out _text))
                Destroy(this);
            if (TimeSpan.TryParse(start, CultureInfo.InvariantCulture, out var startTime))
                _startTime = startTime;
        }

        private void Update() => _text.text = Now.ToString("hh':'mm':'ss");

    }

}
