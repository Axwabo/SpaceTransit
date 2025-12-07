using System.Text;
using SpaceTransit.Routes.Stops;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class VaulterDriverDisplay : VaulterComponentBase
    {

        private static readonly StringBuilder Builder = new();

        private TextMeshProUGUI _text;

        protected override void Awake() => _text = GetComponent<TextMeshProUGUI>();

        public override void OnStopChanged()
        {
            Builder.Clear().AppendLine(Parent.Stop.Station.Name);
            if (Parent.Stop is IArrival {Arrival: var arrival})
                Builder.AppendLine($"Arrive at {arrival.Value:hh':'mm}");
            if (Parent.Stop is IDeparture {Departure: var time})
                Builder.AppendLine($"Depart at {time.Value:hh':'mm}");
            _text.text = Builder.Append("Dock: ").Append(Parent.Stop.DockIndex + 1).ToString();
        }

    }

}
