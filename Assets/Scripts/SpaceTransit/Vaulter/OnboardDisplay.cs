using SpaceTransit.Ships;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class OnboardDisplay : VaulterComponentBase
    {

        private TextMeshProUGUI _text;

        private float _remaining;

        private InformationType _type;

        private string Prefix => Controller.State is ShipState.LiftingOff or ShipState.Sailing ? "Next Stop: " : "";

        protected override void Awake() => _text = GetComponent<TextMeshProUGUI>();

        private void Update()
        {
            if (!IsInService || (_remaining -= Clock.Delta) > 0)
                return;
            _remaining = 5;
            if (++_type > InformationType.Time)
                _type = InformationType.Route;
            _text.text = _type switch
            {
                InformationType.Route => $"» {Parent.Route.Destination.Station.name}",
                InformationType.NextStop => $"{Prefix}{Parent.Stop.Station.name}",
                InformationType.Time => Clock.Now.ToString("hh':'mm"),
                _ => ""
            };
        }

        public override void OnRouteChanged()
        {
            if (!IsInService)
                _text.text = "";
        }

        private enum InformationType
        {

            Route,
            NextStop,
            Time

        }

    }

}
