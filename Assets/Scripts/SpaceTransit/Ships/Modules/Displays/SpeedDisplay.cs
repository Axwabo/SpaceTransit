using TMPro;
using UnityEngine;

namespace SpaceTransit.Ships.Modules.Displays
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class SpeedDisplay : ModuleComponentBase
    {

        private TextMeshProUGUI _text;

        [SerializeField]
        private bool max;

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() => _text.text = max && Mathf.Approximately(Assembly.MaxSpeed, Assembly.CurrentSpeed.Raw)
            ? "MAX"
            : Assembly.CurrentSpeed.RawKmh.ToString("N0");

    }

}
