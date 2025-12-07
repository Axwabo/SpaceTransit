using SpaceTransit.Routes;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class RouteDisplay : VaulterComponentBase
    {

        public static string Format(RouteDescriptor route)
            => $"{route.name} {route.Origin.Station.name} {route.Origin.Departure.Value:hh':'mm} - {route.Destination.Station.name} {route.Destination.Arrival.Value:hh':'mm}";

        private TextMeshProUGUI _text;

        private float _remaining;

        protected override void Awake() => _text = GetComponent<TextMeshProUGUI>();

        public override void OnRouteChanged() => _text.text = IsInService ? Format(Parent.Route) : "Not In Service";

    }

}
