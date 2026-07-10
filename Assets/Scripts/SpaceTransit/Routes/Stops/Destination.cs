using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class Destination : Stop, IArrival, IDestination
    {

        [field: SerializeField]
        public TimeOnly Arrival { get; set; }

    }

}
