using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class Origin : Stop, IDeparture
    {

        [field: SerializeField]
        public TimeOnly Departure { get; private set; }

    }

}
