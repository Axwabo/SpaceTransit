using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit
{

    public sealed class Clock : MonoBehaviour
    {

        private static readonly DateTime Today = DateTime.Today;

        public static double OffsetSeconds { get; set; }

        public static DateTime Date => Today.Add(StartTime).AddSeconds(Time.timeSinceLevelLoadAsDouble + OffsetSeconds);

        public static TimeSpan Now => Date.TimeOfDay;

        public static float UnscaledDelta => Mathf.Min(0.3f, Time.unscaledDeltaTime);

        public static float Delta => UnscaledDelta * Time.timeScale;

        public static float FixedDelta => Mathf.Min(0.3f, Time.fixedUnscaledDeltaTime) * Time.timeScale;

        public static TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        private bool _isUIDocument;

        private TextMeshProUGUI _text;

        private Label _label;

        private int _previous;

        [SerializeField]
        private string start;

        private void Awake()
        {
            if (TryGetComponent(out UIDocument document))
            {
                _label = document.rootVisualElement.Q<Label>("Clock");
                _isUIDocument = true;
            }
            else if (TryGetComponent(out _text))
            {
                Destroy(this);
            }
            else
            {
                Destroy(this);
            }

            if (TimeSpan.TryParse(start, CultureInfo.InvariantCulture, out var startTime))
                StartTime = startTime;
        }

        private void Update()
        {
            var now = Now;
            var seconds = now.Seconds;
            if (seconds == _previous)
                return;
            var content = now.ToString("hh':'mm':'ss");
            if (_isUIDocument)
                _label.text = content;
            else
                _text.text = content;
            _previous = seconds;
        }

    }

}
