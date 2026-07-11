using SpaceTransit.Ships;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Vaulter
{

    public sealed class OnboardDisplay : VaulterComponentBase
    {

        private float _remaining;

        private string _route;

        private InformationType _type;

        private UIDocument _document;

        [CreateProperty]
        public bool Visible { get; set; }

        [CreateProperty]
        public string Stop { get; set; }

        [CreateProperty]
        public string ServiceType { get; set; }

        [CreateProperty]
        public Color Color { get; set; }

        [CreateProperty]
        public string Destination { get; set; }

        private string Prefix => Controller.State is ShipState.LiftingOff or ShipState.Sailing ? "Next Stop: " : "";

        protected override void Awake()
        {
            base.Awake();
            _document = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _document.rootVisualElement.dataSource = this;
            RefreshClass();
        }

        private void Update()
        {
            if (Controller.IsRestarting || !IsInService || (_remaining -= Clock.Delta) > 0)
                return;
            _remaining = 5;
            if (++_type > InformationType.Time)
                _type = InformationType.Route;
            RefreshClass();
            Stop = _type switch
            {
                InformationType.NextStop when IsOrigin && Controller.State is ShipState.Docked or ShipState.WaitingForDeparture => "Welcome!",
                InformationType.NextStop when IsTerminus && Controller.State is ShipState.Landing or ShipState.Docked => "Goodbye!",
                InformationType.NextStop when IsTerminus => "Next Stop Terminus",
                InformationType.NextStop => $"{Prefix}{Parent.Stop.Station.name}",
                InformationType.Time => Clock.Now.ToString(TimeOnly.Format),
                _ => ""
            };
        }

        private void RefreshClass()
        {
            if (_document.rootVisualElement is not { } element)
                return;
            element.ClearClassList();
            element.AddToClassList(_type == InformationType.Route ? "route" : "stop");
        }

        public override void OnRouteChanged()
        {
            if (!IsInService)
                return;
            (ServiceType, Color) = Parent.Route.GetAbbreviation();
            Destination = Parent.Route.Destination.Station.name;
        }

        public override void OnRestarting() => Visible = false;

        public override void OnRestarted() => Visible = true;

        private enum InformationType
        {

            Route,
            NextStop,
            Time

        }

    }

}
