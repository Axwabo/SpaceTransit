using SpaceTransit.Routes.Stops;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class DepartureTimeDisplay : VaulterComponentBase
    {

        private TextMeshProUGUI _text;

        protected override void Awake() => _text = GetComponent<TextMeshProUGUI>();

        public override void OnStopChanged()
        {
            if (Parent.Stop is IDeparture {Departure: var time})
                _text.text = $"Depart at: {time.Value:hh':'mm}";
        }

    }

}
