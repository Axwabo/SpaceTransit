using System;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
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
            type.text = serviceType.ToStringFast();
            station.text = stationName;
            time.text = timeValue.ToString(Stop.TimeFormat);
            dock.text = (dockIndex + 1).ToString();
        }

    }

}
