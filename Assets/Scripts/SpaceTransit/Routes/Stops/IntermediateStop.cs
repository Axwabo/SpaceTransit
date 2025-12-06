using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class IntermediateStop : Stop, IArrival, IDeparture
    {

        [field: SerializeField]
        public TimeOnly Arrival { get; set; }

        [field: SerializeField]
        public TimeOnly Departure { get; set; }

        [field: FormerlySerializedAs("<ExitTowards>k__BackingField")]
        [field: SerializeField]
        public StationId ExitTowards { get; private set; }

        [field: SerializeField]
        public int MinStayMinutes { get; private set; }

    }

}
