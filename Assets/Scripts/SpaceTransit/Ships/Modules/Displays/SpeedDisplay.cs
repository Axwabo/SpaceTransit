using TMPro;
using UnityEngine;

namespace SpaceTransit.Ships.Modules.Displays
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class SpeedDisplay : ModuleComponentBase
    {

        private TextMeshProUGUI _text;

        private ShipSpeed _previous;

        [SerializeField]
        private bool max;

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            var current = Assembly.CurrentSpeed;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (current.Raw != _previous.Raw)
                UpdateText(current);
        }

        protected override void OnInitialized() => UpdateText(Assembly.CurrentSpeed);

        private void UpdateText(ShipSpeed current)
        {
            _text.text = max && Mathf.Approximately(Assembly.MaxSpeed, current.Raw)
                ? "MAX"
                : current.RawKmh.ToString("N0");
            _previous = current;
        }

    }

}
