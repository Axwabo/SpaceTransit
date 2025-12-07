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

        [SerializeField]
        private TextMeshProUGUI destination;

        [SerializeField]
        private TextMeshProUGUI time;

        [SerializeField]
        private TextMeshProUGUI dock;

        public override void Apply(DepartureEntry item)
        {
            type.text = item.Route.Type switch
            {
                ServiceType.Passenger => nameof(ServiceType.Passenger),
                ServiceType.Fast => nameof(ServiceType.Fast),
                ServiceType.InterHub => nameof(ServiceType.InterHub),
                _ => "???"
            };
            destination.text = item.Route.Destination.Station.name;
            time.text = item.Departure.Departure.Value.ToString("hh':'mm");
            dock.text = (item.Departure.DockIndex + 1).ToString();
        }

    }

}
