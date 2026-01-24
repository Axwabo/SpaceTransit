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

        private string _route;

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
                InformationType.Route => _route,
                InformationType.NextStop when IsOrigin && Controller.State is ShipState.Docked or ShipState.WaitingForDeparture => "Welcome!",
                InformationType.NextStop when IsTerminus && Controller.State is ShipState.Landing or ShipState.Docked => "Goodbye!",
                InformationType.NextStop when IsTerminus => "Next Stop Terminus",
                InformationType.NextStop => $"{Prefix}{Parent.Stop.Station.name}",
                InformationType.Time => Clock.Now.ToString(TimeOnly.Format),
                _ => ""
            };
        }

        public override void OnRouteChanged()
        {
            if (!IsInService)
            {
                _text.text = "";
                return;
            }

            var (text, color) = Parent.Route.GetAbbreviation();
            _route = $"<mark=#cccccc09><size=0>.</size><space=0.2em><color={color.ToHex()}>{text}</color><space=0.2em><size=0>.</size></mark> » {Parent.Route.Destination.Station.name}";
        }

        private enum InformationType
        {

            Route,
            NextStop,
            Time

        }

    }

}
