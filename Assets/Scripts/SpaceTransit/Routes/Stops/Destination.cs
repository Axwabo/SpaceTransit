using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class Destination : Stop, IArrival
    {

        [field: SerializeField]
        public TimeOnly Arrival { get; private set; }

    }

}
