using SpaceTransit.Routes.Stops;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class StopRow : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI station;

        [SerializeField]
        private TextMeshProUGUI arrival;

        [SerializeField]
        private TextMeshProUGUI departure;

        [SerializeField]
        private TextMeshProUGUI dock;

        public void Apply(Stop stop)
        {
            arrival.text = stop is IArrival {Arrival: {Value: var arrivalTime}}
                ? arrivalTime.ToString(Stop.TimeFormat)
                : "";
            departure.text = stop is IDeparture {Departure: {Value: var departureTime}}
                ? departureTime.ToString(Stop.TimeFormat)
                : "";
            station.text = stop.Station.name;
            dock.text = (stop.DockIndex + 1).ToString();
        }

    }

}
