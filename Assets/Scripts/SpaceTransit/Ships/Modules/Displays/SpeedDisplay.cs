using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Modules.Displays
{

    public sealed class SpeedDisplay : ModuleUIComponent
    {

        private Label _text;

        private ShipSpeed _previous;

        [SerializeField]
        private bool max;

        private void Update()
        {
            var current = Assembly.CurrentSpeed;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (current.Raw != _previous.Raw)
                UpdateText(current);
        }

        protected override void Initialize(VisualElement root) => _text = root.Q<Label>("Speed");

        private void UpdateText(ShipSpeed current)
        {
            _text.text = max && Mathf.Approximately(Assembly.MaxSpeed, current.Raw)
                ? "MAX"
                : current.RawKmh.ToString("N0");
            _previous = current;
        }

    }

}
