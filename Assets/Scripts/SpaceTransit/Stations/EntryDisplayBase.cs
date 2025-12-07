using System;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Stations
{

    public abstract class EntryDisplayBase<T> : MonoBehaviour
    {

        [SerializeField]
        protected TextMeshProUGUI type;

        [FormerlySerializedAs("destination")]
        [SerializeField]
        protected TextMeshProUGUI station;

        [SerializeField]
        protected TextMeshProUGUI time;

        [SerializeField]
        protected TextMeshProUGUI dock;

        public abstract void Apply(T item);

        protected void Apply(ServiceType serviceType, string stationName, TimeSpan timeValue, int dockIndex)
        {
            type.text = serviceType switch
            {
                ServiceType.Passenger => nameof(ServiceType.Passenger),
                ServiceType.Fast => nameof(ServiceType.Fast),
                ServiceType.InterHub => nameof(ServiceType.InterHub),
                _ => "???"
            };
            station.text = stationName;
            time.text = timeValue.ToString("hh':'mm");
            dock.text = (dockIndex + 1).ToString();
        }

    }

}
