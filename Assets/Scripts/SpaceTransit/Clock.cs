using System;
using TMPro;
using UnityEngine;

namespace SpaceTransit
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class Clock : MonoBehaviour
    {

        public static TimeSpan Now => new TimeSpan(7, 29, 50).Add(TimeSpan.FromSeconds(Time.timeAsDouble));

        public static float UnscaledDelta => Mathf.Min(0.3f, Time.unscaledDeltaTime);

        public static float Delta => UnscaledDelta * Time.timeScale;

        public static float FixedDelta => Mathf.Min(0.3f, Time.fixedUnscaledDeltaTime) * Time.timeScale;

        private TextMeshProUGUI _text;

        private void Awake() => _text = GetComponent<TextMeshProUGUI>();

        private void Update() => _text.text = Now.ToString("hh':'mm':'ss");

    }

}
