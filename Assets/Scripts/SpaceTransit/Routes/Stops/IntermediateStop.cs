using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class IntermediateStop : Stop, IArrival, IDeparture
    {

        [field: SerializeField]
        public StationId ArriveFrom { get; private set; }

        [field: SerializeField]
        public TimeOnly Arrival { get; set; }

        [field: SerializeField]
        public TimeOnly Departure { get; set; }

        [field: FormerlySerializedAs("<ExitTowards>k__BackingField")]
        [field: SerializeField]
        public StationId ExitTowards { get; private set; }

        [field: SerializeField]
        public int MinStayMinutes { get; private set; }

        public IntermediateStop Add(TimeSpan departure) => new()
        {
            Station = Station,
            DockIndex = DockIndex,
            ArriveFrom = ArriveFrom,
            Arrival = Arrival.Value + departure,
            Departure = Departure.Value + departure,
            ExitTowards = ExitTowards,
            MinStayMinutes = MinStayMinutes
        };

    }

}
