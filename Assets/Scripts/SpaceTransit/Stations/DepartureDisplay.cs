using SpaceTransit.Routes;
using SpaceTransit.Vaulter;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Stations
{

    public sealed class DepartureDisplay : EntryDisplayBase<DepartureEntry>
    {

        [SerializeField]
        private TextMeshProUGUI type;

        public override void Apply(DepartureEntry item) => type.text = item.Route.Type switch
        {
            ServiceType.Passenger => nameof(ServiceType.Passenger),
            ServiceType.Fast => nameof(ServiceType.Fast),
            ServiceType.InterHub => nameof(ServiceType.InterHub),
            _ => "???"
        }; // TODO

    }

}
