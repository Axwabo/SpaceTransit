using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Modules.Displays
{

    [RequireComponent(typeof(UIDocument))]
    public sealed class SpeedDisplay : ModuleComponentBase
    {

        private Label _text;

        private ShipSpeed _previous;

        [SerializeField]
        private bool max;

        private void Start() => _text = GetComponent<UIDocument>().rootVisualElement.Q<Label>("Speed");

        private void Update()
        {
            var current = Assembly.CurrentSpeed;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (current.Raw != _previous.Raw)
                UpdateText(current);
        }

        private void UpdateText(ShipSpeed current)
        {
            _text.text = max && Mathf.Approximately(Assembly.MaxSpeed, current.Raw)
                ? "MAX"
                : current.RawKmh.ToString("N0");
            _previous = current;
        }

    }

}
