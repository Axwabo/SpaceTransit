using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class Origin : Stop, IDeparture
    {

        [field: SerializeField]
        public TimeOnly Departure { get; set; }

        [field: SerializeField]
        public StationId ExitTowards { get; private set; }

    }

}
