using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class HouseOrigin : Target, IOrigin
    {

        [field: SerializeField]
        public TimeOnly Departure { get; private set; }

        [field: SerializeField]
        public StationId ExitTowards { get; private set; }

    }

}
