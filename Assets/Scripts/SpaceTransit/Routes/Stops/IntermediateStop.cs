using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class IntermediateStop : Stop, IArrival, IDeparture
    {

        [field: SerializeField]
        public TimeOnly Arrival { get; private set; }

        [field: SerializeField]
        public TimeOnly Departure { get; private set; }

        [field: SerializeField]
        public int MinStayMinutes { get; private set; }

    }

}
