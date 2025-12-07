using TMPro;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class DestinationDisplay : VaulterComponentBase
    {

        private TextMeshProUGUI _text;

        protected override void Awake() => _text = GetComponent<TextMeshProUGUI>();

        public override void OnRouteChanged() => _text.text = IsInService ? Parent.Route.Destination.Station.name : "";

    }

}
